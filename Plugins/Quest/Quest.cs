using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Buddy.Coroutines;
using log4net;
using Triton.Bot;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Game;
using Triton.Game.Mapping;
using Logger = Triton.Common.LogUtilities.Logger;

namespace Quest
{
    public class Quest : IPlugin
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private readonly Stopwatch stopwatch_0 = new Stopwatch();
        private bool _enabled;

        private UserControl _control;

        /// <summary> The name of the plugin. </summary>
        public string Name
        {
            get { return "任务插件"; }
        }

        /// <summary> The description of the plugin. </summary>
        public string Description
        {
            get { return "任务管理插件。"; }
        }

        /// <summary>The author of the plugin.</summary>
        public string Author
        {
            get { return "琴弦上的宇宙+Pik_4"; }
        }

        /// <summary>The version of the plugin.</summary>
        public string Version
        {
            get { return "1.0.0.0"; }
        }

        /// <summary>Initializes this object. This is called when the object is loaded into the bot.</summary>
        public void Initialize()
        {
            Log.DebugFormat("[任务插件] 初始化");
            if (!MainSettings.Instance.EnabledPlugins.Contains(this.Name))
                MainSettings.Instance.EnabledPlugins.Add(this.Name);
        }

        /// <summary> The plugin start callback. Do any initialization here. </summary>
        public void Start()
        {
            stopwatch_0.Restart();
            Log.DebugFormat("[任务插件] 开启");
            GameEventManager.GameOver += GameEventManagerOnGameOver;
            QuestSettings.Instance.ReloadFile();
            UpdateGui(false);
        }

        /// <summary> The plugin tick callback. Do any update logic here. </summary>
        public void Tick()
        {
        }

        /// <summary> The plugin stop callback. Do any pre-dispose cleanup here. </summary>
        public void Stop()
        {
            stopwatch_0.Stop();
            Log.DebugFormat("[任务插件] 停止");
            GameEventManager.GameOver -= GameEventManagerOnGameOver;
        }

        public JsonSettings Settings
        {
            get { return QuestSettings.Instance; }
        }

        /// <summary> The plugin's settings control. This will be added to the Hearthbuddy Settings tab.</summary>
        public UserControl Control
        {
            get
            {
                if (_control != null)
                {
                    return _control;
                }

                using (var fs = new FileStream(@"Plugins\Quest\SettingsGui.xaml", FileMode.Open))
                {
                    var root = (UserControl) XamlReader.Load(fs);

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxDay1", "ProgressDay1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressDay1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxDay2", "ProgressDay2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressDay2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxDay3", "ProgressDay3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressDay3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxWeek1", "ProgressWeek1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressWeek1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxWeek2", "ProgressWeek2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressWeek2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "ProgressTextBoxWeek3", "ProgressWeek3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ProgressWeek3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxDay1", "QuotaDay1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaDay1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxDay2", "QuotaDay2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaDay2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxDay3", "QuotaDay3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaDay3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxWeek1", "QuotaWeek1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaWeek1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxWeek2", "QuotaWeek2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaWeek2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "QuotaTextBoxWeek3", "QuotaWeek3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'QuotaWeek3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxDay1", "XpDay1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpDay1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxDay2", "XpDay2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpDay2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxDay3", "XpDay3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpDay3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxWeek1", "XpWeek1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpWeek1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxWeek2", "XpWeek2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpWeek2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBoxWeek3", "XpWeek3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpWeek3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxDay1", "DesDay1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesDay1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxDay2", "DesDay2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesDay2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxDay3", "DesDay3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesDay3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxWeek1", "DesWeek1",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesWeek1'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxWeek2", "DesWeek2",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesWeek2'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "DesTextBoxWeek3", "DesWeek3",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'DesWeek3'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupCheckBoxBinding(root, "NeedAutoCheckBox", "NeedAuto",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'NeedAutoCheckBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "RunTimeTextBox", "RunTime",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'RunTimeTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "IntervalTextBox", "Interval",
                        BindingMode.TwoWay, QuestSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'IntervalTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    // Your settings event handlers here.
                    var resetButton = Wpf.FindControlByName<Button>(root, "ResetButton");
                    resetButton.Click += ResetButtonOnClick;

                    var ButtonRefreshDay1 = Wpf.FindControlByName<Button>(root, "ButtonRefreshDay1");
                    ButtonRefreshDay1.Click += ButtonRefreshDay1OnClick;

                    var ButtonRefreshDay2 = Wpf.FindControlByName<Button>(root, "ButtonRefreshDay2");
                    ButtonRefreshDay2.Click += ButtonRefreshDay2OnClick;

                    var ButtonRefreshDay3 = Wpf.FindControlByName<Button>(root, "ButtonRefreshDay3");
                    ButtonRefreshDay3.Click += ButtonRefreshDay3OnClick;

                    var ButtonRefreshWeek1 = Wpf.FindControlByName<Button>(root, "ButtonRefreshWeek1");
                    ButtonRefreshWeek1.Click += ButtonRefreshWeek1OnClick;

                    var ButtonRefreshWeek2 = Wpf.FindControlByName<Button>(root, "ButtonRefreshWeek2");
                    ButtonRefreshWeek2.Click += ButtonRefreshWeek2OnClick;

                    var ButtonRefreshWeek3 = Wpf.FindControlByName<Button>(root, "ButtonRefreshWeek3");
                    ButtonRefreshWeek3.Click += ButtonRefreshWeek3OnClick;

                    _control = root;
                }

                return _control;
            }
        }

