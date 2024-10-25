using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using log4net;
using Newtonsoft.Json;
using Triton.Bot.Settings;
using Triton.Common;
using Logger = Triton.Common.LogUtilities.Logger;

namespace Stats
{
    /// <summary>Settings for the Stats plugin. </summary>
    public class StatsSettings : JsonSettings
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        private static StatsSettings _instance;

        /// <summary>The current instance for this class. </summary>
        public static StatsSettings Instance
        {
            get { return _instance ?? (_instance = new StatsSettings()); }
        }

        /// <summary>The default ctor. Will use the settings path "Stats".</summary>
        public StatsSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Stats")))
        {
        }

        public void ReloadFile()
        {
            Reload(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "Stats" + GetMyHashCode())));
        }

        private int _wins;
		private int _wins1;
		private int _wins2;
		private int _wins3;
		private int _wins4;
		private int _wins5;
		private int _wins6;
		private int _wins7;
		private int _wins8;
		private int _wins9;
		private int _wins10;
        private int _wins11;
        private int _losses;
        private int _losses1;
        private int _losses2;
        private int _losses3;
        private int _losses4;
        private int _losses5;
        private int _losses6;
        private int _losses7;
        private int _losses8;
        private int _losses9;
		private int _losses10;
        private int _losses11;
        private string _winrate;
		private string _winrate1;
		private string _winrate2;
		private string _winrate3;
		private string _winrate4;
		private string _winrate5;
		private string _winrate6;
		private string _winrate7;
		private string _winrate8;
		private string _winrate9;
		private string _winrate10;
        private string _winrate11;
        private string _environment;
        private string _environment1;
        private string _environment2;
        private string _environment3;
        private string _environment4;
        private string _environment5;
        private string _environment6;
        private string _environment7;
        private string _environment8;
        private string _environment9;
        private string _environment10;
        private string _environment11;

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
		
        [DefaultValue(0)]
        public int Wins1
        {
            get { return _wins1; }
            set
            {
                if (value.Equals(_wins1))
                {
                    return;
                }
                _wins1 = value;
                NotifyPropertyChanged(() => Wins1);
            }
        }
        [DefaultValue(0)]
        public int Wins2
        {
            get { return _wins2; }
            set
            {
                if (value.Equals(_wins2))
                {
                    return;
                }
                _wins2 = value;
                NotifyPropertyChanged(() => Wins2);
            }
        }
        [DefaultValue(0)]
        public int Wins3
        {
            get { return _wins3; }
            set
            {
                if (value.Equals(_wins3))
                {
                    return;
                }
                _wins3 = value;
                NotifyPropertyChanged(() => Wins3);
            }
        }
        [DefaultValue(0)]
        public int Wins4
        {
            get { return _wins4; }
            set
            {
                if (value.Equals(_wins4))
                {
                    return;
                }
                _wins4 = value;
                NotifyPropertyChanged(() => Wins4);
            }
        }
        [DefaultValue(0)]
        public int Wins5
        {
            get { return _wins5; }
            set
            {
                if (value.Equals(_wins5))
                {
                    return;
                }
                _wins5 = value;
                NotifyPropertyChanged(() => Wins5);
            }
        }
        [DefaultValue(0)]
        public int Wins6
        {
            get { return _wins6; }
            set
            {
                if (value.Equals(_wins6))
                {
                    return;
                }
                _wins6 = value;
                NotifyPropertyChanged(() => Wins6);
            }
        }
        [DefaultValue(0)]
        public int Wins7
        {
            get { return _wins7; }
            set
            {
                if (value.Equals(_wins7))
                {
                    return;
                }
                _wins7 = value;
                NotifyPropertyChanged(() => Wins7);
            }
        }
        [DefaultValue(0)]
        public int Wins8
        {
            get { return _wins8; }
            set
            {
                if (value.Equals(_wins8))
                {
                    return;
                }
                _wins8 = value;
                NotifyPropertyChanged(() => Wins8);
            }
        }
        [DefaultValue(0)]
        public int Wins9
        {
            get { return _wins9; }
            set
            {
                if (value.Equals(_wins9))
                {
                    return;
                }
                _wins9 = value;
                NotifyPropertyChanged(() => Wins9);
            }
        }
        [DefaultValue(0)]
        public int Wins10
        {
            get { return _wins10; }
            set
            {
                if (value.Equals(_wins10))
                {
                    return;
                }
                _wins10 = value;
                NotifyPropertyChanged(() => Wins10);
            }
        }
        [DefaultValue(0)]
        public int Wins11
        {
            get { return _wins11; }
            set
            {
                if (value.Equals(_wins11))
                {
                    return;
                }
                _wins11 = value;
                NotifyPropertyChanged(() => Wins11);
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
		
        [DefaultValue(0)]
        public int Losses1
        {
            get { return _losses1; }
            set
            {
                if (value.Equals(_losses1))
                {
                    return;
                }
                _losses1 = value;
                NotifyPropertyChanged(() => Losses1);
            }
        }
        [DefaultValue(0)]
        public int Losses2
        {
            get { return _losses2; }
            set
            {
                if (value.Equals(_losses2))
                {
                    return;
                }
                _losses2 = value;
                NotifyPropertyChanged(() => Losses2);
            }
        }
        [DefaultValue(0)]
        public int Losses3
        {
            get { return _losses3; }
            set
            {
                if (value.Equals(_losses3))
                {
                    return;
                }
                _losses3 = value;
                NotifyPropertyChanged(() => Losses3);
            }
        }
        [DefaultValue(0)]
        public int Losses4
        {
            get { return _losses4; }
            set
            {
                if (value.Equals(_losses4))
                {
                    return;
                }
                _losses4 = value;
                NotifyPropertyChanged(() => Losses4);
            }
        }
        [DefaultValue(0)]
        public int Losses5
        {
            get { return _losses5; }
            set
            {
                if (value.Equals(_losses5))
                {
                    return;
                }
                _losses5 = value;
                NotifyPropertyChanged(() => Losses5);
            }
        }
        [DefaultValue(0)]
        public int Losses6
        {
            get { return _losses6; }
            set
            {
                if (value.Equals(_losses6))
                {
                    return;
                }
                _losses6 = value;
                NotifyPropertyChanged(() => Losses6);
            }
        }
        [DefaultValue(0)]
        public int Losses7
        {
            get { return _losses7; }
            set
            {
                if (value.Equals(_losses7))
                {
                    return;
                }
                _losses7 = value;
                NotifyPropertyChanged(() => Losses7);
            }
        }
        [DefaultValue(0)]
        public int Losses8
        {
            get { return _losses8; }
            set
            {
                if (value.Equals(_losses8))
                {
                    return;
                }
                _losses8 = value;
                NotifyPropertyChanged(() => Losses8);
            }
        }
        [DefaultValue(0)]
        public int Losses9
        {
            get { return _losses9; }
            set
            {
                if (value.Equals(_losses9))
                {
                    return;
                }
                _losses9 = value;
                NotifyPropertyChanged(() => Losses9);
            }
        }	
        [DefaultValue(0)]
        public int Losses10
        {
            get { return _losses10; }
            set
            {
                if (value.Equals(_losses10))
                {
                    return;
                }
                _losses10 = value;
                NotifyPropertyChanged(() => Losses10);
            }
        }
        [DefaultValue(0)]
        public int Losses11
        {
            get { return _losses11; }
            set
            {
                if (value.Equals(_losses11))
                {
                    return;
                }
                _losses11 = value;
                NotifyPropertyChanged(() => Losses11);
            }
        }

        /// <summary>Current  Winrate.</summary>
        [DefaultValue("0")]
        public string Winrate
        {
            get { return _winrate; }
            set
            {
                if (value.Equals(_winrate))
                {
                    return;
                }
                _winrate = value;
                NotifyPropertyChanged(() => Winrate);
            }
        }
		[DefaultValue("0")]
        public string Winrate1
        {
            get { return _winrate1; }
            set
            {
                if (value.Equals(_winrate1))
                {
                    return;
                }
                _winrate1 = value;
                NotifyPropertyChanged(() => Winrate1);
            }
        }
		[DefaultValue("0")]
        public string Winrate2
        {
            get { return _winrate2; }
            set
            {
                if (value.Equals(_winrate2))
                {
                    return;
                }
                _winrate2 = value;
                NotifyPropertyChanged(() => Winrate2);
            }
        }
		[DefaultValue("0")]
        public string Winrate3
        {
            get { return _winrate3; }
            set
            {
                if (value.Equals(_winrate3))
                {
                    return;
                }
                _winrate3 = value;
                NotifyPropertyChanged(() => Winrate3);
            }
        }
		[DefaultValue("0")]
        public string Winrate4
        {
            get { return _winrate4; }
            set
            {
                if (value.Equals(_winrate4))
                {
                    return;
                }
                _winrate4 = value;
                NotifyPropertyChanged(() => Winrate4);
            }
        }
		[DefaultValue("0")]
        public string Winrate5
        {
            get { return _winrate5; }
            set
            {
                if (value.Equals(_winrate5))
                {
                    return;
                }
                _winrate5 = value;
                NotifyPropertyChanged(() => Winrate5);
            }
        }
		[DefaultValue("0")]
        public string Winrate6
        {
            get { return _winrate6; }
            set
            {
                if (value.Equals(_winrate6))
                {
                    return;
                }
                _winrate6 = value;
                NotifyPropertyChanged(() => Winrate6);
            }
        }
		[DefaultValue("0")]
        public string Winrate7
        {
            get { return _winrate7; }
            set
            {
                if (value.Equals(_winrate7))
                {
                    return;
                }
                _winrate7 = value;
                NotifyPropertyChanged(() => Winrate7);
            }
        }
		[DefaultValue("0")]
        public string Winrate8
        {
            get { return _winrate8; }
            set
            {
                if (value.Equals(_winrate8))
                {
                    return;
                }
                _winrate8 = value;
                NotifyPropertyChanged(() => Winrate8);
            }
        }
		[DefaultValue("0")]
        public string Winrate9
        {
            get { return _winrate9; }
            set
            {
                if (value.Equals(_winrate9))
                {
                    return;
                }
                _winrate9 = value;
                NotifyPropertyChanged(() => Winrate9);
            }
        }
		[DefaultValue("0")]
        public string Winrate10
        {
            get { return _winrate10; }
            set
            {
                if (value.Equals(_winrate10))
                {
                    return;
                }
                _winrate10 = value;
                NotifyPropertyChanged(() => Winrate10);
            }
        }
        [DefaultValue("0")]
        public string Winrate11
        {
            get { return _winrate11; }
            set
            {
                if (value.Equals(_winrate11))
                {
                    return;
                }
                _winrate11 = value;
                NotifyPropertyChanged(() => Winrate11);
            }
        }

        [DefaultValue("0")]
        public string environment
        {
            get { return _environment; }
            set
            {
                if (value.Equals(_environment))
                {
                    return;
                }
                _environment = value;
                NotifyPropertyChanged(() => environment);
            }
        }
        [DefaultValue("0")]
        public string environment1
        {
            get { return _environment1; }
            set
            {
                if (value.Equals(_environment1))
                {
                    return;
                }
                _environment1 = value;
                NotifyPropertyChanged(() => environment1);
            }
        }
        [DefaultValue("0")]
        public string environment2
        {
            get { return _environment2; }
            set
            {
                if (value.Equals(_environment2))
                {
                    return;
                }
                _environment2 = value;
                NotifyPropertyChanged(() => environment2);
            }
        }
        [DefaultValue("0")]
        public string environment3
        {
            get { return _environment3; }
            set
            {
                if (value.Equals(_environment3))
                {
                    return;
                }
                _environment3 = value;
                NotifyPropertyChanged(() => environment3);
            }
        }
        [DefaultValue("0")]
        public string environment4
        {
            get { return _environment4; }
            set
            {
                if (value.Equals(_environment4))
                {
                    return;
                }
                _environment4 = value;
                NotifyPropertyChanged(() => environment4);
            }
        }
        [DefaultValue("0")]
        public string environment5
        {
            get { return _environment5; }
            set
            {
                if (value.Equals(_environment5))
                {
                    return;
                }
                _environment5 = value;
                NotifyPropertyChanged(() => environment5);
            }
        }
        [DefaultValue("0")]
        public string environment6
        {
            get { return _environment6; }
            set
            {
                if (value.Equals(_environment6))
                {
                    return;
                }
                _environment6 = value;
                NotifyPropertyChanged(() => environment6);
            }
        }
        [DefaultValue("0")]
        public string environment7
        {
            get { return _environment7; }
            set
            {
                if (value.Equals(_environment7))
                {
                    return;
                }
                _environment7 = value;
                NotifyPropertyChanged(() => environment7);
            }
        }
        [DefaultValue("0")]
        public string environment8
        {
            get { return _environment8; }
            set
            {
                if (value.Equals(_environment8))
                {
                    return;
                }
                _environment8 = value;
                NotifyPropertyChanged(() => environment8);
            }
        }
        [DefaultValue("0")]
        public string environment9
        {
            get { return _environment9; }
            set
            {
                if (value.Equals(_environment9))
                {
                    return;
                }
                _environment9 = value;
                NotifyPropertyChanged(() => environment9);
            }
        }
        [DefaultValue("0")]
        public string environment10
        {
            get { return _environment10; }
            set
            {
                if (value.Equals(_environment10))
                {
                    return;
                }
                _environment10 = value;
                NotifyPropertyChanged(() => environment10);
            }
        }
        [DefaultValue("0")]
        public string environment11
        {
            get { return _environment11; }
            set
            {
                if (value.Equals(_environment11))
                {
                    return;
                }
                _environment11 = value;
                NotifyPropertyChanged(() => environment11);
            }
        }
    }
}
