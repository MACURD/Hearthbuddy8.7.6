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
using Logger1 = Triton.Common.LogUtilities.Logger;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Reflection;
using Triton.Game.Mapping;
using Triton.Game.Mono;
using Triton.Bot.Logic.Bots.DefaultBot;

namespace Monitor
{
    public class Stats
    {
        public int Wins;
        public int Losses;
        public int Concedes;
        public int Level;
        public int Xp;
        public int XpNeeded;
    }

    public class Monitor : IPlugin
    {
        private static readonly ILog Log = Logger1.GetLoggerInstanceForType();
        Timer _expTimer;
        private bool _enabled;
        Stats oldStats;
        private readonly Stopwatch stopwatch_0 = new Stopwatch();

        public int[] allNeedXp ={
            0,     100,   200,   350,   500,   725,   950,   1250,  1550,  1875,//1-10
            2200,  2550,  2900,  3275,  3650,  4050,  4450,  4875,  5300,  5750,
            6200,  6750,  7350,  8000,  8675,  9350,  10225, 11100, 12100, 13200,
            14400, 15600, 16850, 18100, 19400, 20700, 22050, 23400, 24800, 26200,
            27650, 29100, 30600, 32100, 33650, 35200, 36800, 38400, 40050, 41700,
            43400, 45100, 46850, 48600, 50400, 52200, 54050, 55900, 57800, 59700,
            61650, 63600, 65600, 67600, 69650, 71700, 73825, 75950, 78200, 80450,
            82825, 85200, 87700, 90200, 92700, 95200, 97700, 100200,102700,105200,
            107700,110200,112700,115200,117700,120200,122700,125200,127700,130200,
            132700,135200,137700,140200,142700,145200,147700,150200,152700,155200,//90-100
            156525,157850,159175,160500,161825,163150,164475,165800,167125,168450,
            169775,171100,172425,173750,175075,176550,178025,179500,180975,182450,
            183925,185400,186875,188350,189825,191300,192775,194250,195725,197200};

        /// <summary> The name of the plugin. </summary>
        public string Name
        {
            get { return "监控插件"; }
        }

        /// <summary> The description of the plugin. </summary>
        public string Description
        {
            get { return "监控胜负场、胜率、投降数、经验、等级。"; }
        }

        /// <summary>The author of the plugin.</summary>
        public string Author
        {
            get { return "开源软件"; }
        }

        /// <summary>The version of the plugin.</summary>
        public string Version
        {
            get { return "1.0.0.0"; }
        }

        /// <summary>Initializes this object. This is called when the object is loaded into the bot.</summary>
        public void Initialize()
        {
            Log.DebugFormat("[监控插件] 初始化");
            TritonHs.OnGuiTick += TritonHsOnOnGuiTick;
            if (!MainSettings.Instance.EnabledPlugins.Contains(this.Name))
                MainSettings.Instance.EnabledPlugins.Add(this.Name);
        }

        /// <summary>Deinitializes this object. This is called when the object is being unloaded from the bot.</summary>
        public void Deinitialize()
        {
            TritonHs.OnGuiTick -= TritonHsOnOnGuiTick;
        }

        /// <summary> The plugin start callback. Do any initialization here. </summary>
        public void Start()
        {
            stopwatch_0.Restart();
            Log.DebugFormat("[监控插件] 开启");
            GameEventManager.GameOver += GameEventManager_OnGameOver;
            MonitorSettings.Instance.ReloadFile();
            oldStats = GetStats();
            UpdateMainGuiStats();
        }

        /// <summary> The plugin stop callback. Do any pre-dispose cleanup here. </summary>
        public void Stop()
        {
            stopwatch_0.Stop();
            Log.DebugFormat("[监控插件] 停止");
            GameEventManager.GameOver -= GameEventManager_OnGameOver;
        }

        /// <summary> The plugin tick callback. Do any update logic here. </summary>
        public void Tick(){}

        public JsonSettings Settings
        {
            get { return MonitorSettings.Instance; }
        }

        private UserControl _control;

