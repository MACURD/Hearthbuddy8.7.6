using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using log4net;
using Newtonsoft.Json;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Game;
using Triton.Game.Mapping;
using Logger = Triton.Common.LogUtilities.Logger;

namespace Quest
{
    public class QuestSettings : JsonSettings
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private static QuestSettings _instance;

        public static QuestSettings Instance
        {
            get { return _instance ?? (_instance = new QuestSettings()); }
        }

        public QuestSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Quest")))
        {
        }

        public void ReloadFile()
        {
            Reload(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Quest" + GetMyHashCode())));
        }

        private bool _needAuto;
        private long _runTimeSecond;
        private string _runTime;
        private int _interval;

        private int _questIdDay1;
        private int _questIdDay2;
        private int _questIdDay3;
        private int _questIdWeek1;
        private int _questIdWeek2;
        private int _questIdWeek3;

        private int _rollCntDay1;
        private int _rollCntDay2;
        private int _rollCntDay3;
        private int _rollCntWeek1;
        private int _rollCntWeek2;
        private int _rollCntWeek3;

        private int _progressDay1;
        private int _progressDay2;
        private int _progressDay3;
        private int _progressWeek1;
        private int _progressWeek2;
        private int _progressWeek3;

        private int _quotaDay1;
        private int _quotaDay2;
        private int _quotaDay3;
        private int _quotaWeek1;
        private int _quotaWeek2;
        private int _quotaWeek3;

        private int _xpDay1;
        private int _xpDay2;
        private int _xpDay3;
        private int _xpWeek1;
        private int _xpWeek2;
        private int _xpWeek3;

        private string _desDay1;
        private string _desDay2;
        private string _desDay3;
        private string _desWeek1;
        private string _desWeek2;
        private string _desWeek3;

        public void ResetQuest()
        {
            QuestIdDay1 = 0;
            QuestIdDay2 = 0;
            QuestIdDay3 = 0;
            QuestIdWeek1 = 0;
            QuestIdWeek2 = 0;
            QuestIdWeek3 = 0;
            RollCntDay1 = 0;
            RollCntDay2 = 0;
            RollCntDay3 = 0;
            RollCntWeek1 = 0;
            RollCntWeek2 = 0;
            RollCntWeek3 = 0;
            ProgressDay1 =0;
            ProgressDay2=0;
            ProgressDay3=0;
            ProgressWeek1=0;
            ProgressWeek2=0;
            ProgressWeek3=0;
            QuotaDay1=0;
            QuotaDay2=0;
            QuotaDay3=0;
            QuotaWeek1=0;
            QuotaWeek2=0;
            QuotaWeek3=0;
            XpDay1 = 0;
            XpDay2 = 0;
            XpDay3 = 0;
            XpWeek1 = 0;
            XpWeek2 = 0;
            XpWeek3 = 0;
            DesDay1 = "";
            DesDay2 = "";
            DesDay3 = "";
            DesWeek1 = "";
            DesWeek2 = "";
            DesWeek3 = "";
        }

        public void Reset()
        {
            NeedAuto = true;
            RunTimeSecond=0;
            RunTime = "0.00";
            Interval = 1;
            ResetQuest();
        }

        [DefaultValue(true)]
        public bool NeedAuto
        {
            get { return _needAuto; }
            set
            {
                if (value.Equals(_needAuto))
                {
                    return;
                }
                _needAuto = value;
                NotifyPropertyChanged(() => NeedAuto);
            }
        }

        [DefaultValue("0.00")]
        public string RunTime
        {
            get { return _runTime; }
            set
            {
                if (value.Equals(_runTime))
                {
                    return;
                }
                _runTime = value;
                NotifyPropertyChanged(() => RunTime);
            }
        }

        [DefaultValue(0)]
        public long RunTimeSecond
        {
            get { return _runTimeSecond; }
            set
            {
                if (value.Equals(_runTimeSecond))
                {
                    return;
                }
                _runTimeSecond = value;
                NotifyPropertyChanged(() => RunTimeSecond);
                float t = ((float)_runTimeSecond / 3600);
                RunTime = t.ToString("F2");
            }
        }

        [DefaultValue(1)]
        public int Interval
        {
            get { return _interval; }
            set
            {
                if (value.Equals(_interval))
                {
                    return;
                }
                _interval = value;
                NotifyPropertyChanged(() => Interval);
            }
        }

        [DefaultValue(0)]
        public int QuestIdDay1
        {
            get { return _questIdDay1; }
            set
            {
                if (value.Equals(_questIdDay1))
                {
                    return;
                }
                _questIdDay1 = value;
                NotifyPropertyChanged(() => QuestIdDay1);
            }
        }

        [DefaultValue(0)]
        public int QuestIdDay2
        {
            get { return _questIdDay2; }
            set
            {
                if (value.Equals(_questIdDay2))
                {
                    return;
                }
                _questIdDay2 = value;
                NotifyPropertyChanged(() => QuestIdDay2);
            }
        }

        [DefaultValue(0)]
        public int QuestIdDay3
        {
            get { return _questIdDay3; }
            set
            {
                if (value.Equals(_questIdDay3))
                {
                    return;
                }
                _questIdDay3 = value;
                NotifyPropertyChanged(() => QuestIdDay3);
            }
        }

        [DefaultValue(0)]
        public int QuestIdWeek1
        {
            get { return _questIdWeek1; }
            set
            {
                if (value.Equals(_questIdWeek1))
                {
                    return;
                }
                _questIdWeek1 = value;
                NotifyPropertyChanged(() => QuestIdWeek1);
            }
        }

        [DefaultValue(0)]
        public int QuestIdWeek2
        {
            get { return _questIdWeek2; }
            set
            {
                if (value.Equals(_questIdWeek2))
                {
                    return;
                }
                _questIdWeek2 = value;
                NotifyPropertyChanged(() => QuestIdWeek2);
            }
        }

        [DefaultValue(0)]
        public int QuestIdWeek3
        {
            get { return _questIdWeek3; }
            set
            {
                if (value.Equals(_questIdWeek3))
                {
                    return;
                }
                _questIdWeek3 = value;
                NotifyPropertyChanged(() => QuestIdWeek3);
            }
        }

        [DefaultValue(0)]
        public int RollCntDay1
        {
            get { return _rollCntDay1; }
            set
            {
                if (value.Equals(_rollCntDay1))
                {
                    return;
                }
                _rollCntDay1 = value;
                NotifyPropertyChanged(() => RollCntDay1);
            }
        }

        [DefaultValue(0)]
        public int RollCntDay2
        {
            get { return _rollCntDay2; }
            set
            {
                if (value.Equals(_rollCntDay2))
                {
                    return;
                }
                _rollCntDay2 = value;
                NotifyPropertyChanged(() => RollCntDay2);
            }
        }

        [DefaultValue(0)]
        public int RollCntDay3
        {
            get { return _rollCntDay3; }
            set
            {
                if (value.Equals(_rollCntDay3))
                {
                    return;
                }
                _rollCntDay3 = value;
                NotifyPropertyChanged(() => RollCntDay3);
            }
        }

        [DefaultValue(0)]
        public int RollCntWeek1
        {
            get { return _rollCntWeek1; }
            set
            {
                if (value.Equals(_rollCntWeek1))
                {
                    return;
                }
                _rollCntWeek1 = value;
                NotifyPropertyChanged(() => RollCntWeek1);
            }
        }

        [DefaultValue(0)]
        public int RollCntWeek2
        {
            get { return _rollCntWeek2; }
            set
            {
                if (value.Equals(_rollCntWeek2))
                {
                    return;
                }
                _rollCntWeek2 = value;
                NotifyPropertyChanged(() => RollCntWeek2);
            }
        }

        [DefaultValue(0)]
        public int RollCntWeek3
        {
            get { return _rollCntWeek3; }
            set
            {
                if (value.Equals(_rollCntWeek3))
                {
                    return;
                }
                _rollCntWeek3 = value;
                NotifyPropertyChanged(() => RollCntWeek3);
            }
        }

        [DefaultValue(0)]
        public int ProgressDay1
        {
            get { return _progressDay1; }
            set
            {
                if (value.Equals(_progressDay1))
                {
                    return;
                }
                _progressDay1 = value;
                NotifyPropertyChanged(() => ProgressDay1);
            }
        }

        [DefaultValue(0)]
        public int ProgressDay2
        {
            get { return _progressDay2; }
            set
            {
                if (value.Equals(_progressDay2))
                {
                    return;
                }
                _progressDay2 = value;
                NotifyPropertyChanged(() => ProgressDay2);
            }
        }

        [DefaultValue(0)]
        public int ProgressDay3
        {
            get { return _progressDay3; }
            set
            {
                if (value.Equals(_progressDay3))
                {
                    return;
                }
                _progressDay3 = value;
                NotifyPropertyChanged(() => ProgressDay3);
            }
        }

        [DefaultValue(0)]
        public int ProgressWeek1
        {
            get { return _progressWeek1; }
            set
            {
                if (value.Equals(_progressWeek1))
                {
                    return;
                }
                _progressWeek1 = value;
                NotifyPropertyChanged(() => ProgressWeek1);
            }
        }

        [DefaultValue(0)]
        public int ProgressWeek2
        {
            get { return _progressWeek2; }
            set
            {
                if (value.Equals(_progressWeek2))
                {
                    return;
                }
                _progressWeek2 = value;
                NotifyPropertyChanged(() => ProgressWeek2);
            }
        }

        [DefaultValue(0)]
        public int ProgressWeek3
        {
            get { return _progressWeek3; }
            set
            {
                if (value.Equals(_progressWeek3))
                {
                    return;
                }
                _progressWeek3 = value;
                NotifyPropertyChanged(() => ProgressWeek3);
            }
        }

        [DefaultValue(0)]
        public int QuotaDay1
        {
            get { return _quotaDay1; }
            set
            {
                if (value.Equals(_quotaDay1))
                {
                    return;
                }
                _quotaDay1 = value;
                NotifyPropertyChanged(() => QuotaDay1);
            }
        }

        [DefaultValue(0)]
        public int QuotaDay2
        {
            get { return _quotaDay2; }
            set
            {
                if (value.Equals(_quotaDay2))
                {
                    return;
                }
                _quotaDay2 = value;
                NotifyPropertyChanged(() => QuotaDay2);
            }
        }

        [DefaultValue(0)]
        public int QuotaDay3
        {
            get { return _quotaDay3; }
            set
            {
                if (value.Equals(_quotaDay3))
                {
                    return;
                }
                _quotaDay3 = value;
                NotifyPropertyChanged(() => QuotaDay3);
            }
        }

        [DefaultValue(0)]
        public int QuotaWeek1
        {
            get { return _quotaWeek1; }
            set
            {
                if (value.Equals(_quotaWeek1))
                {
                    return;
                }
                _quotaWeek1 = value;
                NotifyPropertyChanged(() => QuotaWeek1);
            }
        }

        [DefaultValue(0)]
        public int QuotaWeek2
        {
            get { return _quotaWeek2; }
            set
            {
                if (value.Equals(_quotaWeek2))
                {
                    return;
                }
                _quotaWeek2 = value;
                NotifyPropertyChanged(() => QuotaWeek2);
            }
        }

        [DefaultValue(0)]
        public int QuotaWeek3
        {
            get { return _quotaWeek3; }
            set
            {
                if (value.Equals(_quotaWeek3))
                {
                    return;
                }
                _quotaWeek3 = value;
                NotifyPropertyChanged(() => QuotaWeek3);
            }
        }

        [DefaultValue(0)]
        public int XpDay1
        {
            get { return _xpDay1; }
            set
            {
                if (value.Equals(_xpDay1))
                {
                    return;
                }
                _xpDay1 = value;
                NotifyPropertyChanged(() => XpDay1);
            }
        }

        [DefaultValue(0)]
        public int XpDay2
        {
            get { return _xpDay2; }
            set
            {
                if (value.Equals(_xpDay2))
                {
                    return;
                }
                _xpDay2 = value;
                NotifyPropertyChanged(() => XpDay2);
            }
        }

        [DefaultValue(0)]
        public int XpDay3
        {
            get { return _xpDay3; }
            set
            {
                if (value.Equals(_xpDay3))
                {
                    return;
                }
                _xpDay3 = value;
                NotifyPropertyChanged(() => XpDay3);
            }
        }

        [DefaultValue(0)]
        public int XpWeek1
        {
            get { return _xpWeek1; }
            set
            {
                if (value.Equals(_xpWeek1))
                {
                    return;
                }
                _xpWeek1 = value;
                NotifyPropertyChanged(() => XpWeek1);
            }
        }

        [DefaultValue(0)]
        public int XpWeek2
        {
            get { return _xpWeek2; }
            set
            {
                if (value.Equals(_xpWeek2))
                {
                    return;
                }
                _xpWeek2 = value;
                NotifyPropertyChanged(() => XpWeek2);
            }
        }

        [DefaultValue(0)]
        public int XpWeek3
        {
            get { return _xpWeek3; }
            set
            {
                if (value.Equals(_xpWeek3))
                {
                    return;
                }
                _xpWeek3 = value;
                NotifyPropertyChanged(() => XpWeek3);
            }
        }

        [DefaultValue("")]
        public string DesDay1
        {
            get { return _desDay1; }
            set
            {
                if (value.Equals(_desDay1))
                {
                    return;
                }
                _desDay1 = value;
                NotifyPropertyChanged(() => DesDay1);
            }
        }

        [DefaultValue("")]
        public string DesDay2
        {
            get { return _desDay2; }
            set
            {
                if (value.Equals(_desDay2))
                {
                    return;
                }
                _desDay2 = value;
                NotifyPropertyChanged(() => DesDay2);
            }
        }

        [DefaultValue("")]
        public string DesDay3
        {
            get { return _desDay3; }
            set
            {
                if (value.Equals(_desDay3))
                {
                    return;
                }
                _desDay3 = value;
                NotifyPropertyChanged(() => DesDay3);
            }
        }

        [DefaultValue("")]
        public string DesWeek1
        {
            get { return _desWeek1; }
            set
            {
                if (value.Equals(_desWeek1))
                {
                    return;
                }
                _desWeek1 = value;
                NotifyPropertyChanged(() => DesWeek1);
            }
        }

        [DefaultValue("")]
        public string DesWeek2
        {
            get { return _desWeek2; }
            set
            {
                if (value.Equals(_desWeek2))
                {
                    return;
                }
                _desWeek2 = value;
                NotifyPropertyChanged(() => DesWeek2);
            }
        }

        [DefaultValue("")]
        public string DesWeek3
        {
            get { return _desWeek3; }
            set
            {
                if (value.Equals(_desWeek3))
                {
                    return;
                }
                _desWeek3 = value;
                NotifyPropertyChanged(() => DesWeek3);
            }
        }
    }
}
