namespace HREngine.Bots
{
    using System;
    using System.Collections.Generic;

    // 这个类已经基本废弃了，现在没有一个全局的惩罚，每个 behavior 自己定义自己的惩罚
    public class PenalityManager
    {
        //todo acolyteofpain
        //todo better aoe-penality
        //治疗触发数据库
        public Dictionary<CardDB.cardNameEN, int> HealTargetDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //治疗英雄
        public Dictionary<CardDB.cardNameEN, int> HealHeroDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //治疗所有
        public Dictionary<CardDB.cardNameEN, int> HealAllDatabase = new Dictionary<CardDB.cardNameEN, int>();


        //伤害所有
        public Dictionary<CardDB.cardNameEN, int> DamageAllDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //伤害英雄
        public Dictionary<CardDB.cardNameEN, int> DamageHeroDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //随机伤害
        public Dictionary<CardDB.cardNameEN, int> DamageRandomDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //伤害所有敌方
        public Dictionary<CardDB.cardNameEN, int> DamageAllEnemysDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //英雄技能装备武器
        public Dictionary<CardDB.cardNameEN, int> HeroPowerEquipWeapon = new Dictionary<CardDB.cardNameEN, int>();

        //激怒
        public Dictionary<CardDB.cardNameEN, int> enrageDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //沉默
        public Dictionary<CardDB.cardNameEN, int> silenceDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //我方需要 沉默
        public Dictionary<CardDB.cardNameEN, int> OwnNeedSilenceDatabase = new Dictionary<CardDB.cardNameEN, int>();

        //英雄攻击 buff
        Dictionary<CardDB.cardNameEN, int> heroAttackBuffDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //攻击 buff
        public Dictionary<CardDB.cardNameEN, int> attackBuffDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //治疗buff
        public Dictionary<CardDB.cardNameEN, int> healthBuffDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //嘲讽buff
        Dictionary<CardDB.cardNameEN, int> tauntBuffDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //治疗 帮助者
        Dictionary<CardDB.cardNameEN, int> lethalHelpers = new Dictionary<CardDB.cardNameEN, int>();
        //法术依赖
        Dictionary<CardDB.cardNameEN, int> spellDependentDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //龙依赖
        Dictionary<CardDB.cardNameEN, int> dragonDependentDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //元素依赖
        Dictionary<CardDB.cardNameEN, int> elementalLTDependentDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //卡牌丢弃
        public Dictionary<CardDB.cardNameEN, int> cardDiscardDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //毁灭自己
        Dictionary<CardDB.cardNameEN, int> destroyOwnDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //毁灭
        Dictionary<CardDB.cardNameEN, int> destroyDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //buff 随从
        Dictionary<CardDB.cardNameEN, int> buffingMinionsDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //buff 1回合
        Dictionary<CardDB.cardNameEN, int> buffing1TurnDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //英雄伤害 在 aoe?
        Dictionary<CardDB.cardNameEN, int> heroDamagingAoeDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //随机效果
        Dictionary<CardDB.cardNameEN, int> randomEffects = new Dictionary<CardDB.cardNameEN, int>();
        //沉默触发
        public Dictionary<CardDB.cardNameEN, int> silenceTargets = new Dictionary<CardDB.cardNameEN, int>();
        //回手
        Dictionary<CardDB.cardNameEN, int> returnHandDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //连接成组
        Dictionary<CardDB.cardNameEN, int> GangUpDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //buff手牌
        Dictionary<CardDB.cardNameEN, int> buffHandDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //装备武器
        Dictionary<CardDB.cardNameEN, int> equipWeaponPlayDatabase = new Dictionary<CardDB.cardNameEN, int>(); 
        //重点
        public Dictionary<CardDB.cardNameEN, int> priorityDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //有用 需要保持
        Dictionary<CardDB.cardNameEN, int> UsefulNeedKeepDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //选择1
        Dictionary<CardDB.cardNameEN, CardDB.cardIDEnum> choose1database = new Dictionary<CardDB.cardNameEN, CardDB.cardIDEnum>();
        //选择2
        Dictionary<CardDB.cardNameEN, CardDB.cardIDEnum> choose2database = new Dictionary<CardDB.cardNameEN, CardDB.cardIDEnum>();
        //伤害目标
        public Dictionary<CardDB.cardNameEN, int> DamageTargetDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //伤害目标特殊
        public Dictionary<CardDB.cardNameEN, int> DamageTargetSpecialDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //可能造成伤害
        public Dictionary<CardDB.cardNameEN, int> maycauseharmDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //战吼
        public Dictionary<CardDB.cardNameEN, int> cardDrawBattleCryDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //亡语
        public Dictionary<CardDB.cardNameEN, int> cardDrawDeathrattleDatabase = new Dictionary<CardDB.cardNameEN, int>();
        //重点目标
        public Dictionary<CardDB.cardNameEN, int> priorityTargets = new Dictionary<CardDB.cardNameEN, int>();
        //特殊随从
        public Dictionary<CardDB.cardNameEN, int> specialMinions = new Dictionary<CardDB.cardNameEN, int>(); //minions with cardtext, but no battlecry
        //我方 亡语效果 召唤的随从
        public Dictionary<CardDB.cardNameEN, int> ownSummonFromDeathrattle = new Dictionary<CardDB.cardNameEN, int>();
        
        Dictionary<TAG_RACE, int> ClassRacePriorityWarloc = new Dictionary<TAG_RACE, int>();//术士职业 重点
        Dictionary<TAG_RACE, int> ClassRacePriorityHunter = new Dictionary<TAG_RACE, int>();//猎人
        Dictionary<TAG_RACE, int> ClassRacePriorityMage = new Dictionary<TAG_RACE, int>();//法师
        Dictionary<TAG_RACE, int> ClassRacePriorityShaman = new Dictionary<TAG_RACE, int>();//萨满
        Dictionary<TAG_RACE, int> ClassRacePriorityDruid = new Dictionary<TAG_RACE, int>();//德鲁伊
        Dictionary<TAG_RACE, int> ClassRacePriorityPaladin = new Dictionary<TAG_RACE, int>();//圣骑士
        Dictionary<TAG_RACE, int> ClassRacePriorityPriest = new Dictionary<TAG_RACE, int>();//牧师
        Dictionary<TAG_RACE, int> ClassRacePriorityRouge = new Dictionary<TAG_RACE, int>();//盗贼??
        Dictionary<TAG_RACE, int> ClassRacePriorityWarrior = new Dictionary<TAG_RACE, int>();//战士
        
        ComboBreaker cb;//阻断
        Hrtprozis prozis;//对局信息
        Settings settings;//设置
        CardDB cdb;//卡牌数据
        Ai ai;//ai

        private static PenalityManager instance;

        public static PenalityManager Instance
        {
            get
            {
                return instance ?? (instance = new PenalityManager());
            }
        }

        public void setInstances()
        {
            ai = Ai.Instance;
            cb = ComboBreaker.Instance;
            prozis = Hrtprozis.Instance;
            settings = Settings.Instance;
            cdb = CardDB.Instance;
        }

        private PenalityManager()
        {
            setupHealDatabase();
            setupEnrageDatabase();
            setupDamageDatabase();
            setupPriorityList();
            setupsilenceDatabase();
            setupAttackBuff();
            setupHealthBuff();
            setupCardDrawBattlecry();
            setupDiscardCards();
            setupDestroyOwnCards();
            setupSpecialMins();
            setupEnemyTargetPriority();
            setupHeroDamagingAOE();
            setupBuffingMinions();
            setupRandomCards();
            setupLethalHelpMinions();
            setupSilenceTargets();
            setupUsefulNeedKeepDatabase();
            setupRelations();
            setupChooseDatabase();
            setupClassRacePriorityDatabase();
            setupGangUpDatabase();
            setupOwnSummonFromDeathrattle();
            setupbuffHandDatabase();
            setupequipWeaponPlayDatabase();
            setupReturnBackToHandCards();
        }

        public int getSilencePenality(CardDB.cardNameEN name, Minion target, Playfield p)
        {
            if (target != null && target.untouchable) return 1000;
            int pen = 0;

            if (target == null)
            {
                if (name == CardDB.cardNameEN.ironbeakowl || name == CardDB.cardNameEN.spellbreaker || name == CardDB.cardNameEN.keeperofthegrove)
                {
                    return 20;
                }
                return 0;
            }

            if (target.own)
            {
                if (this.silenceDatabase.ContainsKey(name))
                {
                    if (!target.silenced && this.OwnNeedSilenceDatabase.ContainsKey(target.name)) return -5;
                    if (target.Angr < target.handcard.card.Attack || target.maxHp < target.handcard.card.Health
                        || target.enemyPowerWordGlory > 0 || target.enemyBlessingOfWisdom > 0
                        || (target.frozen && !target.playedThisTurn && target.numAttacksThisTurn == 0))
                    {
                        return 0;
                    }
                    pen += 500;
                }
            }
            else
            {
                switch (target.name)
                {
                    case CardDB.cardNameEN.masterchest: return 500;
                }
                if (this.silenceDatabase.ContainsKey(name))
                {
                    pen = 5;
                    if (p.isLethalCheck)
                    {
                        //during lethal we only silence taunt, or if its a mob (owl/spellbreaker) + we can give him charge
                        if (target.taunt || (name == CardDB.cardNameEN.ironbeakowl && (p.ownMinions.Find(x => x.name == CardDB.cardNameEN.tundrarhino) != null || p.owncards.Find(x => x.card.nameEN == CardDB.cardNameEN.charge) != null)) || (name == CardDB.cardNameEN.spellbreaker && p.owncards.Find(x => x.card.nameEN == CardDB.cardNameEN.charge) != null)) return 0;

                        return 500;
                    }

                    if (!target.silenced && this.OwnNeedSilenceDatabase.ContainsKey(target.name))
                    {
                        if (target.taunt) pen += 15;
                        return 500;
                    }

                    if (!target.silenced)
                    {
                        if (this.priorityDatabase.ContainsKey(target.name)) return 0;
                        if (this.silenceTargets.ContainsKey(target.name)) return 0;
                        if (target.handcard.card.deathrattle) return 0;
                    }

                    if (target.Angr <= target.handcard.card.Attack && target.maxHp <= target.handcard.card.Health && !target.taunt && !target.windfury && !target.divineshild && !target.poisonous && !target.lifesteal && !this.specialMinions.ContainsKey(name))
                    {
                        if (name == CardDB.cardNameEN.keeperofthegrove) return 500;
                        return 30;
                    }

                    if (target.Angr > target.handcard.card.Attack || target.maxHp > target.handcard.card.Health)
                    {
                        return 0;
                    }

                    return pen;
                }
            }

            return pen;

        }

        public int getAttackWithMininonPenality(Minion m, Playfield p, Minion target)
        {
            if (target == null || target.untouchable || target.handcard.card.nameCN == CardDB.cardNameCN.毁灭战舰 || target.Hp <= 0) return 1000;
            if (m != null && !m.silenced && (m.untouchable || m.handcard.card.CantAttack)) return 1000;
            if (m.cantAttackHeroes && target.isHero) return 1000;
            int enfaceReward = 0;
            if(printUtils.enfaceReward != 0 && target != null && target.isHero){
                enfaceReward = printUtils.enfaceReward;
            }
            return ai.botBase.getAttackWithMininonPenality(m, p, target) - enfaceReward;
        }
        public int getAttackWithHeroPenality(Minion target, Playfield p)
        {
            if (target == null || target.untouchable || target.handcard.card.nameCN == CardDB.cardNameCN.毁灭战舰 || target.Hp <= 0) return 1000;
            int enfaceReward = 0;
            if(printUtils.enfaceReward != 0 && target != null && target.isHero){
                enfaceReward = printUtils.enfaceReward;
            }
            return ai.botBase.getAttackWithHeroPenality(target, p) - enfaceReward;
        }

        public int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            if (nowHandcard.enchs.Contains(CardDB.cardIDEnum.SW_052t4e)) return 1000;
            if (target != null && target.untouchable) return 1000;
            return ai.botBase.getPlayCardPenality(card, target, p, nowHandcard);
        }

        public CardDB.Card getChooseCard(CardDB.Card c, int choice)
        {
            if (choice == 1 && this.choose1database.ContainsKey(c.nameEN))
            {
                c = cdb.getCardDataFromID(this.choose1database[c.nameEN]);
            }
            else if (choice == 2 && this.choose2database.ContainsKey(c.nameEN))
            {
                c = cdb.getCardDataFromID(this.choose2database[c.nameEN]);
            }
            return c;
        }

        public int getValueOfUsefulNeedKeepPriority(CardDB.cardNameEN name)
        {
            return UsefulNeedKeepDatabase.ContainsKey(name) ? UsefulNeedKeepDatabase[name] : 0;
        }

        private void setupEnrageDatabase()
        {
            enrageDatabase.Add(CardDB.cardNameEN.aberrantberserker, 2);
            enrageDatabase.Add(CardDB.cardNameEN.amaniberserker, 3);
            enrageDatabase.Add(CardDB.cardNameEN.angrychicken, 5);
            enrageDatabase.Add(CardDB.cardNameEN.bloodhoofbrave, 3);
            enrageDatabase.Add(CardDB.cardNameEN.grommashhellscream, 6);
            enrageDatabase.Add(CardDB.cardNameEN.ragingworgen, 2);
            enrageDatabase.Add(CardDB.cardNameEN.spitefulsmith, 2);
            enrageDatabase.Add(CardDB.cardNameEN.taurenwarrior, 3);
            enrageDatabase.Add(CardDB.cardNameEN.warbot, 1);
        }

        private void setupHealDatabase()
        {
            HealAllDatabase.Add(CardDB.cardNameEN.circleofhealing, 4);//allminions
            HealAllDatabase.Add(CardDB.cardNameEN.darkscalehealer, 2);//all friends
            HealAllDatabase.Add(CardDB.cardNameEN.holynova, 2);//to all own minions
            HealAllDatabase.Add(CardDB.cardNameEN.treeoflife, 1000);//all friends

            HealHeroDatabase.Add(CardDB.cardNameEN.amarawardenofhope, 40);
            HealHeroDatabase.Add(CardDB.cardNameEN.antiquehealbot, 8); //tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.bindingheal, 5);
            HealHeroDatabase.Add(CardDB.cardNameEN.cultapothecary, 2);
            HealHeroDatabase.Add(CardDB.cardNameEN.drainlife, 2);//tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.guardianofkings, 6);//tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.holyfire, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.invocationofwater, 12);
            HealHeroDatabase.Add(CardDB.cardNameEN.jinyuwaterspeaker, 6);
            HealHeroDatabase.Add(CardDB.cardNameEN.priestessofelune, 4);//tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.refreshmentvendor, 4);
            HealHeroDatabase.Add(CardDB.cardNameEN.renojackson, 25); //tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.sacrificialpact, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.sealoflight, 4); //tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.siphonsoul, 3); //tohero
            HealHeroDatabase.Add(CardDB.cardNameEN.tidalsurge, 4);
            HealHeroDatabase.Add(CardDB.cardNameEN.tournamentmedic, 2);
            HealHeroDatabase.Add(CardDB.cardNameEN.tuskarrjouster, 7);
            HealHeroDatabase.Add(CardDB.cardNameEN.twilightdarkmender, 10);

            HealTargetDatabase.Add(CardDB.cardNameEN.ancestralhealing, 1000);
            HealTargetDatabase.Add(CardDB.cardNameEN.ancientoflore, 5);
            HealTargetDatabase.Add(CardDB.cardNameEN.ancientsecrets, 5);
            HealTargetDatabase.Add(CardDB.cardNameEN.bindingheal, 5);
            HealTargetDatabase.Add(CardDB.cardNameEN.darkshirealchemist, 5);
            HealTargetDatabase.Add(CardDB.cardNameEN.earthenringfarseer, 3);
            HealTargetDatabase.Add(CardDB.cardNameEN.flashheal, 5);
            HealTargetDatabase.Add(CardDB.cardNameEN.forbiddenhealing, 2);
            HealTargetDatabase.Add(CardDB.cardNameEN.gadgetzansocialite, 2);
            HealTargetDatabase.Add(CardDB.cardNameEN.heal, 4);
            HealTargetDatabase.Add(CardDB.cardNameEN.healingtouch, 8);
            HealTargetDatabase.Add(CardDB.cardNameEN.healingwave, 14);
            HealTargetDatabase.Add(CardDB.cardNameEN.holylight, 6);
            HealTargetDatabase.Add(CardDB.cardNameEN.hotspringguardian, 3);
            HealTargetDatabase.Add(CardDB.cardNameEN.hozenhealer, 30);
            HealTargetDatabase.Add(CardDB.cardNameEN.layonhands, 8);
            HealTargetDatabase.Add(CardDB.cardNameEN.lesserheal, 2);
            HealTargetDatabase.Add(CardDB.cardNameEN.lightofthenaaru, 3);
            HealTargetDatabase.Add(CardDB.cardNameEN.moongladeportal, 6);
            HealTargetDatabase.Add(CardDB.cardNameEN.voodoodoctor, 2);
            HealTargetDatabase.Add(CardDB.cardNameEN.willofmukla, 8);
            //HealTargetDatabase.Add(CardDB.cardName.divinespirit, 2);
        }

