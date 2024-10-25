using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using Buddy.Coroutines;
using HREngine.Bots;
using IronPython.Modules;
using log4net;
using Microsoft.Scripting.Hosting;
using Triton.Bot;
using Triton.Common;
using Triton.Game;
using Triton.Game.Data;


//!CompilerOption|AddRef|IronPython.dll
//!CompilerOption|AddRef|IronPython.Modules.dll
//!CompilerOption|AddRef|Microsoft.Scripting.dll
//!CompilerOption|AddRef|Microsoft.Dynamic.dll
//!CompilerOption|AddRef|Microsoft.Scripting.Metadata.dll
using Triton.Game.Mapping;
using Triton.Bot.Logic.Bots.DefaultBot;
using Logger = Triton.Common.LogUtilities.Logger;
using System.Diagnostics;
using System.Threading;

namespace HREngine.Bots
{
    public class DefaultRoutine : IRoutine
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private readonly ScriptManager _scriptManager = new ScriptManager();

        private readonly List<Tuple<string, string>> _mulliganRules = new List<Tuple<string, string>>();

        private int dirtyTargetSource = -1;
        private int stopAfterWins = 30;
        private int concedeLvl = 5; // the rank, till you want to concede
        private int dirtytarget = -1;
        private int dirtychoice = -1;
        private string choiceCardId = "";
        DateTime starttime = DateTime.Now;
        bool enemyConcede = false;

        bool firstMove = true;
        bool firstTurn = true;
        bool canBeDelay = false;

        public bool learnmode = false;
        public bool printlearnmode = true;

        Silverfish sf = Silverfish.Instance;
        DefaultBotSettings botset
        {
            get { return DefaultBotSettings.Instance; }
        }
        //uncomment the desired option, or leave it as is to select via the interface
        Behavior behave = new Behavior丨通用丨不设惩罚();
        //Behavior behave = new BehaviorRush();


        public DefaultRoutine()
        {
            _mulliganRules.Add(new Tuple<string, string>("True", "card.Entity.Cost >= 4 and card.Entity.Id != \"GVG_063\""));
            Helpfunctions.Instance.ErrorLog("----------------------------");
            Helpfunctions.Instance.ErrorLog("您正在使用的AI版本为" + Silverfish.Instance.versionnumber);
            Helpfunctions.Instance.ErrorLog("----------------------------");
        }

        #region Scripting

        private const string BoilerPlateExecute = @"
import sys
sys.stdout=ioproxy

def Execute():
    return bool({0})";

        public delegate void RegisterScriptVariableDelegate(ScriptScope scope);

        public bool GetCondition(string expression, IEnumerable<RegisterScriptVariableDelegate> variables)
        {
            var code = string.Format(BoilerPlateExecute, expression);
            var scope = _scriptManager.Scope;
            var scriptSource = _scriptManager.Engine.CreateScriptSourceFromString(code);
            scope.SetVariable("ioproxy", _scriptManager.IoProxy);
            foreach (var variable in variables)
            {
                variable(scope);
            }
            scriptSource.Execute(scope);
            return scope.GetVariable<Func<bool>>("Execute")();
        }

        public bool VerifyCondition(string expression,
            IEnumerable<string> variables, out Exception ex)
        {
            ex = null;
            try
            {
                var code = string.Format(BoilerPlateExecute, expression);
                var scope = _scriptManager.Scope;
                var scriptSource = _scriptManager.Engine.CreateScriptSourceFromString(code);
                scope.SetVariable("ioproxy", _scriptManager.IoProxy);
                foreach (var variable in variables)
                {
                    scope.SetVariable(variable, new object());
                }
                scriptSource.Compile();
            }
            catch (Exception e)
            {
                ex = e;
                return false;
            }
            return true;
        }

        #endregion

        #region Implementation of IAuthored

        /// <summary> The name of the routine. </summary>
        public string Name
        {
            get { return "天梯策略"; }
        }

        /// <summary> The description of the routine. </summary>
        public string Description
        {
            get { return "【李文浩+看书范+酸小明】修改的天梯策略."; }
        }

        /// <summary>The author of this routine.</summary>
        public string Author
        {
            get { return "李文浩+酸小明"; }
        }

        /// <summary>The version of this routine.</summary>
        public string Version
        {
            get { return "1.0.0.0"; }
        }

        #endregion

        #region Implementation of IBase

        /// <summary>Initializes this routine.</summary>
        public void Initialize()
        {
            _scriptManager.Initialize(null,
                new List<string>
                {
                    "Triton.Game",
                    "Triton.Bot",
                    "Triton.Common",
                    "Triton.Game.Mapping",
                    "Triton.Game.Abstraction"
                });
        }

        /// <summary>Deinitializes this routine.</summary>
        public void Deinitialize()
        {
            _scriptManager.Deinitialize();
        }

        #endregion

        #region Implementation of IRunnable

        /// <summary> The routine start callback. Do any initialization here. </summary>
        public void Start()
        {
            firstTurn = true;
            GameEventManager.NewGame += GameEventManagerOnNewGame;
            GameEventManager.GameOver += GameEventManagerOnGameOver;
            GameEventManager.QuestUpdate += GameEventManagerOnQuestUpdate;
            GameEventManager.ArenaRewards += GameEventManagerOnArenaRewards;

            DefaultRoutineSettings.Instance.ReloadFile();

            if (Hrtprozis.Instance.settings == null)
            {
                Hrtprozis.Instance.setInstances();
                ComboBreaker.Instance.setInstances();
                PenalityManager.Instance.setInstances();
            }
            behave = sf.getBehaviorByName(DefaultRoutineSettings.Instance.DefaultBehavior);
            foreach (var tuple in _mulliganRules)
            {
                Exception ex;
                if (
                    !VerifyCondition(tuple.Item1, new List<string> { "mulliganData" }, out ex))
                {
                    Log.ErrorFormat("[开始] 发现一个错误的留牌策略为 [{1}]: {0}.", ex,
                        tuple.Item1);
                    BotManager.Stop();
                }

                if (
                    !VerifyCondition(tuple.Item2, new List<string> { "mulliganData", "card" },
                        out ex))
                {
                    Log.ErrorFormat("[开始] 发现一个错误的留牌策略为 [{1}]: {0}.", ex,
                        tuple.Item2);
                    BotManager.Stop();
                }
            }
        }

        /// <summary> The routine tick callback. Do any update logic here. </summary>
        public void Tick()
        {
        }

        /// <summary> The routine stop callback. Do any pre-dispose cleanup here. </summary>
        public void Stop()
        {
            GameEventManager.NewGame -= GameEventManagerOnNewGame;
            GameEventManager.GameOver -= GameEventManagerOnGameOver;
            GameEventManager.QuestUpdate -= GameEventManagerOnQuestUpdate;
            GameEventManager.ArenaRewards -= GameEventManagerOnArenaRewards;
        }

        #endregion

        #region Implementation of IConfigurable

