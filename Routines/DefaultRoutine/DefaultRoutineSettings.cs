using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using log4net;
using Newtonsoft.Json;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Game.Mapping;
using Logger = Triton.Common.LogUtilities.Logger;

using Triton.Bot;
//using Triton.Common;
using Triton.Game;
using Triton.Game.Data;

namespace HREngine.Bots
{
    /// <summary>Settings for the DefaultRoutine. </summary>
    public class DefaultRoutineSettings : JsonSettings
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        private static DefaultRoutineSettings _instance;

        /// <summary>The current instance for this class. </summary>
        public static DefaultRoutineSettings Instance
        {
            get { return _instance ?? (_instance = new DefaultRoutineSettings()); }
        }

        /// <summary>The default ctor. Will use the settings path "DefaultRoutine".</summary>
        public DefaultRoutineSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "DefaultRoutine")))
        {

        }

        public void ReloadFile()
        {
            Reload(GetSettingsFilePath(Configuration.Instance.Name,
                string.Format("{0}.json", "DefaultRoutine" + GetMyHashCode())));
            if (CommandLine.Arguments.Exists("behavior"))
            {
                string[] name =
                {
                    "丨通用丨不设惩罚",
                    "丨通用丨暗牧",
                    "丨通用丨酸鱼人萨",
                    "丨标准丨酸快攻德",
                    "丨标准丨元素法",
                    "丨狂野丨奥秘法",
			        "丨狂野丨剑鱼贼",
			        "丨狂野丨偶数萨",
			        "丨狂野丨暗龙牧",
                    "丨狂野丨快攻暗牧",
                    "丨狂野丨锁喉剑鱼贼",
                    "丨过时丨任务海盗战",
                };
                DefaultBehavior = name[int.Parse(CommandLine.Arguments.Single("behavior"))];
                Log.ErrorFormat("[中控设置] 天梯对战策略 = {0}.", DefaultBehavior);
            }
        }

        private TAG_CLASS _arenaPreferredClass1;
        private TAG_CLASS _arenaPreferredClass2;
        private TAG_CLASS _arenaPreferredClass3;
        private TAG_CLASS _arenaPreferredClass4;
        private TAG_CLASS _arenaPreferredClass5;
        private string _defaultBehavior;

        /// <summary>
        /// The first hero choice for arena if present.
        /// </summary>
        [DefaultValue(TAG_CLASS.HUNTER)]
        public TAG_CLASS ArenaPreferredClass1
        {
            get { return _arenaPreferredClass1; }
            set
            {
                if (!value.Equals(_arenaPreferredClass1))
                {
                    _arenaPreferredClass1 = value;
                    NotifyPropertyChanged(() => ArenaPreferredClass1);
  
                }
                //Log.InfoFormat("[默认策略设置] 竞技场优先种族1 = {0}.", _arenaPreferredClass1);
            }
        }

        /// <summary>
        /// The second hero choice for arena if present.
        /// </summary>
        [DefaultValue(TAG_CLASS.WARLOCK)]
        public TAG_CLASS ArenaPreferredClass2
        {
            get { return _arenaPreferredClass2; }
            set
            {
                if (!value.Equals(_arenaPreferredClass2))
                {
                    _arenaPreferredClass2 = value;
                    NotifyPropertyChanged(() => ArenaPreferredClass2);
                }
                //Log.InfoFormat("[默认策略设置] 竞技场优先种族2 = {0}.", _arenaPreferredClass2);
            }
        }

        /// <summary>
        /// The third hero choice for arena if present.
        /// </summary>
        [DefaultValue(TAG_CLASS.PRIEST)]
        public TAG_CLASS ArenaPreferredClass3
        {
            get { return _arenaPreferredClass3; }
            set
            {
                if (!value.Equals(_arenaPreferredClass3))
                {
                    _arenaPreferredClass3 = value;
                    NotifyPropertyChanged(() => ArenaPreferredClass3);
                }
                //Log.InfoFormat("[默认策略设置] 竞技场优先种族3 = {0}.", _arenaPreferredClass3);
            }
        }

        /// <summary>
        /// The fourth hero choice for arena if present.
        /// </summary>
        [DefaultValue(TAG_CLASS.ROGUE)]
        public TAG_CLASS ArenaPreferredClass4
        {
            get { return _arenaPreferredClass4; }
            set
            {
                if (!value.Equals(_arenaPreferredClass4))
                {
                    _arenaPreferredClass4 = value;
                    NotifyPropertyChanged(() => ArenaPreferredClass4);
                }
                //Log.InfoFormat("[默认策略设置] 竞技场优先种族4 = {0}.", _arenaPreferredClass4);
            }
        }

        /// <summary>
        /// The fifth hero choice for arena if present.
        /// </summary>
        [DefaultValue(TAG_CLASS.WARRIOR)]
        public TAG_CLASS ArenaPreferredClass5
        {
            get { return _arenaPreferredClass5; }
            set
            {
                if (!value.Equals(_arenaPreferredClass5))
                {
                    _arenaPreferredClass5 = value;
                    NotifyPropertyChanged(() => ArenaPreferredClass5);
                }
                //Log.InfoFormat("[默认策略设置] 竞技场优先种族5 = {0}.", _arenaPreferredClass5);
            }
        }

        private ObservableCollection<TAG_CLASS> _allClasses;

        /// <summary>All enum values for this type.</summary>
        [JsonIgnore]
        public ObservableCollection<TAG_CLASS> AllClasses
        {
            get
            {
                return _allClasses ?? (_allClasses = new ObservableCollection<TAG_CLASS>
                {
                    TAG_CLASS.DRUID,
                    TAG_CLASS.HUNTER,
                    TAG_CLASS.MAGE,
                    TAG_CLASS.PALADIN,
                    TAG_CLASS.PRIEST,
                    TAG_CLASS.ROGUE,
                    TAG_CLASS.SHAMAN,
                    TAG_CLASS.WARLOCK,
                    TAG_CLASS.WARRIOR,
                });
            }
        }

        // Behavior choice.
        [DefaultValue("丨通用丨不设惩罚")]
        public string DefaultBehavior
        {
            get { return _defaultBehavior; }
            set
            {
                if (!value.Equals(_defaultBehavior))
                {
                    _defaultBehavior = value;
                    NotifyPropertyChanged(() => DefaultBehavior);
                }
                /*
                Log.InfoFormat("#############################################", _defaultBehavior);                
                Log.InfoFormat("#############################################", _defaultBehavior);                
                Log.InfoFormat(@"酸小明牛通");
                Log.InfoFormat("#############################################", _defaultBehavior);                
                Log.InfoFormat("#############################################", _defaultBehavior);                */
                Log.InfoFormat("[默认策略设置] 天梯对战策略 = {0}.", _defaultBehavior);                
            }
        }

        private ObservableCollection<string> _allBehav;

        /// <summary>All enum values for this type.</summary>
        [JsonIgnore]
        public ObservableCollection<string> AllBehav
        {
            get
            {
                return _allBehav ?? (_allBehav = new ObservableCollection<string>(Silverfish.Instance.BehaviorDB.Keys));
            }
        }



        // Emote choice.
        [DefaultValue("无")]
        public string DefaultEmote
        {
            get { return printUtils.emoteMode; }
            set
            {
                if (!value.Equals(printUtils.emoteMode))
                {
                    printUtils.emoteMode = value;
                    NotifyPropertyChanged(() => DefaultEmote);
                }
            }
        }

        private ObservableCollection<string> AllEmotes;

        /// <summary>All enum values for this type.</summary>
        [JsonIgnore]
        public ObservableCollection<string> AllEmote
        {
            get
            {
                return AllEmotes ?? (AllEmotes = new ObservableCollection<string>() { "无", "友善模式", "嘴臭模式", "乞讨模式", "摊牌了我是脚本", "精神污染模式", "抱歉", "主动投降" } );
            }
        }

        private readonly List<int> _questIdsToCancel = new List<int>();

		[JsonIgnore]
	    public List<int> QuestIdsToCancel
	    {
		    get { return _questIdsToCancel; }
	    }

        private int _maxWide;

        /// <summary>
        /// AI值.
        /// </summary>
        [DefaultValue(3000)]
        public int MaxWide
        {
            get { return Ai.Instance.maxwide; }
            set
            {
                if (!value.Equals(Ai.Instance.maxwide))
                {
                    Ai.Instance.setMaxWide(value);
                    NotifyPropertyChanged(() => MaxWide);
                }
                Log.InfoFormat("[默认策略设置] AI值 = {0}.", Ai.Instance.maxwide);
            }
        }


        /// <summary>
        /// 最大考虑步数
        /// </summary>
        [DefaultValue(12)]
        public int MaxDeep
        {
            get { return Ai.Instance.maxdeep; }
            set
            {
                if (!value.Equals(Ai.Instance.maxdeep))
                {
                    Ai.Instance.setMaxDeep(value);
                    NotifyPropertyChanged(() => MaxWide);
                }
                Log.InfoFormat("[默认策略设置] 最大考虑步数 = {0}.", Ai.Instance.maxdeep);
            }
        }


        /// <summary>
        /// 每步最大保留场面数
        /// </summary>
        [DefaultValue(60)]
        public int MaxCal
        {
            get { return Ai.Instance.maxCal; }
            set
            {
                if (!value.Equals(Ai.Instance.maxCal))
                {
                    Ai.Instance.setMaxCal(value);
                    NotifyPropertyChanged(() => MaxWide);
                }
                Log.InfoFormat("[默认策略设置] 每步最大保留场面数 = {0}.", Ai.Instance.maxCal);
            }
        }

        private bool _useSecretsPlayAround;

        /// <summary>
        /// 是否开启防奥秘.
        /// </summary>
        [DefaultValue(false)]
        public bool UseSecretsPlayAround
        {
            get { return _useSecretsPlayAround; }
            set
            {
                if (!value.Equals(_useSecretsPlayAround))
                {
                    _useSecretsPlayAround = value;
                    NotifyPropertyChanged(() => UseSecretsPlayAround);
  
                }
                Log.InfoFormat("[默认策略设置] 防奥秘 = {0}.", _useSecretsPlayAround);
            }
        }

        /// <summary>
        /// 是否开启生成日志.
        /// </summary>
        [DefaultValue(true)]
        public bool SetLog
        {
            get { return !Helpfunctions.Instance.writelogg; }
            set
            {
                if (!value.Equals(!Helpfunctions.Instance.writelogg))
                {
                    Helpfunctions.Instance.writelogg = !value;
                    NotifyPropertyChanged(() => Helpfunctions.Instance.writelogg);
                }
                Log.InfoFormat("[默认策略设置] 不生成对战日志 = {0}.", !Helpfunctions.Instance.writelogg);
            }
        }

        /// <summary>
        /// 是否打印出牌惩罚.
        /// </summary>
        [DefaultValue(false)]
        public bool UsePrintNextMove
        {
            get { return printUtils.printNextMove; }
            set
            {
                if (!value.Equals(printUtils.printNextMove))
                {
                    printUtils.printNextMove = value;
                    NotifyPropertyChanged(() => UsePrintPenalties);
                }
                Log.InfoFormat("[默认策略设置] 打印出牌惩罚 = {0}.", printUtils.printNextMove);
            }
        }

        /// <summary>
        /// 是否打印自定义惩罚.
        /// </summary>
        [DefaultValue(false)]
        public bool UsePrintPenalties
        {
            get { return printUtils.printPentity; }
            set
            {
                if (!value.Equals(printUtils.printPentity))
                {
                    printUtils.printPentity = value;
                    NotifyPropertyChanged(() => UsePrintPenalties);
                }
                Log.InfoFormat("[默认策略设置] 是否打印自定义惩罚 = {0}.", printUtils.printPentity);
            }
        }


        private bool _berserkIfCanFinishNextTour;

        /// <summary>
        /// 是否开启下回合斩杀本回合打脸.
        /// </summary>
        [DefaultValue(false)]
        public bool BerserkIfCanFinishNextTour
        {
            get { return _berserkIfCanFinishNextTour; }
            set
            {
                if (!value.Equals(_berserkIfCanFinishNextTour))
                {
                    _berserkIfCanFinishNextTour = value;
                    NotifyPropertyChanged(() => BerserkIfCanFinishNextTour);
  
                }
                Log.InfoFormat("[默认策略设置] 下回合斩杀本回合打脸 = {0}.", _berserkIfCanFinishNextTour);
            }
        }


        private int _enfacehp;
        /// <summary>
        /// 打脸阈值.
        /// </summary>
        [DefaultValue(27)]
        public int Enfacehp
        {
            get { return _enfacehp; }
            set
            {
                if (!value.Equals(_enfacehp))
                {
                    _enfacehp = value;
                    NotifyPropertyChanged(() => Enfacehp);
  
                }
                Log.InfoFormat("[默认策略设置] 打脸阈值 = {0}.", _enfacehp);
            }
        }


        /// <summary>
        /// 打脸奖励.
        /// </summary>
        [DefaultValue(0)]
        public int EnfaceReward
        {
            get { return printUtils.enfaceReward; }
            set
            {
                if (!value.Equals(printUtils.enfaceReward))
                {
                    printUtils.enfaceReward = value;
                    NotifyPropertyChanged(() => Enfacehp);
  
                }
                Log.InfoFormat("[默认策略设置] 打脸奖励 = {0}.", printUtils.enfaceReward);
            }
        }


    }
}