        private void setupDamageDatabase()
        {
            //DamageAllDatabase.Add(CardDB.cardName.flameleviathan, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.abomination, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.abyssalenforcer, 3);
            DamageAllDatabase.Add(CardDB.cardNameEN.anomalus, 8);
            DamageAllDatabase.Add(CardDB.cardNameEN.barongeddon, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.corruptedseer, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.demonwrath, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.dragonfirepotion, 5);
            DamageAllDatabase.Add(CardDB.cardNameEN.dreadinfernal, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.dreadscale, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.elementaldestruction, 4);
            DamageAllDatabase.Add(CardDB.cardNameEN.excavatedevil, 3);
            DamageAllDatabase.Add(CardDB.cardNameEN.explosivesheep, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.felbloom, 4);
            DamageAllDatabase.Add(CardDB.cardNameEN.felfirepotion, 5);
            DamageAllDatabase.Add(CardDB.cardNameEN.hellfire, 3);
            DamageAllDatabase.Add(CardDB.cardNameEN.lava, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.lightbomb, 5);
            DamageAllDatabase.Add(CardDB.cardNameEN.magmapulse, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.primordialdrake, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.ravagingghoul, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.revenge, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.scarletpurifier, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.sleepwiththefishes, 3);
            DamageAllDatabase.Add(CardDB.cardNameEN.tentacleofnzoth, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.unstableghoul, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.volcanicpotion, 2);
            DamageAllDatabase.Add(CardDB.cardNameEN.whirlwind, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.yseraawakens, 5);
            DamageAllDatabase.Add(CardDB.cardNameEN.spiritlash, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.defile, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.bloodrazor, 1);
            DamageAllDatabase.Add(CardDB.cardNameEN.bladestorm, 1);

            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.arcaneexplosion, 1);//魔爆术
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.bladeflurry, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.blizzard, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.consecration, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.cthun, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.darkironskulker, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.fanofknives, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.flamestrike, 4);//烈焰风暴
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.holynova, 2);//神圣新星
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.invocationofair, 3);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.lightningstorm, 3);//闪电风暴
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.livingbomb, 5);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.locustswarm, 3);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.maelstromportal, 1);//大漩涡
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.poisoncloud, 1);//todo 1 or 2
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.sergeantsally, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.shadowflame, 2);//暗影烈焰
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.sporeburst, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.starfall, 2);//星辰坠落
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.stomp, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.swipe, 1);//横扫
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.twilightflamecaller, 1);//暮光烈焰召唤者
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.explodingbloatbat, 2);//自爆肿胀蝠
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.deathstalkerrexxar, 2);//死亡猎手雷克萨
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.deathanddecay, 3);//死亡凋零
            DamageAllEnemysDatabase.Add(CardDB.cardNameEN.bonestorm, 1);//白骨风暴

            DamageHeroDatabase.Add(CardDB.cardNameEN.backstreetleper, 2);
            DamageHeroDatabase.Add(CardDB.cardNameEN.burningadrenaline, 2);
            DamageHeroDatabase.Add(CardDB.cardNameEN.curseofrafaam, 2);
            DamageHeroDatabase.Add(CardDB.cardNameEN.emeraldreaver, 1);
            DamageHeroDatabase.Add(CardDB.cardNameEN.frostblast, 3);
            DamageHeroDatabase.Add(CardDB.cardNameEN.headcrack, 2);
            DamageHeroDatabase.Add(CardDB.cardNameEN.invocationoffire, 6);
            DamageHeroDatabase.Add(CardDB.cardNameEN.lepergnome, 2);
            DamageHeroDatabase.Add(CardDB.cardNameEN.mindblast, 5);
            DamageHeroDatabase.Add(CardDB.cardNameEN.necroticaura, 3);
            DamageHeroDatabase.Add(CardDB.cardNameEN.nightblade, 3);
            DamageHeroDatabase.Add(CardDB.cardNameEN.purecold, 8);
            DamageHeroDatabase.Add(CardDB.cardNameEN.shadowbomber, 3);
            DamageHeroDatabase.Add(CardDB.cardNameEN.sinisterstrike, 3);

            DamageRandomDatabase.Add(CardDB.cardNameEN.arcanemissiles, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.avengingwrath, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.bomblobber, 4);
            DamageRandomDatabase.Add(CardDB.cardNameEN.boombot, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.boombotjr, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.bouncingblade, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.cleave, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.demolisher, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.dieinsect, 8);
            DamageRandomDatabase.Add(CardDB.cardNameEN.dieinsects, 8);
            DamageRandomDatabase.Add(CardDB.cardNameEN.fierybat, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.flamecannon, 4);
            DamageRandomDatabase.Add(CardDB.cardNameEN.flamejuggler, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.flamewaker, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.forkedlightning, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.goblinblastmage, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.greaterarcanemissiles, 3);
            DamageRandomDatabase.Add(CardDB.cardNameEN.hugetoad, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.knifejuggler, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.madbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.madderbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.multishot, 3);
            DamageRandomDatabase.Add(CardDB.cardNameEN.ragnarosthefirelord, 8);
            DamageRandomDatabase.Add(CardDB.cardNameEN.rumblingelemental, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.shadowboxer, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.shipscannon, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.spreadingmadness, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.throwrocks, 3);
            DamageRandomDatabase.Add(CardDB.cardNameEN.timepieceofhorror, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.volatileelemental, 3);
            DamageRandomDatabase.Add(CardDB.cardNameEN.volcano, 1);
            DamageRandomDatabase.Add(CardDB.cardNameEN.breathofsindragosa, 2);
            DamageRandomDatabase.Add(CardDB.cardNameEN.blackguard, 3);

            DamageTargetDatabase.Add(CardDB.cardNameEN.arcaneblast, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.arcaneshot, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.backstab, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.ballistashot, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.barreltoss, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.betrayal, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.blackwingcorruptor, 3);//if dragon in hand
            DamageTargetDatabase.Add(CardDB.cardNameEN.blazecaller, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.blowgillsniper, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.bombsquad, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.cobrashot, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.coneofcold, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.crackle, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.darkbomb, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.discipleofcthun, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.dispatchkodo, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.dragonsbreath, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.drainlife, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.elvenarcher, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.eviscerate, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.explosiveshot, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.felcannon, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.fireball, 6);
            DamageTargetDatabase.Add(CardDB.cardNameEN.fireblast, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.fireblastrank2, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.firebloomtoxin, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.fireelemental, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.firelandsportal, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.fireplumephoenix, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.flamelance, 8);
            DamageTargetDatabase.Add(CardDB.cardNameEN.forbiddenflame, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.forgottentorch, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.frostbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.frostshock, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.gormoktheimpaler, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.greaterhealingpotion, 12);
            DamageTargetDatabase.Add(CardDB.cardNameEN.grievousbite, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.heartoffire, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.hoggersmash, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.holyfire, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.holysmite, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.icelance, 4);//only if iced
            DamageTargetDatabase.Add(CardDB.cardNameEN.implosion, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.ironforgerifleman, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.jadelightning, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.jadeshuriken, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.keeperofthegrove, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.killcommand, 3);//or 5
            DamageTargetDatabase.Add(CardDB.cardNameEN.lavaburst, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.lavashock, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.lightningbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.lightningjolt, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.livingroots, 2);//choice 1
            DamageTargetDatabase.Add(CardDB.cardNameEN.medivhsvalet, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.meteor, 15);
            DamageTargetDatabase.Add(CardDB.cardNameEN.mindshatter, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.mindspike, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.moonfire, 1); 
            DamageTargetDatabase.Add(CardDB.cardNameEN.mortalcoil, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.mortalstrike, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.northseakraken, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.onthehunt, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.perditionsblade, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.powershot, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.pyroblast, 10);
            DamageTargetDatabase.Add(CardDB.cardNameEN.razorpetal, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.roaringtorch, 6);
            DamageTargetDatabase.Add(CardDB.cardNameEN.shadowbolt, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.shadowform, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.shadowstrike, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.shotgunblast, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.si7agent, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.sonicbreath, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.sonoftheflame, 6);
            DamageTargetDatabase.Add(CardDB.cardNameEN.starfall, 5);//2 to all enemy
            DamageTargetDatabase.Add(CardDB.cardNameEN.starfire, 5);//draw a card
            DamageTargetDatabase.Add(CardDB.cardNameEN.steadyshot, 2);//or 1 + card
            DamageTargetDatabase.Add(CardDB.cardNameEN.stormcrack, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.stormpikecommando, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.swipe, 4);//1 to others
            DamageTargetDatabase.Add(CardDB.cardNameEN.tidalsurge, 4);
            DamageTargetDatabase.Add(CardDB.cardNameEN.unbalancingstrike, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.undercityvaliant, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.wrath, 1);//todo 3 or 1+card
            DamageTargetDatabase.Add(CardDB.cardNameEN.voidform, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.vampiricleech, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.ultimateinfestation, 5);
            DamageTargetDatabase.Add(CardDB.cardNameEN.toxicarrow, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.siphonlife, 3);
            DamageTargetDatabase.Add(CardDB.cardNameEN.icytouch, 1);
            DamageTargetDatabase.Add(CardDB.cardNameEN.iceclaw, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.drainsoul, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.doomerang, 2);
            DamageTargetDatabase.Add(CardDB.cardNameEN.avalanche, 0);

            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.baneofdoom, 2); 
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.bash, 3); //+3 armor
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.bloodtoichor, 1); 
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.crueltaskmaster, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.deathbloom, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.demonfire, 2); // friendly demon get +2/+2
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.demonheart, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.earthshock, 1); //SILENCE /good for raggy etc or iced
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.feedingtime, 3); 
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.flamegeyser, 2); 
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.hammerofwrath, 3); //draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.holywrath, 2);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.innerrage, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.quickshot, 3); //draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.roguesdoit, 4);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.savagery, 1);//dmg=herodamage
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.shieldslam, 1);//dmg=armor
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.shiv, 1);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.slam, 2);//draw card if it survives
            DamageTargetSpecialDatabase.Add(CardDB.cardNameEN.soulfire, 4);//delete a card

            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.daggermastery, 1);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.direshapeshift, 2);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.echolocate, 0);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.enraged, 2);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.poisoneddaggers, 2);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.shapeshift, 1);
            HeroPowerEquipWeapon.Add(CardDB.cardNameEN.plaguelord, 3);

            
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.arcaneblast, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.arcaneshot, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.backstab, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.baneofdoom, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.barreltoss, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.bash, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.blastcrystalpotion, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.bloodthistletoxin, 3);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.bloodtoichor, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.chromaticmutation, 5);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.cobrashot, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.coneofcold, 6);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.crackle, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.crush, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.darkbomb, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.deathbloom, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.demonfire, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.demonheart, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.dispel, 4);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.dragonsbreath, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.drainlife, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.drakkisathscommand, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.dream, 3);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.dynamite, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.earthshock, 4);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.emergencycoolant, 6);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.eviscerate, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.explosiveshot, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.feedingtime, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.fireball, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.firebloomtoxin, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.firelandsportal, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.flamegeyser, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.flamelance, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.forbiddenflame, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.forgottentorch, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.frostbolt, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.grievousbite, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.hakkaribloodgoblet, 5);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.hammerofwrath, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.hex, 5);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.hoggersmash, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.holyfire, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.holysmite, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.holywrath, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.humility, 7);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.huntersmark, 7);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.icelance, 6);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.implosion, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.innerrage, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.jadelightning, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.jadeshuriken, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.keeperofthegrove, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.killcommand, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.lavaburst, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.lavashock, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.lightningbolt, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.livingroots, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.madbomber, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.madderbomber, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.meteor, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.moonfire, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.mortalcoil, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.mortalstrike, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.mulch, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.naturalize, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.necroticpoison, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.onthehunt, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.polymorph, 5);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.powershot, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.pyroblast, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.quickshot, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.razorpetal, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.roaringtorch, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.roguesdoit, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.rottenbanana, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.savagery, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shadowbolt, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shadowstep, 3);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shadowstrike, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shadowworddeath, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shadowwordpain, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shatter, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shieldslam, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.shiv, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.silence, 4);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.siphonsoul, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.slam, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.sonicbreath, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.soulfire, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.spreadingmadness, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.starfall, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.starfire, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.stormcrack, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.swipe, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.tailswipe, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.thetruewarchief, 2);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.tidalsurge, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.timerewinder, 3);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.volcano, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.wrath, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.drainsoul, 1);
            this.maycauseharmDatabase.Add(CardDB.cardNameEN.obliterate, 2);
        }

        private void setupsilenceDatabase()
        {
            
            this.silenceDatabase.Add(CardDB.cardNameEN.defiascleaner, 1);//迪菲亚清道夫
            this.silenceDatabase.Add(CardDB.cardNameEN.dispel, 1);//禁魔
            this.silenceDatabase.Add(CardDB.cardNameEN.earthshock, 1);//大地震击
            this.silenceDatabase.Add(CardDB.cardNameEN.ironbeakowl, 1);//铁喙猫头鹰
            this.silenceDatabase.Add(CardDB.cardNameEN.kabalsongstealer, 1);//暗金教窃歌者
            this.silenceDatabase.Add(CardDB.cardNameEN.lightschampion, 1);//圣光勇士
            this.silenceDatabase.Add(CardDB.cardNameEN.massdispel, 1);//群体驱散
            this.silenceDatabase.Add(CardDB.cardNameEN.purify, -1);//净化
            this.silenceDatabase.Add(CardDB.cardNameEN.silence, 1);//沉默
            this.silenceDatabase.Add(CardDB.cardNameEN.spellbreaker, 1);//破法者
            this.silenceDatabase.Add(CardDB.cardNameEN.wailingsoul, -2);//哀嚎的灵魂
            this.silenceDatabase.Add(CardDB.cardNameEN.consumemagic, 1);//吞噬魔法
            this.silenceDatabase.Add(CardDB.cardNameEN.magehunter, 1);//法师猎手
            this.silenceDatabase.Add(CardDB.cardNameEN.thenamelessone, 1);//无名者
            this.silenceDatabase.Add(CardDB.cardNameEN.unsleepingsoul, -1);//不眠之魂
            this.silenceDatabase.Add(CardDB.cardNameEN.plagueofdeath, -1);//死亡之灾祸
            this.silenceDatabase.Add(CardDB.cardNameEN.shieldbreaker, 1);//破盾者
            this.silenceDatabase.Add(CardDB.cardNameEN.dalaranlibrarian, -1);//达拉然图书管理员
            this.silenceDatabase.Add(CardDB.cardNameEN.showstopper, -2);//砸场游客

            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.ancientwatcher, 2);//上古看守者
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.animagolem, 1);//心能魔像
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.bittertidehydra, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.blackknight, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.bombsquad, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.corruptedhealbot, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.dancingswords, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.deathcharger, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.eeriestatue, 0);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.emeraldhivequeen, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.felorcsoulfiend, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.felreaver, 3);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.frozencrusher, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.humongousrazorleaf, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.icehowl, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.mogortheogre, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.natthedarkfisher, 0);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.spectralrider, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.spectraltrainee, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.spectralwarrior, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.spore, 3);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.thebeast, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.unlicensedapothecary, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.unrelentingrider, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.unrelentingtrainee, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.unrelentingwarrior, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.venturecomercenary, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.whiteknight, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.wrathguard, 1);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.zombiechow, 2);
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.tickingabomination, 0);//自爆憎恶
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.rattlingrascal, 1);//骷髅捣蛋鬼
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.masterchest, 1);//大师宝箱
            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.generousmummy, 1);//慷慨的木乃伊

            OwnNeedSilenceDatabase.Add(CardDB.cardNameEN.mortuarymachine, 1);//机械法医
            //OwnNeedSilenceDatabase.Add(CardDB.cardName., 1);
            //OwnNeedSilenceDatabase.Add(CardDB.cardName., 1);
            //OwnNeedSilenceDatabase.Add(CardDB.cardName., 1);
            //OwnNeedSilenceDatabase.Add(CardDB.cardName., 1);


        }


        private void setupPriorityList()
        {
            priorityDatabase.Add(CardDB.cardNameEN.acidmaw, 3);
            priorityDatabase.Add(CardDB.cardNameEN.animatedarmor, 2);
            priorityDatabase.Add(CardDB.cardNameEN.archmageantonidas, 6);
            priorityDatabase.Add(CardDB.cardNameEN.aviana, 5);
            priorityDatabase.Add(CardDB.cardNameEN.brannbronzebeard, 4);
            priorityDatabase.Add(CardDB.cardNameEN.cloakedhuntress, 2);
            priorityDatabase.Add(CardDB.cardNameEN.confessorpaletress, 7);
            priorityDatabase.Add(CardDB.cardNameEN.crowdfavorite, 6);
            priorityDatabase.Add(CardDB.cardNameEN.darkshirecouncilman, 2);
            priorityDatabase.Add(CardDB.cardNameEN.direwolfalpha, 6);
            priorityDatabase.Add(CardDB.cardNameEN.emperorthaurissan, 5);
            priorityDatabase.Add(CardDB.cardNameEN.fandralstaghelm, 6);
            priorityDatabase.Add(CardDB.cardNameEN.flametonguetotem, 6);
            priorityDatabase.Add(CardDB.cardNameEN.flamewaker, 5);
            priorityDatabase.Add(CardDB.cardNameEN.frothingberserker, 1);
            priorityDatabase.Add(CardDB.cardNameEN.gadgetzanauctioneer, 1);
            priorityDatabase.Add(CardDB.cardNameEN.grimestreetenforcer, 1);
            priorityDatabase.Add(CardDB.cardNameEN.grimpatron, 5);
            priorityDatabase.Add(CardDB.cardNameEN.grimscaleoracle, 5);
            priorityDatabase.Add(CardDB.cardNameEN.grimygadgeteer, 1);
            priorityDatabase.Add(CardDB.cardNameEN.holychampion, 5);
            priorityDatabase.Add(CardDB.cardNameEN.knifejuggler, 2);
            priorityDatabase.Add(CardDB.cardNameEN.kodorider, 6);
            priorityDatabase.Add(CardDB.cardNameEN.kvaldirraider, 1);
            priorityDatabase.Add(CardDB.cardNameEN.leokk, 5);
            priorityDatabase.Add(CardDB.cardNameEN.lyrathesunshard, 1);
            priorityDatabase.Add(CardDB.cardNameEN.malchezaarsimp, 1);
            priorityDatabase.Add(CardDB.cardNameEN.malganis, 10);
            priorityDatabase.Add(CardDB.cardNameEN.manatidetotem, 5);
            priorityDatabase.Add(CardDB.cardNameEN.mechwarper, 1);
            priorityDatabase.Add(CardDB.cardNameEN.muklaschampion, 5);
            priorityDatabase.Add(CardDB.cardNameEN.murlocknight, 5);
            priorityDatabase.Add(CardDB.cardNameEN.murlocwarleader, 5);
            priorityDatabase.Add(CardDB.cardNameEN.nexuschampionsaraad, 6);
            priorityDatabase.Add(CardDB.cardNameEN.northshirecleric, 4);
            priorityDatabase.Add(CardDB.cardNameEN.pintsizedsummoner, 3);
            priorityDatabase.Add(CardDB.cardNameEN.primalfintotem, 5);
            priorityDatabase.Add(CardDB.cardNameEN.prophetvelen, 5);
            priorityDatabase.Add(CardDB.cardNameEN.questingadventurer, 1);
            priorityDatabase.Add(CardDB.cardNameEN.radiantelemental, 3);
            priorityDatabase.Add(CardDB.cardNameEN.ragnaroslightlord, 5);
            priorityDatabase.Add(CardDB.cardNameEN.raidleader, 5);
            priorityDatabase.Add(CardDB.cardNameEN.recruiter, 1);
            priorityDatabase.Add(CardDB.cardNameEN.scavenginghyena, 5);
            priorityDatabase.Add(CardDB.cardNameEN.secretkeeper, 5);
            priorityDatabase.Add(CardDB.cardNameEN.sorcerersapprentice, 3);
            priorityDatabase.Add(CardDB.cardNameEN.southseacaptain, 5);
            priorityDatabase.Add(CardDB.cardNameEN.spiritsingerumbra, 5);
            priorityDatabase.Add(CardDB.cardNameEN.stormwindchampion, 5);
            priorityDatabase.Add(CardDB.cardNameEN.summoningportal, 5);
            priorityDatabase.Add(CardDB.cardNameEN.summoningstone, 5);
            priorityDatabase.Add(CardDB.cardNameEN.thevoraxx, 2);
            priorityDatabase.Add(CardDB.cardNameEN.thunderbluffvaliant, 2);
            priorityDatabase.Add(CardDB.cardNameEN.timberwolf, 5);
            priorityDatabase.Add(CardDB.cardNameEN.tunneltrogg, 1);
            priorityDatabase.Add(CardDB.cardNameEN.viciousfledgling, 4);
            priorityDatabase.Add(CardDB.cardNameEN.violetillusionist, 10);
            priorityDatabase.Add(CardDB.cardNameEN.violetteacher, 1);
            priorityDatabase.Add(CardDB.cardNameEN.warhorsetrainer, 5);
            priorityDatabase.Add(CardDB.cardNameEN.warsongcommander, 5);
            priorityDatabase.Add(CardDB.cardNameEN.wickedwitchdoctor, 5);
            priorityDatabase.Add(CardDB.cardNameEN.wilfredfizzlebang, 1);
            priorityDatabase.Add(CardDB.cardNameEN.rotface, 1);
            priorityDatabase.Add(CardDB.cardNameEN.professorputricide, 1);
            priorityDatabase.Add(CardDB.cardNameEN.moorabi, 1);
        }

        private void setupAttackBuff()
        {
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.bite, 4);
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.claw, 2);
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.evolvespines, 4);
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.feralrage, 4);
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.heroicstrike, 4);
            this.heroAttackBuffDatabase.Add(CardDB.cardNameEN.gnash, 3);

            this.attackBuffDatabase.Add(CardDB.cardNameEN.abusivesergeant, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.adaptation, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.bananas, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.bestialwrath, 2); // NEVER ON enemy MINION
            this.attackBuffDatabase.Add(CardDB.cardNameEN.blessingofkings, 4);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.blessingofmight, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.bloodfurypotion, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.briarthorntoxin, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.clockworkknight, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.coldblood, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.crueltaskmaster, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.darkirondwarf, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.darkwispers, 5);//choice 2
            this.attackBuffDatabase.Add(CardDB.cardNameEN.demonfuse, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.dinomancy, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.dinosize, 10);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.divinestrength, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.earthenscales, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.explorershat, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.innerrage, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.lancecarrier, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.lanternofpower, 10);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.lightfusedstegodon, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.markofnature, 4);//choice1 
            this.attackBuffDatabase.Add(CardDB.cardNameEN.markofthewild, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.markofyshaarj, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.mutatinginjection, 4);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.nightmare, 5); //destroy minion on next turn
            this.attackBuffDatabase.Add(CardDB.cardNameEN.powerwordtentacles, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.primalfusion, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.rampage, 3);//only damaged minion 
            this.attackBuffDatabase.Add(CardDB.cardNameEN.rockbiterweapon, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.rockpoolhunter, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.screwjankclunker, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.sealofchampions, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.silvermoonportal, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.spikeridgedsteed, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.velenschosen, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.whirlingblades, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.antimagicshell, 2);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.fallensuncleric, 1);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.cryostasis, 3);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.bonemare, 4);
            this.attackBuffDatabase.Add(CardDB.cardNameEN.acherusveteran, 1);
        }

        private void setupHealthBuff()
        {
            //healthBuffDatabase.Add(CardDB.cardName.ancientofwar, 5);//choice2 is only buffing himself!
            //healthBuffDatabase.Add(CardDB.cardName.rooted, 5);
            healthBuffDatabase.Add(CardDB.cardNameEN.adaptation, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.armorplating, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.bananas, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.blessingofkings, 4);
            healthBuffDatabase.Add(CardDB.cardNameEN.clockworkknight, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.darkwispers, 5);//choice2
            healthBuffDatabase.Add(CardDB.cardNameEN.demonfuse, 3);
            healthBuffDatabase.Add(CardDB.cardNameEN.dinomancy, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.dinosize, 10);
            healthBuffDatabase.Add(CardDB.cardNameEN.divinestrength, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.earthenscales, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.explorershat, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.lanternofpower, 10);
            healthBuffDatabase.Add(CardDB.cardNameEN.lightfusedstegodon, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.markofnature, 4);//choice2
            healthBuffDatabase.Add(CardDB.cardNameEN.markofthewild, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.markofyshaarj, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.mutatinginjection, 4);
            healthBuffDatabase.Add(CardDB.cardNameEN.nightmare, 5);
            healthBuffDatabase.Add(CardDB.cardNameEN.powerwordshield, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.powerwordtentacles, 6);
            healthBuffDatabase.Add(CardDB.cardNameEN.primalfusion, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.rampage, 3);
            healthBuffDatabase.Add(CardDB.cardNameEN.rockpoolhunter, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.screwjankclunker, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.silvermoonportal, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.spikeridgedsteed, 6);
            healthBuffDatabase.Add(CardDB.cardNameEN.upgradedrepairbot, 4);
            healthBuffDatabase.Add(CardDB.cardNameEN.velenschosen, 4);
            healthBuffDatabase.Add(CardDB.cardNameEN.wildwalker, 3);
            healthBuffDatabase.Add(CardDB.cardNameEN.antimagicshell, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.sunbornevalkyr, 2);
            healthBuffDatabase.Add(CardDB.cardNameEN.fallensuncleric, 1);
            healthBuffDatabase.Add(CardDB.cardNameEN.cryostasis, 3);
            healthBuffDatabase.Add(CardDB.cardNameEN.bonemare, 4);

            tauntBuffDatabase.Add(CardDB.cardNameEN.ancestralhealing, 1);//先祖治疗
            tauntBuffDatabase.Add(CardDB.cardNameEN.darkwispers, 1);//黑暗私语
            tauntBuffDatabase.Add(CardDB.cardNameEN.markofnature, 1);//自然印记
            tauntBuffDatabase.Add(CardDB.cardNameEN.markofthewild, 1);//野性印记
            tauntBuffDatabase.Add(CardDB.cardNameEN.mutatinginjection, 1);
            tauntBuffDatabase.Add(CardDB.cardNameEN.rustyhorn, 1);//生锈的号角 零件牌
            tauntBuffDatabase.Add(CardDB.cardNameEN.sparringpartner, 1);//格斗陪练师
            tauntBuffDatabase.Add(CardDB.cardNameEN.spikeridgedsteed, 1);//剑龙骑术
        }

        private void setupCardDrawBattlecry()
        {
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.alightinthedarkness, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ancestralknowledge, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ancientoflore, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ancientteachings, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.arcaneintellect, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.archthiefrafaam, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.azuredrake, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.babblingbook, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.battlerage, 0);//only if wounded own minions or hero
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.bloodwarriors, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.burgle, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.cabaliststome, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.callpet, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.carnassasbrood, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.chitteringtunneler, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.chooseyourpath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.coldlightoracle, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.commandingshout, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.convert, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.darkpeddler, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.darkshirelibrarian, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.desertcamel, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.divinefavor, 0);//only if enemy has more cards than you
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.drakonidoperative, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.echoofmedivh, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.elisestarseeker, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.elitetaurenchieftain, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.etherealconjurer, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.excessmana, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.fanofknives, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.farsight, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.fightpromoter, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.finderskeepers, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.firefly, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.flamegeyser, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.flameheart, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.flare, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.freefromamber, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.giftofcards, 1); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.gnomishexperimenter, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.gnomishinventor, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.goldenmonkey, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.gorillabota3, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.grandcrusader, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.grimestreetinformant, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.hallucination, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.hammerofwrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.harrisonjones, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.harvest, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.holywrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.hydrologist, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.iknowaguy, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ivoryknight, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.jeweledmacaw, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.jeweledscarab, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.journeybelow, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kabalchemist, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kabalcourier, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kazakus, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kingmukla, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kingsblood, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kingsbloodtoxin, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.kingselekk, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.layonhands, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.lifetap, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.lockandload, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.lotusagents, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.lunarvisions, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.maptothegoldenmonkey, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.markofyshaarj, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.massdispel, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.megafin, 9);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.mimicpod, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.mindpocalypse, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.mindvision, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.mortalcoil, 0);//only if kills
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.muklatyrantofthevale, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.museumcurator, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.nefarian, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.neptulon, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.netherspitehistorian, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.nourish, 3); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.noviceengineer, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.powerwordshield, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.primalfinlookout, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.primordialglyph, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.purify, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.quickshot, 0);//only if your hand is empty
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ravenidol, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.razorpetallasher, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.razorpetalvolley, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.roguesdoit, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.servantofkalimos, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.shadowcaster, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.shadowoil, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.shadowvisions, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.shieldblock, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.shiv, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.slam, 0); //if survives
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.smalltimerecruits, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.solemnvigil, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.soultap, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.spellslinger, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.sprint, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.stampede, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.starfire, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.stonehilldefender, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.swashburglar, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.thecurator, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.thistletea, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.thoughtsteal, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tinkertowntechnician, 0); // If you have a Mech
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tolvirwarden, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tombspider, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tortollanforager, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tortollanprimalist, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.toshley, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tracking, 1); //NOT SUPPORTED YET
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ungoropack, 5);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.unholyshadow, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.unstableportal, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.varianwrynn, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.wildmagic, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.wrath, 1); //choice=2
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.wrathion, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.xarilpoisonedmind, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ultimateinfestation, 5);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.tomblurker, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.ghastlyconjurer, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.deathgrip, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.stitchedtracker, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.rollthebones, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.icefishing, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.howlingcommander, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.frozenclone, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.forgeofsouls, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardNameEN.devourmind, 3);

            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.acolyteofpain, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.anubarak, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.bloodmagethalnos, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.clockworkgnome, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.crystallineoracle, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.dancingswords, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.deadlyfork, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.hook, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.igneouselemental, 2);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.loothoarder, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.meanstreetmarshal, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.mechanicalyeti, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.mechbearcat, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.pollutedhoarder, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.pyros, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.rhonin, 3);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.runicegg, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.shiftingshade, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.shimmeringtempest, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.tentaclesforarms, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.tombpillager, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.toshley, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.undercityhuckster, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.webspinner, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.xarilpoisonedmind, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.shallowgravedigger, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.frozenchampion, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.bonedrake, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.bonebaron, 2);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.arfus, 1);
            cardDrawDeathrattleDatabase.Add(CardDB.cardNameEN.glacialmysteries, 1);
        }

        // 不造成阿古斯不架
        private void setupUsefulNeedKeepDatabase()
        {
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.acidmaw, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.addledgrizzly, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.airelemental, 6);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.alarmobot, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.ancientharbinger, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.animatedarmor, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.archmageantonidas, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.armorsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.aviana, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.backroombouncer, 1);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.bloodimp, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.brannbronzebeard, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.burlyrockjawtrogg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.cloakedhuntress, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.cobaltguardian, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.coldarradrake, 15);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.confessorpaletress, 32);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.cultmaster, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.cultsorcerer, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.darkshirecouncilman, 0);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.dementedfrostcaller, 15);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.demolisher, 11);
            //UsefulNeedKeepDatabase.Add(CardDB.cardName.direwolfalpha, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.dragonkinsorcerer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.emboldener3000, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.emperorthaurissan, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.faeriedragon, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.fallenhero, 15);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.masterchest, 48);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.fandralstaghelm, 15);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.felcannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.flametonguetotem, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.flamewaker, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.flesheatingghoul, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.floatingwatcher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.frothingberserker, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.gadgetzanauctioneer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.garrisoncommander, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.gazlowe, 6);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.grimestreetenforcer, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.grimscaleoracle, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.grimygadgeteer, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.gruul, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.hallazealtheascended, 16);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.healingtotem, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.hobgoblin, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.hogger, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.homingchicken, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.illidanstormrage, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.illuminator, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.impmaster, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.ironsensei, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.jeeves, 0);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.junkbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.kabaltrafficker, 1);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.kelthuzad, 18);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.knifejuggler, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.kodorider, 20);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.kvaldirraider, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.leokk, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.lightwarden, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.lightwell, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.lyrathesunshard, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.maidenofthelake, 18);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.malchezaarsimp, 0);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.malganis, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.manatidetotem, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.manawyrm, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.masterswordsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.mechwarper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.mekgineerthermaplugg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.micromachine, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.moroes, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.muklaschampion, 14);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.murlocknight, 16);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.murloctidecaller, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.murlocwarleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.natpagle, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.nexuschampionsaraad, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.northshirecleric, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.obsidiandestroyer, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.pintsizedsummoner, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.priestofthefeast, 3);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.primalfintotem, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.prophetvelen, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.questingadventurer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.radiantelemental, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.ragnaroslightlord, 19);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.ragnarosthefirelord, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.raidleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.recruiter, 15);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.redmanawyrm, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.repairbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.rumblingelemental, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.scavenginghyena, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.secretkeeper, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.shadeofnaxxramas, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.shadowboxer, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.shakuthecollector, 25);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.shipscannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.siegeengine, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.siltfinspiritwalker, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.silverhandregent, 14);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.sorcerersapprentice, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.southseacaptain, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.spiritsingerumbra, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.starvingbuzzard, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.stonesplintertrogg, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.stormwindchampion, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.summoningportal, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.summoningstone, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.thunderbluffvaliant, 16);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.timberwolf, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.tradeprincegallywix, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.troggzortheearthinator, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.twilightelder, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.undertaker, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.usherofsouls, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.viciousfledgling, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.violetillusionist, 14);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.violetteacher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.vitalitytotem, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.warhorsetrainer, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.warsongcommander, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.weespellstopper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.wickedwitchdoctor, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.wilfredfizzlebang, 16);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.windupburglebot, 5);
            //UsefulNeedKeepDatabase.Add(CardDB.cardName.youngpriestess, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.shadowascendant, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.festergut, 25);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.drakkarienchanter, 17);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.despicabledreadlord, 14);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.cobaltscalebane, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.valkyrsoulclaimer, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.runeforgehaunter, 1);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.rotface, 26);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.necroticgeist, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.moorabi, 16);
            UsefulNeedKeepDatabase.Add(CardDB.cardNameEN.icewalker, 15);
        }

        private void setupDiscardCards()
        {
            cardDiscardDatabase.Add(CardDB.cardNameEN.darkbargain, 6);
            cardDiscardDatabase.Add(CardDB.cardNameEN.darkshirelibrarian, 2);
            cardDiscardDatabase.Add(CardDB.cardNameEN.doomguard, 5);
            cardDiscardDatabase.Add(CardDB.cardNameEN.lakkarifelhound, 4);
            cardDiscardDatabase.Add(CardDB.cardNameEN.soulfire, 1);
            cardDiscardDatabase.Add(CardDB.cardNameEN.felstalker, 2);
        }

        private void setupDestroyOwnCards()
        {
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.brawl, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.deathwing, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.golakkacrawler, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.hungrycrab, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.kingmosh, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.naturalize, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.ravenouspterrordax, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.sacrificialpact, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.siphonsoul, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.shadowflame, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.twistingnether, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.unwillingsacrifice, 0);
            this.destroyOwnDatabase.Add(CardDB.cardNameEN.sanguinereveler, 0);

            this.destroyDatabase.Add(CardDB.cardNameEN.assassinate, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.biggamehunter, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.bladeofcthun, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.blastcrystalpotion, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.bookwyrm, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.corruption, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.crush, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.darkbargain, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.deadlyshot, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.doom, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.drakkisathscommand, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.enterthecoliseum, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.execute, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.hemetnesingwary, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.mindcontrol, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.moatlurker, 1);
            this.destroyDatabase.Add(CardDB.cardNameEN.mulch, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.necroticpoison, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.rendblackhand, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.sabotage, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.shadowworddeath, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.shadowwordhorror, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.shadowwordpain, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.shatter, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.theblackknight, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardNameEN.thetruewarchief, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.vilespineslayer, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.voidcrusher, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.obsidianstatue, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.obliterate, 0);
            this.destroyDatabase.Add(CardDB.cardNameEN.doompact, 0);
        }

        private void setupReturnBackToHandCards()
        {
            returnHandDatabase.Add(CardDB.cardNameEN.ancientbrewmaster, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.bloodthistletoxin, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.dream, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.gadgetzanferryman, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.kidnapper, 0);//if combo
            returnHandDatabase.Add(CardDB.cardNameEN.recycle, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.shadowstep, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.timerewinder, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.vanish, 0);
            returnHandDatabase.Add(CardDB.cardNameEN.youthfulbrewmaster, 0);
        }

        private void setupHeroDamagingAOE()
        {
            this.heroDamagingAoeDatabase.Add(CardDB.cardNameEN.unknown, 0);
        }

        private void setupSpecialMins()
        {
            //specialMinions.Add(CardDB.cardName.aberrantberserker, 0); //畸变狂战士
            //specialMinions.Add(CardDB.cardName.abomination, 0);  //憎恶
            specialMinions.Add(CardDB.cardNameEN.acidmaw, 0);  //  酸喉
            //specialMinions.Add(CardDB.cardName.acolyteofpain, 0); //苦痛侍僧  Todo 可以考虑
            specialMinions.Add(CardDB.cardNameEN.addledgrizzly, 0); //腐化灰熊
            specialMinions.Add(CardDB.cardNameEN.alarmobot, 0); //报警机器人
            specialMinions.Add(CardDB.cardNameEN.amaniberserker, 0); //狂战士
            specialMinions.Add(CardDB.cardNameEN.ancientharbinger, 0); //上古之神先驱
            specialMinions.Add(CardDB.cardNameEN.angrychicken, 0);
            specialMinions.Add(CardDB.cardNameEN.animatedarmor, 0);
            specialMinions.Add(CardDB.cardNameEN.anubarak, 0);
            specialMinions.Add(CardDB.cardNameEN.anubarambusher, 0);
            specialMinions.Add(CardDB.cardNameEN.anubisathsentinel, 0);
            specialMinions.Add(CardDB.cardNameEN.arcaneanomaly, 0);
            specialMinions.Add(CardDB.cardNameEN.arcaneflakmage, 0); //对空奥术法师
            specialMinions.Add(CardDB.cardNameEN.archmage, 0);
            specialMinions.Add(CardDB.cardNameEN.archmageantonidas, 0);
            specialMinions.Add(CardDB.cardNameEN.armorsmith, 0);
            specialMinions.Add(CardDB.cardNameEN.auchenaisoulpriest, 0);
            specialMinions.Add(CardDB.cardNameEN.aviana, 0);
            specialMinions.Add(CardDB.cardNameEN.axeflinger, 0);
            specialMinions.Add(CardDB.cardNameEN.ayablackpaw, 0);
            specialMinions.Add(CardDB.cardNameEN.azuredrake, 0);
            specialMinions.Add(CardDB.cardNameEN.backroombouncer, 0);
            specialMinions.Add(CardDB.cardNameEN.backstreetleper, 0);
            specialMinions.Add(CardDB.cardNameEN.barongeddon, 0);
            specialMinions.Add(CardDB.cardNameEN.baronrivendare, 0);
            specialMinions.Add(CardDB.cardNameEN.blackwaterpirate, 0);
            specialMinions.Add(CardDB.cardNameEN.bloodhoofbrave, 0);
            specialMinions.Add(CardDB.cardNameEN.bloodimp, 0);
            specialMinions.Add(CardDB.cardNameEN.bloodmagethalnos, 0);
            specialMinions.Add(CardDB.cardNameEN.bolframshield, 0);
            specialMinions.Add(CardDB.cardNameEN.boneguardlieutenant, 0);
            specialMinions.Add(CardDB.cardNameEN.brannbronzebeard, 0);
            specialMinions.Add(CardDB.cardNameEN.bravearcher, 0);
            specialMinions.Add(CardDB.cardNameEN.buccaneer, 0);
            specialMinions.Add(CardDB.cardNameEN.burglybully, 0);
            specialMinions.Add(CardDB.cardNameEN.burlyrockjawtrogg, 0);
            specialMinions.Add(CardDB.cardNameEN.cairnebloodhoof, 0);
            specialMinions.Add(CardDB.cardNameEN.chromaggus, 0);
            specialMinions.Add(CardDB.cardNameEN.chromaticdragonkin, 0);
            specialMinions.Add(CardDB.cardNameEN.cloakedhuntress, 0);
            specialMinions.Add(CardDB.cardNameEN.clockworkgnome, 0);
            specialMinions.Add(CardDB.cardNameEN.cobaltguardian, 0);
            specialMinions.Add(CardDB.cardNameEN.cogmaster, 0);
            specialMinions.Add(CardDB.cardNameEN.coldarradrake, 0);
            specialMinions.Add(CardDB.cardNameEN.coliseummanager, 0);
            specialMinions.Add(CardDB.cardNameEN.confessorpaletress, 0);
            specialMinions.Add(CardDB.cardNameEN.crabrider,0);// 螃蟹骑士 
            specialMinions.Add(CardDB.cardNameEN.crazedworshipper, 0);
            specialMinions.Add(CardDB.cardNameEN.crowdfavorite, 0);
            specialMinions.Add(CardDB.cardNameEN.crueldinomancer, 0);
            specialMinions.Add(CardDB.cardNameEN.crystallineoracle, 0);
            specialMinions.Add(CardDB.cardNameEN.cthun, 0);
            specialMinions.Add(CardDB.cardNameEN.cultmaster, 0);
            specialMinions.Add(CardDB.cardNameEN.cultsorcerer, 0);
            specialMinions.Add(CardDB.cardNameEN.cutpurse, 0);
            specialMinions.Add(CardDB.cardNameEN.dalaranaspirant, 0);
            specialMinions.Add(CardDB.cardNameEN.dalaranmage, 0);
            specialMinions.Add(CardDB.cardNameEN.dancingswords, 0);
            specialMinions.Add(CardDB.cardNameEN.darkcultist, 0);
            specialMinions.Add(CardDB.cardNameEN.darkshirecouncilman, 0);
            specialMinions.Add(CardDB.cardNameEN.deadlyfork, 0);
            specialMinions.Add(CardDB.cardNameEN.deathlord, 0);
            specialMinions.Add(CardDB.cardNameEN.dementedfrostcaller, 0);
            specialMinions.Add(CardDB.cardNameEN.demolisher, 0);
            specialMinions.Add(CardDB.cardNameEN.direhornhatchling, 0);
            specialMinions.Add(CardDB.cardNameEN.direwolfalpha, 0);
            specialMinions.Add(CardDB.cardNameEN.djinniofzephyrs, 0);
            specialMinions.Add(CardDB.cardNameEN.doomsayer, 0);
            specialMinions.Add(CardDB.cardNameEN.dragonegg, 0);
            specialMinions.Add(CardDB.cardNameEN.dragonhawkrider, 0);
            specialMinions.Add(CardDB.cardNameEN.dragonkinsorcerer, 0);
            specialMinions.Add(CardDB.cardNameEN.dreadscale, 0);
            specialMinions.Add(CardDB.cardNameEN.dreadsteed, 0);
            specialMinions.Add(CardDB.cardNameEN.eggnapper, 0);
            specialMinions.Add(CardDB.cardNameEN.emperorcobra, 0);
            specialMinions.Add(CardDB.cardNameEN.emperorthaurissan, 0);
            specialMinions.Add(CardDB.cardNameEN.etherealarcanist, 0);
            specialMinions.Add(CardDB.cardNameEN.evolvedkobold, 0);
            specialMinions.Add(CardDB.cardNameEN.explosivesheep, 0);
            specialMinions.Add(CardDB.cardNameEN.eydisdarkbane, 0);
            specialMinions.Add(CardDB.cardNameEN.fallenhero, 0);
            specialMinions.Add(CardDB.cardNameEN.fandralstaghelm, 0);
            specialMinions.Add(CardDB.cardNameEN.felcannon, 0);
            specialMinions.Add(CardDB.cardNameEN.feugen, 0);
            specialMinions.Add(CardDB.cardNameEN.finjatheflyingstar, 0);
            specialMinions.Add(CardDB.cardNameEN.fjolalightbane, 0);
            specialMinions.Add(CardDB.cardNameEN.flametonguetotem, 0);
            specialMinions.Add(CardDB.cardNameEN.flamewaker, 0); //火妖
            specialMinions.Add(CardDB.cardNameEN.flesheatingghoul, 0);
            specialMinions.Add(CardDB.cardNameEN.floatingwatcher, 0);
            specialMinions.Add(CardDB.cardNameEN.foereaper4000, 0);
            specialMinions.Add(CardDB.cardNameEN.gadgetzanauctioneer, 0);
            specialMinions.Add(CardDB.cardNameEN.gahzrilla, 0);
            specialMinions.Add(CardDB.cardNameEN.garr, 0);
            specialMinions.Add(CardDB.cardNameEN.garrisoncommander, 0);
            specialMinions.Add(CardDB.cardNameEN.gazlowe, 0);
            specialMinions.Add(CardDB.cardNameEN.genzotheshark, 0);
            specialMinions.Add(CardDB.cardNameEN.giantanaconda, 0);
            specialMinions.Add(CardDB.cardNameEN.giantsandworm, 0);
            specialMinions.Add(CardDB.cardNameEN.giantwasp, 0);
            specialMinions.Add(CardDB.cardNameEN.goblinsapper, 0);
            specialMinions.Add(CardDB.cardNameEN.grimestreetenforcer, 0);
            specialMinions.Add(CardDB.cardNameEN.grimpatron, 0);
            specialMinions.Add(CardDB.cardNameEN.grimscaleoracle, 0);
            specialMinions.Add(CardDB.cardNameEN.grimygadgeteer, 0);
            specialMinions.Add(CardDB.cardNameEN.grommashhellscream, 0);
            specialMinions.Add(CardDB.cardNameEN.gruul, 0);
            specialMinions.Add(CardDB.cardNameEN.gurubashiberserker, 0);
            specialMinions.Add(CardDB.cardNameEN.hallazealtheascended, 0);
            specialMinions.Add(CardDB.cardNameEN.harvestgolem, 0);
            specialMinions.Add(CardDB.cardNameEN.hauntedcreeper, 0);
            specialMinions.Add(CardDB.cardNameEN.hobgoblin, 0);
            specialMinions.Add(CardDB.cardNameEN.hogger, 0);
            specialMinions.Add(CardDB.cardNameEN.hoggerdoomofelwynn, 0);
            specialMinions.Add(CardDB.cardNameEN.holychampion, 0);
            specialMinions.Add(CardDB.cardNameEN.hoodedacolyte, 0);
            specialMinions.Add(CardDB.cardNameEN.hugetoad, 0);
            specialMinions.Add(CardDB.cardNameEN.igneouselemental, 0);
            specialMinions.Add(CardDB.cardNameEN.illidanstormrage, 0);
            specialMinions.Add(CardDB.cardNameEN.impgangboss, 0);
            specialMinions.Add(CardDB.cardNameEN.impmaster, 0);
            specialMinions.Add(CardDB.cardNameEN.infestedtauren, 0);
            specialMinions.Add(CardDB.cardNameEN.infestedwolf, 0);
            specialMinions.Add(CardDB.cardNameEN.ironsensei, 0);
            specialMinions.Add(CardDB.cardNameEN.jadeswarmer, 0);
            specialMinions.Add(CardDB.cardNameEN.jeeves, 0);
            specialMinions.Add(CardDB.cardNameEN.junglemoonkin, 0);
            specialMinions.Add(CardDB.cardNameEN.junkbot, 0);
            specialMinions.Add(CardDB.cardNameEN.kabaltrafficker, 0);
            specialMinions.Add(CardDB.cardNameEN.kelthuzad, 0);
            specialMinions.Add(CardDB.cardNameEN.kindlygrandmother, 0);
            specialMinions.Add(CardDB.cardNameEN.knifejuggler, 0);
            specialMinions.Add(CardDB.cardNameEN.knuckles, 0);
            specialMinions.Add(CardDB.cardNameEN.koboldgeomancer, 0);
            specialMinions.Add(CardDB.cardNameEN.kodorider, 0);
            specialMinions.Add(CardDB.cardNameEN.kvaldirraider, 0);
            specialMinions.Add(CardDB.cardNameEN.lepergnome, 0);
            specialMinions.Add(CardDB.cardNameEN.lightspawn, 0);
            specialMinions.Add(CardDB.cardNameEN.lightwarden, 0);
            specialMinions.Add(CardDB.cardNameEN.lightwell, 0);
            specialMinions.Add(CardDB.cardNameEN.loothoarder, 0);
            specialMinions.Add(CardDB.cardNameEN.lorewalkercho, 0);
            specialMinions.Add(CardDB.cardNameEN.lotusassassin, 0);
            specialMinions.Add(CardDB.cardNameEN.lowlysquire, 0);
            specialMinions.Add(CardDB.cardNameEN.lyrathesunshard, 0);
            specialMinions.Add(CardDB.cardNameEN.maexxna, 0);
            specialMinions.Add(CardDB.cardNameEN.magnatauralpha, 0);
            specialMinions.Add(CardDB.cardNameEN.maidenofthelake, 0);
            specialMinions.Add(CardDB.cardNameEN.majordomoexecutus, 0);
            specialMinions.Add(CardDB.cardNameEN.malchezaarsimp, 0);
            specialMinions.Add(CardDB.cardNameEN.malganis, 0);
            specialMinions.Add(CardDB.cardNameEN.malorne, 0);
            specialMinions.Add(CardDB.cardNameEN.malygos, 0);
            specialMinions.Add(CardDB.cardNameEN.manaaddict, 0);
            specialMinions.Add(CardDB.cardNameEN.manageode, 0);
            specialMinions.Add(CardDB.cardNameEN.manatidetotem, 0);
            specialMinions.Add(CardDB.cardNameEN.manatreant, 0);
            specialMinions.Add(CardDB.cardNameEN.manawraith, 0);
            specialMinions.Add(CardDB.cardNameEN.manawyrm, 0);
            specialMinions.Add(CardDB.cardNameEN.masterswordsmith, 0);
            specialMinions.Add(CardDB.cardNameEN.mechanicalyeti, 0);
            specialMinions.Add(CardDB.cardNameEN.mechbearcat, 0);
            specialMinions.Add(CardDB.cardNameEN.mechwarper, 0);
            specialMinions.Add(CardDB.cardNameEN.mekgineerthermaplugg, 0);
            specialMinions.Add(CardDB.cardNameEN.micromachine, 0);
            specialMinions.Add(CardDB.cardNameEN.mimironshead, 0);
            specialMinions.Add(CardDB.cardNameEN.queenofpain, 0);
            specialMinions.Add(CardDB.cardNameEN.moroes, 0);
            specialMinions.Add(CardDB.cardNameEN.mozakimasterduelist, 0); // 决斗大师莫扎奇  法师otk卡组核心牌
            specialMinions.Add(CardDB.cardNameEN.muklaschampion, 0);
            specialMinions.Add(CardDB.cardNameEN.murlocknight, 0);
            specialMinions.Add(CardDB.cardNameEN.murloctidecaller, 0);
            specialMinions.Add(CardDB.cardNameEN.murlocwarleader, 0);
            specialMinions.Add(CardDB.cardNameEN.natpagle, 0);
            specialMinions.Add(CardDB.cardNameEN.nerubarweblord, 0);
            specialMinions.Add(CardDB.cardNameEN.nexuschampionsaraad, 0);
            specialMinions.Add(CardDB.cardNameEN.northshirecleric, 0);
            specialMinions.Add(CardDB.cardNameEN.obsidiandestroyer, 0);
            specialMinions.Add(CardDB.cardNameEN.ogremagi, 0);
            specialMinions.Add(CardDB.cardNameEN.oldmurkeye, 0);
            specialMinions.Add(CardDB.cardNameEN.orgrimmaraspirant, 0);
            specialMinions.Add(CardDB.cardNameEN.patientassassin, 0);
            specialMinions.Add(CardDB.cardNameEN.pilotedshredder, 0);
            specialMinions.Add(CardDB.cardNameEN.pilotedskygolem, 0);
            specialMinions.Add(CardDB.cardNameEN.pintsizedsummoner, 0);
            specialMinions.Add(CardDB.cardNameEN.pitsnake, 0);
            specialMinions.Add(CardDB.cardNameEN.possessedvillager, 0);
            specialMinions.Add(CardDB.cardNameEN.priestofthefeast, 0);
            specialMinions.Add(CardDB.cardNameEN.primalfinchampion, 0);
            specialMinions.Add(CardDB.cardNameEN.primalfintotem, 0);
            specialMinions.Add(CardDB.cardNameEN.prophetvelen, 0);
            specialMinions.Add(CardDB.cardNameEN.pyros, 0);
            specialMinions.Add(CardDB.cardNameEN.questingadventurer, 0);
            specialMinions.Add(CardDB.cardNameEN.radiantelemental, 0);
            specialMinions.Add(CardDB.cardNameEN.ragingworgen, 0);
            specialMinions.Add(CardDB.cardNameEN.ragnaroslightlord, 0);
            specialMinions.Add(CardDB.cardNameEN.raidleader, 0);
            specialMinions.Add(CardDB.cardNameEN.raptorhatchling, 0);
            specialMinions.Add(CardDB.cardNameEN.ratpack, 0);
            specialMinions.Add(CardDB.cardNameEN.recruiter, 0);
            specialMinions.Add(CardDB.cardNameEN.redmanawyrm, 0);
            specialMinions.Add(CardDB.cardNameEN.rumblingelemental, 0);
            specialMinions.Add(CardDB.cardNameEN.satedthreshadon, 0);
            specialMinions.Add(CardDB.cardNameEN.savagecombatant, 0);
            specialMinions.Add(CardDB.cardNameEN.savannahhighmane, 0);
            specialMinions.Add(CardDB.cardNameEN.scalednightmare, 0);
            specialMinions.Add(CardDB.cardNameEN.scavenginghyena, 0);
            specialMinions.Add(CardDB.cardNameEN.secretkeeper, 0);
            specialMinions.Add(CardDB.cardNameEN.selflesshero, 0);
            specialMinions.Add(CardDB.cardNameEN.sergeantsally, 0);
            specialMinions.Add(CardDB.cardNameEN.shadeofnaxxramas, 0);
            specialMinions.Add(CardDB.cardNameEN.shadowboxer, 0);
            specialMinions.Add(CardDB.cardNameEN.shadowfiend, 0);
            specialMinions.Add(CardDB.cardNameEN.shakuthecollector, 0);
            specialMinions.Add(CardDB.cardNameEN.sherazincorpseflower, 0);
            specialMinions.Add(CardDB.cardNameEN.shiftingshade, 0);
            specialMinions.Add(CardDB.cardNameEN.shimmeringtempest, 0);
            specialMinions.Add(CardDB.cardNameEN.shipscannon, 0);
            specialMinions.Add(CardDB.cardNameEN.siltfinspiritwalker, 0);
            specialMinions.Add(CardDB.cardNameEN.silverhandregent, 0);
            specialMinions.Add(CardDB.cardNameEN.smalltimebuccaneer, 0);
            specialMinions.Add(CardDB.cardNameEN.sneedsoldshredder, 0);
            specialMinions.Add(CardDB.cardNameEN.snowchugger, 0);
            specialMinions.Add(CardDB.cardNameEN.sorcerersapprentice, 0);
            specialMinions.Add(CardDB.cardNameEN.southseacaptain, 0);
            specialMinions.Add(CardDB.cardNameEN.southseasquidface, 0);
            specialMinions.Add(CardDB.cardNameEN.spawnofnzoth, 0);
            specialMinions.Add(CardDB.cardNameEN.spawnofshadows, 0);
            specialMinions.Add(CardDB.cardNameEN.spiritsingerumbra, 0);
            specialMinions.Add(CardDB.cardNameEN.spitefulsmith, 0);
            specialMinions.Add(CardDB.cardNameEN.stalagg, 0);
            specialMinions.Add(CardDB.cardNameEN.starvingbuzzard, 0);
            specialMinions.Add(CardDB.cardNameEN.steamwheedlesniper, 0);
            specialMinions.Add(CardDB.cardNameEN.stewardofdarkshire, 0);
            specialMinions.Add(CardDB.cardNameEN.stonesplintertrogg, 0);
            specialMinions.Add(CardDB.cardNameEN.stormwindchampion, 0);
            specialMinions.Add(CardDB.cardNameEN.summoningportal, 0);
            specialMinions.Add(CardDB.cardNameEN.summoningstone, 0);
            specialMinions.Add(CardDB.cardNameEN.sylvanaswindrunner, 0);
            specialMinions.Add(CardDB.cardNameEN.tarcreeper, 0);
            specialMinions.Add(CardDB.cardNameEN.tarlord, 0);
            specialMinions.Add(CardDB.cardNameEN.tarlurker, 0);
            specialMinions.Add(CardDB.cardNameEN.taurenwarrior, 0);
            specialMinions.Add(CardDB.cardNameEN.tentacleofnzoth, 0);
            specialMinions.Add(CardDB.cardNameEN.thebeast, 0);
            specialMinions.Add(CardDB.cardNameEN.theboogeymonster, 0);
            specialMinions.Add(CardDB.cardNameEN.thevoraxx, 0);
            specialMinions.Add(CardDB.cardNameEN.thunderbluffvaliant, 0);
            specialMinions.Add(CardDB.cardNameEN.timberwolf, 0);
            specialMinions.Add(CardDB.cardNameEN.tinyknightofevil, 0);
            specialMinions.Add(CardDB.cardNameEN.tirionfordring, 0);
            specialMinions.Add(CardDB.cardNameEN.tortollanshellraiser, 0);
            specialMinions.Add(CardDB.cardNameEN.toshley, 0);
            specialMinions.Add(CardDB.cardNameEN.tournamentmedic, 0);
            specialMinions.Add(CardDB.cardNameEN.tradeprincegallywix, 0);
            specialMinions.Add(CardDB.cardNameEN.troggzortheearthinator, 0);
            specialMinions.Add(CardDB.cardNameEN.tundrarhino, 0);
            specialMinions.Add(CardDB.cardNameEN.tunneltrogg, 0);
            specialMinions.Add(CardDB.cardNameEN.twilightelder, 0);
            specialMinions.Add(CardDB.cardNameEN.twilightsummoner, 0);
            specialMinions.Add(CardDB.cardNameEN.unboundelemental, 0);
            specialMinions.Add(CardDB.cardNameEN.undercityhuckster, 0);
            specialMinions.Add(CardDB.cardNameEN.undertaker, 0);
            specialMinions.Add(CardDB.cardNameEN.unstableghoul, 0);
            specialMinions.Add(CardDB.cardNameEN.usherofsouls, 0);
            specialMinions.Add(CardDB.cardNameEN.viciousfledgling, 0);
            specialMinions.Add(CardDB.cardNameEN.violetillusionist, 0);
            specialMinions.Add(CardDB.cardNameEN.violetteacher, 0);
            specialMinions.Add(CardDB.cardNameEN.vitalitytotem, 0);
            specialMinions.Add(CardDB.cardNameEN.voidcaller, 0);
            specialMinions.Add(CardDB.cardNameEN.voidcrusher, 0);
            specialMinions.Add(CardDB.cardNameEN.volatileelemental, 0);
            specialMinions.Add(CardDB.cardNameEN.warbot, 0);
            specialMinions.Add(CardDB.cardNameEN.warhorsetrainer, 0);
            specialMinions.Add(CardDB.cardNameEN.warsongcommander, 0);
            specialMinions.Add(CardDB.cardNameEN.waterelemental, 0);
            specialMinions.Add(CardDB.cardNameEN.voodoohexxer, 0);
            specialMinions.Add(CardDB.cardNameEN.webspinner, 0);
            specialMinions.Add(CardDB.cardNameEN.whiteeyes, 0);
            specialMinions.Add(CardDB.cardNameEN.wickedwitchdoctor, 0);
            specialMinions.Add(CardDB.cardNameEN.wickerflameburnbristle, 0);
            specialMinions.Add(CardDB.cardNameEN.wilfredfizzlebang, 0);
            specialMinions.Add(CardDB.cardNameEN.windupburglebot, 0);
            specialMinions.Add(CardDB.cardNameEN.wobblingrunts, 0);
            specialMinions.Add(CardDB.cardNameEN.xarilpoisonedmind, 0);
            specialMinions.Add(CardDB.cardNameEN.ysera, 0);
            specialMinions.Add(CardDB.cardNameEN.yshaarjrageunbound, 0);
            specialMinions.Add(CardDB.cardNameEN.zealousinitiate, 0);
            specialMinions.Add(CardDB.cardNameEN.vryghoul, 0);
            specialMinions.Add(CardDB.cardNameEN.thelichking, 0);
            specialMinions.Add(CardDB.cardNameEN.skelemancer, 0);
            specialMinions.Add(CardDB.cardNameEN.shallowgravedigger, 0);
            specialMinions.Add(CardDB.cardNameEN.shadowascendant, 0);
            specialMinions.Add(CardDB.cardNameEN.obsidianstatue, 0);
            specialMinions.Add(CardDB.cardNameEN.mountainfirearmor, 0);
            specialMinions.Add(CardDB.cardNameEN.meatwagon, 0);
            specialMinions.Add(CardDB.cardNameEN.hadronox, 0);
            specialMinions.Add(CardDB.cardNameEN.festergut, 0);
            specialMinions.Add(CardDB.cardNameEN.fatespinner, 0);
            specialMinions.Add(CardDB.cardNameEN.explodingbloatbat, 0);
            specialMinions.Add(CardDB.cardNameEN.drakkarienchanter, 0);
            specialMinions.Add(CardDB.cardNameEN.despicabledreadlord, 0);
            specialMinions.Add(CardDB.cardNameEN.deadscaleknight, 0);
            specialMinions.Add(CardDB.cardNameEN.cobaltscalebane, 0);
            specialMinions.Add(CardDB.cardNameEN.chillbladechampion, 0);
            specialMinions.Add(CardDB.cardNameEN.bonedrake, 0);
            specialMinions.Add(CardDB.cardNameEN.bonebaron, 0);
            specialMinions.Add(CardDB.cardNameEN.bloodworm, 0);
            specialMinions.Add(CardDB.cardNameEN.bloodqueenlanathel, 0);
            specialMinions.Add(CardDB.cardNameEN.blackguard, 0);
            specialMinions.Add(CardDB.cardNameEN.arrogantcrusader, 0);
            specialMinions.Add(CardDB.cardNameEN.arfus, 0);
            specialMinions.Add(CardDB.cardNameEN.acolyteofagony, 0);
            specialMinions.Add(CardDB.cardNameEN.abominablebowman, 0);
            specialMinions.Add(CardDB.cardNameEN.venomancer, 0);
            specialMinions.Add(CardDB.cardNameEN.valkyrsoulclaimer, 0);
            specialMinions.Add(CardDB.cardNameEN.stubborngastropod, 0);
            specialMinions.Add(CardDB.cardNameEN.runeforgehaunter, 0);
            specialMinions.Add(CardDB.cardNameEN.rotface, 0);
            specialMinions.Add(CardDB.cardNameEN.professorputricide, 0);
            specialMinions.Add(CardDB.cardNameEN.nighthowler, 0);
            specialMinions.Add(CardDB.cardNameEN.nerubianunraveler, 0);
            specialMinions.Add(CardDB.cardNameEN.necroticgeist, 0);
            specialMinions.Add(CardDB.cardNameEN.moorabi, 0);
            specialMinions.Add(CardDB.cardNameEN.mindbreaker, 0);
            specialMinions.Add(CardDB.cardNameEN.icewalker, 0);
            specialMinions.Add(CardDB.cardNameEN.graveshambler, 0);
            specialMinions.Add(CardDB.cardNameEN.doomedapprentice, 0);
            specialMinions.Add(CardDB.cardNameEN.cryptlord, 0);
            specialMinions.Add(CardDB.cardNameEN.corpsewidow, 0);
        }

        private void setupOwnSummonFromDeathrattle()
        {
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.anubarak, -10);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.ayablackpaw, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.cairnebloodhoof, 5);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.crueldinomancer, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.devilsauregg, -20);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.dreadsteed, -1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.eggnapper, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.giantanaconda, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.harvestgolem, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.hauntedcreeper, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.infestedtauren, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.infestedwolf, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.jadeswarmer, -1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.kindlygrandmother, -10);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.moirabronzebeard, 3);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.mountedraptor, 3);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.nerubianegg, -16);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.pilotedshredder, 4);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.pilotedskygolem, 4);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.possessedvillager, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.ratpack, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.satedthreshadon, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.savannahhighmane, 8);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.sludgebelcher, 10);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.sneedsoldshredder, 5);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.twilightsummoner, -14);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.whiteeyes, -10);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.wobblingrunts, 1);
            ownSummonFromDeathrattle.Add(CardDB.cardNameEN.frozenchampion, -12);
        }

        private void setupBuffingMinions()
        {
            buffingMinionsDatabase.Add(CardDB.cardNameEN.abusivesergeant, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.beckonerofevil, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.bladeofcthun, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.bloodsailcultist, 5);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.captaingreenskin, 5);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.cenarius, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.clockworkknight, 2);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.coldlightseer, 3);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.crueltaskmaster, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.cthunschosen, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.cultsorcerer, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.darkarakkoa, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.darkirondwarf, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.defenderofargus, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.direwolfalpha, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.discipleofcthun, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.doomcaller, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.flametonguetotem, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.goblinautobarber, 5);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.grimscaleoracle, 3);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.hoodedacolyte, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.houndmaster, 1);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.lancecarrier, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.leokk, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.malganis, 8);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.metaltoothleaper, 2);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.murlocwarleader, 3);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.quartermaster, 6);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.raidleader, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.screwjankclunker, 2);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.shatteredsuncleric, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.skeramcultist, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.southseacaptain, 4);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.spitefulsmith, 5);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.stormwindchampion, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.templeenforcer, 0);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.thunderbluffvaliant, 9);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.timberwolf, 1);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.upgradedrepairbot, 2);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.usherofsouls, 10);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.warhorsetrainer, 6);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.warsongcommander, 7);
            buffingMinionsDatabase.Add(CardDB.cardNameEN.worshipper, 0);

            buffing1TurnDatabase.Add(CardDB.cardNameEN.abusivesergeant, 0);
            buffing1TurnDatabase.Add(CardDB.cardNameEN.bloodlust, 3);
            buffing1TurnDatabase.Add(CardDB.cardNameEN.darkirondwarf, 0);
            buffing1TurnDatabase.Add(CardDB.cardNameEN.rockbiterweapon, 0);
            buffing1TurnDatabase.Add(CardDB.cardNameEN.worshipper, 0);
        }

        private void setupEnemyTargetPriority()
        {
            priorityTargets.Add(CardDB.cardNameEN.acidmaw, 10);//酸吼
            priorityTargets.Add(CardDB.cardNameEN.acolyteofpain, 10);
            priorityTargets.Add(CardDB.cardNameEN.addledgrizzly, 10);
            priorityTargets.Add(CardDB.cardNameEN.alarmobot, 10);
            priorityTargets.Add(CardDB.cardNameEN.angrychicken, 10);
            priorityTargets.Add(CardDB.cardNameEN.animatedarmor, 10);
            priorityTargets.Add(CardDB.cardNameEN.anubarak, 10);
            priorityTargets.Add(CardDB.cardNameEN.anubisathsentinel, 10);
            priorityTargets.Add(CardDB.cardNameEN.archmageantonidas, 10);
            priorityTargets.Add(CardDB.cardNameEN.auchenaisoulpriest, 10);
            priorityTargets.Add(CardDB.cardNameEN.auctionmasterbeardo, 10);
            priorityTargets.Add(CardDB.cardNameEN.aviana, 10);
            priorityTargets.Add(CardDB.cardNameEN.ayablackpaw, 10);
            priorityTargets.Add(CardDB.cardNameEN.backroombouncer, 10);
            priorityTargets.Add(CardDB.cardNameEN.barongeddon, 10);
            priorityTargets.Add(CardDB.cardNameEN.baronrivendare, 10);
            priorityTargets.Add(CardDB.cardNameEN.bloodmagethalnos, 10);
            priorityTargets.Add(CardDB.cardNameEN.boneguardlieutenant, 10);
            priorityTargets.Add(CardDB.cardNameEN.brannbronzebeard, 10);
            priorityTargets.Add(CardDB.cardNameEN.burglybully, 10);
            priorityTargets.Add(CardDB.cardNameEN.chromaggus, 10);
            priorityTargets.Add(CardDB.cardNameEN.cloakedhuntress, 10);
            priorityTargets.Add(CardDB.cardNameEN.coldarradrake, 10);
            priorityTargets.Add(CardDB.cardNameEN.confessorpaletress, 10);
            priorityTargets.Add(CardDB.cardNameEN.crowdfavorite, 10);
            priorityTargets.Add(CardDB.cardNameEN.crueldinomancer, 10);
            priorityTargets.Add(CardDB.cardNameEN.crystallineoracle, 10);
            priorityTargets.Add(CardDB.cardNameEN.cthun, 10);
            priorityTargets.Add(CardDB.cardNameEN.cultmaster, 10);
            priorityTargets.Add(CardDB.cardNameEN.cultsorcerer, 10);
            priorityTargets.Add(CardDB.cardNameEN.dalaranaspirant, 10);
            priorityTargets.Add(CardDB.cardNameEN.daringreporter, 10);
            priorityTargets.Add(CardDB.cardNameEN.darkshirecouncilman, 10);
            priorityTargets.Add(CardDB.cardNameEN.dementedfrostcaller, 10);
            priorityTargets.Add(CardDB.cardNameEN.demolisher, 10);
            priorityTargets.Add(CardDB.cardNameEN.devilsauregg, 10);
            priorityTargets.Add(CardDB.cardNameEN.nerubianegg, 10);
            priorityTargets.Add(CardDB.cardNameEN.direhornhatchling, 10);
            priorityTargets.Add(CardDB.cardNameEN.direwolfalpha, 10);
            priorityTargets.Add(CardDB.cardNameEN.djinniofzephyrs, 10);
            priorityTargets.Add(CardDB.cardNameEN.doomsayer, 10);
            priorityTargets.Add(CardDB.cardNameEN.dragonegg, 0);
            priorityTargets.Add(CardDB.cardNameEN.dragonhawkrider, 10);
            priorityTargets.Add(CardDB.cardNameEN.dragonkinsorcerer, 4);
            priorityTargets.Add(CardDB.cardNameEN.dreadscale, 10);
            priorityTargets.Add(CardDB.cardNameEN.dustdevil, 10);
            priorityTargets.Add(CardDB.cardNameEN.eggnapper, 10);
            priorityTargets.Add(CardDB.cardNameEN.emperorthaurissan, 10);
            priorityTargets.Add(CardDB.cardNameEN.etherealarcanist, 10);
            priorityTargets.Add(CardDB.cardNameEN.eydisdarkbane, 10);
            priorityTargets.Add(CardDB.cardNameEN.fallenhero, 10);
            priorityTargets.Add(CardDB.cardNameEN.masterchest, 10);
            priorityTargets.Add(CardDB.cardNameEN.fandralstaghelm, 10);
            priorityTargets.Add(CardDB.cardNameEN.finjatheflyingstar, 10);
            priorityTargets.Add(CardDB.cardNameEN.flametonguetotem, 10);
            priorityTargets.Add(CardDB.cardNameEN.flamewaker, 10);
            priorityTargets.Add(CardDB.cardNameEN.flesheatingghoul, 10);
            priorityTargets.Add(CardDB.cardNameEN.floatingwatcher, 10);
            priorityTargets.Add(CardDB.cardNameEN.foereaper4000, 10);
            priorityTargets.Add(CardDB.cardNameEN.friendlybartender, 10);
            priorityTargets.Add(CardDB.cardNameEN.frothingberserker, 10);
            priorityTargets.Add(CardDB.cardNameEN.gadgetzanauctioneer, 10);
            priorityTargets.Add(CardDB.cardNameEN.gahzrilla, 10);
            priorityTargets.Add(CardDB.cardNameEN.garr, 10);
            priorityTargets.Add(CardDB.cardNameEN.garrisoncommander, 10);
            priorityTargets.Add(CardDB.cardNameEN.genzotheshark, 10);
            priorityTargets.Add(CardDB.cardNameEN.giantanaconda, 10);
            priorityTargets.Add(CardDB.cardNameEN.giantsandworm, 10);
            priorityTargets.Add(CardDB.cardNameEN.grimestreetenforcer, 10);
            priorityTargets.Add(CardDB.cardNameEN.grimpatron, 10);
            priorityTargets.Add(CardDB.cardNameEN.grimygadgeteer, 10);
            priorityTargets.Add(CardDB.cardNameEN.gurubashiberserker, 10);
            priorityTargets.Add(CardDB.cardNameEN.hobgoblin, 10);
            priorityTargets.Add(CardDB.cardNameEN.hogger, 10);
            priorityTargets.Add(CardDB.cardNameEN.hoggerdoomofelwynn, 10);
            priorityTargets.Add(CardDB.cardNameEN.holychampion, 10);
            priorityTargets.Add(CardDB.cardNameEN.hoodedacolyte, 10);
            priorityTargets.Add(CardDB.cardNameEN.igneouselemental, 10);
            priorityTargets.Add(CardDB.cardNameEN.illidanstormrage, 10);
            priorityTargets.Add(CardDB.cardNameEN.impgangboss, 10);
            priorityTargets.Add(CardDB.cardNameEN.impmaster, 10);
            priorityTargets.Add(CardDB.cardNameEN.ironsensei, 10);
            priorityTargets.Add(CardDB.cardNameEN.junglemoonkin, 10);
            priorityTargets.Add(CardDB.cardNameEN.kabaltrafficker, 10);
            priorityTargets.Add(CardDB.cardNameEN.kelthuzad, 10);
            priorityTargets.Add(CardDB.cardNameEN.knifejuggler, 10);
            priorityTargets.Add(CardDB.cardNameEN.knuckles, 10);
            priorityTargets.Add(CardDB.cardNameEN.koboldgeomancer, 10);
            priorityTargets.Add(CardDB.cardNameEN.kodorider, 10);
            priorityTargets.Add(CardDB.cardNameEN.kvaldirraider, 10);
            priorityTargets.Add(CardDB.cardNameEN.leeroyjenkins, 10);
            priorityTargets.Add(CardDB.cardNameEN.leokk, 10);
            priorityTargets.Add(CardDB.cardNameEN.lightwarden, 10);
            priorityTargets.Add(CardDB.cardNameEN.lightwell, 10);
            priorityTargets.Add(CardDB.cardNameEN.lowlysquire, 10);
            priorityTargets.Add(CardDB.cardNameEN.lyrathesunshard, 10);
            priorityTargets.Add(CardDB.cardNameEN.maexxna, 10);
            priorityTargets.Add(CardDB.cardNameEN.maidenofthelake, 10);
            priorityTargets.Add(CardDB.cardNameEN.malchezaarsimp, 10);
            priorityTargets.Add(CardDB.cardNameEN.malganis, 10);
            priorityTargets.Add(CardDB.cardNameEN.malygos, 10);
            priorityTargets.Add(CardDB.cardNameEN.manaaddict, 10);
            priorityTargets.Add(CardDB.cardNameEN.manatidetotem, 10);
            priorityTargets.Add(CardDB.cardNameEN.manatreant, 10);
            priorityTargets.Add(CardDB.cardNameEN.manawyrm, 10);
            priorityTargets.Add(CardDB.cardNameEN.masterswordsmith, 10);
            priorityTargets.Add(CardDB.cardNameEN.mechwarper, 10);
            priorityTargets.Add(CardDB.cardNameEN.micromachine, 10);
            priorityTargets.Add(CardDB.cardNameEN.mogortheogre, 10);
            priorityTargets.Add(CardDB.cardNameEN.moroes, 10);
            priorityTargets.Add(CardDB.cardNameEN.muklaschampion, 10);
            priorityTargets.Add(CardDB.cardNameEN.murlocknight, 10);
            priorityTargets.Add(CardDB.cardNameEN.natpagle, 10);
            priorityTargets.Add(CardDB.cardNameEN.nerubarweblord, 10);
            priorityTargets.Add(CardDB.cardNameEN.nexuschampionsaraad, 10);
            priorityTargets.Add(CardDB.cardNameEN.northshirecleric, 10);
            priorityTargets.Add(CardDB.cardNameEN.obsidiandestroyer, 10);
            priorityTargets.Add(CardDB.cardNameEN.orgrimmaraspirant, 10);
            priorityTargets.Add(CardDB.cardNameEN.pintsizedsummoner, 10);
            priorityTargets.Add(CardDB.cardNameEN.priestofthefeast, 10);
            priorityTargets.Add(CardDB.cardNameEN.primalfintotem, 10);
            priorityTargets.Add(CardDB.cardNameEN.prophetvelen, 10);
            priorityTargets.Add(CardDB.cardNameEN.pyros, 10);
            priorityTargets.Add(CardDB.cardNameEN.questingadventurer, 10);
            priorityTargets.Add(CardDB.cardNameEN.radiantelemental, 10);
            priorityTargets.Add(CardDB.cardNameEN.ragnaroslightlord, 10);
            priorityTargets.Add(CardDB.cardNameEN.raidleader, 10);
            priorityTargets.Add(CardDB.cardNameEN.raptorhatchling, 10);
            priorityTargets.Add(CardDB.cardNameEN.recruiter, 10);
            priorityTargets.Add(CardDB.cardNameEN.redmanawyrm, 10);
            priorityTargets.Add(CardDB.cardNameEN.rhonin, 10);
            priorityTargets.Add(CardDB.cardNameEN.rumblingelemental, 10);
            priorityTargets.Add(CardDB.cardNameEN.satedthreshadon, 10);
            priorityTargets.Add(CardDB.cardNameEN.savagecombatant, 10);
            priorityTargets.Add(CardDB.cardNameEN.scalednightmare, 10);
            priorityTargets.Add(CardDB.cardNameEN.scavenginghyena, 10);
            priorityTargets.Add(CardDB.cardNameEN.secretkeeper, 10);
            priorityTargets.Add(CardDB.cardNameEN.shadeofnaxxramas, 10);
            priorityTargets.Add(CardDB.cardNameEN.shakuthecollector, 10);
            priorityTargets.Add(CardDB.cardNameEN.sherazincorpseflower, 10);
            priorityTargets.Add(CardDB.cardNameEN.shimmeringtempest, 10);
            priorityTargets.Add(CardDB.cardNameEN.silverhandregent, 10);
            priorityTargets.Add(CardDB.cardNameEN.sorcerersapprentice, 10);
            priorityTargets.Add(CardDB.cardNameEN.spiritsingerumbra, 10);
            priorityTargets.Add(CardDB.cardNameEN.starvingbuzzard, 10);
            priorityTargets.Add(CardDB.cardNameEN.steamwheedlesniper, 10);
            priorityTargets.Add(CardDB.cardNameEN.stormwindchampion, 10);
            priorityTargets.Add(CardDB.cardNameEN.summoningportal, 10);
            priorityTargets.Add(CardDB.cardNameEN.summoningstone, 10);
            priorityTargets.Add(CardDB.cardNameEN.theboogeymonster, 10);
            priorityTargets.Add(CardDB.cardNameEN.thevoraxx, 10);
            priorityTargets.Add(CardDB.cardNameEN.thrallmarfarseer, 10);
            priorityTargets.Add(CardDB.cardNameEN.thunderbluffvaliant, 10);
            priorityTargets.Add(CardDB.cardNameEN.timberwolf, 10);
            priorityTargets.Add(CardDB.cardNameEN.troggzortheearthinator, 10);
            priorityTargets.Add(CardDB.cardNameEN.tundrarhino, 10);
            priorityTargets.Add(CardDB.cardNameEN.tunneltrogg, 10);
            priorityTargets.Add(CardDB.cardNameEN.twilightsummoner, 10);
            priorityTargets.Add(CardDB.cardNameEN.unboundelemental, 10);
            priorityTargets.Add(CardDB.cardNameEN.undertaker, 10);
            priorityTargets.Add(CardDB.cardNameEN.viciousfledgling, 10);
            priorityTargets.Add(CardDB.cardNameEN.violetillusionist, 10);
            priorityTargets.Add(CardDB.cardNameEN.violetteacher, 10);
            priorityTargets.Add(CardDB.cardNameEN.vitalitytotem, 10);
            priorityTargets.Add(CardDB.cardNameEN.warhorsetrainer, 10);
            priorityTargets.Add(CardDB.cardNameEN.warsongcommander, 10);
            priorityTargets.Add(CardDB.cardNameEN.whiteeyes, 10);
            priorityTargets.Add(CardDB.cardNameEN.wickedwitchdoctor, 10);
            priorityTargets.Add(CardDB.cardNameEN.wildpyromancer, 10);
            priorityTargets.Add(CardDB.cardNameEN.wilfredfizzlebang, 10);
            priorityTargets.Add(CardDB.cardNameEN.windupburglebot, 10);
            priorityTargets.Add(CardDB.cardNameEN.youngdragonhawk, 10);
            priorityTargets.Add(CardDB.cardNameEN.yshaarjrageunbound, 10);
            priorityTargets.Add(CardDB.cardNameEN.thelichking, 10);
            priorityTargets.Add(CardDB.cardNameEN.obsidianstatue, 10);
            priorityTargets.Add(CardDB.cardNameEN.moirabronzebeard, 10);
            priorityTargets.Add(CardDB.cardNameEN.highjusticegrimstone, 10);
            priorityTargets.Add(CardDB.cardNameEN.hadronox, 10);
            priorityTargets.Add(CardDB.cardNameEN.festergut, 10);
            priorityTargets.Add(CardDB.cardNameEN.despicabledreadlord, 10);
            priorityTargets.Add(CardDB.cardNameEN.rotface, 10);
            priorityTargets.Add(CardDB.cardNameEN.professorputricide, 10);
            priorityTargets.Add(CardDB.cardNameEN.nighthowler, 10);
            priorityTargets.Add(CardDB.cardNameEN.necroticgeist, 10);
            priorityTargets.Add(CardDB.cardNameEN.moorabi, 10);
            priorityTargets.Add(CardDB.cardNameEN.icewalker, 10);
            priorityTargets.Add(CardDB.cardNameEN.cryptlord, 10);
            priorityTargets.Add(CardDB.cardNameEN.corpsewidow, 10);
            priorityTargets.Add(CardDB.cardNameEN.arcaneflakmage, 10);
            priorityTargets.Add(CardDB.cardNameEN.henchclanthug, 10);
            priorityTargets.Add(CardDB.cardNameEN.priestessoffury, 10);
            priorityTargets.Add(CardDB.cardNameEN.battlefiend, 10);
            priorityTargets.Add(CardDB.cardNameEN.satyroverseer, 10);
            priorityTargets.Add(CardDB.cardNameEN.stargazerluna, 10);
            priorityTargets.Add(CardDB.cardNameEN.healingtotem, 10);
            priorityTargets.Add(CardDB.cardNameEN.wrathofairtotem, 10);
        }

        private void setupLethalHelpMinions()
        {//设置 对伤害有帮助的 随从 法强生物
            //spellpower minions
            lethalHelpers.Add(CardDB.cardNameEN.ancientmage, 0);
            lethalHelpers.Add(CardDB.cardNameEN.arcanotron, 0);
            lethalHelpers.Add(CardDB.cardNameEN.archmage, 0);//大法师
            lethalHelpers.Add(CardDB.cardNameEN.auchenaisoulpriest, 0);
            lethalHelpers.Add(CardDB.cardNameEN.azuredrake, 0);
            lethalHelpers.Add(CardDB.cardNameEN.bloodmagethalnos, 0);
            lethalHelpers.Add(CardDB.cardNameEN.cultsorcerer, 0);
            lethalHelpers.Add(CardDB.cardNameEN.dalaranaspirant, 0);
            lethalHelpers.Add(CardDB.cardNameEN.dalaranmage, 0);
            lethalHelpers.Add(CardDB.cardNameEN.evolvedkobold, 0);
            lethalHelpers.Add(CardDB.cardNameEN.frigidsnobold, 0);
            lethalHelpers.Add(CardDB.cardNameEN.junglemoonkin, 0);
            lethalHelpers.Add(CardDB.cardNameEN.koboldgeomancer, 0);
            lethalHelpers.Add(CardDB.cardNameEN.malygos, 0);
            lethalHelpers.Add(CardDB.cardNameEN.minimage, 0);
            lethalHelpers.Add(CardDB.cardNameEN.ogremagi, 0);
            lethalHelpers.Add(CardDB.cardNameEN.prophetvelen, 0);
            lethalHelpers.Add(CardDB.cardNameEN.sootspewer, 0);
            lethalHelpers.Add(CardDB.cardNameEN.streettrickster, 0);
            lethalHelpers.Add(CardDB.cardNameEN.wrathofairtotem, 0);
            lethalHelpers.Add(CardDB.cardNameEN.tuskarrfisherman, 0);
            lethalHelpers.Add(CardDB.cardNameEN.taintedzealot, 1);//被污染的狂热者
            lethalHelpers.Add(CardDB.cardNameEN.spellweaver, 2);
        }
        
        //设置关系
        private void setupRelations()
        {
            spellDependentDatabase.Add(CardDB.cardNameEN.arcaneanomaly, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.archmageantonidas, 2);
            spellDependentDatabase.Add(CardDB.cardNameEN.burglybully, -1);
            spellDependentDatabase.Add(CardDB.cardNameEN.burlyrockjawtrogg, -1);
            spellDependentDatabase.Add(CardDB.cardNameEN.chromaticdragonkin, -1);
            spellDependentDatabase.Add(CardDB.cardNameEN.cultsorcerer, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.dementedfrostcaller, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.djinniofzephyrs, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.flamewaker, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.gadgetzanauctioneer, 2);
            spellDependentDatabase.Add(CardDB.cardNameEN.gazlowe, 2);
            spellDependentDatabase.Add(CardDB.cardNameEN.hallazealtheascended, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.lorewalkercho, 0);
            spellDependentDatabase.Add(CardDB.cardNameEN.lyrathesunshard, 2);
            spellDependentDatabase.Add(CardDB.cardNameEN.manaaddict, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.manawyrm, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.priestofthefeast, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.redmanawyrm, 1);
            spellDependentDatabase.Add(CardDB.cardNameEN.stonesplintertrogg, -1);
            spellDependentDatabase.Add(CardDB.cardNameEN.summoningstone, 3);
            spellDependentDatabase.Add(CardDB.cardNameEN.tradeprincegallywix, -1);
            spellDependentDatabase.Add(CardDB.cardNameEN.troggzortheearthinator, -2);
            spellDependentDatabase.Add(CardDB.cardNameEN.violetteacher, 3);
            spellDependentDatabase.Add(CardDB.cardNameEN.wickedwitchdoctor, 3);
            spellDependentDatabase.Add(CardDB.cardNameEN.wildpyromancer, 1);

            dragonDependentDatabase.Add(CardDB.cardNameEN.alexstraszaschampion, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.blackwingcorruptor, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.blackwingtechnician, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.bookwyrm, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.drakonidoperative, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.netherspitehistorian, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.nightbanetemplar, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.rendblackhand, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.twilightguardian, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.twilightwhelp, 1);
            dragonDependentDatabase.Add(CardDB.cardNameEN.wyrmrestagent, 1);

            elementalLTDependentDatabase.Add(CardDB.cardNameEN.blazecaller, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.kalimosprimallord, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.ozruk, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.servantofkalimos, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.steamsurger, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.stonesentinel, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.thunderlizard, 1);
            elementalLTDependentDatabase.Add(CardDB.cardNameEN.tolvirstoneshaper, 1);
        }

        private void setupSilenceTargets()
        {
            silenceTargets.Add(CardDB.cardNameEN.abomination, 0);
            silenceTargets.Add(CardDB.cardNameEN.acidmaw, 0);
            silenceTargets.Add(CardDB.cardNameEN.acolyteofpain, 0);
            silenceTargets.Add(CardDB.cardNameEN.addledgrizzly, 0);
            silenceTargets.Add(CardDB.cardNameEN.ancientharbinger, 0);
            silenceTargets.Add(CardDB.cardNameEN.animatedarmor, 0);
            silenceTargets.Add(CardDB.cardNameEN.anomalus, 0);
            silenceTargets.Add(CardDB.cardNameEN.anubarak, 0);
            silenceTargets.Add(CardDB.cardNameEN.anubisathsentinel, 0);
            silenceTargets.Add(CardDB.cardNameEN.archmageantonidas, 0);
            silenceTargets.Add(CardDB.cardNameEN.armorsmith, 0);
            silenceTargets.Add(CardDB.cardNameEN.auchenaisoulpriest, 0);
            silenceTargets.Add(CardDB.cardNameEN.aviana, 0);
            silenceTargets.Add(CardDB.cardNameEN.axeflinger, 0);
            silenceTargets.Add(CardDB.cardNameEN.ayablackpaw, 0);
            silenceTargets.Add(CardDB.cardNameEN.backroombouncer, 0);
            silenceTargets.Add(CardDB.cardNameEN.barongeddon, 0);
            silenceTargets.Add(CardDB.cardNameEN.baronrivendare, 0);
            silenceTargets.Add(CardDB.cardNameEN.blackwaterpirate, 0);
            silenceTargets.Add(CardDB.cardNameEN.bloodimp, 0);
            silenceTargets.Add(CardDB.cardNameEN.blubberbaron, 0);
            silenceTargets.Add(CardDB.cardNameEN.bolvarfordragon, 0);
            silenceTargets.Add(CardDB.cardNameEN.boneguardlieutenant, 0);
            silenceTargets.Add(CardDB.cardNameEN.brannbronzebeard, 0);
            silenceTargets.Add(CardDB.cardNameEN.bravearcher, 0);
            silenceTargets.Add(CardDB.cardNameEN.burlyrockjawtrogg, 0);
            silenceTargets.Add(CardDB.cardNameEN.cairnebloodhoof, 0);
            silenceTargets.Add(CardDB.cardNameEN.chillmaw, 0);
            silenceTargets.Add(CardDB.cardNameEN.chromaggus, 0);
            silenceTargets.Add(CardDB.cardNameEN.cobaltguardian, 0);
            silenceTargets.Add(CardDB.cardNameEN.coldarradrake, 0);
            silenceTargets.Add(CardDB.cardNameEN.coliseummanager, 0);
            silenceTargets.Add(CardDB.cardNameEN.confessorpaletress, 0);
            silenceTargets.Add(CardDB.cardNameEN.crazedworshipper, 0);
            silenceTargets.Add(CardDB.cardNameEN.crowdfavorite, 0);
            silenceTargets.Add(CardDB.cardNameEN.crueldinomancer, 0);
            silenceTargets.Add(CardDB.cardNameEN.crystallineoracle, 0);
            silenceTargets.Add(CardDB.cardNameEN.cthun, 0);
            silenceTargets.Add(CardDB.cardNameEN.cultmaster, 0);
            silenceTargets.Add(CardDB.cardNameEN.cultsorcerer, 0);
            silenceTargets.Add(CardDB.cardNameEN.dalaranaspirant, 0);
            silenceTargets.Add(CardDB.cardNameEN.darkcultist, 0);
            silenceTargets.Add(CardDB.cardNameEN.darkshirecouncilman, 0);
            silenceTargets.Add(CardDB.cardNameEN.dementedfrostcaller, 0);
            silenceTargets.Add(CardDB.cardNameEN.devilsauregg, 0);
            silenceTargets.Add(CardDB.cardNameEN.nerubianegg, 0);
            silenceTargets.Add(CardDB.cardNameEN.direhornhatchling, 0);
            silenceTargets.Add(CardDB.cardNameEN.direwolfalpha, 0);
            silenceTargets.Add(CardDB.cardNameEN.djinniofzephyrs, 0);
            silenceTargets.Add(CardDB.cardNameEN.doomsayer, 0);
            silenceTargets.Add(CardDB.cardNameEN.voidcaller, 0);
            silenceTargets.Add(CardDB.cardNameEN.dragonegg, 0);
            silenceTargets.Add(CardDB.cardNameEN.dragonhawkrider, 0);
            silenceTargets.Add(CardDB.cardNameEN.dragonkinsorcerer, 0);
            silenceTargets.Add(CardDB.cardNameEN.dreadscale, 0);
            silenceTargets.Add(CardDB.cardNameEN.eggnapper, 0);
            silenceTargets.Add(CardDB.cardNameEN.emboldener3000, 0);
            silenceTargets.Add(CardDB.cardNameEN.emperorcobra, 0);
            silenceTargets.Add(CardDB.cardNameEN.emperorthaurissan, 0);
            silenceTargets.Add(CardDB.cardNameEN.etherealarcanist, 0);
            silenceTargets.Add(CardDB.cardNameEN.evolvedkobold, 0);
            silenceTargets.Add(CardDB.cardNameEN.explosivesheep, 0);
            silenceTargets.Add(CardDB.cardNameEN.eydisdarkbane, 0);
            silenceTargets.Add(CardDB.cardNameEN.fallenhero, 0);
            silenceTargets.Add(CardDB.cardNameEN.masterchest, 0);            
            silenceTargets.Add(CardDB.cardNameEN.fandralstaghelm, 0);
            silenceTargets.Add(CardDB.cardNameEN.feugen, 0);
            silenceTargets.Add(CardDB.cardNameEN.finjatheflyingstar, 0);
            silenceTargets.Add(CardDB.cardNameEN.fjolalightbane, 0);
            silenceTargets.Add(CardDB.cardNameEN.flametonguetotem, 0);
            silenceTargets.Add(CardDB.cardNameEN.flamewaker, 0);
            silenceTargets.Add(CardDB.cardNameEN.flesheatingghoul, 0);
            silenceTargets.Add(CardDB.cardNameEN.floatingwatcher, 0);
            silenceTargets.Add(CardDB.cardNameEN.foereaper4000, 0);
            silenceTargets.Add(CardDB.cardNameEN.frothingberserker, 0);
            silenceTargets.Add(CardDB.cardNameEN.gadgetzanauctioneer, 10);
            silenceTargets.Add(CardDB.cardNameEN.gahzrilla, 0);
            silenceTargets.Add(CardDB.cardNameEN.garr, 0);
            silenceTargets.Add(CardDB.cardNameEN.garrisoncommander, 0);
            silenceTargets.Add(CardDB.cardNameEN.giantanaconda, 0);
            silenceTargets.Add(CardDB.cardNameEN.giantsandworm, 0);
            silenceTargets.Add(CardDB.cardNameEN.giantwasp, 0);
            silenceTargets.Add(CardDB.cardNameEN.grimestreetenforcer, 0);
            silenceTargets.Add(CardDB.cardNameEN.grimpatron, 0);
            silenceTargets.Add(CardDB.cardNameEN.grimscaleoracle, 0);
            silenceTargets.Add(CardDB.cardNameEN.grimygadgeteer, 0);
            silenceTargets.Add(CardDB.cardNameEN.grommashhellscream, 0);
            silenceTargets.Add(CardDB.cardNameEN.gruul, 0);
            silenceTargets.Add(CardDB.cardNameEN.gurubashiberserker, 0);
            silenceTargets.Add(CardDB.cardNameEN.hallazealtheascended, 0);
            silenceTargets.Add(CardDB.cardNameEN.hauntedcreeper, 0);
            silenceTargets.Add(CardDB.cardNameEN.hobgoblin, 0);
            silenceTargets.Add(CardDB.cardNameEN.hogger, 0);
            silenceTargets.Add(CardDB.cardNameEN.hoggerdoomofelwynn, 0);
            silenceTargets.Add(CardDB.cardNameEN.holychampion, 0);
            silenceTargets.Add(CardDB.cardNameEN.homingchicken, 0);
            silenceTargets.Add(CardDB.cardNameEN.hoodedacolyte, 0);
            silenceTargets.Add(CardDB.cardNameEN.igneouselemental, 0);
            silenceTargets.Add(CardDB.cardNameEN.illidanstormrage, 0);
            silenceTargets.Add(CardDB.cardNameEN.impgangboss, 0);
            silenceTargets.Add(CardDB.cardNameEN.impmaster, 0);
            silenceTargets.Add(CardDB.cardNameEN.ironsensei, 0);
            silenceTargets.Add(CardDB.cardNameEN.jadeswarmer, 0);
            silenceTargets.Add(CardDB.cardNameEN.jeeves, 0);
            silenceTargets.Add(CardDB.cardNameEN.junkbot, 0);
            silenceTargets.Add(CardDB.cardNameEN.kabaltrafficker, 0);
            silenceTargets.Add(CardDB.cardNameEN.kelthuzad, 10);
            silenceTargets.Add(CardDB.cardNameEN.kindlygrandmother, 0);
            silenceTargets.Add(CardDB.cardNameEN.knifejuggler, 0);
            silenceTargets.Add(CardDB.cardNameEN.knuckles, 0);
            silenceTargets.Add(CardDB.cardNameEN.kodorider, 0);
            silenceTargets.Add(CardDB.cardNameEN.kvaldirraider, 0);
            silenceTargets.Add(CardDB.cardNameEN.leokk, 0);
            silenceTargets.Add(CardDB.cardNameEN.lightspawn, 0);
            silenceTargets.Add(CardDB.cardNameEN.lightwarden, 0);
            silenceTargets.Add(CardDB.cardNameEN.lightwell, 0);
            silenceTargets.Add(CardDB.cardNameEN.lorewalkercho, 0);
            silenceTargets.Add(CardDB.cardNameEN.lowlysquire, 0);
            silenceTargets.Add(CardDB.cardNameEN.lyrathesunshard, 0);
            silenceTargets.Add(CardDB.cardNameEN.madscientist, 0);
            silenceTargets.Add(CardDB.cardNameEN.maexxna, 0);
            silenceTargets.Add(CardDB.cardNameEN.magnatauralpha, 0);
            silenceTargets.Add(CardDB.cardNameEN.maidenofthelake, 0);
            silenceTargets.Add(CardDB.cardNameEN.majordomoexecutus, 0);
            silenceTargets.Add(CardDB.cardNameEN.malganis, 0);
            silenceTargets.Add(CardDB.cardNameEN.malorne, 0);
            silenceTargets.Add(CardDB.cardNameEN.malygos, 0);
            silenceTargets.Add(CardDB.cardNameEN.manaaddict, 0);
            silenceTargets.Add(CardDB.cardNameEN.manageode, 0);
            silenceTargets.Add(CardDB.cardNameEN.manatidetotem, 0);
            silenceTargets.Add(CardDB.cardNameEN.manatreant, 0);
            silenceTargets.Add(CardDB.cardNameEN.manawraith, 0);
            silenceTargets.Add(CardDB.cardNameEN.manawyrm, 0);
            silenceTargets.Add(CardDB.cardNameEN.masterswordsmith, 0);
            silenceTargets.Add(CardDB.cardNameEN.mekgineerthermaplugg, 0);
            silenceTargets.Add(CardDB.cardNameEN.micromachine, 0);
            silenceTargets.Add(CardDB.cardNameEN.mogortheogre, 0);
            silenceTargets.Add(CardDB.cardNameEN.muklaschampion, 0);
            silenceTargets.Add(CardDB.cardNameEN.murlocknight, 0);
            silenceTargets.Add(CardDB.cardNameEN.murloctidecaller, 0);
            silenceTargets.Add(CardDB.cardNameEN.murlocwarleader, 0);
            silenceTargets.Add(CardDB.cardNameEN.natpagle, 0);
            silenceTargets.Add(CardDB.cardNameEN.nerubarweblord, 0);
            silenceTargets.Add(CardDB.cardNameEN.nexuschampionsaraad, 0);
            silenceTargets.Add(CardDB.cardNameEN.northshirecleric, 0);
            silenceTargets.Add(CardDB.cardNameEN.obsidiandestroyer, 0);
            silenceTargets.Add(CardDB.cardNameEN.oldmurkeye, 0);
            silenceTargets.Add(CardDB.cardNameEN.oneeyedcheat, 0);
            silenceTargets.Add(CardDB.cardNameEN.orgrimmaraspirant, 0);
            silenceTargets.Add(CardDB.cardNameEN.pilotedskygolem, 0);
            silenceTargets.Add(CardDB.cardNameEN.pitsnake, 0);
            silenceTargets.Add(CardDB.cardNameEN.primalfinchampion, 0);
            silenceTargets.Add(CardDB.cardNameEN.primalfintotem, 0);
            silenceTargets.Add(CardDB.cardNameEN.prophetvelen, 0);
            silenceTargets.Add(CardDB.cardNameEN.pyros, 0);
            silenceTargets.Add(CardDB.cardNameEN.questingadventurer, 0);
            silenceTargets.Add(CardDB.cardNameEN.radiantelemental, 0);
            silenceTargets.Add(CardDB.cardNameEN.ragingworgen, 0);
            silenceTargets.Add(CardDB.cardNameEN.raidleader, 0);
            silenceTargets.Add(CardDB.cardNameEN.raptorhatchling, 0);
            silenceTargets.Add(CardDB.cardNameEN.ratpack, 0);
            silenceTargets.Add(CardDB.cardNameEN.recruiter, 0);
            silenceTargets.Add(CardDB.cardNameEN.redmanawyrm, 0);
            silenceTargets.Add(CardDB.cardNameEN.rhonin, 0);
            silenceTargets.Add(CardDB.cardNameEN.rumblingelemental, 0);
            silenceTargets.Add(CardDB.cardNameEN.satedthreshadon, 0);
            silenceTargets.Add(CardDB.cardNameEN.savagecombatant, 0);
            silenceTargets.Add(CardDB.cardNameEN.savannahhighmane, 0);
            silenceTargets.Add(CardDB.cardNameEN.scalednightmare, 0);
            silenceTargets.Add(CardDB.cardNameEN.scavenginghyena, 0);
            silenceTargets.Add(CardDB.cardNameEN.secretkeeper, 0);
            silenceTargets.Add(CardDB.cardNameEN.selflesshero, 0);
            silenceTargets.Add(CardDB.cardNameEN.sergeantsally, 0);
            silenceTargets.Add(CardDB.cardNameEN.shadeofnaxxramas, 0);
            silenceTargets.Add(CardDB.cardNameEN.shadowboxer, 0);
            silenceTargets.Add(CardDB.cardNameEN.shakuthecollector, 0);
            silenceTargets.Add(CardDB.cardNameEN.sherazincorpseflower, 0);
            silenceTargets.Add(CardDB.cardNameEN.shiftingshade, 0);
            silenceTargets.Add(CardDB.cardNameEN.shimmeringtempest, 0);
            silenceTargets.Add(CardDB.cardNameEN.shipscannon, 0);
            silenceTargets.Add(CardDB.cardNameEN.siegeengine, 0);
            silenceTargets.Add(CardDB.cardNameEN.siltfinspiritwalker, 0);
            silenceTargets.Add(CardDB.cardNameEN.silverhandregent, 0);
            silenceTargets.Add(CardDB.cardNameEN.sneedsoldshredder, 0);
            silenceTargets.Add(CardDB.cardNameEN.sorcerersapprentice, 0);
            silenceTargets.Add(CardDB.cardNameEN.southseacaptain, 0);
            silenceTargets.Add(CardDB.cardNameEN.southseasquidface, 0);
            silenceTargets.Add(CardDB.cardNameEN.spawnofnzoth, 0);
            silenceTargets.Add(CardDB.cardNameEN.spawnofshadows, 0);
            silenceTargets.Add(CardDB.cardNameEN.spiritsingerumbra, 0);
            silenceTargets.Add(CardDB.cardNameEN.spitefulsmith, 0);
            silenceTargets.Add(CardDB.cardNameEN.stalagg, 0);
            silenceTargets.Add(CardDB.cardNameEN.starvingbuzzard, 0);
            silenceTargets.Add(CardDB.cardNameEN.steamwheedlesniper, 0);
            silenceTargets.Add(CardDB.cardNameEN.stewardofdarkshire, 0);
            silenceTargets.Add(CardDB.cardNameEN.stonesplintertrogg, 0);
            silenceTargets.Add(CardDB.cardNameEN.stormwindchampion, 0);
            silenceTargets.Add(CardDB.cardNameEN.summoningportal, 0);
            silenceTargets.Add(CardDB.cardNameEN.summoningstone, 0);
            silenceTargets.Add(CardDB.cardNameEN.sylvanaswindrunner, 0);
            silenceTargets.Add(CardDB.cardNameEN.tarcreeper, 0);
            silenceTargets.Add(CardDB.cardNameEN.tarlord, 0);
            silenceTargets.Add(CardDB.cardNameEN.tarlurker, 0);
            silenceTargets.Add(CardDB.cardNameEN.theboogeymonster, 0);
            silenceTargets.Add(CardDB.cardNameEN.theskeletonknight, 0);
            silenceTargets.Add(CardDB.cardNameEN.thevoraxx, 0);
            silenceTargets.Add(CardDB.cardNameEN.thunderbluffvaliant, 0);
            silenceTargets.Add(CardDB.cardNameEN.timberwolf, 0);
            silenceTargets.Add(CardDB.cardNameEN.tirionfordring, 0);
            silenceTargets.Add(CardDB.cardNameEN.tortollanshellraiser, 0);
            silenceTargets.Add(CardDB.cardNameEN.tournamentmedic, 0);
            silenceTargets.Add(CardDB.cardNameEN.tradeprincegallywix, 0);
            silenceTargets.Add(CardDB.cardNameEN.troggzortheearthinator, 0);
            silenceTargets.Add(CardDB.cardNameEN.tundrarhino, 0);
            silenceTargets.Add(CardDB.cardNameEN.twilightelder, 0);
            silenceTargets.Add(CardDB.cardNameEN.twilightsummoner, 0);
            silenceTargets.Add(CardDB.cardNameEN.unboundelemental, 0);
            silenceTargets.Add(CardDB.cardNameEN.undercityhuckster, 0);
            silenceTargets.Add(CardDB.cardNameEN.undertaker, 0);
            silenceTargets.Add(CardDB.cardNameEN.usherofsouls, 0);
            silenceTargets.Add(CardDB.cardNameEN.v07tr0n, 0);
            silenceTargets.Add(CardDB.cardNameEN.viciousfledgling, 0);
            silenceTargets.Add(CardDB.cardNameEN.violetillusionist, 0);
            silenceTargets.Add(CardDB.cardNameEN.violetteacher, 0);
            silenceTargets.Add(CardDB.cardNameEN.vitalitytotem, 0);
            silenceTargets.Add(CardDB.cardNameEN.voidcrusher, 0);
            silenceTargets.Add(CardDB.cardNameEN.volatileelemental, 0);
            silenceTargets.Add(CardDB.cardNameEN.warhorsetrainer, 0);
            silenceTargets.Add(CardDB.cardNameEN.warsongcommander, 0);
            silenceTargets.Add(CardDB.cardNameEN.webspinner, 0);
            silenceTargets.Add(CardDB.cardNameEN.whiteeyes, 0);
            silenceTargets.Add(CardDB.cardNameEN.wickedwitchdoctor, 0);
            silenceTargets.Add(CardDB.cardNameEN.wilfredfizzlebang, 0);
            silenceTargets.Add(CardDB.cardNameEN.windupburglebot, 0);
            silenceTargets.Add(CardDB.cardNameEN.wobblingrunts, 0);
            silenceTargets.Add(CardDB.cardNameEN.youngpriestess, 0);
            silenceTargets.Add(CardDB.cardNameEN.ysera, 0);
            silenceTargets.Add(CardDB.cardNameEN.yshaarjrageunbound, 0);
            silenceTargets.Add(CardDB.cardNameEN.zealousinitiate, 0);
            silenceTargets.Add(CardDB.cardNameEN.vryghoul, 0);
            silenceTargets.Add(CardDB.cardNameEN.thelichking, 0);
            silenceTargets.Add(CardDB.cardNameEN.skelemancer, 0);
            silenceTargets.Add(CardDB.cardNameEN.shallowgravedigger, 0);
            silenceTargets.Add(CardDB.cardNameEN.shadowascendant, 0);
            silenceTargets.Add(CardDB.cardNameEN.moirabronzebeard, 0);
            silenceTargets.Add(CardDB.cardNameEN.meatwagon, 0);
            silenceTargets.Add(CardDB.cardNameEN.highjusticegrimstone, 0);
            silenceTargets.Add(CardDB.cardNameEN.hadronox, 0);
            silenceTargets.Add(CardDB.cardNameEN.frozenchampion, 0);
            silenceTargets.Add(CardDB.cardNameEN.festergut, 0);
            silenceTargets.Add(CardDB.cardNameEN.fatespinner, 0);
            silenceTargets.Add(CardDB.cardNameEN.explodingbloatbat, 0);
            silenceTargets.Add(CardDB.cardNameEN.drakkarienchanter, 0);
            silenceTargets.Add(CardDB.cardNameEN.despicabledreadlord, 0);
            silenceTargets.Add(CardDB.cardNameEN.cobaltscalebane, 0);
            silenceTargets.Add(CardDB.cardNameEN.bonedrake, 0);
            silenceTargets.Add(CardDB.cardNameEN.bonebaron, 0);
            silenceTargets.Add(CardDB.cardNameEN.blackguard, 0);
            silenceTargets.Add(CardDB.cardNameEN.arrogantcrusader, 0);
            silenceTargets.Add(CardDB.cardNameEN.arfus, 0);
            silenceTargets.Add(CardDB.cardNameEN.abominablebowman, 0);
            silenceTargets.Add(CardDB.cardNameEN.voodoohexxer, 0);
            silenceTargets.Add(CardDB.cardNameEN.venomancer, 0);
            silenceTargets.Add(CardDB.cardNameEN.valkyrsoulclaimer, 0);
            silenceTargets.Add(CardDB.cardNameEN.stubborngastropod, 0);
            silenceTargets.Add(CardDB.cardNameEN.runeforgehaunter, 0);
            silenceTargets.Add(CardDB.cardNameEN.rotface, 0);
            silenceTargets.Add(CardDB.cardNameEN.professorputricide, 0);
            silenceTargets.Add(CardDB.cardNameEN.nighthowler, 0);
            silenceTargets.Add(CardDB.cardNameEN.nerubianunraveler, 0);
            silenceTargets.Add(CardDB.cardNameEN.necroticgeist, 0);
            silenceTargets.Add(CardDB.cardNameEN.moorabi, 0);
            silenceTargets.Add(CardDB.cardNameEN.icewalker, 0);
            silenceTargets.Add(CardDB.cardNameEN.doomedapprentice, 0);
            silenceTargets.Add(CardDB.cardNameEN.cryptlord, 0);
            silenceTargets.Add(CardDB.cardNameEN.corpsewidow, 0);
            silenceTargets.Add(CardDB.cardNameEN.bolvarfireblood, 0);
            silenceTargets.Add(CardDB.cardNameEN.bloodqueenlanathel, 0);
        }

        private void setupRandomCards()
        {
            randomEffects.Add(CardDB.cardNameEN.alightinthedarkness, 1);
            randomEffects.Add(CardDB.cardNameEN.ancestorscall, 1);
            randomEffects.Add(CardDB.cardNameEN.animalcompanion, 1);
            randomEffects.Add(CardDB.cardNameEN.arcanemissiles, 3);
            randomEffects.Add(CardDB.cardNameEN.archthiefrafaam, 1);
            randomEffects.Add(CardDB.cardNameEN.armoredwarhorse, 1);
            randomEffects.Add(CardDB.cardNameEN.avengingwrath, 8);
            randomEffects.Add(CardDB.cardNameEN.babblingbook, 1);
            randomEffects.Add(CardDB.cardNameEN.barnes, 1);
            randomEffects.Add(CardDB.cardNameEN.bomblobber, 1);
            randomEffects.Add(CardDB.cardNameEN.bouncingblade, 3);
            randomEffects.Add(CardDB.cardNameEN.brawl, 1);
            randomEffects.Add(CardDB.cardNameEN.captainsparrot, 1);
            randomEffects.Add(CardDB.cardNameEN.chitteringtunneler, 1);
            randomEffects.Add(CardDB.cardNameEN.chooseyourpath, 1);
            randomEffects.Add(CardDB.cardNameEN.cleave, 2);
            randomEffects.Add(CardDB.cardNameEN.coghammer, 1);
            randomEffects.Add(CardDB.cardNameEN.crackle, 1);
            randomEffects.Add(CardDB.cardNameEN.cthun, 10);
            randomEffects.Add(CardDB.cardNameEN.darkbargain, 2);
            randomEffects.Add(CardDB.cardNameEN.darkpeddler, 1);
            randomEffects.Add(CardDB.cardNameEN.deadlyshot, 1);
            randomEffects.Add(CardDB.cardNameEN.desertcamel, 1);
            randomEffects.Add(CardDB.cardNameEN.elementaldestruction, 1);
            randomEffects.Add(CardDB.cardNameEN.elitetaurenchieftain, 1);
            randomEffects.Add(CardDB.cardNameEN.enhanceomechano, 1);
            randomEffects.Add(CardDB.cardNameEN.etherealconjurer, 1);
            randomEffects.Add(CardDB.cardNameEN.fierybat, 1);
            randomEffects.Add(CardDB.cardNameEN.finderskeepers, 1);
            randomEffects.Add(CardDB.cardNameEN.firelandsportal, 1);
            randomEffects.Add(CardDB.cardNameEN.flamecannon, 1);
            randomEffects.Add(CardDB.cardNameEN.flamejuggler, 1);
            randomEffects.Add(CardDB.cardNameEN.flamewaker, 2);
            randomEffects.Add(CardDB.cardNameEN.forkedlightning, 1);
            randomEffects.Add(CardDB.cardNameEN.freefromamber, 1);
            randomEffects.Add(CardDB.cardNameEN.zarogscrown, 1);
            randomEffects.Add(CardDB.cardNameEN.gelbinmekkatorque, 1);
            randomEffects.Add(CardDB.cardNameEN.glaivezooka, 1);
            randomEffects.Add(CardDB.cardNameEN.grandcrusader, 1);
            randomEffects.Add(CardDB.cardNameEN.greaterarcanemissiles, 3);
            randomEffects.Add(CardDB.cardNameEN.grimestreetinformant, 1);
            randomEffects.Add(CardDB.cardNameEN.hallucination, 1);
            randomEffects.Add(CardDB.cardNameEN.harvest, 1);
            randomEffects.Add(CardDB.cardNameEN.hungrydragon, 1);
            randomEffects.Add(CardDB.cardNameEN.hydrologist, 1);
            randomEffects.Add(CardDB.cardNameEN.iammurloc, 3);
            randomEffects.Add(CardDB.cardNameEN.iknowaguy, 1);
            randomEffects.Add(CardDB.cardNameEN.ironforgeportal, 1);
            randomEffects.Add(CardDB.cardNameEN.ivoryknight, 1);
            randomEffects.Add(CardDB.cardNameEN.jeweledscarab, 1);
            randomEffects.Add(CardDB.cardNameEN.journeybelow, 1);
            randomEffects.Add(CardDB.cardNameEN.kabalchemist, 1);
            randomEffects.Add(CardDB.cardNameEN.kabalcourier, 1);
            randomEffects.Add(CardDB.cardNameEN.kazakus, 1);
            randomEffects.Add(CardDB.cardNameEN.kingsblood, 1);
            randomEffects.Add(CardDB.cardNameEN.lifetap, 1);
            randomEffects.Add(CardDB.cardNameEN.lightningstorm, 1);
            randomEffects.Add(CardDB.cardNameEN.lockandload, 10);
            randomEffects.Add(CardDB.cardNameEN.lotusagents, 1);
            randomEffects.Add(CardDB.cardNameEN.madbomber, 3);
            randomEffects.Add(CardDB.cardNameEN.madderbomber, 1);
            randomEffects.Add(CardDB.cardNameEN.maelstromportal, 1);
            randomEffects.Add(CardDB.cardNameEN.masterjouster, 1);
            randomEffects.Add(CardDB.cardNameEN.menageriemagician, 0);
            randomEffects.Add(CardDB.cardNameEN.mindcontroltech, 1);
            randomEffects.Add(CardDB.cardNameEN.mindgames, 1);
            randomEffects.Add(CardDB.cardNameEN.mindvision, 1);
            randomEffects.Add(CardDB.cardNameEN.mogorschampion, 1);
            randomEffects.Add(CardDB.cardNameEN.mogortheogre, 1);
            randomEffects.Add(CardDB.cardNameEN.moongladeportal, 1);
            randomEffects.Add(CardDB.cardNameEN.multishot, 2);
            randomEffects.Add(CardDB.cardNameEN.museumcurator, 1);
            randomEffects.Add(CardDB.cardNameEN.mysteriouschallenger, 2);
            randomEffects.Add(CardDB.cardNameEN.pileon, 1);
            randomEffects.Add(CardDB.cardNameEN.powerofthehorde, 1);
            randomEffects.Add(CardDB.cardNameEN.primordialglyph, 1);
            randomEffects.Add(CardDB.cardNameEN.ramwrangler, 1);
            randomEffects.Add(CardDB.cardNameEN.ravenidol, 1);
            randomEffects.Add(CardDB.cardNameEN.resurrect, 1);
            randomEffects.Add(CardDB.cardNameEN.sabotage, 0);
            randomEffects.Add(CardDB.cardNameEN.sensedemons, 2);
            randomEffects.Add(CardDB.cardNameEN.servantofkalimos, 1);
            randomEffects.Add(CardDB.cardNameEN.shadowoil, 1);
            randomEffects.Add(CardDB.cardNameEN.shadowvisions, 1);
            randomEffects.Add(CardDB.cardNameEN.silvermoonportal, 1);
            randomEffects.Add(CardDB.cardNameEN.sirfinleymrrgglton, 1);
            randomEffects.Add(CardDB.cardNameEN.soultap, 1);
            randomEffects.Add(CardDB.cardNameEN.spellslinger, 1);
            randomEffects.Add(CardDB.cardNameEN.spreadingmadness, 9);
            randomEffects.Add(CardDB.cardNameEN.stampede, 10);
            randomEffects.Add(CardDB.cardNameEN.stonehilldefender, 1);
            randomEffects.Add(CardDB.cardNameEN.swashburglar, 1);
            randomEffects.Add(CardDB.cardNameEN.timepieceofhorror, 10);
            randomEffects.Add(CardDB.cardNameEN.tinkmasteroverspark, 1);
            randomEffects.Add(CardDB.cardNameEN.tombspider, 1);
            randomEffects.Add(CardDB.cardNameEN.tortollanprimalist, 1);
            randomEffects.Add(CardDB.cardNameEN.totemiccall, 1);
            randomEffects.Add(CardDB.cardNameEN.tuskarrtotemic, 1);
            randomEffects.Add(CardDB.cardNameEN.unholyshadow, 2);
            randomEffects.Add(CardDB.cardNameEN.unstableportal, 1);
            randomEffects.Add(CardDB.cardNameEN.varianwrynn, 2);
            randomEffects.Add(CardDB.cardNameEN.volcano, 15);
            randomEffects.Add(CardDB.cardNameEN.xarilpoisonedmind, 1);
            randomEffects.Add(CardDB.cardNameEN.zoobot, 0);
            randomEffects.Add(CardDB.cardNameEN.tomblurker, 1);
            randomEffects.Add(CardDB.cardNameEN.ghastlyconjurer, 1);
            randomEffects.Add(CardDB.cardNameEN.shadowessence, 1);

        }


        private void setupChooseDatabase()
        {
            this.choose1database.Add(CardDB.cardNameEN.ancientoflore, CardDB.cardIDEnum.NEW1_008a);
            this.choose1database.Add(CardDB.cardNameEN.ancientofwar, CardDB.cardIDEnum.EX1_178b);
            this.choose1database.Add(CardDB.cardNameEN.anodizedrobocub, CardDB.cardIDEnum.GVG_030a);
            this.choose1database.Add(CardDB.cardNameEN.cenarius, CardDB.cardIDEnum.EX1_573a);
            this.choose1database.Add(CardDB.cardNameEN.darkwispers, CardDB.cardIDEnum.GVG_041b);
            this.choose1database.Add(CardDB.cardNameEN.druidoftheclaw, CardDB.cardIDEnum.EX1_165t1);
            this.choose1database.Add(CardDB.cardNameEN.druidoftheflame, CardDB.cardIDEnum.BRM_010t);
            this.choose1database.Add(CardDB.cardNameEN.druidofthesaber, CardDB.cardIDEnum.AT_042t);
            this.choose1database.Add(CardDB.cardNameEN.feralrage, CardDB.cardIDEnum.OG_047a);
            this.choose1database.Add(CardDB.cardNameEN.grovetender, CardDB.cardIDEnum.GVG_032a);
            this.choose1database.Add(CardDB.cardNameEN.jadeidol, CardDB.cardIDEnum.CFM_602a);
            this.choose1database.Add(CardDB.cardNameEN.keeperofthegrove, CardDB.cardIDEnum.EX1_166a);
            this.choose1database.Add(CardDB.cardNameEN.kuntheforgottenking, CardDB.cardIDEnum.CFM_308a);
            this.choose1database.Add(CardDB.cardNameEN.livingroots, CardDB.cardIDEnum.AT_037a);
            this.choose1database.Add(CardDB.cardNameEN.markofnature, CardDB.cardIDEnum.EX1_155a);
            this.choose1database.Add(CardDB.cardNameEN.mirekeeper, CardDB.cardIDEnum.OG_202a);
            this.choose1database.Add(CardDB.cardNameEN.nourish, CardDB.cardIDEnum.EX1_164a);
            this.choose1database.Add(CardDB.cardNameEN.powerofthewild, CardDB.cardIDEnum.EX1_160b);
            this.choose1database.Add(CardDB.cardNameEN.ravenidol, CardDB.cardIDEnum.LOE_115a);
            this.choose1database.Add(CardDB.cardNameEN.shellshifter, CardDB.cardIDEnum.UNG_101t);
            this.choose1database.Add(CardDB.cardNameEN.starfall, CardDB.cardIDEnum.NEW1_007b);
            this.choose1database.Add(CardDB.cardNameEN.wispsoftheoldgods, CardDB.cardIDEnum.OG_195a);
            this.choose1database.Add(CardDB.cardNameEN.wrath, CardDB.cardIDEnum.EX1_154a);
            this.choose1database.Add(CardDB.cardNameEN.malfurionthepestilent, CardDB.cardIDEnum.ICC_832b);
            this.choose1database.Add(CardDB.cardNameEN.plaguelord, CardDB.cardIDEnum.ICC_832pb);
            this.choose1database.Add(CardDB.cardNameEN.druidoftheswarm, CardDB.cardIDEnum.ICC_051t);

            this.choose2database.Add(CardDB.cardNameEN.ancientoflore, CardDB.cardIDEnum.NEW1_008b);
            this.choose2database.Add(CardDB.cardNameEN.ancientofwar, CardDB.cardIDEnum.EX1_178a);
            this.choose2database.Add(CardDB.cardNameEN.anodizedrobocub, CardDB.cardIDEnum.GVG_030b);
            this.choose2database.Add(CardDB.cardNameEN.cenarius, CardDB.cardIDEnum.EX1_573b);
            this.choose2database.Add(CardDB.cardNameEN.darkwispers, CardDB.cardIDEnum.GVG_041a);
            this.choose2database.Add(CardDB.cardNameEN.druidoftheclaw, CardDB.cardIDEnum.EX1_165t2);
            this.choose2database.Add(CardDB.cardNameEN.druidoftheflame, CardDB.cardIDEnum.BRM_010t2);
            this.choose2database.Add(CardDB.cardNameEN.druidofthesaber, CardDB.cardIDEnum.AT_042t2);
            this.choose2database.Add(CardDB.cardNameEN.feralrage, CardDB.cardIDEnum.OG_047b);
            this.choose2database.Add(CardDB.cardNameEN.grovetender, CardDB.cardIDEnum.GVG_032b);
            this.choose2database.Add(CardDB.cardNameEN.jadeidol, CardDB.cardIDEnum.CFM_602b);
            this.choose2database.Add(CardDB.cardNameEN.keeperofthegrove, CardDB.cardIDEnum.EX1_166b);
            this.choose2database.Add(CardDB.cardNameEN.kuntheforgottenking, CardDB.cardIDEnum.CFM_308b);
            this.choose2database.Add(CardDB.cardNameEN.livingroots, CardDB.cardIDEnum.AT_037b);
            this.choose2database.Add(CardDB.cardNameEN.markofnature, CardDB.cardIDEnum.EX1_155b);
            this.choose2database.Add(CardDB.cardNameEN.mirekeeper, CardDB.cardIDEnum.OG_202ae);
            this.choose2database.Add(CardDB.cardNameEN.nourish, CardDB.cardIDEnum.EX1_164b);
            this.choose2database.Add(CardDB.cardNameEN.powerofthewild, CardDB.cardIDEnum.EX1_160t);
            this.choose2database.Add(CardDB.cardNameEN.ravenidol, CardDB.cardIDEnum.LOE_115b);
            this.choose2database.Add(CardDB.cardNameEN.shellshifter, CardDB.cardIDEnum.UNG_101t2);
            this.choose2database.Add(CardDB.cardNameEN.starfall, CardDB.cardIDEnum.NEW1_007a);
            this.choose2database.Add(CardDB.cardNameEN.wispsoftheoldgods, CardDB.cardIDEnum.OG_195b);
            this.choose2database.Add(CardDB.cardNameEN.wrath, CardDB.cardIDEnum.EX1_154b);
            this.choose2database.Add(CardDB.cardNameEN.malfurionthepestilent, CardDB.cardIDEnum.ICC_832a);
            this.choose2database.Add(CardDB.cardNameEN.plaguelord, CardDB.cardIDEnum.ICC_832pa);
            this.choose2database.Add(CardDB.cardNameEN.druidoftheswarm, CardDB.cardIDEnum.ICC_051t2);
        }


        public int getClassRacePriorityPenality(TAG_CLASS opponentHeroClass, TAG_RACE minionRace)
        {
            int retval = 0;
            switch (opponentHeroClass)
            {
                case TAG_CLASS.WARLOCK:
                    if (this.ClassRacePriorityWarloc.ContainsKey(minionRace)) retval += this.ClassRacePriorityWarloc[minionRace];
                    break;
                case TAG_CLASS.WARRIOR:
                    if (this.ClassRacePriorityWarrior.ContainsKey(minionRace)) retval += this.ClassRacePriorityWarrior[minionRace];
					break;
                case TAG_CLASS.ROGUE:
                    if (this.ClassRacePriorityRouge.ContainsKey(minionRace)) retval += this.ClassRacePriorityRouge[minionRace];
					break;
                case TAG_CLASS.SHAMAN:
                    if (this.ClassRacePriorityShaman.ContainsKey(minionRace)) retval += this.ClassRacePriorityShaman[minionRace];
					break;
                case TAG_CLASS.PRIEST:
                    if (this.ClassRacePriorityPriest.ContainsKey(minionRace)) retval += this.ClassRacePriorityPriest[minionRace];
					break;
                case TAG_CLASS.PALADIN:
                    if (this.ClassRacePriorityPaladin.ContainsKey(minionRace)) retval += this.ClassRacePriorityPaladin[minionRace];
					break;
                case TAG_CLASS.MAGE:
                    if (this.ClassRacePriorityMage.ContainsKey(minionRace)) retval += this.ClassRacePriorityMage[minionRace];
					break;
                case TAG_CLASS.HUNTER:
                    if (this.ClassRacePriorityHunter.ContainsKey(minionRace)) retval += this.ClassRacePriorityHunter[minionRace];
					break;
                case TAG_CLASS.DRUID:
                    if (this.ClassRacePriorityDruid.ContainsKey(minionRace)) retval += this.ClassRacePriorityDruid[minionRace];
                    break;
                default:
                    break;
			}
            return retval;
        }

        private void setupClassRacePriorityDatabase()
        {
            this.ClassRacePriorityWarloc.Add(TAG_RACE.MURLOC, 2);
            this.ClassRacePriorityWarloc.Add(TAG_RACE.DEMON, 2);
            this.ClassRacePriorityWarloc.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityWarloc.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityWarloc.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityHunter.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityHunter.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityHunter.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityHunter.Add(TAG_RACE.PET, 2);
            this.ClassRacePriorityHunter.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityMage.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityMage.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityMage.Add(TAG_RACE.MECHANICAL, 2);
            this.ClassRacePriorityMage.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityMage.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityShaman.Add(TAG_RACE.MURLOC, 2);
            this.ClassRacePriorityShaman.Add(TAG_RACE.PIRATE, 1);
            this.ClassRacePriorityShaman.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityShaman.Add(TAG_RACE.MECHANICAL, 2);
            this.ClassRacePriorityShaman.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityShaman.Add(TAG_RACE.TOTEM, 2);

            this.ClassRacePriorityDruid.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityDruid.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityDruid.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityDruid.Add(TAG_RACE.PET, 1);
            this.ClassRacePriorityDruid.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityPaladin.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityPaladin.Add(TAG_RACE.PIRATE, 1);
            this.ClassRacePriorityPaladin.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityPaladin.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityPaladin.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityPaladin.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityPriest.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityPriest.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityPriest.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityPriest.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityPriest.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityRouge.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityRouge.Add(TAG_RACE.PIRATE, 2);
            this.ClassRacePriorityRouge.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityRouge.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityRouge.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityRouge.Add(TAG_RACE.TOTEM, 0);

            this.ClassRacePriorityWarrior.Add(TAG_RACE.MURLOC, 1);
            this.ClassRacePriorityWarrior.Add(TAG_RACE.DEMON, 0);
            this.ClassRacePriorityWarrior.Add(TAG_RACE.MECHANICAL, 1);
            this.ClassRacePriorityWarrior.Add(TAG_RACE.PET, 0);
            this.ClassRacePriorityWarrior.Add(TAG_RACE.TOTEM, 0);
            this.ClassRacePriorityWarrior.Add(TAG_RACE.PIRATE, 2);
        }
        // 架嘲讽惩罚
        private void setupGangUpDatabase()
        {
            GangUpDatabase.Add(CardDB.cardNameEN.addledgrizzly, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.alakirthewindlord, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.aldorpeacekeeper, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.ancientoflore, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.ancientofwar, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.antiquehealbot, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.anubarak, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.archmageantonidas, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.armorsmith, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.ayablackpaw, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.azuredrake, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.baronrivendare, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.biggamehunter, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.biteweed, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.bladeofcthun, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.bloodimp, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.bomblobber, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.boneguardlieutenant, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.burglybully, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.burlyrockjawtrogg, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.cabalshadowpriest, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.cairnebloodhoof, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.cenarius, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.chromaggus, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.cobaltguardian, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.coldarradrake, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.coldlightoracle, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.confessorpaletress, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.corendirebrew, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.cthun, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.cultapothecary, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.cultmaster, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.cultsorcerer, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.dementedfrostcaller, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.demolisher, 1);
            //GangUpDatabase.Add(CardDB.cardName.direwolfalpha, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.doppelgangster, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.dragonkinsorcerer, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.drboom, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.earthenringfarseer, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.edwinvancleef, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.emboldener3000, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.emperorthaurissan, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.etherealpeddler, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.felcannon, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.fireelemental, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.fireguarddestroyer, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.flametonguetotem, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.flamewaker, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.flesheatingghoul, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.floatingwatcher, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.foereaper4000, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.friendlybartender, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.frothingberserker, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.gadgetzanauctioneer, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.gahzrilla, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.garr, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.gazlowe, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.gelbinmekkatorque, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.genzotheshark, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.grimestreetenforcer, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.grimscaleoracle, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.grimygadgeteer, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.gruul, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.harrisonjones, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.hemetnesingwary, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.highjusticegrimstone, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.hobgoblin, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.hogger, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.hoggerdoomofelwynn, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.igneouselemental, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.illidanstormrage, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.impmaster, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.infestedtauren, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.ironbeakowl, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.ironjuggernaut, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.ironsensei, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.jadeswarmer, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.jeeves, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.junkbot, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.kelthuzad, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.kingkrush, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.knifejuggler, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.kodorider, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.leeroyjenkins, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.leokk, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.lightwarden, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.lightwell, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.loatheb, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.lucifron, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.lyrathesunshard, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.maexxna, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.malganis, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.malkorok, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.malorne, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.malygos, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.manatidetotem, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.manawyrm, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.masterswordsmith, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.mechwarper, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.medivhtheguardian, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.mekgineerthermaplugg, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.micromachine, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.misha, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.moatlurker, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.moirabronzebeard, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.murlocknight, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.murloctidecaller, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.murlocwarleader, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.nefarian, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.nexuschampionsaraad, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.northshirecleric, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.obsidiandestroyer, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.oldmurkeye, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.onyxia, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.pilotedshredder, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.pintsizedsummoner, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.prophetvelen, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.pyros, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.questingadventurer, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.radiantelemental, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.ragnaroslightlord, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.ragnarosthefirelord, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.raidleader, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.ratpack, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.razorgore, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.recruiter, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.repairbot, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.satedthreshadon, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.savagecombatant, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.savannahhighmane, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.scalednightmare, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.scavenginghyena, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.shadeofnaxxramas, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.shadopanrider, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.shadowboxer, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.shadowfiend, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.shakuthecollector, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.shipscannon, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.siltfinspiritwalker, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.sludgebelcher, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.sneedsoldshredder, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.sorcerersapprentice, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.southseacaptain, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.starvingbuzzard, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.stonesplintertrogg, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.stormwindchampion, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.summoningportal, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.summoningstone, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.swashburglar, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.sylvanaswindrunner, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.theblackknight, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.theboogeymonster, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.timberwolf, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.tirionfordring, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.toshley, 4);
            GangUpDatabase.Add(CardDB.cardNameEN.tradeprincegallywix, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.troggzortheearthinator, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.undercityhuckster, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.undertaker, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.unearthedraptor, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.usherofsouls, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.v07tr0n, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.vaelastrasz, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.viciousfledgling, 2);
            GangUpDatabase.Add(CardDB.cardNameEN.violetillusionist, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.violetteacher, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.vitalitytotem, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.voljin, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.warsongcommander, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.weespellstopper, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.wickedwitchdoctor, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.wobblingrunts, 3);
            GangUpDatabase.Add(CardDB.cardNameEN.xarilpoisonedmind, 3);
            //GangUpDatabase.Add(CardDB.cardName.youngpriestess, 1);
            GangUpDatabase.Add(CardDB.cardNameEN.ysera, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.yshaarjrageunbound, 5);
            GangUpDatabase.Add(CardDB.cardNameEN.valkyrsoulclaimer, 0);
            GangUpDatabase.Add(CardDB.cardNameEN.cryptlord, 4);
        }

        private void setupbuffHandDatabase()
        {
            buffHandDatabase.Add(CardDB.cardNameEN.brassknuckles, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.donhancho, 5);
            buffHandDatabase.Add(CardDB.cardNameEN.grimestreetenforcer, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.grimestreetoutfitter, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.grimestreetpawnbroker, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.grimestreetsmuggler, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.grimscalechum, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.grimygadgeteer, 2);
            buffHandDatabase.Add(CardDB.cardNameEN.hiddencache, 2);
            buffHandDatabase.Add(CardDB.cardNameEN.hobartgrapplehammer, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.shakyzipgunner, 2);
            buffHandDatabase.Add(CardDB.cardNameEN.smugglerscrate, 2);
            buffHandDatabase.Add(CardDB.cardNameEN.smugglersrun, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.stolengoods, 3);
            buffHandDatabase.Add(CardDB.cardNameEN.themistcaller, 1);
            buffHandDatabase.Add(CardDB.cardNameEN.troggbeastrager, 1);
        }

        private void setupequipWeaponPlayDatabase()
        {//添加 装备武器的卡 到 装备武器数据库
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.arathiweaponsmith, 2);//战士职业卡 阿拉希武器匠
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.blingtron3000, 3);//布林顿3000型
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.daggermastery, 1);//匕首精通 盗贼技能1功小刀
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.echolocate, 2);//
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.instructorrazuvious, 5);//教官拉苏维奥斯
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.malkorok, 3);//马尔考罗克 不认识
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.medivhtheguardian, 1);//守护者麦迪文
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.musterforbattle, 1);//作战动员
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.nzothsfirstmate, 1);//恩佐斯的副官
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.poisoneddaggers, 2);//浸毒匕首
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.upgrade, 1);//战士 升级法术
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.visionsoftheassassin, 1);//刺客视界 乱斗牌
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.utheroftheebonblade, 5);//骑士dk 黑锋骑士乌瑟尔
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.scourgelordgarrosh, 4);//战士dk 天灾领主加尔鲁什
            equipWeaponPlayDatabase.Add(CardDB.cardNameEN.swordeater, 3);//吞剑艺人
            //自己可以加入后面出的 会装备武器的新卡
        }

        /// <summary>
        /// 获取使用地标的惩罚值
        /// </summary>
        /// <param name="locationMinion">地标对应的随从对象</param>
        /// <param name="target">地标的目标随从</param>
        /// <param name="p">当前场面</param>
        /// <returns>惩罚值</returns>
        public int getUseLocationPenality(Minion locationMinion, Minion target, Playfield p)
        {
            int enfaceReward = 0;
            if (target != null)
            {
                // 如果目标为不可攻击或已被销毁，返回高惩罚值
                if (target.untouchable || target.Hp <= 0)
                {
                    return 1000; // 高惩罚值，表明不应执行此操作
                }

                // 检查是否有针对英雄攻击的奖励
                if (printUtils.enfaceReward != 0 && target.isHero)
                {
                    enfaceReward = printUtils.enfaceReward; // 获取对敌方英雄的奖励
                }
            }

            // 如果地标本身有特殊条件，例如地标在冷却中或耐久度已为0
            if (locationMinion.CooldownTurn > 0 || locationMinion.Hp <= 0)
            {
                return 1000; // 地标在冷却或已无耐久度，不应使用
            }

            // 调用AI的基础逻辑来获取惩罚值，并减去奖励值
            return ai.botBase.getUseLocationPenality(locationMinion, target, p) - enfaceReward;
        }

        /// <summary>
        /// 获取使用泰坦技能的惩罚值
        /// </summary>
        /// <param name="titanMinion">泰坦随从对象</param>
        /// <param name="target">泰坦技能的目标随从</param>
        /// <param name="p">当前场面</param>
        /// <returns>惩罚值</returns>
        public int getUseTitanAbilityPenality(Minion titanMinion, Minion target, Playfield p)
        {
            // 如果泰坦随从本身有特殊条件，例如冷却中或无技能可用
            if ((titanMinion.handcard.card.TitanAbilityUsed1 && titanMinion.handcard.card.TitanAbilityUsed2 && titanMinion.handcard.card.TitanAbilityUsed3) 
                || titanMinion.handcard.card.Health <= 0)
            {
                return 1000; // 泰坦技能在冷却或已无可用资源，不应使用
            }

            // 调用AI的基础逻辑来获取惩罚值
            return ai.botBase.getUseTitanAbilityPenality(titanMinion, target, p);
        }
    }

}