        private void ButtonRefreshDay1OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdDay1 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdDay1);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ButtonRefreshDay2OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdDay2 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdDay2);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ButtonRefreshDay3OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdDay3 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdDay3);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ButtonRefreshWeek1OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdWeek1 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdWeek1);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ButtonRefreshWeek2OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdWeek2 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdWeek2);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ButtonRefreshWeek3OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestManager quest = QuestManager.Get();
            if (quest != null)
            {
                if (QuestSettings.Instance.QuestIdWeek3 > 0)
                {
                    quest.RerollQuest(QuestSettings.Instance.QuestIdWeek3);
                    UpdateGui(true);
                }
                else Log.ErrorFormat("任务无效，不可切换");
            }
        }

        private void ResetButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            QuestSettings.Instance.Reset();
            UpdateGui(false);
        }

        /// <summary>Is this plugin currently enabled?</summary>
        public bool IsEnabled
        {
            get { return _enabled; }
        }

        /// <summary> The plugin is being enabled.</summary>
        public void Enable()
        {
            Log.DebugFormat("[任务插件] 启用");
            _enabled = true;
        }

        /// <summary> The plugin is being disabled.</summary>
        public void Disable()
        {
            Log.DebugFormat("[任务插件] 禁用");
            _enabled = false;
        }

        /// <summary>Deinitializes this object. This is called when the object is being unloaded from the bot.</summary>
        public void Deinitialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ": " + Description;
        }

        /*
         *  Log.WarnFormat("QuestId={0}", item.QuestId);
            Log.WarnFormat("Name={0}", item.Name);
            Log.WarnFormat("Description={0}", item.Description);
            Log.WarnFormat("RewardTrackXp={0}", item.RewardTrackXp);
            Log.WarnFormat("RerollCount={0}", item.RerollCount);
            Log.WarnFormat("PoolId={0}", item.PoolId);
            Log.WarnFormat("Progress={0}", item.Progress);
            Log.WarnFormat("Quota={0}", item.Quota);
            Log.WarnFormat("TimeUntilExpiration={0}", item.TimeUntilExpiration);
            Log.WarnFormat("TimeUntilNextQuest={0}", item.TimeUntilNextQuest);
            Log.WarnFormat("Abandonable={0}", item.Abandonable);
        */
        private void RefreshQuestData()
        {
            using (TritonHs.AcquireFrame())
            {
                try
                {
                    //禁用所有更换按钮
                    var ButtonRefreshDay1 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshDay1");
                    ButtonRefreshDay1.IsEnabled = false;
                    var ButtonRefreshDay2 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshDay2");
                    ButtonRefreshDay2.IsEnabled = false;
                    var ButtonRefreshDay3 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshDay3");
                    ButtonRefreshDay3.IsEnabled = false;
                    var ButtonRefreshWeek1 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshWeek1");
                    ButtonRefreshWeek1.IsEnabled = false;
                    var ButtonRefreshWeek2 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshWeek2");
                    ButtonRefreshWeek2.IsEnabled = false;
                    var ButtonRefreshWeek3 = Wpf.FindControlByName<Button>(_control, "ButtonRefreshWeek3");
                    ButtonRefreshWeek3.IsEnabled = false;

                    //刷新任务最新数据
                    QuestSettings.Instance.ResetQuest();
                    QuestManager quest = QuestManager.Get();
                    if (quest != null)
                    {
                        int i = 0;
                        QuestListDataModel questDay = quest.CreateActiveQuestsDataModel(
                            QuestPool.QuestPoolType.DAILY, Assets.QuestPool.RewardTrackType.GLOBAL, true);
                        foreach (QuestDataModel item in questDay.Quests)
                        {
                            if (item != null)
                            {
                                if (i == 0)
                                {
                                    QuestSettings.Instance.QuestIdDay1 = item.QuestId;
                                    QuestSettings.Instance.RollCntDay1 = item.RerollCount;
                                    QuestSettings.Instance.ProgressDay1 = item.Progress;
                                    QuestSettings.Instance.QuotaDay1 = item.Quota;
                                    QuestSettings.Instance.XpDay1 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesDay1 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshDay1.IsEnabled = true;
                                    }
                                }
                                else if (i == 1)
                                {
                                    QuestSettings.Instance.QuestIdDay2 = item.QuestId;
                                    QuestSettings.Instance.RollCntDay2 = item.RerollCount;
                                    QuestSettings.Instance.ProgressDay2 = item.Progress;
                                    QuestSettings.Instance.QuotaDay2 = item.Quota;
                                    QuestSettings.Instance.XpDay2 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesDay2 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshDay2.IsEnabled = true;
                                    }
                                }
                                else if (i == 2)
                                {
                                    QuestSettings.Instance.QuestIdDay3 = item.QuestId;
                                    QuestSettings.Instance.RollCntDay3 = item.RerollCount;
                                    QuestSettings.Instance.ProgressDay3 = item.Progress;
                                    QuestSettings.Instance.QuotaDay3 = item.Quota;
                                    QuestSettings.Instance.XpDay3 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesDay3 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshDay3.IsEnabled = true;
                                    }
                                }
                                i++;
                            }
                        }

                        i = 0;
                        QuestListDataModel questWeek = quest.CreateActiveQuestsDataModel(
                            QuestPool.QuestPoolType.WEEKLY,Assets.QuestPool.RewardTrackType.GLOBAL, true);
                        foreach (QuestDataModel item in questWeek.Quests)
                        {
                            if (item != null)
                            {
                                if (i == 0)
                                {
                                    QuestSettings.Instance.QuestIdWeek1 = item.QuestId;
                                    QuestSettings.Instance.RollCntWeek1 = item.RerollCount;
                                    QuestSettings.Instance.ProgressWeek1 = item.Progress;
                                    QuestSettings.Instance.QuotaWeek1 = item.Quota;
                                    QuestSettings.Instance.XpWeek1 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesWeek1 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshWeek1.IsEnabled = true;
                                    }
                                }
                                else if (i == 1)
                                {
                                    QuestSettings.Instance.QuestIdWeek2 = item.QuestId;
                                    QuestSettings.Instance.RollCntWeek2 = item.RerollCount;
                                    QuestSettings.Instance.ProgressWeek2 = item.Progress;
                                    QuestSettings.Instance.QuotaWeek2 = item.Quota;
                                    QuestSettings.Instance.XpWeek2 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesWeek2 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshWeek2.IsEnabled = true;
                                    }
                                }
                                else if (i == 2)
                                {
                                    QuestSettings.Instance.QuestIdWeek3 = item.QuestId;
                                    QuestSettings.Instance.RollCntWeek3 = item.RerollCount;
                                    QuestSettings.Instance.ProgressWeek3 = item.Progress;
                                    QuestSettings.Instance.QuotaWeek3 = item.Quota;
                                    QuestSettings.Instance.XpWeek3 = item.RewardTrackXp;
                                    QuestSettings.Instance.DesWeek3 =
                                        item.QuestId > 0 ? item.Description : item.TimeUntilNextQuest;
                                    if (item.QuestId > 0 && item.RerollCount > 0)
                                    {
                                        ButtonRefreshWeek3.IsEnabled = true;
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void updateGui(object bSleep)
        {
            if ((bool)bSleep) Thread.Sleep(5000);
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                RefreshQuestData();
            });
        }

        private void UpdateGui(bool bSleep)
        {
            Thread t = new Thread(new ParameterizedThreadStart(updateGui));
            t.Start(bSleep);
        }

        private void GameEventManagerOnGameOver(object sender, GameOverEventArgs gameOverEventArgs)
        {
            try
            {
                if (QuestSettings.Instance.NeedAuto)
                {
                    QuestSettings.Instance.RunTimeSecond += (long)stopwatch_0.Elapsed.TotalSeconds;
                }
                else QuestSettings.Instance.RunTimeSecond = 0;
                stopwatch_0.Restart();

                //凌晨重置时间
                /*
                //判断时间，重置TodayRebootCnt
                TimeSpan nowDt = DateTime.Now.TimeOfDay;
                TimeSpan workStart = DateTime.Parse("00:00:00").TimeOfDay;
                TimeSpan workStop = DateTime.Parse("00:00:05").TimeOfDay;
                if (nowDt > workStart && nowDt < workStop)
                {
                    Log($"当前时间0点0分，重启次数重置为0");
                    TodayRebootCnt = -1;
                    TimerMonitor.Stop();
                    Delay(1000 * 6);
                    TimerMonitor.Interval = 1000;
                    TimerMonitor.Start();
                }
                if (TodayRebootCnt == -1)
                {
                    TodayRebootCnt = 0;
                    TodayRebootCntStr = TodayRebootCnt.ToString() + "/";
                    if (!IsRunning)
                    {
                        if (NeedPushMessage)
                        {
                            string result;
                            PushStone.PostMessage(PushPlusToken, "",
                                CurrRunningAccount.Email, TodayRebootCnt, RebootCntMax,
                                PushStone.MSG_TYPE.MSG_START, out result);
                            Log(result);
                        }
                        Log($"一日之计在于晨，兄弟中控自动开始运行");
                        StartRun();
                    }
                }*/

                bool bSleep = false;
                if (QuestSettings.Instance.NeedAuto &&
                    QuestSettings.Instance.RunTimeSecond > QuestSettings.Instance.Interval * 3600)
                {
                    Random random = new Random();
                    QuestManager quest = QuestManager.Get();

                    {
                        //获取所有每日任务
                        QuestListDataModel questDay = quest.CreateActiveQuestsDataModel(
                            QuestPool.QuestPoolType.DAILY,Assets.QuestPool.RewardTrackType.GLOBAL, true);

                        //筛选0进度每日任务
                        List<QuestDataModel> questValidDay=new List<QuestDataModel>();
                        foreach (QuestDataModel item in questDay.Quests)
                        {
                            if (item == null) break;
                            if (item.QuestId > 0 && item.Progress == 0 &&
                                item.Quota > 0 && item.RerollCount>0)
                            {
                                questValidDay.Add(item);
                            }
                        }

                        //随机更换1个任务
                        if (questValidDay.Count > 0)
                        {
                            bSleep = true;
                            int idx = random.Next(questValidDay.Count);
                            QuestDataModel questRe = questValidDay[idx];
                            quest.RerollQuest(questRe.QuestId);
                            Log.ErrorFormat("随机更换无法完成的每日任务{0}：{1}",
                                questRe.QuestId, questRe.Description);
                        }
                    }

                    {
                        //获取所有每周任务
                        QuestListDataModel quesWeek = quest.CreateActiveQuestsDataModel(
                            QuestPool.QuestPoolType.WEEKLY, Assets.QuestPool.RewardTrackType.GLOBAL,true);

                        //筛选0进度每周任务
                        List<QuestDataModel> questValidWeek = new List<QuestDataModel>();
                        foreach (QuestDataModel item in quesWeek.Quests)
                        {
                            if (item == null) break;
                            if (item.QuestId > 0 && item.Progress == 0 &&
                                item.Quota > 0 && item.RerollCount > 0)
                            {
                                questValidWeek.Add(item);
                            }
                        }

                        //随机更换1个任务
                        if (questValidWeek.Count > 0)
                        {
                            bSleep = true;
                            int idx = random.Next(questValidWeek.Count);
                            QuestDataModel questRe = questValidWeek[idx];
                            quest.RerollQuest(questRe.QuestId);
                            Log.ErrorFormat("随机更换无法完成的每周任务{0}：{1}",
                                questRe.QuestId, questRe.Description);
                        }
                    }

                    QuestSettings.Instance.RunTimeSecond = 0;
                }

                UpdateGui(bSleep);
            }
            catch (Exception)
            {

            }
        }
    }
}