        /// <summary> The plugin's settings control. This will be added to the Hearthbuddy Settings tab.</summary>
        public UserControl Control
        {
            get
            {
                if (_control != null)
                {
                    return _control;
                }

                using (var fs = new FileStream(@"Plugins\Monitor\SettingsGui.xaml", FileMode.Open))
                {
                    var root = (UserControl)XamlReader.Load(fs);

                    if (!Wpf.SetupTextBoxBinding(root, "LevelTextBox", "Level",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'LevelTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpTextBox", "Xp",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "XpNeededTextBox", "XpNeeded",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'XpNeededTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "AllXpTextBox", "AllXp",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AllXpTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "AllXpNeededTextBox", "AllXpNeeded",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AllXpNeededTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "AllRunTimeTextBox", "AllRunTimeText",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AllRunTimeTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "AllGetXpTextBox", "AllGetXp",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AllGetXpTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "PerHourXpStrTextBox", "PerHourXpStr",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'PerHourXpStrTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "FullXpNeededTextBox", "FullXpNeeded",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'FullXpNeededTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "FullTimeNeededTextBox", "FullTimeNeeded",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'FullTimeNeededTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "CollectionTextBox", "Collection",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'CollectionTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "PassportEndTextBox", "PassportEnd",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'PassportEndTextBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "TwistInfoBox", "TwistInfo",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'TwistInfoBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "StandardInfoBox", "StandardInfo",
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'StandardInfoBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    if (!Wpf.SetupTextBoxBinding(root, "WildInfoBox", "WildInfo", 
                        BindingMode.TwoWay, MonitorSettings.Instance))
                    {
                        Log.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'WildInfoBox'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    var resetButton = Wpf.FindControlByName<Button>(root, "ResetButton");
                    resetButton.Click += ResetButtonOnClick;

                    _control = root;
                }

                return _control;
            }
        }

        /// <summary>Is this plugin currently enabled?</summary>
        public bool IsEnabled
        {
            get { return _enabled; }
        }

        /// <summary> The plugin is being enabled.</summary>
        public void Enable()
        {
            Log.DebugFormat("[监控插件] 启用");
            _enabled = true;
        }

        /// <summary> The plugin is being disabled.</summary>
        public void Disable()
        {
            Log.DebugFormat("[监控插件] 禁用");
            _enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ": " + Description;
        }

        private void GameEventManager_OnGameOver(object sender, GameOverEventArgs gameOverEventArgs)
        {
            if (gameOverEventArgs.Result == GameOverFlag.Victory)
            {
                MonitorSettings.Instance.Wins++;
            }
            else if (gameOverEventArgs.Result == GameOverFlag.Defeat)
            {
                if (gameOverEventArgs.Conceded)
                {
                    MonitorSettings.Instance.Concedes++;
                }
                else
                {
                    MonitorSettings.Instance.Losses++;
                }
            }

            if (oldStats.Level < 400)
                MonitorSettings.Instance.AllRunTime += (long)stopwatch_0.Elapsed.TotalSeconds;

            var stats = GetStats();
            if (stats != null)
            {
                int total = 0;
                if (oldStats.Level > 130)
                    total = allNeedXp[129] + (oldStats.Level - 130) * 1500 + oldStats.Xp;
                else total = allNeedXp[oldStats.Level - 1] + oldStats.Xp;
                MonitorSettings.Instance.AllGetXp += (MonitorSettings.Instance.AllXp - total);
                oldStats = stats;
            }

            stopwatch_0.Restart();
            UpdateMainGuiStats();
        }

        private void ResetButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            MonitorSettings.Instance.Wins = 0;
            MonitorSettings.Instance.Losses = 0;
            MonitorSettings.Instance.Concedes = 0;
            MonitorSettings.Instance.Level = 0;
            MonitorSettings.Instance.Xp = 0;
            MonitorSettings.Instance.XpNeeded = 0;
            MonitorSettings.Instance.AllXp = 0;
            MonitorSettings.Instance.AllXpNeeded = 602200;
            MonitorSettings.Instance.AllRunTime = 0;
            MonitorSettings.Instance.AllGetXp = 0;
            MonitorSettings.Instance.PerHourXp = 500;
            MonitorSettings.Instance.FullXpNeeded = 602200;
            MonitorSettings.Instance.FullTimeNeeded = "未知";
            MonitorSettings.Instance.Collection = "未知";
            MonitorSettings.Instance.PassportEnd = "未知";
            MonitorSettings.Instance.ClassicInfo = "未知";
            MonitorSettings.Instance.StandardInfo = "未知";
            MonitorSettings.Instance.WildInfo = "未知";
            oldStats = GetStats();
            UpdateMainGuiStats();
        }

