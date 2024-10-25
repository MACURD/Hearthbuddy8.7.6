using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using log4net;
using Newtonsoft.Json;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Game.Mapping;
using Logger1 = Triton.Common.LogUtilities.Logger;

namespace Monitor
{
    /// <summary>Settings for the Stats plugin. </summary>
    public class MonitorSettings : JsonSettings
    {
        private static readonly ILog Log = Logger1.GetLoggerInstanceForType();

        private static MonitorSettings _instance;

        /// <summary>The current instance for this class. </summary>
        public static MonitorSettings Instance
        {
            get { return _instance ?? (_instance = new MonitorSettings()); }
        }

        /// <summary>The default ctor. Will use the settings path "Stats".</summary>
        public MonitorSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Monitor")))
        {
        }

        public void ReloadFile()
        {
            Reload(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Monitor" + GetMyHashCode())));
        }

        private int _wins;
        private int _losses;
        private int _concedes;
        private int _level;
        private int _xp;
        private int _xpNeeded;
        private int _allXp;
        private int _allXpNeeded;
        private long _allRunTime;       //秒
        private long _allGetXp;         //总经验
        private int _perHourXp;         //均值每小时
        private string _perHourXpStr;   //均值每小时(字符串)
        private int _fullXpNeeded;
        private string _fullTimeNeeded;
        private string _collection;
        private string _passportEnd;
        private string _classicInfo;
        private string _standardInfo;
        private string _wildInfo;
        private string _twistInfo;

        /// <summary>Current stored wins.</summary>
        [DefaultValue(0)]
        public int Wins
        {
            get { return _wins; }
            set
            {
                if (value.Equals(_wins))
                {
                    return;
                }
                _wins = value;
                NotifyPropertyChanged(() => Wins);
            }
        }

        /// <summary>Current stored losses.</summary>
        [DefaultValue(0)]
        public int Losses
        {
            get { return _losses; }
            set
            {
                if (value.Equals(_losses))
                {
                    return;
                }
                _losses = value;
                NotifyPropertyChanged(() => Losses);
            }
        }

        /// <summary>Current stored concedes.</summary>
        [DefaultValue(0)]
        public int Concedes
        {
            get { return _concedes; }
            set
            {
                if (value.Equals(_concedes))
                {
                    return;
                }
                _concedes = value;
                NotifyPropertyChanged(() => Concedes);
            }
        }

        [DefaultValue(0)]
        public int Level
        {
            get { return _level; }
            set
            {
                if (value.Equals(_level))
                {
                    return;
                }
                _level = value;
                NotifyPropertyChanged(() => Level);
            }
        }

        [DefaultValue(0)]
        public int Xp
        {
            get { return _xp; }
            set
            {
                if (value.Equals(_xp))
                {
                    return;
                }
                _xp = value;
                NotifyPropertyChanged(() => Xp);
            }
        }

        [DefaultValue(0)]
        public int XpNeeded
        {
            get { return _xpNeeded; }
            set
            {
                if (value.Equals(_xpNeeded))
                {
                    return;
                }
                _xpNeeded = value;
                NotifyPropertyChanged(() => XpNeeded);
            }
        }

        [DefaultValue(0)]
        public int AllXp
        {
            get { return _allXp; }
            set
            {
                if (value.Equals(_allXp))
                {
                    return;
                }
                _allXp = value;
                NotifyPropertyChanged(() => AllXp);
            }
        }

        [DefaultValue(602200)]
        public int AllXpNeeded
        {
            get { return _allXpNeeded; }
            set
            {
                if (value.Equals(_allXpNeeded))
                {
                    return;
                }
                _allXpNeeded = value;
                NotifyPropertyChanged(() => AllXpNeeded);
            }
        }

        [DefaultValue(0)]
        public long AllRunTime
        {
            get { return _allRunTime; }
            set
            {
                if (value.Equals(_allRunTime))
                {
                    return;
                }
                _allRunTime = value;
                NotifyPropertyChanged(() => AllRunTime);
                float t = ((float)_allRunTime / 3600);
                AllRunTimeText = t.ToString("F2");
                if (t > 0) PerHourXp = (int)(AllGetXp / t);
                else PerHourXp = 0;
                if (PerHourXp <= 300 || PerHourXp >= 700) PerHourXp = 500;
            }
        }

        string allRunTimeText;
        [DefaultValue("0.0")]
        public string AllRunTimeText
        {
            get { return allRunTimeText; }
            set
            {
                if (!value.Equals(allRunTimeText))
                {
                    allRunTimeText = value;
                    NotifyPropertyChanged(() => AllRunTimeText);
                }
            }
        }

        [DefaultValue(0)]
        public long AllGetXp
        {
            get { return _allGetXp; }
            set
            {
                if (value.Equals(_allGetXp))
                {
                    return;
                }
                _allGetXp = value;
                NotifyPropertyChanged(() => AllGetXp);
                float t = ((float)AllRunTime / 3600);
                if (t > 0) PerHourXp = (int)(value / t);
                else PerHourXp = 0;
                if (PerHourXp <= 300 || PerHourXp >= 700) PerHourXp = 500;
            }
        }

        [DefaultValue(500)]
        public int PerHourXp
        {
            get { return _perHourXp; }
            set
            {
                if (value.Equals(_perHourXp))
                {
                    return;
                }
                _perHourXp = value;
                PerHourXpStr = string.Format("{0}/小时", _perHourXp);
                NotifyPropertyChanged(() => PerHourXp);
            }
        }

        [DefaultValue("500/小时")]
        public string PerHourXpStr
        {
            get { return _perHourXpStr; }
            set
            {
                if (value.Equals(_perHourXpStr))
                {
                    return;
                }
                _perHourXpStr = value;
                NotifyPropertyChanged(() => PerHourXpStr);
            }
        }

        [DefaultValue(602200)]
        public int FullXpNeeded
        {
            get { return _fullXpNeeded; }
            set
            {
                if (value.Equals(_fullXpNeeded))
                {
                    return;
                }
                _fullXpNeeded = value;
                NotifyPropertyChanged(() => FullXpNeeded);
            }
        }

        [DefaultValue("未知")]
        public string FullTimeNeeded
        {
            get { return _fullTimeNeeded; }
            set
            {
                if (value.Equals(_fullTimeNeeded))
                {
                    return;
                }
                _fullTimeNeeded = value;
                NotifyPropertyChanged(() => FullTimeNeeded);
            }
        }

        [DefaultValue("未知")]
        public string Collection
        {
            get { return _collection; }
            set
            {
                if (value.Equals(_collection))
                {
                    return;
                }
                _collection = value;
                NotifyPropertyChanged(() => Collection);
            }
        }

        [DefaultValue("未知")]
        public string PassportEnd
        {
            get { return _passportEnd; }
            set
            {
                if (value.Equals(_passportEnd))
                {
                    return;
                }
                _passportEnd = value;
                NotifyPropertyChanged(() => PassportEnd);
            }
        }

        [DefaultValue("未知")]
        public string ClassicInfo
        {
            get { return _classicInfo; }
            set
            {
                if (value.Equals(_classicInfo))
                {
                    return;
                }
                _classicInfo = value;
                NotifyPropertyChanged(() => ClassicInfo);
            }
        }

        [DefaultValue("未知")]
        public string StandardInfo
        {
            get { return _standardInfo; }
            set
            {
                if (value.Equals(_standardInfo))
                {
                    return;
                }
                _standardInfo = value;
                NotifyPropertyChanged(() => StandardInfo);
            }
        }

        [DefaultValue("未知")]
        public string WildInfo
        {
            get { return _wildInfo; }
            set
            {
                if (value.Equals(_wildInfo))
                {
                    return;
                }
                _wildInfo = value;
                NotifyPropertyChanged(() => WildInfo);
            }
        }

        [DefaultValue("未知")]
        public string TwistInfo
        {
            get { return _twistInfo; }
            set
            {
                if (value.Equals(_twistInfo))
                {
                    return;
                }
                _twistInfo = value;
                NotifyPropertyChanged(() => TwistInfo);
            }
        }
    }
}
