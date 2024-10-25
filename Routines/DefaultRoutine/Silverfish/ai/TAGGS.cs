namespace HREngine.Bots
{

    public enum searchmode
    {
        searchLowestHP,
        searchHighestHP,
        searchLowestAttack,
        searchHighestAttack,
        searchHighAttackLowHP, //get max attack/hp ratio
        searchHighHPLowAttack, //get max hp/attack ratio
        searchLowestCost,
        searchHighesCost,
    }
    
    public enum GAME_TAGs
    {
        TAG_SCRIPT_DATA_NUM_1 = 2,  // 脚本数据编号1
        TAG_SCRIPT_DATA_NUM_2 = 3,  // 脚本数据编号2
        TAG_SCRIPT_DATA_ENT_1 = 4,  // 脚本数据实体1
        TAG_SCRIPT_DATA_ENT_2 = 5,  // 脚本数据实体2
        MISSION_EVENT = 6,          // 任务事件
        TIMEOUT = 7,                // 超时
        TURN_START = 8,             // 回合开始
        TURN_TIMER_SLUSH = 9,       // 回合计时器调节
        PREMIUM = 12,               // 高级卡（如金卡）
        GOLD_REWARD_STATE = 13,     // 金币奖励状态
        PLAYSTATE = 17,             // 游戏状态
        LAST_AFFECTED_BY = 18,      // 最后影响该卡牌的实体
        STEP = 19,                  // 游戏步骤
        TURN = 20,                  // 当前回合数
        FATIGUE = 22,               // 疲劳
        CURRENT_PLAYER = 23,        // 当前玩家
        FIRST_PLAYER = 24,          // 先手玩家
        RESOURCES_USED = 25,        // 使用的资源数
        RESOURCES = 26,             // 当前可用资源数
        HERO_ENTITY = 27,           // 英雄实体ID
        MAXHANDSIZE = 28,           // 最大手牌数量
        STARTHANDSIZE = 29,         // 起始手牌数量
        PLAYER_ID = 30,             // 玩家ID
        TEAM_ID = 31,               // 队伍ID
        TRIGGER_VISUAL = 32,        // 触发视觉效果
        RECENTLY_ARRIVED = 33,      // 最近到达
        PROTECTING = 34,            // 正在保护
        PROTECTED = 35,             // 被保护
        DEFENDING = 36,             // 防御中
        PROPOSED_DEFENDER = 37,     // 提议的防御者
        ATTACKING = 38,             // 进攻中
        PROPOSED_ATTACKER = 39,     // 提议的进攻者
        ATTACHED = 40,              // 已附着
        EXHAUSTED = 43,             // 精疲力尽（不能攻击）
        DAMAGE = 44,                // 伤害
        HEALTH = 45,                // 生命值
        ATK = 47,                   // 攻击力
        COST = 48,                  // 费用
        ZONE = 49,                  // 所在区域（手牌、牌库、战场等）
        CONTROLLER = 50,            // 控制者
        OWNER = 51,                 // 拥有者
        DEFINITION = 52,            // 定义
        ENTITY_ID = 53,             // 实体ID
        HISTORY_PROXY = 54,         // 历史代理
        COPY_DEATHRATTLE = 55,      // 复制亡语
        ELITE = 114,                // 精英卡
        MAXRESOURCES = 176,         // 最大资源数
        CARD_SET = 183,             // 卡牌所属的扩展包
        CARDTEXT_INHAND = 184,      // 手牌中的卡牌文字
        CARDNAME = 185,             // 卡牌名称
        CARD_ID = 186,              // 卡牌ID
        DURABILITY = 187,           // 武器耐久度
        SILENCED = 188,             // 被沉默
        WINDFURY = 189,             // 风怒
        TAUNT = 190,                // 嘲讽
        STEALTH = 191,              // 潜行
        SPELLPOWER = 192,           // 法术伤害
        DIVINE_SHIELD = 194,        // 圣盾
        CHARGE = 197,               // 冲锋
        NEXT_STEP = 198,            // 下一步
        CLASS = 199,                // 职业
        CARDRACE = 200,             // 卡牌种族
        FACTION = 201,              // 阵营
        CARDTYPE = 202,             // 卡牌类型（随从、法术、武器等）
        RARITY = 203,               // 稀有度
        STATE = 204,                // 状态
        SUMMONED = 205,             // 被召唤
        FREEZE = 208,               // 冰冻
        ENRAGED = 212,              // 激怒
        OVERLOAD = 215,             // 过载
        LOYALTY = 216,              // 忠诚
        DEATHRATTLE = 217,          // 亡语
        BATTLECRY = 218,            // 战吼
        SECRET = 219,               // 奥秘
        NATURE = 643,               // 自然属性
        COMBO = 220,                // 连击
        CANT_HEAL = 221,            // 不能被治疗
        CANT_DAMAGE = 222,          // 不能造成伤害
        CANT_SET_ASIDE = 223,       // 不能被移开
        CANT_REMOVE_FROM_GAME = 224,// 不能移出游戏
        CANT_READY = 225,           // 不能准备就绪
        CANT_ATTACK = 227,          // 不能攻击
        CANT_DISCARD = 230,         // 不能弃牌
        CANT_PLAY = 231,            // 不能使用
        CANT_DRAW = 232,            // 不能抽牌
        CANT_BE_HEALED = 239,       // 不能被治疗
        IMMUNE = 240,               // 免疫
        CANT_BE_SET_ASIDE = 241,    // 不能被移开
        CANT_BE_REMOVED_FROM_GAME = 242,// 不能移出游戏
        CANT_BE_READIED = 243,      // 不能准备
        CANT_BE_ATTACKED = 245,     // 不能被攻击
        CANT_BE_TARGETED = 246,     // 不能成为目标
        CANT_BE_DESTROYED = 247,    // 不能被摧毁
        CANT_BE_SUMMONING_SICK = 253,// 不能进入召唤疲劳状态
        FROZEN = 260,               // 冰冻状态
        JUST_PLAYED = 261,          // 刚刚被使用
        LINKED_ENTITY = 262,        // 链接的实体
        ZONE_POSITION = 263,        // 区域位置
        CANT_BE_FROZEN = 264,       // 不能被冰冻
        COMBO_ACTIVE = 266,         // 连击激活
        CARD_TARGET = 267,          // 卡牌目标
        NUM_CARDS_PLAYED_THIS_TURN = 269,// 本回合已使用的卡牌数
        CANT_BE_TARGETED_BY_OPPONENTS = 270,// 不能成为对手的目标
        NUM_TURNS_IN_PLAY = 271,    // 已经在场上的回合数
        NUM_TURNS_LEFT = 272,       // 剩余回合数
        CURRENT_SPELLPOWER = 291,   // 当前法术伤害值
        ARMOR = 292,                // 护甲值
        MORPH = 293,                // 变形
        IS_MORPHED = 294,           // 是否已变形
        TEMP_RESOURCES = 295,       // 临时资源
        OVERLOAD_OWED = 296,        // 过载欠债
        NUM_ATTACKS_THIS_TURN = 297,// 本回合攻击次数
        NEXT_ALLY_BUFF = 302,       // 下一个友方随从增强
        MAGNET = 303,               // 磁力
        FIRST_CARD_PLAYED_THIS_TURN = 304,// 本回合首次使用的卡牌
        MULLIGAN_STATE = 305,       // 换牌阶段状态
        TAUNT_READY = 306,          // 嘲讽就绪
        STEALTH_READY = 307,        // 潜行就绪
        CHARGE_READY = 308,         // 冲锋就绪
        CANT_BE_TARGETED_BY_SPELLS = 311,// 不能成为法术目标
        SHOULDEXITCOMBAT = 312,     // 应退出战斗
        CREATOR = 313,              // 创建者
        CANT_BE_SILENCED = 314,     // 不能被沉默
        PARENT_CARD = 316,          // 父卡牌
        NUM_MINIONS_PLAYED_THIS_TURN = 317,// 本回合已使用的随从数
        PREDAMAGE = 318,// 预伤害：在计算实际伤害前的预先伤害值标识。
        COLLECTIBLE = 321,// 可收集：表示该卡牌是否可以被玩家收集。
        TARGETING_ARROW_TEXT = 325,// 瞄准箭头文本：用于描述瞄准箭头的文本内容。
        ENCHANTMENT_BIRTH_VISUAL = 330,// 附魔出生视觉效果：在附魔生效时显示的视觉效果。
        ENCHANTMENT_IDLE_VISUAL = 331,// 附魔闲置视觉效果：在附魔处于闲置状态时显示的视觉效果。
        CANT_BE_TARGETED_BY_HERO_POWERS = 332,// 不能被英雄技能选中：该卡牌不能作为英雄技能的目标。
        HEALTH_MINIMUM = 337,// 最低生命值：卡牌所能拥有的最低生命值。
        TAG_ONE_TURN_EFFECT = 338,// 单回合效果标识：用于标记只能在单回合内生效的效果。
        SILENCE = 339,// 沉默：用于表示卡牌是否处于沉默状态，效果将被取消。
        COUNTER = 340,// 计数器：标识卡牌上是否有计数器（用于追踪效果次数等）。
        ARTISTNAME = 342,// 艺术家名称：卡牌的艺术插画作者的名字。
        ZONES_REVEALED = 348,// 区域已揭示：表示卡牌的可见性，是否已被对方揭示。
        ADJACENT_BUFF = 350,// 相邻增益：该标识符用于表示相邻随从的增益效果。
        FLAVORTEXT = 351,// 趣味文本：用于显示卡牌的背景故事或趣味描述。
        FORCED_PLAY = 352,// 强制出牌：标识该卡牌在某些情况下会被强制使用。
        LOW_HEALTH_THRESHOLD = 353,// 低生命值阈值：用于触发在低生命值状态下的效果。
        SPELLPOWER_DOUBLE = 356,// 法术强度加倍：该卡牌的法术强度效果加倍。
        HEALING_DOUBLE = 357,// 治疗效果加倍：该卡牌的治疗效果加倍。
        NUM_OPTIONS_PLAYED_THIS_TURN = 358,// 本回合已使用选项数：用于记录当前回合内已使用的不同选项的数量。
        TO_BE_DESTROYED = 360,// 即将被摧毁：标记该卡牌将在某些效果下被摧毁。
        AURA = 362,// 光环：该标识符用于表示卡牌的光环效果，影响其他单位。
        POISONOUS = 363,// 剧毒：表示该随从能够消灭任何受到其伤害的随从。
        HOW_TO_EARN = 364,// 获取方式：描述如何获得该卡牌。
        HOW_TO_EARN_GOLDEN = 365,// 获取金色版本方式：描述如何获得该卡牌的金色版本。
        HERO_POWER_DOUBLE = 366,// 英雄技能加倍：该卡牌的英雄技能效果加倍。
        AI_MUST_PLAY = 367,// AI必须出牌：标记AI在某些情况下必须使用此卡牌。
        NUM_MINIONS_PLAYER_KILLED_THIS_TURN = 368,// 本回合玩家消灭的随从数量：用于记录玩家在当前回合消灭的随从数量。
        NUM_MINIONS_KILLED_THIS_TURN = 369,// 本回合消灭的随从数量：用于记录当前回合内总共消灭的随从数量。
        AFFECTED_BY_SPELL_POWER = 370,// 受法术强度影响：标识该卡牌是否受法术强度加成的影响。
        EXTRA_DEATHRATTLES = 371,// 额外亡语：该标识符表示该卡牌可以触发额外的亡语效果。
        START_WITH_1_HEALTH = 372,// 以1点生命值开始：表示该卡牌以1点生命值开始游戏。
        IMMUNE_WHILE_ATTACKING = 373,// 攻击时免疫：该卡牌在攻击时免疫所有伤害。
        MULTIPLY_HERO_DAMAGE = 374,// 英雄伤害倍增：该卡牌会倍增英雄造成的伤害。
        MULTIPLY_BUFF_VALUE = 375,// 增益效果倍增：该标识符表示该卡牌会倍增获得的增益效果。
        CUSTOM_KEYWORD_EFFECT = 376,// 自定义关键词效果：表示该卡牌具有自定义的关键词效果。
        TOPDECK = 377,// 顶牌：该标识符用于标记抽取卡组顶部的卡牌。
        CANT_BE_TARGETED_BY_BATTLECRIES = 379,// 不能成为战吼目标：该卡牌不能被战吼效果选中。
        HERO_POWER = 380,// 英雄技能：标识与英雄技能相关的效果。
        DEATHRATTLE_RETURN_ZONE = 382,// 亡语返回区域：亡语效果触发后返回的区域标识。
        STEADY_SHOT_CAN_TARGET = 383,// 稳固射击可选目标：该标识符表示稳固射击技能的目标可选性。
        DISPLAYED_CREATOR = 385,// 显示创建者：显示该卡牌的创建者信息。
        POWERED_UP = 386,// 强化：标识该卡牌处于强化状态。
        SPARE_PART = 388,// 备用零件：标识该卡牌为备用零件类型。
        FORGETFUL = 389,// 健忘：该标识符表示卡牌会有随机的效果。
        CAN_SUMMON_MAXPLUSONE_MINION = 390,// 能召唤最大值+1的随从：该标识符表示能召唤比最大数量多一个随从。
        OBFUSCATED = 391,// 模糊：该标识符表示卡牌效果的模糊性或不可见性。
        BURNING = 392,// 燃烧：标识该卡牌处于燃烧状态，将受到持续伤害。
        OVERLOAD_LOCKED = 393,// 过载锁定：该标识符表示卡牌由于过载效果被锁定。
        NUM_TIMES_HERO_POWER_USED_THIS_GAME = 394,// 本局游戏中使用英雄技能的次数：记录游戏过程中使用英雄技能的总次数。
        CURRENT_HEROPOWER_DAMAGE_BONUS = 395,// 当前英雄技能伤害加成：表示当前英雄技能的伤害加成值。
        HEROPOWER_DAMAGE = 396,// 英雄技能伤害：表示英雄技能的基础伤害值。
        LAST_CARD_PLAYED = 397,// 上一张使用的卡牌：标记玩家上次使用的卡牌。
        NUM_FRIENDLY_MINIONS_THAT_DIED_THIS_TURN = 398,// 本回合死亡的友方随从数量：记录当前回合内死亡的友方随从数量。
        NUM_CARDS_DRAWN_THIS_TURN = 399,// 本回合抽取的卡牌数量：记录当前回合内抽取的卡牌数量。
        AI_ONE_SHOT_KILL = 400,// AI一击必杀：标识AI能够使用该卡牌进行一击必杀。
        EVIL_GLOW = 401,// 邪恶光环：该标识符表示卡牌具有邪恶光环效果。
        HIDE_STATS = 402,// 隐藏状态：该卡牌的具体状态（如攻击力和生命值）会被隐藏。
        INSPIRE = 403,// 激励：标识该卡牌具有激励效果。
        RECEIVES_DOUBLE_SPELLDAMAGE_BONUS = 404,// 接受双倍法术伤害加成：该卡牌会接受双倍的法术伤害加成。
        HEROPOWER_ADDITIONAL_ACTIVATIONS = 405,// 英雄技能额外激活：该卡牌能够在一回合内多次激活英雄技能。
        HEROPOWER_ACTIVATIONS_THIS_TURN = 406,// 本回合激活英雄技能的次数：记录当前回合内激活英雄技能的次数。
        REVEALED = 410,// 已揭示：表示卡牌是否已被揭示给对手。
        NUM_FRIENDLY_MINIONS_THAT_DIED_THIS_GAME = 412,// 本局游戏死亡的友方随从数量：记录本局游戏中死亡的友方随从数量。
        CANNOT_ATTACK_HEROES = 413,// 不能攻击英雄：该卡牌不能直接攻击对方英雄。
        LOCK_AND_LOAD = 414,// 备战待命：该标识符表示卡牌具有备战待命的效果。
        DISCOVER = 415,// 发现：标识该卡牌具有“发现”效果。
        SHADOWFORM = 416,// 暗影形态：标识该卡牌具有暗影形态效果。
        NUM_FRIENDLY_MINIONS_THAT_ATTACKED_THIS_TURN = 417,// 本回合攻击的友方随从数量：记录当前回合内
        NUM_RESOURCES_SPENT_THIS_GAME = 418,
        CHOOSE_BOTH = 419,
        ELECTRIC_CHARGE_LEVEL = 420,
        HEAVILY_ARMORED = 421,
        DONT_SHOW_IMMUNE = 422,
        RITUAL = 424,
        PREHEALING = 425,
        APPEAR_FUNCTIONALLY_DEAD = 426,
        OVERLOAD_THIS_GAME = 427,
        SPELLS_COST_HEALTH = 431,
        HISTORY_PROXY_NO_BIG_CARD = 432,
        PROXY_CTHUN = 434,
        TRANSFORMED_FROM_CARD = 435,
        CTHUN = 436,
        CAST_RANDOM_SPELLS = 437,
        SHIFTING = 438,
        JADE_GOLEM = 441,
        EMBRACE_THE_SHADOW = 442,
        CHOOSE_ONE = 443,
        EXTRA_ATTACKS_THIS_TURN = 444,
        SEEN_CTHUN = 445,
        MINION_TYPE_REFERENCE = 447,
        UNTOUCHABLE = 448,
        RED_MANA_CRYSTALS = 449,
        SCORE_LABELID_1 = 450,
        SCORE_VALUE_1 = 451,
        SCORE_LABELID_2 = 452,
        SCORE_VALUE_2 = 453,
        SCORE_LABELID_3 = 454,
        SCORE_VALUE_3 = 455,
        CANT_BE_FATIGUED = 456,
        AUTOATTACK = 457,
        ARMS_DEALING = 458,
        PENDING_EVOLUTIONS = 461,
        QUEST = 462,
        TAG_LAST_KNOWN_COST_IN_HAND = 466,
        DEFINING_ENCHANTMENT = 469,
        FINISH_ATTACK_SPELL_ON_DAMAGE = 470,
        MODULAR_ENTITY_PART_1 = 471,
        MODULAR_ENTITY_PART_2 = 472,
        MODIFY_DEFINITION_ATTACK = 473,
        MODIFY_DEFINITION_HEALTH = 474,
        MODIFY_DEFINITION_COST = 475,
        MULTIPLE_CLASSES = 476,
        ALL_TARGETS_RANDOM = 477,
        MULTI_CLASS_GROUP = 480,
        CARD_COSTS_HEALTH = 481,
        GRIMY_GOONS = 482,
        JADE_LOTUS = 483,
        KABAL = 484,
        ADDITIONAL_PLAY_REQS_1 = 515,
        ADDITIONAL_PLAY_REQS_2 = 516,
        ELEMENTAL_POWERED_UP = 532,
        QUEST_PROGRESS = 534,
        QUEST_PROGRESS_TOTAL = 535,
        QUEST_CONTRIBUTOR = 541,
        ADAPT = 546,
        IS_CURRENT_TURN_AN_EXTRA_TURN = 547,
        EXTRA_TURNS_TAKEN_THIS_GAME = 548,
        SHIFTING_MINION = 549,
        SHIFTING_WEAPON = 550,
        DEATH_KNIGHT = 554,
        BOSS = 556,
        STAMPEDE = 564,
        IS_VAMPIRE = 680,
        CORRUPTED = 681,
        LIFESTEAL = 685,
        OVERRIDE_EMOTE_0 = 740,
        OVERRIDE_EMOTE_1 = 741,
        OVERRIDE_EMOTE_2 = 742,
        OVERRIDE_EMOTE_3 = 743,
        OVERRIDE_EMOTE_4 = 744,
        OVERRIDE_EMOTE_5 = 745,
        SCORE_FOOTERID = 751,
        HERO_POWER_DISABLED = 777,
        VALEERASHADOW = 779,
        OVERRIDECARDNAME = 781,
        OVERRIDECARDTEXTBUILDER = 782,
        HIDDEN_CHOICE = 813,
        ZOMBEAST = 823,
        HERO_EMOTE_SILENCED = 832,
        MODULAR = 849,
        GLORIOUSGLOOP = 1044,
        REBORN = 1085,
        HAS_BEEN_REBORN = 1336,
        POISONOUS_INSTANT = 1528,
        REPLACEMENT_ENTITY = 1632,
        SPELLS_CAST_TWICE = 1681,
        DONT_PICK_FROM_SUBSETS = 676,
        LUNAHIGHLIGHTHINT = 1054,
        EMPOWER = 1263,
        EMPOWER_PRIEST,
        EMPOWER_ROGUE,
        EMPOWER_SHAMAN,
        EMPOWER_WARLOCK,
        EMPOWER_WARRIOR,
        GOLDSPARKLES_HINT = 1984,
        CASTS_WHEN_DRAWN = 1077,
        DORMANT_VISUAL = 1090,
        DORMANT = 1518,
        DORMANT_AWAKEN_CONDITION_ENCHANT,
        SHRINE = 1057,
        FATIGUE_REFERENCE = 1290,
        WAND = 1015,
        TWINSPELL = 1193,
        TWINSPELL_COPY = 1186,
        TWINSPELLPENDING = 1269,
        CREATED_BY_TWINSPELL = 3432,
        CREATED_BY_MINIATURIZE,
        CREATED_BY_GIGANTIFY,
        EVILZUG = 994,
        RUSH = 791,
        ATTACKABLE_BY_RUSH = 930,
        ECHO = 846,
        NON_KEYWORD_ECHO = 1114,
        OVERKILL = 923,
        PROPHECY,
        ETHEREAL = 880,
        REVEAL_CHOICES = 792,
        REAL_TIME_TRANSFORM = 859,
        HEALTH_DISPLAY = 917,
        ENABLE_HEALTH_DISPLAY = 920,
        HEALTH_DISPLAY_COLOR = 1046,
        HEALTH_DISPLAY_NEGATIVE,
        VOODOO_LINK = 921,
        FAN_LINK = 3052,
        COLLECTIONMANAGER_FILTER_MANA_EVEN = 956,
        COLLECTIONMANAGER_FILTER_MANA_ODD,
        SUPPRESS_DEATH_SOUND = 959,
        ECHOING_OOZE_SPELL = 963,
        ENCHANTMENT_INVISIBLE = 976,
        WILD = 991,
        HALL_OF_FAME,
        FAST_BATTLECRY = 998,
        CARD_DOES_NOTHING = 1075,
        FORCE_NO_CUSTOM_SPELLS = 1529,
        FORCE_NO_CUSTOM_SUMMON_SPELLS = 1614,
        FORCE_NO_CUSTOM_LIFETIME_SPELLS = 1613,
        FORCE_NO_CUSTOM_KEYWORD_SPELLS = 1615,
        FORCE_GREEN_GLOW_ACTIVE = 1693,
        START_OF_COMBAT = 1531,
        PUZZLE = 979,
        PUZZLE_PROGRESS,
        PUZZLE_PROGRESS_TOTAL,
        PUZZLE_TYPE,
        PUZZLE_COMPLETED = 984,
        PUZZLE_NAME = 1026,
        PREVIOUS_PUZZLE_COMPLETED = 1042,
        PUZZLE_MODE = 1073,
        CONCEDE_BUTTON_ALTERNATIVE_TEXT = 985,
        HIDE_RESTART_BUTTON = 990,
        END_TURN_BUTTON_ALTERNATIVE_APPEARANCE = 1000,
        TURN_INDICATOR_ALTERNATIVE_APPEARANCE = 1027,
        DISABLE_TURN_INDICATORS = 1448,
        DECK_RULE_MOD_DECK_SIZE = 997,
        IGNORE_DECK_RULESET = 1896,
        HIDE_OUT_OF_CARDS_WARNING = 1050,
        SUPPRESS_JOBS_DONE_VO = 1055,
        BLOCK_ALL_INPUT = 1071,
        SUPPRESS_ALL_SUMMON_VO = 1458,
        SUPPRESS_SUMMON_VO_FOR_PLAYER = 1521,
        SUPPRES_ALL_SOUNDS_FOR_ENTITY = 3093,
        DONT_SUPPRESS_SUMMON_VO = 2440,
        DONT_SUPPRESS_KEYWORD_VO = 2636,
        PLAYER_BASE_SHRINE_DECK_ID = 1099,
        DISPLAY_CARD_ON_MOUSEOVER = 1078,
        DECK_POWER_UP = 1080,
        SIDEKICK,
        SIDEKICK_HERO_POWER,
        RUN_PROGRESS = 1113,
        ALTERNATE_MOUSE_OVER_CARD = 1132,
        ENCHANTMENT_BANNER_TEXT = 1135,
        MOUSE_OVER_CARD_APPEARANCE = 1142,
        IS_ADVENTURE_SCENARIO = 1172,
        COIN_MANA_GEM = 1199,
        COIN_MANA_GEM_FOR_CHOICE_CARDS = 1643,
        TECH_LEVEL_MANA_GEM = 1442,
        BACON_COIN_ON_ENEMY_MINIONS = 1467,
        ALWAYS_USE_FAST_ACTOR_TRIGGERS = 1473,
        HEROIC_HERO_POWER = 1282,
        UI_BUFF_ATK_UP = 1297,
        UI_BUFF_COST_DOWN = 1296,
        UI_BUFF_COST_UP = 1298,
        UI_BUFF_HEALTH_UP = 1294,
        UI_BUFF_SET_COST_ZERO,
        UI_BUFF_DURABILITY_UP = 1443,
        GALAKROND_IN_PLAY = 1194,
        OUTCAST = 1333,
        STUDY = 1414,
        SPELLBURST = 1427,
        NON_KEYWORD_SPELLBURST = 2672,
        CORRUPT = 1524,
        CORRUPTED_CARD = 1551,
        FRENZY = 1637,
        TRADEABLE = 1720,
        FORGE = 2785,
        FORGED = 3011,
        FORGES_INTO = 3074,
        BACON_DUO_PASSABLE = 3178,
        DECK_ACTION_COST = 1743,
        IS_USING_TRADE_OPTION = 2045,
        IS_USING_FORGE_OPTION = 2869,
        IS_USING_PASS_OPTION = 3185,
        TOOL = 1722,
        QUESTLINE = 1725,
        HONORABLE_KILL = 1920,
        COLOSSAL = 2247,
        COLOSSAL_LIMB,
        COLOSSAL_LIMB_ON_LEFT = 2469,
        DREDGE = 2332,
        INFUSE = 2456,
        INFUSED,
        CORPSE = 2559,
        MANATHIRST = 2498,
        MAGNETIC_TO_RACE = 2859,
        MAX_SIDEBOARD_CARDS = 2931,
        MIN_SIDEBOARD_CARDS = 3459,
        FINALE = 2820,
        OVERHEAL,
        BONUS_EFFECTS = 2934,
        SIDEBOARD_TYPE = 3427,
        RED_MANA_GEM = 449,
        BACON_IS_KEL_THUZAD = 1423,
        BACON_TRIPLE_UPGRADE_MINION_ID = 1429,
        BACON_TRIPLE_CANDIDATE = 1460,
        BACON_TRIPLED_BASE_MINION_ID = 1471,
        BACON_TRIPLED_BASE_MINION_ID2 = 3499,
        BACON_TRIPLED_BASE_MINION_ID3,
        BACON_PAIR_CANDIDATE = 3031,
        BACON_DUO_TRIPLE_CANDIDATE_TEAMMATE = 3145,
        BACON_DUO_PAIR_CANDIDATE_TEAMMATE,
        BATTLEGROUNDS_PREMIUM_EMOTES = 1463,
        PLAYER_ID_LOOKUP = 1740,
        BACON_BLOOD_GEM_TOOLTIP = 1966,
        BACON_REFRESH_TOOLTIP = 2104,
        AVENGE = 2129,
        BACON_SPELLCRAFT_ID = 2359,
        VENOMOUS = 2853,
        BACON_HERO_EARLY_ACCESS = 1554,
        BACON_STARSTOBOUNCEOFF = 2155,
        BACON_CHOSEN_BOARD_SKIN_ID = 2264,
        BACON_COMPANION_ID = 2130,
        BACON_BUDDY = 2154,
        BATTLEGROUNDS_FAVORITE_FINISHER = 2348,
        BACON_OMIT_WHEN_OUT_OF_ROTATION = 2387,
        BACON_FREEZE_TOOLTIP = 2455,
        BACON_STEALTH_TOOLTIP = 2704,
        BACON_QUEST_TOOLTIP,
        BACON_REBORN_TOOLTIP = 2870,
        BACON_PUTRICIDES_CREATION_TOOLTIP = 2875,
        BACON_MINION_TYPE_REWARD = 2571,
        BACON_CARD_DBID_REWARD = 2673,
        BACON_IS_HEROPOWER_QUESTREWARD = 2706,
        BACON_HERO_QUEST_REWARD_DATABASE_ID = 2713,
        BACON_HERO_HEROPOWER_QUEST_REWARD_DATABASE_ID,
        BACON_HERO_QUEST_REWARD_COMPLETED,
        BACON_HERO_HEROPOWER_QUEST_REWARD_COMPLETED,
        BACON_DOUBLE_QUEST_HERO_POWER = 2803,
        BACON_IS_BOB_QUEST = 2732,
        BACON_HERO_REWARD_CARD_DBID = 2748,
        BACON_HERO_HEROPOWER_REWARD_CARD_DBID,
        BACON_HERO_REWARD_MINION_TYPE,
        BACON_HERO_HEROPOWER_REWARD_MINION_TYPE,
        BACON_DIED_LAST_COMBAT = 2483,
        BACON_GLOBAL_ANOMALY_DBID = 2897,
        HAS_DRAG_TO_BUY = 2458,
        TRANSIENT_ENTITY = 1493,
        BACON_TRIGGER_UPBEAT = 3046,
        BACON_TRIGGER_XY,
        BACON_SELL_VALUE = 1587,
        BACON_CURRENT_COMBAT_PLAYER_ID = 2989,
        BACON_CONSUME_TOOLTIP = 3254,
        FX_DATANUM_1 = 1436,
        FX_DATANUM_2 = 3077,
        FX_DATANUM_3 = 3109,
        BACON_VERDANTSPHERES = 1598,
        METAMORPHOSIS = 1644,
        BACON_AVALANCHE = 1744,
        BACON_COMEONECOMEALL = 1789,
        BACON_DIED_LAST_COMBAT_HINT = 2780,
        ROOTED = 2179,
        VULNERABLE,
        IMMOLATING = 2505,
        SPELLCRAFT_HINT = 2557,
        COPIED_HINT = 2572,
        BLEEDING = 2575,
        IMMOLATESTAGE = 2600,
        SINFUL_BRAND = 2613,
        HAUNTED_SECRET = 2634,
        SECRET_LOCKED = 2676,
        LITERALLY_UNPLAYABLE = 1020,
        UNPLAYABLE_VISUALS = 2798,
        DEATH_SPELL_OVERRIDE = 2722,
        EXTRA_TURNS_SPELL_OVERRIDE = 3465,
        BUILDING_UP = 3016,
        TUTORIAL_TARGET_OPPONENT_ANIM = 3192,
        TUTORIAL_TARGET_MINION_ANIM,
        TUTORIAL_PLAY_MINION_ANIM = 3195,
        TUTORIAL_HERO_POWER_TARGET_MINION_ANIM,
        TUTORIAL_HERO_POWER_TARGET_OPPONENT_ANIM,
        ALONE_RANGER = 3258,
        SUPPRESS_HERO_STANDARD_SUMMON_FX = 3438,
        SHUDDERWOCKHIGHLIGHTHINT = 3463,
        OUROBOSDEATHRATTLE = 3716,
        FAKE_ZONE = 1702,
        FAKE_ZONE_POSITION,
        FAKE_CONTROLLER = 2032,
        CUTSCENE_CARD_TYPE = 3265,
        LETTUCE_CONTROLLER = 1653,
        LETTUCE_MERCENARY = 1665,
        LETTUCE_ABILITY_OWNER = 1654,
        LETTUCE_SELECTED_TARGET = 1657,
        LETTUCE_SELECTED_SUBCARD_INDEX = 1661,
        LETTUCE_ROLE = 1666,
        LETTUCE_IS_COMBAT_ACTION_TAKEN = 1668,
        LETTUCE_COOLDOWN_CONFIG,
        LETTUCE_CURRENT_COOLDOWN,
        LETTUCE_ABILITY_SUMMONED_MINION = 1676,
        LETTUCE_ABILITY_TILE_VISUAL_SELF_ONLY = 1697,
        LETTUCE_ABILITY_TILE_VISUAL_ALL_VISIBLE,
        LETTUCE_MAX_IN_PLAY_MERCENARIES = 1704,
        LETTUCE_MERCENARIES_TO_NOMINATE,
        LETTUCE_COOLDOWN_WHILE_BENCHED = 1708,
        LETTUCE_MERCENARY_RESERVE = 1731,
        LETTUCE_SKIP_MERCENARY_RESERVE,
        LETTUCE_DISABLE_AUTO_SELECT_NEXT_MERC = 1753,
        LETTUCE_ABILITY_USED_LAST_TURN = 1807,
        LETTUCE_MERCENARY_EXPERIENCE = 1852,
        LETTUCE_IS_EQUPIMENT = 1855,
        LETTUCE_EQUIPMENT_ID,
        LETTUCE_SELECTED_ABILITY_QUEUE_ORDER = 1991,
        LETTUCE_HAS_MANUALLY_SELECTED_ABILITY = 1967,
        LETTUCE_KEEP_LAST_STANDING_MINION_ACTOR = 1976,
        LETTUCE_USE_DETERMINISTIC_TEAM_ABILITY_QUEUING = 1990,
        LETTUCE_NODE_TYPE = 1808,
        LETTUCE_PASSIVE_ABILITY = 1671,
        LETTUCE_BOUNTY_BOSS = 2168,
        LETTUCE_IS_TREASURE_CARD = 2170,
        LETTUCE_CURRENT_BOUNTY_ID = 2120,
        LETTUCE_SHOW_OPPOSING_FAKE_HAND = 2224,
        LETTUCE_VERSUS_SPELL_STATE = 2228,
        LETTUCE_START_OF_GAME_ABILITY = 2241,
        LETTUCE_CURSED_ABILITY_VISUAL = 2381,
        LETTUCE_OVERTIME = 2123,
        LETTUCE_ABILITY_TILE_VISUAL_PUBLIC_SPEED = 2470,
        LETTUCE_FACTION = 2720,
        LETTUCE_COMBAT_FROM_HIGH_TO_LOW = 1712,
        LETTUCE_ABILITY_TIER = 2493,
        LETTUCE_EQUIPMENT_TIER,
        MERCENARIES_DISCOVER_SOURCE = 2752,
        SUMMONED_WHEN_DRAWN = 3128,
        QUICKDRAW = 2905,
        EXCAVATE = 3114,
        MINIATURIZE = 3318,
        MINI,
        BACON_PASS_TOOLTIP = 3321,
        ZILLIAX_CUSTOMIZABLE_COSMETICMODULE = 3376,
        ZILLIAX_CUSTOMIZABLE_FUNCTIONALMODULE,
        ZILLIAX_CUSTOMIZABLE_LINKED_COSMETICMOUDLE = 3450,
        ZILLIAX_CUSTOMIZABLE_LINKED_FUNCTIONALMOUDLE = 3470,
        ZILLIAX_CUSTOMIZABLE_SAVED_VERSION = 3477,
        GIGANTIFY = 3399,
        GIGANTIC,
        MAGE_TOURIST = 3606,
        DRUID_TOURIST = 3605,
        WARRIOR_TOURIST = 3604,
        HUNTER_TOURIST = 3603,
        PRIEST_TOURIST = 3602,
        DEMON_HUNTER_TOURIST = 3601,
        SHAMAN_TOURIST = 3600,
        DEATH_KNIGHT_TOURIST = 3599,
        WARLOCK_TOURIST = 3598,
        ROGUE_TOURIST = 3597,
        PALADIN_TOURIST = 3607,

        None,
        Mob,
        Spell,
        Weapon
    }

    public enum TAG_ZONE
    {
        INVALID,
        PLAY,
        DECK,
        HAND,
        GRAVEYARD,
        REMOVEDFROMGAME,
        SETASIDE,
        SECRET,
        LETTUCE_ABILITY,
    }

    public enum TAG_CLASS
    {
        INVALID = 0,
        DEATHKNIGHT = 1,
        DRUID = 2,
        HUNTER = 3,
        MAGE = 4,
        PALADIN = 5,
        PRIEST = 6,
        ROGUE = 7,
        SHAMAN = 8,
        WARLOCK = 9,
        WARRIOR = 10,
        DREAM = 11,
        NEUTRAL = 12,
        WHIZBANG = 13,
		DEMONHUNTER = 14,
    }

    public enum TAG_RACE
    {
        INVALID = 0,
        BLOODELF = 1,
        DRAENEI = 2,
        DWARF = 3,
        GNOME = 4,
        GOBLIN = 5,
        HUMAN = 6,
        NIGHTELF = 7,
        ORC = 8,
        TAUREN = 9,
        TROLL = 10,
        UNDEAD = 11,
        WORGEN = 12,
        GOBLIN2 = 13,
        MURLOC = 14,
        DEMON = 15,
        SCOURGE = 16,
        MECHANICAL = 17,
        ELEMENTAL = 18,
        OGRE = 19,
        PET = 20,
        TOTEM = 21,
        NERUBIAN = 22,
        PIRATE = 23,
        DRAGON = 24,
        BLANK,
        ALL,
        EGG = 38,
        QUILBOAR = 43,
        CENTAUR = 80,
        FURBOLG,
        HIGHELF = 83,
        TREANT,
        OWLKIN,
        HALFORC = 88,
        LOCK,
        NAGA = 92,
        OLDGOD,
        PANDAREN,
        GRONN,
        CELESTIAL,
        GNOLL,
        GOLEM,
        HARPY,
        VULPERA
    }
    
    public enum TAG_CARDTYPE
    {
        NONE = 0,
        GAME = 1,
        PLAYER = 2,
        HERO = 3,//英雄
        MOB = 4,//随从
        SPELL = 5,//法术
        ENCHANTMENT = 6,//增幅（例如：变形术，救赎，力量的代价，自然之力的附加效果）
        WEAPON = 7,//武器
        ITEM = 8,
        TOKEN = 9,
        HEROPWR = 10,//英雄技能
        BLANK = 11,
        GAME_MODE_BUTTON = 12,
        MOVE_MINION_HOVER_TARGET = 22,
        LETTUCE_ABILITY,
        BATTLEGROUND_HERO_BUDDY,//战旗伙伴
        LOCATION = 39,//地标
        BATTLEGROUND_QUEST_REWARD,//战旗奖励
        BATTLEGROUND_ANOMALY = 43,//战旗畸变
        BATTLEGROUND_SPELL = 42,//战旗法术
        BATTLEGROUND_TRINKET = 44,//战旗饰品
    }

    public enum TAG_CARD_SET
    {
        INVALID = 0,
        TEST_TEMPORARY = 1,
        CORE = 2,
        EXPERT1 = 3,
        REWARD = 4,
        MISSIONS = 5,
        DEMO = 6,
        NONE = 7,
        CHEAT = 8,
        BLANK = 9,
        DEBUG_SP = 10,
        PROMO = 11,
        FP1 = 12,
        PE1 = 13,
        BRM = 14,
        TGT = 15,
        CREDITS = 16,
        HERO_SKINS = 17,
        TB = 18,
        SLUSH = 19,
        LOE = 20,
        OG = 21,
        OG_RESERVE = 22,
        KARA = 23,
        KARA_RESERVE = 24,
        GANGS = 25,
        GANGS_RESERVE = 26,
        UNGORO = 27,
        ICECROWN = 1001
    }

    public enum TAG_RARITY
    {
        INVALID = 0,
        COMMON = 1,
        FREE = 2,
        RARE = 3,
        EPIC = 4,
        LEGENDARY = 5,
    }

    public enum TAG_SPELL_SCHOOL
    {
        NONE = 0,
        ARCANE = 1,
        FIRE = 2,
        FROST = 3,
        NATURE = 4,
        HOLY = 5,
        SHADOW = 6,
        FEL = 7,
        PHYSICAL_COMBAT = 8,
        TAVERN = 9,
        SPELLCRAFT = 10
    }
}