        private void UpdateMainGuiStats()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var leftControl = Wpf.FindControlByName<Label>(Application.Current.MainWindow, "StatusBarLeftLabel");
                var rightControl = Wpf.FindControlByName<Label>(Application.Current.MainWindow, "StatusBarRightLabel");

                if (rightControl != null)
                {
                    string show = "";
                    if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.标准)
                    {
                        show = "标准" + MonitorSettings.Instance.StandardInfo;
                    }
                    else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.狂野)
                    {
                        show = "狂野" + MonitorSettings.Instance.WildInfo;
                    }
                    else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.经典)
                    {
                        show = "经典" + MonitorSettings.Instance.ClassicInfo;
                    }
                    else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.幻变)
                    {
                        show = "幻变" + MonitorSettings.Instance.TwistInfo;
                    }
                    else show = "休闲模式";
                    rightControl.Content = string.Format(
                        "战令{0}级({1}/{2})({3}/小时) {4} [{5}投] ",
                            MonitorSettings.Instance.Level,
                            MonitorSettings.Instance.Xp,
                            MonitorSettings.Instance.XpNeeded,
                            MonitorSettings.Instance.PerHourXp,
                            show,
                            MonitorSettings.Instance.Concedes);
                    Log.InfoFormat("[监控插件] 合计: {0}", rightControl.Content);
                    Configuration.Instance.SaveAll();
                }
            }));
        }

        private void TritonHsOnOnGuiTick(object sender, GuiTickEventArgs guiTickEventArgs)
        {
            // Only update if we're actually enabled.
            if (IsEnabled)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var leftControl = Wpf.FindControlByName<Label>(Application.Current.MainWindow, "StatusBarLeftLabel");
                    if (leftControl != null)
                    {
                        leftControl.Content = string.Format("运行时间: {0}", TritonHs.Runtime.Elapsed.ToString("h'小时 'm'分 's'秒'"));
                    }
                }));
            }
        }

        private string RankNumToString(int num)
        {
            string str;
            switch ((num - 1) / 10)
            {
                case 0:
                    str = "青铜";
                    break;
                case 1:
                    str = "白银";
                    break;
                case 2:
                    str = "黄金";
                    break;
                case 3:
                    str = "白金";
                    break;
                case 4:
                    str = "钻石";
                    break;
                case 5:
                    return "传说";
                default:
                    str = "未知";
                    break;
            }
            return str + (11 - (num - (num - 1) / 10 * 10)).ToString();
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002D94 File Offset: 0x00000F94
        private string GetRankInfo(int mode)
        {
            string result = "未知";

            RankMgr rank = RankMgr.Get();
            if (rank != null)
            {
                MedalInfoTranslator translator = rank.GetLocalPlayerMedalInfo();
                TranslatedMedalInfo medalInfoData = translator.GetCurrentMedal(FormatType.FT_TWIST);
                if (mode == 1)
                {
                    medalInfoData = translator.GetCurrentMedal(FormatType.FT_STANDARD);
                }
                else if (mode == 2)
                {
                    medalInfoData = translator.GetCurrentMedal(FormatType.FT_WILD);
                }
                else if (mode == 3)
                {
                    medalInfoData = translator.GetCurrentMedal(FormatType.FT_CLASSIC);
                }
                StringBuilder stringBuilder = new StringBuilder();
                string text = this.RankNumToString(medalInfoData.starLevel);
                string text2 = (text == "传说") ? (medalInfoData.legendIndex.ToString() + "名") : (" " + medalInfoData.earnedStars.ToString() + "星");
                string text3 = (medalInfoData.seasonGames > 0) ? string.Format("{0:0.00}%", 100f * (float)medalInfoData.seasonWins / (float)medalInfoData.seasonGames) : "0.00%";
                stringBuilder.Append(string.Format("{0}{1} {2}/{3}(胜{4})", new object[]
                {
                        text,
                        text2,
                        medalInfoData.seasonWins,
                        medalInfoData.seasonGames,
                        text3
                }));
                result = stringBuilder.ToString();
            }
            return result;
        }

        private Stats GetStats()
        {
            try
            {
                NetCache netCacheData = NetCache.Get();
                if (netCacheData != null)
                {
                    MonitorSettings.Instance.Collection = 
                        string.Format("金币({0}) 粉尘({1}) 门票({2})",
                        netCacheData.GetGoldBalance(),
                        netCacheData.GetArcaneDustBalance(),
                        netCacheData.GetArenaTicketBalance());
                }

                RewardTrackManager rewardTrackManager = RewardTrackManager.Get();
                if (rewardTrackManager != null)
                {
                    RewardTrack rewardTrack = rewardTrackManager.GetRewardTrack(
                        Assets.Global.RewardTrackType.GLOBAL);
                    if (rewardTrack != null)
                    {
                        MonitorSettings.Instance.Level = rewardTrack.TrackDataModel.Level;
                        MonitorSettings.Instance.Xp = rewardTrack.TrackDataModel.Xp;
                        MonitorSettings.Instance.XpNeeded = rewardTrack.TrackDataModel.XpNeeded;
                    }
                    else Log.DebugFormat("[监控插件] rewardTrack=null");
                }
                else Log.DebugFormat("[监控插件] rewardTrackManager=null");

                MonitorSettings.Instance.TwistInfo = GetRankInfo(0);
                MonitorSettings.Instance.StandardInfo = GetRankInfo(1);
                MonitorSettings.Instance.WildInfo = GetRankInfo(2);
                MonitorSettings.Instance.ClassicInfo = GetRankInfo(3);

                var stats = new Stats
                {
                    Wins = MonitorSettings.Instance.Wins,
                    Losses = MonitorSettings.Instance.Losses,
                    Concedes = MonitorSettings.Instance.Concedes,
                    Level = MonitorSettings.Instance.Level,
                    Xp = MonitorSettings.Instance.Xp,
                    XpNeeded = MonitorSettings.Instance.XpNeeded,
                };

                int total=0;
                if (stats.Level > 130)
                    total = allNeedXp[129] + (stats.Level - 130) * 1500 + stats.Xp;
                else total = allNeedXp[stats.Level - 1] + stats.Xp;
                MonitorSettings.Instance.AllXp = total;
                MonitorSettings.Instance.FullXpNeeded =
                    MonitorSettings.Instance.AllXpNeeded - MonitorSettings.Instance.AllXp;

                int hour = MonitorSettings.Instance.FullXpNeeded / MonitorSettings.Instance.PerHourXp;
                MonitorSettings.Instance.FullTimeNeeded = hour == 0 ? "恭喜满级":
                    ((hour / 24).ToString() + "天" + (hour % 24).ToString() + "小时");

                TimeSpan ts1 = (DateTime.Now - new DateTime(2024, 7, 24, 0, 0, 0, 0));
                TimeSpan ts2 = (new DateTime(2024, 11, 24, 0, 0, 0, 0) - DateTime.Now);
                MonitorSettings.Instance.PassportEnd = "7.24-11.24 已过" +
                    ts1.Days.ToString() + "天" +
                    ts1.Hours.ToString() + "小时" +
                    ts1.Minutes.ToString() + "分 还剩" +
                    ts2.Days.ToString() + "天";

                return stats;
            }
            catch (Exception ex)
            {
                Log.WarnFormat("{0}\r\n{1}", ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