        /// <summary> The routine's settings control. This will be added to the Hearthbuddy Settings tab.</summary>
        public UserControl Control
        {
            get
            {


                using (var fs = new FileStream(@"Routines\DefaultRoutine\SettingsGui.xaml", FileMode.Open))
                {
                    var root = (UserControl)XamlReader.Load(fs);

                    // Your settings binding here.

                    // ArenaPreferredClass1
                    // if (
                    //     !Wpf.SetupComboBoxItemsBinding(root, "ArenaPreferredClass1ComboBox", "AllClasses",
                    //         BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxItemsBinding failed for 'ArenaPreferredClass1ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // if (
                    //     !Wpf.SetupComboBoxSelectedItemBinding(root, "ArenaPreferredClass1ComboBox",
                    //         "ArenaPreferredClass1", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ArenaPreferredClass1ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // // ArenaPreferredClass2
                    // if (
                    //     !Wpf.SetupComboBoxItemsBinding(root, "ArenaPreferredClass2ComboBox", "AllClasses",
                    //         BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxItemsBinding failed for 'ArenaPreferredClass2ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // if (
                    //     !Wpf.SetupComboBoxSelectedItemBinding(root, "ArenaPreferredClass2ComboBox",
                    //         "ArenaPreferredClass2", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ArenaPreferredClass2ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // // ArenaPreferredClass3
                    // if (
                    //     !Wpf.SetupComboBoxItemsBinding(root, "ArenaPreferredClass3ComboBox", "AllClasses",
                    //         BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxItemsBinding failed for 'ArenaPreferredClass3ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // if (
                    //     !Wpf.SetupComboBoxSelectedItemBinding(root, "ArenaPreferredClass3ComboBox",
                    //         "ArenaPreferredClass3", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ArenaPreferredClass3ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // // ArenaPreferredClass4
                    // if (
                    //     !Wpf.SetupComboBoxItemsBinding(root, "ArenaPreferredClass4ComboBox", "AllClasses",
                    //         BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxItemsBinding failed for 'ArenaPreferredClass4ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // if (
                    //     !Wpf.SetupComboBoxSelectedItemBinding(root, "ArenaPreferredClass4ComboBox",
                    //         "ArenaPreferredClass4", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ArenaPreferredClass4ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // // ArenaPreferredClass5
                    // if (
                    //     !Wpf.SetupComboBoxItemsBinding(root, "ArenaPreferredClass5ComboBox", "AllClasses",
                    //         BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxItemsBinding failed for 'ArenaPreferredClass5ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // if (
                    //     !Wpf.SetupComboBoxSelectedItemBinding(root, "ArenaPreferredClass5ComboBox",
                    //         "ArenaPreferredClass5", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    // {
                    //     Log.DebugFormat(
                    //         "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ArenaPreferredClass5ComboBox'.");
                    //     throw new Exception("The SettingsControl could not be created.");
                    // }

                    // defaultBehaviorComboBox1
                    if (
                        !Wpf.SetupComboBoxItemsBinding(root, "defaultBehaviorComboBox1", "AllBehav",
                            BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupComboBoxItemsBinding failed for 'defaultBehaviorComboBox1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }



                    if (
                        !Wpf.SetupComboBoxSelectedItemBinding(root, "defaultBehaviorComboBox1",
                            "DefaultBehavior", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'defaultBehaviorComboBox1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // 表情
                    if (
                        !Wpf.SetupComboBoxItemsBinding(root, "emoteComboBox", "AllEmote",
                            BindingMode.OneWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupComboBoxItemsBinding failed for 'emoteComboBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (
                        !Wpf.SetupComboBoxSelectedItemBinding(root, "emoteComboBox",
                            "DefaultEmote", BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'emoteComboBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }
                    // Your settings event handlers here.

                    // MaxWide
                    if (
                        !Wpf.SetupTextBoxBinding(root, "MaxWideTextBox", "MaxWide",
                        BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'MaxWideTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // MaxDeep
                    if (
                        !Wpf.SetupTextBoxBinding(root, "MaxDeepTextBox", "MaxDeep",
                        BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'MaxDeepTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // MaxCal
                    if (
                        !Wpf.SetupTextBoxBinding(root, "MaxCalTextBox", "MaxCal",
                        BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'MaxCalTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }


                    // UseSecretsPlayAround
                    if (
                        !Wpf.SetupCheckBoxBinding(root, "UseSecretsPlayAroundCheckBox",
                            "UseSecretsPlayAround",
                            BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupCheckBoxBinding failed for 'UseSecretsPlayAroundCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // SetLogCheckBox
                    if (
                        !Wpf.SetupCheckBoxBinding(root, "SetLogCheckBox",
                            "SetLog",
                            BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupCheckBoxBinding failed for 'SetLogCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // 打印自定义惩罚值
                    if (
                        !Wpf.SetupCheckBoxBinding(root, "PrintPenaltiesCheckBox",
                            "UsePrintPenalties",
                            BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupCheckBoxBinding failed for 'PrintPenaltiesCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // 打印最终出牌
                    if (
                        !Wpf.SetupCheckBoxBinding(root, "PrintNextMoveCheckBox",
                            "UsePrintNextMove",
                            BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupCheckBoxBinding failed for 'PrintNextMoveCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }



                    // BerserkIfCanFinishNextTour
                    if (
                        !Wpf.SetupCheckBoxBinding(root, "BerserkIfCanFinishNextTourCheckBox",
                            "BerserkIfCanFinishNextTour",
                            BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupCheckBoxBinding failed for 'BerserkIfCanFinishNextTourCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // 打脸阈值
                    if (
                        !Wpf.SetupTextBoxBinding(root, "EnfacehpTextBox", "Enfacehp",
                        BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'EnfacehpTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // 打脸奖励
                    if (
                        !Wpf.SetupTextBoxBinding(root, "EnfaceRewardTextBox", "EnfaceReward",
                        BindingMode.TwoWay, DefaultRoutineSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'EnfaceRewardTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    var openButton = Wpf.FindControlByName<Button>(root, "lastMatch");
                    openButton.Click += LastMatchOnClick;

                    var clearLogButton = Wpf.FindControlByName<Button>(root, "clearLogButton");
                    clearLogButton.Click += clearLog;

                    return root;
                }
            }
        }

        private void LastMatchOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (printUtils.recordPath != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", printUtils.recordPath);
            }
        }

        private void clearLog(object sender, RoutedEventArgs routedEventArgs)
        {
            string pLocalFilePath = @".\Routines\delLog.bat";//要复制的文件路径
            string pSaveFilePath = @".\delLog.bat";//指定存储的路径
            if (File.Exists(pLocalFilePath))//必须判断要复制的文件是否存在
            {
                File.Copy(pLocalFilePath, pSaveFilePath, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
            }
            Process proc = null;
            try
            {
                proc = new Process();
                proc.StartInfo.FileName = @".\delLog.bat";
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();
                File.Delete(pSaveFilePath);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>The settings object. This will be registered in the current configuration.</summary>
        public JsonSettings Settings
        {
            get { return DefaultRoutineSettings.Instance; }
        }

        #endregion

        #region Implementation of IRoutine

        /// <summary>
        /// Sends data to the routine with the associated name.
        /// </summary>
        /// <param name="name">The name of the configuration.</param>
        /// <param name="param">The data passed for the configuration.</param>
        public void SetConfiguration(string name, params object[] param)
        {
            if (name == "DefaultBehavior")
            {
                DefaultRoutineSettings.Instance.DefaultBehavior = param[0] as string;
            }
        }

        /// <summary>
        /// Requests data from the routine with the associated name.
        /// </summary>
        /// <param name="name">The name of the configuration.</param>
        /// <returns>Data from the routine.</returns>
        public object GetConfiguration(string name)
        {
            return null;
        }

        /// <summary>
        /// The routine's coroutine logic to execute.
        /// </summary>
        /// <param name="type">The requested type of logic to execute.</param>
        /// <param name="context">Data sent to the routine from the bot for the current logic.</param>
        /// <returns>true if logic was executed to handle this type and false otherwise.</returns>
        public async Task<bool> Logic(string type, object context)
        {
            if (type == "new_game")
            {
                await NewGameLogic();
                return true;
            }

            // The bot is requesting mulligan logic.
            if (type == "mulligan")
            {
                await MulliganLogic(context as MulliganData);
                return true;
            }

            // The bot is requesting emote logic.
            if (type == "emote")
            {
                await EmoteLogic(context as EmoteData);
                return true;
            }

            // The bot is requesting our turn logic.
            if (type == "our_turn")
            {
                await OurTurnLogic();
                return true;
            }

            // The bot is requesting opponent turn logic.
            if (type == "opponent_turn")
            {
                await OpponentTurnLogic();
                return true;
            }

            // The bot is requesting our turn logic.
            if (type == "our_turn_combat")
            {
                await OurTurnCombatLogic();
                return true;
            }

            // The bot is requesting opponent turn logic.
            if (type == "opponent_turn_combat")
            {
                await OpponentTurnCombatLogic();
                return true;
            }

            // The bot is requesting arena draft logic.
            if (type == "arena_draft")
            {
                await ArenaDraftLogic(context as ArenaDraftData);
                return true;
            }

            // The bot is requesting quest handling logic.
            if (type == "handle_quests")
            {
                await HandleQuestsLogic(context as QuestData);
                return true;
            }

            // Whatever the current logic type is, this routine doesn't implement it.
            return false;
        }

        #region Mulligan

        private int RandomMulliganThinkTime()
        {
            var random = Client.Random;
            var type = random.Next(0, 100) % 4;

            if (type == 0) return random.Next(200, 400);
            if (type == 1) return random.Next(400, 600);
            if (type == 2) return random.Next(600, 800);
            return 0;
        }

        public async Task NewGameLogic()
        {
            Log.InfoFormat("[日志档案:] 开始创建");
            Hrtprozis prozis = Hrtprozis.Instance;
            prozis.clearAllNewGame();
            Silverfish.Instance.setnewLoggFile();
            Log.InfoFormat("[日志档案:] 创建完成");
            Helpfunctions.Instance.logg("NewGame");
        }

        /// <summary>
        /// This task implements custom mulligan choosing logic for the bot.
        /// The user is expected to set the Mulligans list elements to true/false 
        /// to signal to the bot which cards should/shouldn't be mulliganed. 
        /// This task should also implement humanization factors, such as hovering 
        /// over cards, or delaying randomly before returning, as the mulligan 
        /// process takes place as soon as the task completes.
        /// </summary>
        /// <param name="mulliganData">An object that contains relevant data for the mulligan process.</param>
        /// <returns></returns>
        public async Task MulliganLogic(MulliganData mulliganData)
        {
            // 创建日志位置
            printUtils.recordPath = string.Format(@".\Routines\DefaultRoutine\Silverfish\Test\Data\对局记录\日期{0}\{1}-{2}-{3}\", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH-mm-ss"), mulliganData.UserClass, mulliganData.OpponentClass);
            if (Directory.Exists(printUtils.recordPath) == false)
            {
                Directory.CreateDirectory(printUtils.recordPath);
            }
            //每局游戏开始时初始化数据

            Extensions.ResetMaxId();
            Silverfish.Instance.updateStartDeck();

            Log.InfoFormat("[开局留牌] {0} 对阵 {1}.", mulliganData.UserClass, mulliganData.OpponentClass);
            var count = mulliganData.Cards.Count;

            if (this.behave.BehaviorName() != DefaultRoutineSettings.Instance.DefaultBehavior)
            {
                behave = sf.getBehaviorByName(DefaultRoutineSettings.Instance.DefaultBehavior);
            }
            if (!Mulligan.Instance.getHoldList(mulliganData, this.behave))
            {
                for (var i = 0; i < count; i++)
                {
                    var card = mulliganData.Cards[i];

                    try
                    {
                        foreach (var tuple in _mulliganRules)
                        {
                            if (GetCondition(tuple.Item1,
                                new List<RegisterScriptVariableDelegate>
                            {
                                scope => scope.SetVariable("mulliganData", mulliganData)
                            }))
                            {
                                if (GetCondition(tuple.Item2,
                                    new List<RegisterScriptVariableDelegate>
                                {
                                    scope => scope.SetVariable("mulliganData", mulliganData),
                                    scope => scope.SetVariable("card", card)
                                }))
                                {
                                    mulliganData.Mulligans[i] = true;
                                    Log.InfoFormat(
                                        "[开局留牌] {0} 这张卡片符合自定义留牌规则: [{1}] ({2}).",
                                        card.Entity.Id, tuple.Item2, tuple.Item1);
                                }
                            }
                            else
                            {
                                Log.InfoFormat(
                                    "[开局留牌] 留牌策略检测发现 [{0}] 的规则错误, 所以 [{1}] 的规则不执行.",
                                    tuple.Item1, tuple.Item2);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("[Mulligan] An exception occurred: {0}.", ex);
                        BotManager.Stop();
                        return;
                    }
                }
            }

            var thinkList = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < count; i++)
            {
                thinkList.Add(new KeyValuePair<int, int>(i % count, RandomMulliganThinkTime()));
            }
            thinkList.Shuffle();

            foreach (var entry in thinkList)
            {
                var card = mulliganData.Cards[entry.Key];

                Log.InfoFormat("[开局留牌] 现在开始思考留牌 {0} 时间已经过去 {1} 毫秒.", card.Entity.Id, entry.Value);

                // Instant think time, skip the card.
                if (entry.Value == 0)
                    continue;

                Client.MouseOver(card.InteractPoint);

                await Coroutine.Sleep(entry.Value);
            }
        }

        private void playEmote(EmoteType data)
        {
            int height = Screen.Height;
            int width = Screen.Width;

            if (data == EmoteType.GREETINGS)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 - height / 6, height / 4 * 3 + height / 20);
            }
            else if (data == EmoteType.WELL_PLAYED)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 - height / 6, height / 4 * 3 - height / 18);
            }
            else if (data == EmoteType.OOPS)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 + height / 6, height / 4 * 3 - height / 18);
            }
            else if (data == EmoteType.THREATEN)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 + height / 6, height / 4 * 3 + height / 20);
            }
            else if (data == EmoteType.THANKS)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 - height / 6, height / 4 * 3 - height / 10);
            }
            else if (data == EmoteType.WOW)
            {
                Client.RightClickAt(width / 2, height / 4 * 3);
                Client.LeftClickAt(width / 2 + height / 6, height / 4 * 3 - height / 10);
            }
        }


        #endregion

        #region Emote

        /// <summary>
        /// This task implements player emote detection logic for the bot.
        /// </summary>
        /// <param name="data">An object that contains relevant data for the emote event.</param>
        /// <returns></returns>
        public async Task EmoteLogic(EmoteData data)
        {
            Log.InfoFormat("[表情] 使用表情 [{0}].", data.Emote);

            if (data.Emote == EmoteType.GREETINGS)
            {
                Triton.Game.Mapping.GameState.Get().GetCurrentPlayer().GetHeroCard().PlayEmote(EmoteType.THANKS);
            }
            else if (data.Emote == EmoteType.WELL_PLAYED)
            {
                Triton.Game.Mapping.GameState.Get().GetCurrentPlayer().GetHeroCard().PlayEmote(EmoteType.THANKS);
            }
            else if (data.Emote == EmoteType.OOPS)
            {
                Triton.Game.Mapping.GameState.Get().GetCurrentPlayer().GetHeroCard().PlayEmote(EmoteType.WELL_PLAYED);
            }
            else if (data.Emote == EmoteType.THREATEN)
            {
                Triton.Game.Mapping.GameState.Get().GetCurrentPlayer().GetHeroCard().PlayEmote(EmoteType.THANKS);
            }
            else if (data.Emote == EmoteType.THANKS)
            {
            }
            else if (data.Emote == EmoteType.SORRY)
            {
            }
        }

        #endregion

        #region Turn

        public async Task OurTurnCombatLogic()
        {
            Log.InfoFormat("[我方回合]");
            await Coroutine.Sleep(555 + makeChoice());
            ChooseOneClick(dirtychoice);
            dirtychoice = -1;
            await Coroutine.Sleep(555);
            Silverfish.Instance.lastpf = null;
            return;
        }

        public async Task OpponentTurnCombatLogic()
        {
            Log.Info("[对手回合]");
        }


        public void ChooseOneClick(int dirty)
        {
            if (dirty >= 0)
            {
                switch (dirtychoice)
                {
                    case 0: TritonHs.ChooseOneClickMiddle(); break;
                    case 1: TritonHs.ChooseOneClickLeft(); break;
                    default:
                        {
                            List<Card> friendlyCards = ChoiceCardMgr.Get().GetFriendlyCards();
                            if (friendlyCards.Count > dirty)
                                Client.LeftClickAt(Client.CardInteractPoint(friendlyCards[dirty]));
                            else
                                TritonHs.ChooseOneClickRight();//抉择
                            break;
                        }
                }
            }
        }

        private Action titanAction = null;

        /// <summary>
        /// 我方回合的逻辑处理，包括表情的播放、卡牌的使用、随从攻击、地标使用等。
        /// </summary>
        public async Task OurTurnLogic()
        {
            // 首回合播放表情，根据不同模式选择不同的表情类型
            if (firstMove && "嘴臭模式".Equals(printUtils.emoteMode))
            {
                firstMove = false;
                playEmote(EmoteType.WELL_PLAYED);
            }
            else if (!firstTurn && firstMove && "乞讨模式".Equals(printUtils.emoteMode))
            {
                firstMove = false;
                if (new Random().Next(0, 10) < 4)
                    playEmote(EmoteType.THANKS);
            }
            else if (firstTurn && "乞讨模式".Equals(printUtils.emoteMode))
            {
                firstTurn = false;
                playEmote(EmoteType.THANKS);
            }
            else if (firstTurn && "友善模式".Equals(printUtils.emoteMode))
            {
                firstTurn = false;
                playEmote(EmoteType.GREETINGS);
            }
            else if ("摊牌了我是脚本".Equals(printUtils.emoteMode))
            {
                EmoteType[] emoteTypes = { EmoteType.CONCEDE, EmoteType.DEATH_LINE, EmoteType.EVENT_FIRE_FESTIVAL_GREETINGS, EmoteType.EVENT_HAPPY_NEW_YEAR, EmoteType.EVENT_LUNAR_NEW_YEAR, EmoteType.EVENT_WINTER_VEIL, EmoteType.LOW_CARDS, EmoteType.MIRROR_START, EmoteType.NO_CARDS, EmoteType.SORRY, EmoteType.START, EmoteType.THINK1, EmoteType.THINK2, EmoteType.THINK3, EmoteType.TIMER };
                playEmote(emoteTypes[new Random().Next(emoteTypes.Length)]);
            }
            else if ("精神污染模式".Equals(printUtils.emoteMode))
            {
                EmoteType[] emoteTypes = { EmoteType.GREETINGS, EmoteType.THANKS, EmoteType.OOPS, EmoteType.WELL_PLAYED, EmoteType.WOW, EmoteType.THREATEN };
                playEmote(emoteTypes[new Random().Next(emoteTypes.Length)]);
            }

            // 当最佳动作值大于5000时的处理
            if (Ai.Instance.bestmoveValue > 5000)
            {
                if ("嘴臭模式".Equals(printUtils.emoteMode))
                {
                    playEmote(EmoteType.THREATEN);
                }
                if (firstMove && ("摊牌了我是脚本".Equals(printUtils.emoteMode)) || "抱歉".Equals(printUtils.emoteMode))
                {
                    firstMove = false;
                    playEmote(EmoteType.SORRY);
                }
                if (firstMove && "友善模式".Equals(printUtils.emoteMode))
                {
                    firstMove = false;
                    playEmote(EmoteType.WELL_PLAYED);
                }
            }

            // 当最佳动作值小于等于-700时的处理
            if (Ai.Instance.bestmoveValue <= -700)
            {
                if ("主动投降".Equals(printUtils.emoteMode))
                {
                    playEmote(EmoteType.WELL_PLAYED);
                    TritonHs.Concede(true);
                }
                if ("乞讨模式".Equals(printUtils.emoteMode))
                {
                    playEmote(EmoteType.THANKS);
                }
            }

            // 检查行为模式是否已更改
            if (this.behave.BehaviorName() != DefaultRoutineSettings.Instance.DefaultBehavior)
            {
                behave = sf.getBehaviorByName(DefaultRoutineSettings.Instance.DefaultBehavior);
                Silverfish.Instance.lastpf = null;
            }

            // 如果在目标或选择模式，等待
            if (this.learnmode && (TritonHs.IsInTargetMode() || TritonHs.IsInChoiceMode()))
            {
                await Coroutine.Sleep(50);
                return;
            }

            // 处理目标模式
            if (TritonHs.IsInTargetMode())
            {
                if (dirtytarget >= 0)
                {
                    Log.Info("瞄准中...");
                    HSCard source = dirtyTargetSource == 9000 ? TritonHs.OurHeroPowerCard : getEntityWithNumber(dirtyTargetSource);
                    HSCard target = getEntityWithNumber(dirtytarget);

                    if (target == null)
                    {
                        Log.Error("目标为空...");
                        TritonHs.CancelTargetingMode();
                        return;
                    }

                    dirtytarget = -1;
                    dirtyTargetSource = -1;

                    if (source == null)
                        await TritonHs.DoTarget(target);
                    else
                        await source.DoTarget(target);

                    await Coroutine.Sleep(555);
                    return;
                }

                Log.Error("目标丢失...");
                TritonHs.CancelTargetingMode();
                return;
            }

            // 处理选择模式
            if (TritonHs.IsInChoiceMode())
            {
                await Coroutine.Sleep(555 + makeChoice());
                switch (dirtychoice)
                {
                    case 0: 
                        TritonHs.ChooseOneClickMiddle();
                        break;
                    case 1: 
                        TritonHs.ChooseOneClickLeft(); 
                        break;
                    case 2: 
                        TritonHs.ChooseOneClickRight(); 
                        break;
                }

                dirtychoice = -1;
                await Coroutine.Sleep(555);
                // 指向泰坦技能的使用目标
                await TitanAbilityUseOnTagets();
                return;
            }

            // 更新一切
            bool sleepRetry = false;
            bool templearn = Silverfish.Instance.updateEverything(behave, 0, out sleepRetry);
            if (sleepRetry)
            {
                Log.Error("[AI] 随从没能动起来，再试一次...");
                await Coroutine.Sleep(500);
                Thread.Sleep(2000);
                templearn = Silverfish.Instance.updateEverything(behave, 1, out sleepRetry);
            }

            if (templearn == true)
                this.printlearnmode = true;

            if (this.learnmode)
            {
                if (this.printlearnmode)
                {
                    Ai.Instance.simmulateWholeTurnandPrint();
                }
                this.printlearnmode = false;

                await Coroutine.Sleep(50);
                return;
            }

            // 执行最佳动作
            var moveTodo = Ai.Instance.bestmove;

            if (moveTodo == null || moveTodo.actionType == actionEnum.endturn || Ai.Instance.bestmoveValue < -9999)
            {
                firstMove = true;
                bool doEndTurn = false;
                bool doConcede = false;

                if (Ai.Instance.bestmoveValue > -10000)
                    doEndTurn = true;
                else if (HREngine.Bots.Settings.Instance.concedeMode != 0)
                    doConcede = true;
                else
                {
                    if (new Playfield().ownHeroHasDirectLethal())
                    {
                        Playfield lastChancePl = Ai.Instance.bestplay;
                        bool lastChance = false;
                        if (lastChancePl.owncarddraw > 0)
                        {
                            foreach (Handmanager.Handcard hc in lastChancePl.owncards)
                            {
                                if (hc.card.nameEN == CardDB.cardNameEN.unknown)
                                    lastChance = true;
                            }
                            if (!lastChance) doConcede = true;
                        }
                        else
                            doConcede = true;

                        if (doConcede)
                        {
                            foreach (Minion m in lastChancePl.ownMinions)
                            {
                                if (!m.playedThisTurn) continue;
                                switch (m.handcard.card.nameEN)
                                {
                                    case CardDB.cardNameEN.cthun: lastChance = true; break;
                                    case CardDB.cardNameEN.nzoththecorruptor: lastChance = true; break;
                                    case CardDB.cardNameEN.yoggsaronhopesend: lastChance = true; break;
                                    case CardDB.cardNameEN.sirfinleymrrgglton: lastChance = true; break;
                                    case CardDB.cardNameEN.ragnarosthefirelord: if (lastChancePl.enemyHero.Hp < 9) lastChance = true; break;
                                    case CardDB.cardNameEN.barongeddon: if (lastChancePl.enemyHero.Hp < 3) lastChance = true; break;
                                }
                            }
                        }
                        if (lastChance) doConcede = false;
                    }
                    else if (moveTodo == null || moveTodo.actionType == actionEnum.endturn)
                        doEndTurn = true;
                }

                if (doEndTurn)
                {
                    Helpfunctions.Instance.ErrorLog("回合结束");
                    //地标减少冷却回合
                    Playfield doEndTurnPlay = Ai.Instance.bestplay;
                    foreach (Minion m in doEndTurnPlay.ownMinions)
                    {
                        if (m.handcard.card.type == CardDB.cardtype.LOCATION && m.CooldownTurn > 0)
                        {
                            m.CooldownTurn -= 1;
                            m.handcard.card.CooldownTurn -= 1;
                            if (m.CooldownTurn <= 0)
                            {
                                m.Ready = true;
                            }
                            Helpfunctions.Instance.logg("卡牌名称 - " + m.handcard.card.nameCN + " 地标冷却回合 - " + m.CooldownTurn);
                        }
                    }
                    await TritonHs.EndTurn();
                    return;
                }
                else if (doConcede)
                {
                    playEmote(EmoteType.WELL_PLAYED);
                    Helpfunctions.Instance.ErrorLog("我方败局已定. 投降...");
                    Helpfunctions.Instance.logg("投降... 败局已定###############################################");
                    TritonHs.Concede(true);   // 如果需要自动投降，取消注释这行代码
                    return;
                }
            }

            Helpfunctions.Instance.ErrorLog("开始行动");
            if (moveTodo == null)
            {
                playEmote(EmoteType.OOPS);
                Helpfunctions.Instance.ErrorLog("实在支不出招啦. 结束当前回合");
                await Coroutine.Sleep(500);
                Thread.Sleep(2000);
                //地标减少冷却回合
                Playfield nullPlay = Ai.Instance.bestplay;
                foreach (Minion m in nullPlay.ownMinions)
                {
                    if (m.handcard.card.type == CardDB.cardtype.LOCATION && m.CooldownTurn > 0)
                    {
                        m.CooldownTurn -= 1;
                        m.handcard.card.CooldownTurn -= 1;
                        if (m.CooldownTurn <= 0)
                        {
                            m.Ready = true;
                        }
                        Helpfunctions.Instance.logg("卡牌名称 - " + m.handcard.card.nameCN + " 地标冷却回合 - " + m.CooldownTurn);
                    }
                }
                await TritonHs.EndTurn();
                return;
            }

            // 执行当前的行动逻辑
            moveTodo.print();

            switch (moveTodo.actionType)
            {
                case actionEnum.playcard:
                    await PlayCard(moveTodo);
                    break;
                case actionEnum.attackWithMinion:
                    await AttackWithMinion(moveTodo);
                    break;
                case actionEnum.attackWithHero:
                    await AttackWithHero(moveTodo);
                    break;
                case actionEnum.useHeroPower:
                    await UseHeroPower(moveTodo);
                    break;
                case actionEnum.trade:
                    await HandleTrade(moveTodo);
                    break;
                case actionEnum.useLocation:
                    await UseLocation(moveTodo);
                    break;
                case actionEnum.useTitanAbility:
                    await UseTitanAbility(moveTodo);
                    break;
                case actionEnum.forge:
                    await HandleForge(moveTodo);
                    break;
                default:
                    break;
            }
            //地标减少冷却回合
            Playfield lastPlay = Ai.Instance.bestplay;
            foreach (Minion m in lastPlay.ownMinions)
            {
                if (m.handcard.card.type == CardDB.cardtype.LOCATION && m.CooldownTurn > 0)
                {
                    m.CooldownTurn -= 1;
                    m.handcard.card.CooldownTurn -= 1;
                    if (m.CooldownTurn <= 0)
                    {
                        m.Ready = true;
                    }
                    Helpfunctions.Instance.logg("卡牌名称 - " + m.handcard.card.nameCN + " 地标冷却回合 - " + m.CooldownTurn);
                }
            }
            await TritonHs.EndTurn();
        }

        /// <summary>
        /// 指向泰坦技能的使用目标
        /// </summary>
        /// <returns></returns>
        private async Task TitanAbilityUseOnTagets()
        {
            //处理泰坦技能的使用目标
            if (titanAction != null)
            {
                if (titanAction.target != null)
                {
                    HSCard titan = getEntityWithNumber(titanAction.own.entitiyID);
                    HSCard target = getEntityWithNumber(titanAction.target.entitiyID);
                    await titan.UseOn(target.Card);
                    await Coroutine.Sleep(200);
                    titanAction = null;
                }
            }
        }

        /// <summary>
        /// 处理打出卡牌的动作。
        /// </summary>
        private async Task PlayCard(Action moveTodo)
        {
            Questmanager.Instance.updatePlayedCardFromHand(moveTodo.card);
            HSCard cardtoplay = getCardWithNumber(moveTodo.card.entity);
            if (cardtoplay == null)
            {
                Helpfunctions.Instance.ErrorLog("[提示] 实在支不出招啦");
                return;
            }
            if (moveTodo.target != null)
            {
                HSCard target = getEntityWithNumber(moveTodo.target.entitiyID);
                if (target != null)
                {
                    Helpfunctions.Instance.ErrorLog("使用: " + cardtoplay.Name + " 瞄准: " + target.Name);
                    if (moveTodo.druidchoice >= 1)
                    {
                        dirtytarget = moveTodo.target.entitiyID;
                        dirtychoice = moveTodo.druidchoice;
                        choiceCardId = moveTodo.card.card.cardIDenum.ToString();
                    }
                    dirtyTargetSource = moveTodo.card.entity;
                    dirtytarget = moveTodo.target.entitiyID;
                    await cardtoplay.Pickup();

                    switch (moveTodo.card.card.type)
                    {
                        case CardDB.cardtype.MOB:
                            await cardtoplay.UseAt(moveTodo.place);
                            break;
                        case CardDB.cardtype.WEAPON:
                        case CardDB.cardtype.SPELL:
                            await cardtoplay.UseOn(target.Card);
                            break;
                        default:
                            await cardtoplay.UseOn(target.Card);
                            break;
                    }
                }
                else
                {
                    playEmote(EmoteType.OOPS);
                    Helpfunctions.Instance.ErrorLog("[AI] 目标丢失，再试一次...");
                    await Coroutine.Sleep(3000);
                }
                await Coroutine.Sleep(500);
            }
            else
            {
                Helpfunctions.Instance.ErrorLog("使用: " + cardtoplay.Name + " 暂时没有目标");
                if (moveTodo.druidchoice >= 1)
                {
                    dirtychoice = moveTodo.druidchoice;
                    choiceCardId = moveTodo.card.card.cardIDenum.ToString();
                }
                dirtyTargetSource = -1;
                dirtytarget = -1;
                await cardtoplay.Pickup();
                await cardtoplay.UseAt(moveTodo.place);
            }
            await Coroutine.Sleep(500);
        }

        /// <summary>
        /// 处理随从攻击的动作。
        /// </summary>
        private async Task AttackWithMinion(Action moveTodo)
        {
            HSCard attacker = getEntityWithNumber(moveTodo.own.entitiyID);
            HSCard target = getEntityWithNumber(moveTodo.target.entitiyID);
            if (attacker != null && target != null)
            {
                Helpfunctions.Instance.ErrorLog("随从攻击: " + attacker.Name + " 目标为: " + target.Name);
                await attacker.DoAttack(target);
            }
            else
            {
                playEmote(EmoteType.OOPS);
                Helpfunctions.Instance.ErrorLog("[AI] 随从攻击失败，再次重试...");
                await Coroutine.Sleep(2000);
            }
            await Coroutine.Sleep(250);
        }

        /// <summary>
        /// 处理英雄攻击的动作。
        /// </summary>
        private async Task AttackWithHero(Action moveTodo)
        {
            HSCard attacker = getEntityWithNumber(moveTodo.own.entitiyID);
            HSCard target = getEntityWithNumber(moveTodo.target.entitiyID);
            if (attacker != null && target != null)
            {
                dirtytarget = moveTodo.target.entitiyID;
                Helpfunctions.Instance.ErrorLog("英雄攻击: " + attacker.Name + " 目标为: " + target.Name);
                dirtyTargetSource = moveTodo.own.entitiyID;
                dirtytarget = moveTodo.target.entitiyID;
                await attacker.DoAttack(target);
            }
            else
            {
                playEmote(EmoteType.OOPS);
                Helpfunctions.Instance.ErrorLog("[AI] 英雄攻击目标丢失，再次重试...");
                await Coroutine.Sleep(2000);
            }
            await Coroutine.Sleep(250);
        }

        /// <summary>
        /// 处理英雄技能使用的动作。
        /// </summary>
        private async Task UseHeroPower(Action moveTodo)
        {
            HSCard cardtoplay = TritonHs.OurHeroPowerCard;

            if (moveTodo.target != null)
            {
                HSCard target = getEntityWithNumber(moveTodo.target.entitiyID);
                if (target != null)
                {
                    Helpfunctions.Instance.ErrorLog("使用英雄技能: " + cardtoplay.Name + " 目标为 " + target.Name);
                    if (moveTodo.druidchoice > 0)
                    {
                        dirtytarget = moveTodo.target.entitiyID;
                        dirtychoice = moveTodo.druidchoice;
                        choiceCardId = moveTodo.card.card.cardIDenum.ToString();
                    }
                    dirtyTargetSource = 9000;
                    dirtytarget = moveTodo.target.entitiyID;

                    await cardtoplay.Pickup();
                    await cardtoplay.UseOn(target.Card);
                }
                else
                {
                    playEmote(EmoteType.OOPS);
                    Helpfunctions.Instance.ErrorLog("[AI] 目标丢失，再次重试...");
                    await Coroutine.Sleep(3000);
                }
                await Coroutine.Sleep(500);
            }
            else
            {
                Helpfunctions.Instance.ErrorLog("使用英雄技能: " + cardtoplay.Name + " 暂时没有目标");
                if (moveTodo.druidchoice >= 1)
                {
                    dirtychoice = moveTodo.druidchoice;
                    choiceCardId = moveTodo.card.card.cardIDenum.ToString();
                }
                dirtyTargetSource = -1;
                dirtytarget = -1;
                await cardtoplay.Pickup();
            }
        }

        /// <summary>
        /// 处理交易动作。
        /// </summary>
        private async Task HandleTrade(Action moveTodo)
        {
            var cardtoTrade = getCardWithNumber(moveTodo.card.entity);
            Helpfunctions.Instance.ErrorLog("交易: " + cardtoTrade.Name);
            Helpfunctions.Instance.logg("交易: " + cardtoTrade.Name);
            await cardtoTrade.DeckAction();
            await Coroutine.Sleep(300);
        }

        /// <summary>
        /// 处理锻造动作。
        /// </summary>
        private async Task HandleForge(Action moveTodo)
        {
            var cardtoTrade = getCardWithNumber(moveTodo.card.entity);
            Helpfunctions.Instance.ErrorLog("锻造: " + cardtoTrade.Name);
            Helpfunctions.Instance.logg("锻造: " + cardtoTrade.Name);
            await cardtoTrade.DeckAction();
            await Coroutine.Sleep(300);
        }

        /// <summary>
        /// 处理使用地标的动作。
        /// </summary>
        /// <param name="moveTodo"></param>
        /// <returns></returns>
        private async Task UseLocation(Action moveTodo)
        {
            HSCard location = getEntityWithNumber(moveTodo.own.entitiyID);
            if (location != null)
            {
                if (moveTodo.target != null)
                {
                    HSCard target = getEntityWithNumber(moveTodo.target.entitiyID);
                    if (target != null)
                    {
                        Helpfunctions.Instance.logg("使用地标 " + location.Name + " 目标为 " + target.Name);
                        await location.LeftClickCard();
                        await location.UseOn(target.Card);
                        // 更新使用次数及地标是否准备好
                        moveTodo.own.handcard.card.Health--;
                        moveTodo.own.CooldownTurn = 2;//地标标记为冷却中
                        moveTodo.own.handcard.card.CooldownTurn = 2;
                        moveTodo.own.Ready = false;
                        Helpfunctions.Instance.logg("地标 " + location.Name + " 标记为冷却中...");
                    }
                    else
                    {
                        Helpfunctions.Instance.ErrorLog("[AI] 目标 " + moveTodo.target.entitiyID + "丢失. 再次重试...");
                        Helpfunctions.Instance.logg("[AI] 目标 " + moveTodo.target.entitiyID + "丢失. 再次重试...");
                        await Coroutine.Sleep(3000);
                    }
                }
                else
                {
                    Helpfunctions.Instance.ErrorLog("使用地标: " + location.Name + " 暂时没有目标");
                    await location.LeftClickCard();
                    // 更新使用次数及地标是否准备好
                    moveTodo.own.handcard.card.Health--;
                    moveTodo.own.CooldownTurn = 2;//地标标记为冷却中
                    moveTodo.own.handcard.card.CooldownTurn = 2;
                    moveTodo.own.Ready = false;
                    Helpfunctions.Instance.logg("地标 " + location.Name + " 标记为冷却中...");
                }
            }
            else
            {
                Helpfunctions.Instance.ErrorLog("[AI] 地标丢失，再次重试...");
                await Coroutine.Sleep(3000);
            }
        }

        /// <summary>
        /// 处理使用泰坦技能的动作
        /// </summary>
        /// <param name="moveTodo"></param>
        /// <returns></returns>
        private async Task UseTitanAbility(Action moveTodo)
        {
            HSCard titan = getEntityWithNumber(moveTodo.own.entitiyID);
            if (titan != null) 
            {
                CardDB.Card card = moveTodo.own.handcard.card;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("使用泰坦 " + titan.Name + " 技能 " + moveTodo.titanAbilityNO + " 目标为 ");
                stringBuilder.Append(moveTodo.target != null && moveTodo.target.handcard != null ? moveTodo.target.handcard.card.nameCN.ToString() : "空");
                Helpfunctions.Instance.logg(stringBuilder.ToString());
                await titan.LeftClickCard();
                await Coroutine.Sleep(500);
                // 更新技能是否已使用，泰坦可攻击
                switch (moveTodo.titanAbilityNO)
                {
                    case 1:
                        card.TitanAbilityUsed1 = true;
                        break;
                    case 2:
                        card.TitanAbilityUsed2 = true;
                        break;
                    case 3:
                        card.TitanAbilityUsed3 = true;
                        break;
                }
                if (card.TitanAbilityUsed1 && card.TitanAbilityUsed2 && card.TitanAbilityUsed3)
                {
                    moveTodo.own.Ready = true;
                }
                //保存使用的技能编号，以及技能使用目标
                titanAction = moveTodo;
                Helpfunctions.Instance.logg("泰坦 " + titan.Name + " 技能 " + moveTodo.titanAbilityNO + " 标记为已使用...");
            }
            else
            {
                Helpfunctions.Instance.ErrorLog("[AI] 泰坦丢失，再次重试...");
                await Coroutine.Sleep(3000);
            }
        }

        /// <summary>
        /// 选择卡牌
        /// </summary>
        /// <returns></returns>
        private int makeChoice()
        {
            if (dirtychoice < 1)
            {
                var ccm = ChoiceCardMgr.Get();
                var lscc = ccm.m_lastShownChoiceState;
                GAME_TAG choiceMode = GAME_TAG.CHOOSE_ONE;
                int sourceEntityId = -1;
                CardDB.cardIDEnum sourceEntityCId = CardDB.cardIDEnum.None;
                if (lscc != null)
                {
                    sourceEntityId = lscc.m_sourceEntityId;
                    Entity entity = GameState.Get().GetEntity(lscc.m_sourceEntityId);
                    sourceEntityCId = CardDB.Instance.cardIdstringToEnum(entity.GetCardId());
                    if (entity != null)
                    {
                        var sourceCard = entity.GetCard();
                        if (sourceCard != null)
                        {
                            //发现
                            if (sourceCard.GetEntity().HasTag(GAME_TAG.DISCOVER))
                            {
                                choiceMode = GAME_TAG.DISCOVER;
                                dirtychoice = -1;
                            }
                            //进化
                            else if (sourceCard.GetEntity().HasTag(GAME_TAG.ADAPT))
                            {
                                choiceMode = GAME_TAG.ADAPT;
                                dirtychoice = -1;
                            }
                            //探底
                            else if (sourceCard.GetEntity().HasTag(GAME_TAG.DREDGE))
                            {
                                choiceMode = GAME_TAG.DREDGE;
                                dirtychoice = -1;
                            }
                            //泰坦
                            else if (sourceCard.GetEntity().HasTag(GAME_TAG.TITAN))
                            {
                                choiceMode = GAME_TAG.TITAN;
                                dirtychoice = -1;
                            }
                        }
                    }
                }

                Ai ai = Ai.Instance;
                List<Handmanager.Handcard> discoverCards = new List<Handmanager.Handcard>();
                float bestDiscoverValue = -2000000;
                var choiceCardMgr = ChoiceCardMgr.Get();
                var cards = choiceCardMgr.GetFriendlyCards();

                for (int i = 0; i < cards.Count; i++)
                {
                    var hc = new Handmanager.Handcard();
                    hc.card = CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(cards[i].GetCardId()));
                    hc.position = 100 + i;
                    hc.entity = cards[i].GetEntityId();
                    hc.manacost = hc.card.calculateManaCost(ai.nextMoveGuess);
                    discoverCards.Add(hc);
                }

                int sirFinleyChoice = -1;
                if (ai.bestmove == null) Log.ErrorFormat("[提示] 没有获得卡牌数据");
                // 芬利爵士的发现
                else if (ai.bestmove.actionType == actionEnum.playcard && ai.bestmove.card.card.nameEN == CardDB.cardNameEN.sirfinleymrrgglton)
                {
                    sirFinleyChoice = ai.botBase.getSirFinleyPriority(discoverCards);
                }

                DateTime tmp = DateTime.Now;
                int discoverCardsCount = discoverCards.Count;
                if (sirFinleyChoice != -1) dirtychoice = sirFinleyChoice;
                else
                {
                    int dirtyTwoTurnSim = ai.mainTurnSimulator.getSecondTurnSimu();
                    ai.mainTurnSimulator.setSecondTurnSimu(true, 50);
                    using (TritonHs.Memory.ReleaseFrame(true))
                    {
                        Playfield testPl = new Playfield();
                        Playfield basePlf = new Playfield(ai.nextMoveGuess);
                        for (int i = 0; i < discoverCardsCount; i++)
                        {
                            Playfield tmpPlf = new Playfield(basePlf);
                            Playfield nextPlf = new Playfield(basePlf);
                            Playfield featurePlf = new Playfield(basePlf);

                            tmpPlf.isLethalCheck = false;
                            featurePlf.isLethalCheck = false;
                            nextPlf.mana = tmpPlf.mana + 1 > 10 ? 10 : tmpPlf.mana + 1;
                            featurePlf.mana = 10;

                            float bestval = bestDiscoverValue;
                            switch (choiceMode)
                            {
                                // 发现
                                case GAME_TAG.DISCOVER:
                                    // 考虑当前回合、下回合和未来收益，权重5：3：2吧
                                    try
                                    {
                                        switch (ai.bestmove.card.card.nameEN)
                                        {
                                            case CardDB.cardNameEN.eternalservitude:
                                            case CardDB.cardNameEN.freefromamber:
                                                tmpPlf.callKid(discoverCards[i].card, tmpPlf.ownMinions.Count - 1, true);
                                                nextPlf.callKid(discoverCards[i].card, tmpPlf.ownMinions.Count - 1, true);
                                                featurePlf.callKid(discoverCards[i].card, tmpPlf.ownMinions.Count - 1, true);
                                                bestval = ai.mainTurnSimulator.doallmoves(tmpPlf) * 0.5f + ai.mainTurnSimulator.doallmoves(nextPlf) * 0.3f + ai.mainTurnSimulator.doallmoves(featurePlf) * 0.2f;
                                                break;
                                            // 芬利爵士
                                            case CardDB.cardNameEN.sirfinleymrrgglton:
                                                bestval = ai.botBase.getSirFinleyPriority(discoverCards[i].card);
                                                switch (discoverCards[i].card.nameEN)
                                                {
                                                    case CardDB.cardNameEN.demonclaws:
                                                    case CardDB.cardNameEN.shapeshift:
                                                    case CardDB.cardNameEN.fireblast:
                                                    case CardDB.cardNameEN.daggermastery:
                                                        if (tmpPlf.enemyHero.Hp <= 1) bestval += 100; break;
                                                    case CardDB.cardNameEN.steadyshot:
                                                        if (tmpPlf.enemyHero.Hp <= 1) bestval += 100; break;
                                                    case CardDB.cardNameEN.lifetap:
                                                        if (tmpPlf.owncards.Count <= 3) bestval += 5; break;
                                                }
                                                break;
                                            default:
                                                bestval = ai.botBase.getDiscoverVal(discoverCards[i].card, tmpPlf);
                                                break;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        bestval = ai.botBase.getDiscoverVal(discoverCards[i].card, tmpPlf);
                                    }
                                    Helpfunctions.Instance.ErrorLog(discoverCards[i].card.nameCN + "最优场面" + bestval);
                                    break;
                                // 进化
                                case GAME_TAG.ADAPT:
                                    bool found = false;
                                    foreach (Minion m in tmpPlf.ownMinions)
                                    {
                                        if (m.entitiyID == sourceEntityId)
                                        {
                                            bool forbidden = false;
                                            switch (discoverCards[i].card.cardIDenum)
                                            {
                                                case CardDB.cardIDEnum.UNG_999t5: if (m.handcard.card.Elusive) forbidden = true; break;
                                                case CardDB.cardIDEnum.UNG_999t6: if (m.taunt) forbidden = true; break;
                                                case CardDB.cardIDEnum.UNG_999t7: if (m.windfury) forbidden = true; break;
                                                case CardDB.cardIDEnum.UNG_999t8: if (m.divineshild) forbidden = true; break;
                                                case CardDB.cardIDEnum.UNG_999t10: if (m.stealth) forbidden = true; break;
                                                case CardDB.cardIDEnum.UNG_999t13: if (m.poisonous) forbidden = true; break;
                                            }
                                            if (forbidden) bestval = -2000000;
                                            else
                                            {
                                                discoverCards[i].card.sim_card.onCardPlay(tmpPlf, true, m, 0);
                                                bestval = ai.mainTurnSimulator.doallmoves(tmpPlf);
                                            }
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found) Log.ErrorFormat("[AI] 触发卡牌丢失...");
                                    break;
                                // 探底
                                case GAME_TAG.DREDGE:
                                    bestval = ai.botBase.getDredgeVal(discoverCards[i].card, tmpPlf);
                                    break;
                                // 泰坦
                                case GAME_TAG.TITAN:
                                    if (titanAction != null)
                                    {
                                        // 技能已经在生成动作的时候，就已经选择好了
                                        if (i == titanAction.titanAbilityNO - 1)
                                        {
                                            bestval = 100;
                                        }
                                    }
                                    break;
                                default:
                                    bestval = ai.botBase.getDiscoverVal(discoverCards[i].card, tmpPlf);
                                    break;
                            }
                            if (bestDiscoverValue <= bestval)
                            {
                                bestDiscoverValue = bestval;
                                dirtychoice = i;
                            }
                        }
                    }
                    ai.mainTurnSimulator.setSecondTurnSimu(true, dirtyTwoTurnSim);
                }
                if (sourceEntityCId == CardDB.cardIDEnum.UNG_035) dirtychoice = new Random().Next(0, 2);
                if (dirtychoice > -1)
                {
                    Hrtprozis.Instance.enchs.Add(discoverCards[dirtychoice].card.cardIDenum);
                }
                if (dirtychoice == 0) dirtychoice = 1;
                else if (dirtychoice == 1) dirtychoice = 0;
                int ttf = (int)(DateTime.Now - tmp).TotalMilliseconds;
                Helpfunctions.Instance.ErrorLog("发现卡牌: " + dirtychoice + (discoverCardsCount > 1 ? " " + discoverCards[1].card.nameCN : "") + (discoverCardsCount > 0 ? " " + discoverCards[0].card.nameCN : "") + (discoverCardsCount > 2 ? " " + discoverCards[2].card.nameCN : ""));
                Helpfunctions.Instance.logg("发现卡牌: " + dirtychoice + (discoverCardsCount > 1 ? " " + discoverCards[1].card.cardIDenum : "") + (discoverCardsCount > 0 ? " " + discoverCards[0].card.cardIDenum : "") + (discoverCardsCount > 2 ? " " + discoverCards[2].card.cardIDenum : ""));
                
                if (ttf < 3000) return (new Random().Next(ttf < 1300 ? 1300 - ttf : 0, 3100 - ttf));

            }
            else
            {
                Helpfunctions.Instance.logg("选择这张卡牌: " + dirtychoice);
                return (new Random().Next(1100, 3200));
            }
            return 0;
        }

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <returns></returns>
        public async Task OpponentTurnLogic()
        {
            Log.InfoFormat("[对手回合]");


        }

        #endregion

        #region ArenaDraft

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ArenaDraftLogic(ArenaDraftData data)
        {
            Log.InfoFormat("[ArenaDraft]");

            // We don't have a hero yet, so choose one.
            if (data.Hero == null)
            {
                Log.InfoFormat("[ArenaDraft] Hero: [{0} ({3}) | {1} ({4}) | {2} ({5})].",
                    data.Choices[0].EntityDef.CardId, data.Choices[1].EntityDef.CardId, data.Choices[2].EntityDef.CardId,
                    data.Choices[0].EntityDef.Name, data.Choices[1].EntityDef.Name, data.Choices[2].EntityDef.Name);

                // Quest support logic!
                var questIds = TritonHs.CurrentQuests.Select(q => q.Id).ToList();
                foreach (var choice in data.Choices)
                {
                    var @class = choice.EntityDef.Class;
                    foreach (var questId in questIds)
                    {
                        if (TritonHs.IsQuestForClass(questId, @class))
                        {
                            data.Selection = choice;
                            Log.InfoFormat(
                                "[ArenaDraft] Choosing hero \"{0}\" because it matches a current quest.",
                                data.Selection.EntityDef.Name);
                            return;
                        }
                    }
                }

                // #1
                foreach (var choice in data.Choices)
                {
                    if ((TAG_CLASS)choice.EntityDef.Class == DefaultRoutineSettings.Instance.ArenaPreferredClass1)
                    {
                        data.Selection = choice;
                        Log.InfoFormat(
                            "[ArenaDraft] Choosing hero \"{0}\" because it matches the first preferred arena class.",
                            data.Selection.EntityDef.Name);
                        return;
                    }
                }

                // #2
                foreach (var choice in data.Choices)
                {
                    if ((TAG_CLASS)choice.EntityDef.Class == DefaultRoutineSettings.Instance.ArenaPreferredClass2)
                    {
                        data.Selection = choice;
                        Log.InfoFormat(
                            "[ArenaDraft] Choosing hero \"{0}\" because it matches the second preferred arena class.",
                            data.Selection.EntityDef.Name);
                        return;
                    }
                }

                // #3
                foreach (var choice in data.Choices)
                {
                    if ((TAG_CLASS)choice.EntityDef.Class == DefaultRoutineSettings.Instance.ArenaPreferredClass3)
                    {
                        data.Selection = choice;
                        Log.InfoFormat(
                            "[ArenaDraft] Choosing hero \"{0}\" because it matches the third preferred arena class.",
                            data.Selection.EntityDef.Name);
                        return;
                    }
                }

                // #4
                foreach (var choice in data.Choices)
                {
                    if ((TAG_CLASS)choice.EntityDef.Class == DefaultRoutineSettings.Instance.ArenaPreferredClass4)
                    {
                        data.Selection = choice;
                        Log.InfoFormat(
                            "[ArenaDraft] Choosing hero \"{0}\" because it matches the fourth preferred arena class.",
                            data.Selection.EntityDef.Name);
                        return;
                    }
                }

                // #5
                foreach (var choice in data.Choices)
                {
                    if ((TAG_CLASS)choice.EntityDef.Class == DefaultRoutineSettings.Instance.ArenaPreferredClass5)
                    {
                        data.Selection = choice;
                        Log.InfoFormat(
                            "[ArenaDraft] Choosing hero \"{0}\" because it matches the fifth preferred arena class.",
                            data.Selection.EntityDef.Name);
                        return;
                    }
                }

                // Choose a random hero.
                data.RandomSelection();

                Log.InfoFormat(
                    "[ArenaDraft] Choosing hero \"{0}\" because no other preferred arena classes were available.",
                    data.Selection.EntityDef.Name);

                return;
            }

            // Normal card choices.
            Log.InfoFormat("[ArenaDraft] Card: [{0} ({3}) | {1} ({4}) | {2} ({5})].", data.Choices[0].EntityDef.CardId,
                data.Choices[1].EntityDef.CardId, data.Choices[2].EntityDef.CardId, data.Choices[0].EntityDef.Name,
                data.Choices[1].EntityDef.Name, data.Choices[2].EntityDef.Name);

            var actor =
                data.Choices.Where(c => ArenavaluesReader.Get.ArenaValues.ContainsKey(c.EntityDef.CardId))
                    .OrderByDescending(c => ArenavaluesReader.Get.ArenaValues[c.EntityDef.CardId]).FirstOrDefault();
            if (actor != null)
            {
                data.Selection = actor;
            }
            else
            {
                data.RandomSelection();
            }
        }

        #endregion

        #region Handle Quests

        /// <summary>
        /// Under construction.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task HandleQuestsLogic(QuestData data)
        {
            Log.InfoFormat("[处理日常任务]");

            // Loop though all quest tiles.
            foreach (var questTile in data.QuestTiles)
            {
                // If we can't cancel a quest, we shouldn't try to.
                if (questTile.IsCancelable)
                {
                    if (DefaultRoutineSettings.Instance.QuestIdsToCancel.Contains(questTile.Achievement.Id))
                    {
                        // Mark the quest tile to be canceled.
                        questTile.ShouldCancel = true;

                        StringBuilder questsInfo = new StringBuilder("", 1000);
                        questsInfo.Append("[处理日常任务] 任务列表: ");
                        int qNum = data.QuestTiles.Count;
                        for (int i = 0; i < qNum; i++)
                        {
                            var q = data.QuestTiles[i].Achievement;
                            if (q.RewardData.Count > 0)
                            {
                                questsInfo.Append("[").Append(q.RewardData[0].Count).Append("x ").Append(q.RewardData[0].Type).Append("] ");
                            }
                            questsInfo.Append(q.Name);
                            if (i < qNum - 1) questsInfo.Append(", ");
                        }
                        questsInfo.Append(". 尝试取消任务: ").Append(questTile.Achievement.Name);
                        Log.InfoFormat(questsInfo.ToString());
                        await Coroutine.Sleep(new Random().Next(4000, 8000));
                        return;
                    }
                }
                else if (DefaultRoutineSettings.Instance.QuestIdsToCancel.Count > 0)
                {
                    Log.InfoFormat("取消任务失败.");
                }
            }
        }

        #endregion

        #endregion

        #region Override of Object

        /// <summary>
        /// ToString override.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ": " + Description;
        }

        #endregion

        private void GameEventManagerOnGameOver(object sender, GameOverEventArgs gameOverEventArgs)
        {
            firstTurn = true;
            Log.InfoFormat("[游戏结束] {0}{2} => {1}.", gameOverEventArgs.Result,
                GameEventManager.Instance.LastGamePresenceStatus, gameOverEventArgs.Conceded ? " [conceded]" : "");
        }
        //标记一下
        private void GameEventManagerOnNewGame(object sender, NewGameEventArgs newGameEventArgs)
        {
            //Log.InfoFormat("[Set new log file:] Start");
            Hrtprozis prozis = Hrtprozis.Instance;
            prozis.clearAllNewGame();
            Silverfish.Instance.setnewLoggFile();
            //Log.InfoFormat("[Set new log file:] End");
        }

        private void GameEventManagerOnQuestUpdate(object sender, QuestUpdateEventArgs questUpdateEventArgs)
        {
            Log.InfoFormat("[任务刷新]");
            foreach (var quest in TritonHs.CurrentQuests)
            {
                Log.InfoFormat("[任务刷新][{0}]{1}: {2} ({3} / {4}) [{6}x {5}]", quest.Id, quest.Name, quest.Description, quest.CurProgress,
                    quest.MaxProgress, quest.RewardData[0].Type, quest.RewardData[0].Count);
            }
        }

        private void GameEventManagerOnArenaRewards(object sender, ArenaRewardsEventArgs arenaRewardsEventArgs)
        {
            Log.InfoFormat("[竞技场奖励]");
            foreach (var reward in arenaRewardsEventArgs.Rewards)
            {
                Log.InfoFormat("[竞技场奖励] {1}x {0}.", reward.Type, reward.Count);
            }
        }

        private HSCard getEntityWithNumber(int number)
        {
            foreach (HSCard e in getallEntitys())
            {
                if (number == e.EntityId) return e;
            }
            return null;
        }

        private HSCard getCardWithNumber(int number)
        {
            foreach (HSCard e in getallHandCards())
            {
                if (number == e.EntityId) return e;
            }
            return null;
        }

        private List<HSCard> getallEntitys()
        {
            var result = new List<HSCard>();
            HSCard ownhero = TritonHs.OurHero;
            HSCard enemyhero = TritonHs.EnemyHero;
            HSCard ownHeroAbility = TritonHs.OurHeroPowerCard;
            List<HSCard> list2 = TritonHs.GetCards(CardZone.Battlefield, true);
            List<HSCard> list3 = TritonHs.GetCards(CardZone.Battlefield, false);

            result.Add(ownhero);
            result.Add(enemyhero);
            result.Add(ownHeroAbility);

            result.AddRange(list2);
            result.AddRange(list3);

            return result;
        }

        private List<HSCard> getallHandCards()
        {
            List<HSCard> list = TritonHs.GetCards(CardZone.Hand, true);
            return list;
        }
    }
}
