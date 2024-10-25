using System.Collections.Generic;
using System;
using System.Linq;
using log4net;
using Triton.Common.LogUtilities;

namespace HREngine.Bots
{
    public partial class Behavior丨通用丨不设惩罚 : Behavior
    {
        private static readonly ILog ilog_0 = Logger.GetLoggerInstanceForType();

        private int bonus_enemy = 4;
        private int bonus_mine = 4;
        // 危险血线
        private int hpboarder = 15;
        // 抢脸血线
        private int aggroboarder = 15;
        

        public override string BehaviorName() { return "丨通用丨不设惩罚"; }
        PenalityManager penman = PenalityManager.Instance;

        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            // 无法选中
            if (target != null && target.untouchable)
            {
                return 100000;
            }
            // 初始惩罚值
            int penalty = 0;

            switch (card.nameCN.ToString())
            {
                case "树篱迷宫":
                    penalty = -47; // 优先级数值转为负值作为惩罚值
                    break;
                case "远足步道":
                    penalty = -49;
                    break;
                case "尤格萨隆的监狱":
                    penalty = -52;
                    break;
                case "惊险悬崖":
                    penalty = -68;
                    break;
                case "鹦鹉乐园":
                    penalty = -59;
                    break;
                case "大地之末号":
                    penalty = -59;
                    break;
                case "潮汐之地":
                    penalty = -50;
                    break;
                case "小玩物小屋":
                    penalty = -48;
                    break;
                default:
                    penalty = 0; // 如果卡牌名称不匹配，使用初始惩罚值
                    break;
            }
            return penalty;
        }

        // 核心，场面值
        public override float getPlayfieldValue(Playfield p)
        {
            if (p.value > -200000) return p.value;
            float retval = 0;
            retval += getGeneralVal(p);            
            retval += getHpValue(p, hpboarder, aggroboarder);
            // 出牌序列数量
            int count = p.playactions.Count;
            int ownActCount = 0;
            bool useAb = false;
            // 排序问题！！！！
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i];
                ownActCount++;
                switch (a.actionType)
                {
                    case actionEnum.trade:
                        retval -= 20;
                        continue;
                    case actionEnum.useLocation:
                        retval -= (10 * a.own.handcard.getManaCost(p));
                        continue;
                    case actionEnum.useTitanAbility:
                        retval -= 20;
                        continue;
                    case actionEnum.forge:
                        retval -= 20;
                        continue;
                    // 英雄攻击
                    case actionEnum.attackWithHero:
                        continue;
                    case actionEnum.useHeroPower:
                        useAb = true;
                        break;
                    case actionEnum.playcard:
                        break;
                    default:
                        continue;
                }
                switch (a.card.card.nameCN)
                {
                    case CardDB.cardNameCN.幸运币:
                        retval -= i;
                        break;
                }
            }
            // 对手基本随从交换模拟
            retval += enemyTurnPen(p);
            retval -= p.lostDamage;
            retval += getSecretPenality(p); // 奥秘的影响
            retval -= p.enemyWeapon.Angr * 3 + p.enemyWeapon.Durability * 3;
            return retval;
        }


        // 敌方随从价值 主要等于 （HP + Angr） * 4  
        public override int getEnemyMinionValue(Minion m, Playfield p)
        {
            bool dieNextTurn = false;
            foreach (Minion mm in p.enemyMinions)
            {
                if (mm.handcard.card.nameCN == CardDB.cardNameCN.末日预言者)
                {
                    dieNextTurn = true;
                    break;
                }
            }
            foreach (CardDB.cardIDEnum s in p.ownSecretsIDList)
            {
                if (s == CardDB.cardIDEnum.EX1_610 || s == CardDB.cardIDEnum.VAN_EX1_610)
                {
                    if (m.Hp <= 2)
                    {
                        dieNextTurn = true;
                        break;
                    }
                }
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            if (m.Hp <= 0) return 0;
            int retval = 4;
            if (m.Angr > 0 || p.enemyHeroStartClass == TAG_CLASS.PRIEST)
                retval += m.Hp * bonus_enemy;
            retval += m.spellpower * bonus_enemy * 3 / 2;
            if (!m.frozen && !m.cantAttack)
            {
                retval += m.Angr * bonus_enemy;
                if (m.windfury) retval += m.Angr * bonus_enemy / 2;
            }
            if (m.silenced) return retval;

            if (m.taunt) retval += 2;
            if (m.divineshild) retval += m.Angr * 2;
            if (m.divineshild && m.taunt) retval += 5;
            if (m.stealth) retval += 2;

            if (m.lifesteal) retval += m.Angr * bonus_enemy;

            if (m.poisonous)
            {
                retval += 4;
                if (p.ownMinions.Count < p.enemyMinions.Count) retval += 10;
            }
            // 异能价值
            switch (m.handcard.card.nameCN)
            {
                // 解不掉游戏结束
                case CardDB.cardNameCN.对空奥术法师:
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                case CardDB.cardNameCN.黑眼:
                case CardDB.cardNameCN.聒噪怪:
                case CardDB.cardNameCN.鲨鱼之灵:
                case CardDB.cardNameCN.农夫:
                case CardDB.cardNameCN.尼鲁巴蛛网领主:
                case CardDB.cardNameCN.前沿哨所:
                case CardDB.cardNameCN.洛萨:
                case CardDB.cardNameCN.考内留斯罗姆:
                case CardDB.cardNameCN.战场军官:
                case CardDB.cardNameCN.大领主弗塔根:
                case CardDB.cardNameCN.圣殿蜡烛商:
                case CardDB.cardNameCN.伯尔纳锤喙:
                case CardDB.cardNameCN.魅影歹徒:
                case CardDB.cardNameCN.灵魂窃贼:
                case CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameCN.原野联络人:
                case CardDB.cardNameCN.狂欢报幕员:
                case CardDB.cardNameCN.巫师学徒:
                case CardDB.cardNameCN.塔姆辛罗姆:
                case CardDB.cardNameCN.导师火心:
                case CardDB.cardNameCN.伊纳拉碎雷:
                case CardDB.cardNameCN.暗影珠宝师汉纳尔:
                case CardDB.cardNameCN.伦萨克大王:
                case CardDB.cardNameCN.洛卡拉:
                case CardDB.cardNameCN.布莱恩铜须:
                case CardDB.cardNameCN.观星者露娜:
                case CardDB.cardNameCN.大法师瓦格斯:
                case CardDB.cardNameCN.火妖:
                case CardDB.cardNameCN.下水道渔人:
                case CardDB.cardNameCN.空中炮艇:
                case CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameCN.团伙核心:
                case CardDB.cardNameCN.巡游领队:
                case CardDB.cardNameCN.科卡尔驯犬者:
                case CardDB.cardNameCN.火舌图腾:
                    retval += bonus_enemy * 8;
                    break;
                // 不解巨大劣势
                case CardDB.cardNameCN.拍卖师亚克森:
                case CardDB.cardNameCN.法师猎手:
                case CardDB.cardNameCN.萨特监工:
                case CardDB.cardNameCN.甩笔侏儒:
                case CardDB.cardNameCN.精英牛头人酋长金属之神:
                case CardDB.cardNameCN.莫尔杉哨所:
                case CardDB.cardNameCN.凯瑞尔罗姆:
                case CardDB.cardNameCN.鱼人领军:
                case CardDB.cardNameCN.南海船长:
                case CardDB.cardNameCN.坎雷萨德埃伯洛克:
                case CardDB.cardNameCN.人偶大师多里安:
                case CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameCN.灭龙弩炮:
                case CardDB.cardNameCN.神秘女猎手:
                case CardDB.cardNameCN.鲨鳍后援:
                case CardDB.cardNameCN.怪盗图腾:
                case CardDB.cardNameCN.矮人神射手:
                case CardDB.cardNameCN.任务达人:
                case CardDB.cardNameCN.贪婪的书虫:
                case CardDB.cardNameCN.战马训练师:
                case CardDB.cardNameCN.相位追猎者:
                case CardDB.cardNameCN.鱼人宝宝车队:
                case CardDB.cardNameCN.科多兽骑手:
                case CardDB.cardNameCN.奥秘守护者:
                case CardDB.cardNameCN.获救的流民:
                case CardDB.cardNameCN.白银之手新兵:
                case CardDB.cardNameCN.低阶侍从:
                    retval += bonus_enemy * 3;
                    break;
                // 算有点用
                case CardDB.cardNameCN.幽灵狼前锋:
                case CardDB.cardNameCN.战斗邪犬:
                case CardDB.cardNameCN.饥饿的秃鹫:
                case CardDB.cardNameCN.法力浮龙:
                case CardDB.cardNameCN.加基森拍卖师:
                case CardDB.cardNameCN.飞刀杂耍者:
                case CardDB.cardNameCN.锈水海盗:
                case CardDB.cardNameCN.大法师安东尼达斯:
                    retval += bonus_enemy * 2;
                    break;
            }
            // 血量越低，解怪优先度越高
            if (p.ownHero.Hp <= 15)
            {
                retval += (16 - p.ownHero.Hp) * 3;
                if (p.ownHero.Hp <= 6) retval *= 2;
            }
            return retval;
        }
        /// <summary>
        /// 我方随从价值
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getMyMinionValue(Minion m, Playfield p)
        {
            bool dieNextTurn = false;
            foreach (Minion mm in p.enemyMinions)
            {
                if (mm.handcard.card.nameCN == CardDB.cardNameCN.末日预言者)
                {
                    dieNextTurn = true;
                    break;
                }
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            int retval = 5;
            if (m.Hp <= 0) return 0;
            retval += m.Hp * bonus_mine;
            retval += m.Angr * bonus_mine;
            if (m.Hp <= 1 && !m.divineshild) retval -= (m.Angr - 1) * (bonus_mine - 1);
            // 高攻低血是垃圾
            if (m.Angr > m.Hp + 4) retval -= (m.Angr - m.Hp) * (bonus_mine - 1);
            // 风怒价值
            if ((!m.playedThisTurn || m.rush == 1 || m.charge == 1) && m.windfury) retval += m.Angr;
            // 圣盾价值
            if (m.divineshild) retval += m.Angr * 3;
            // 潜行价值
            if (m.stealth) retval += m.Angr / 2 + 1;
            // 吸血
            if (m.lifesteal) retval += m.Angr / 2 + 1;
            // 圣盾嘲讽
            if (m.divineshild && m.taunt) retval += 4;
            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.黑眼:
                    break;
            }
            if (m.dormant > 0)
            {
                retval -= bonus_mine * m.dormant;
            }
            return retval;
        }

        public override int getSirFinleyPriority(List<Handmanager.Handcard> discoverCards)
        {

            return -1; //comment out or remove this to set manual priority
            int sirFinleyChoice = -1;
            int tmp = int.MinValue;
            for (int i = 0; i < discoverCards.Count; i++)
            {
                CardDB.cardNameEN name = discoverCards[i].card.nameEN;
                if (SirFinleyPriorityList.ContainsKey(name) && SirFinleyPriorityList[name] > tmp)
                {
                    tmp = SirFinleyPriorityList[name];
                    sirFinleyChoice = i;
                }
            }
            return sirFinleyChoice;
        }
        public override int getSirFinleyPriority(CardDB.Card card)
        {
            return SirFinleyPriorityList[card.nameEN];
        }

        private Dictionary<CardDB.cardNameEN, int> SirFinleyPriorityList = new Dictionary<CardDB.cardNameEN, int>
        {
            //{HeroPowerName, Priority}, where 0-9 = manual priority
            { CardDB.cardNameEN.lesserheal, 0 },
            { CardDB.cardNameEN.shapeshift, 6 },
            { CardDB.cardNameEN.fireblast, 7 },
            { CardDB.cardNameEN.totemiccall, 1 },
            { CardDB.cardNameEN.lifetap, 9 },
            { CardDB.cardNameEN.daggermastery, 5 },
            { CardDB.cardNameEN.reinforce, 4 },
            { CardDB.cardNameEN.armorup, 2 },
            { CardDB.cardNameEN.steadyshot, 8 }
        };

        public override int getHpValue(Playfield p, int hpboarder, int aggroboarder)
        {
            int offset_enemy = 0;

            int retval = 0;
            // 血线安全
            if (p.ownHero.Hp + p.ownHero.armor > hpboarder)
            {
                retval += (5 + p.ownHero.Hp + p.ownHero.armor - hpboarder);
            }
            // 快死了
            else
            {
                //if (p.nextTurnWin()) retval -= (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
                retval -= 5 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
            }
            if (p.ownHero.Hp + p.ownHero.armor < 10 && p.ownHero.Hp + p.ownHero.armor > 0)
            {
                retval -= 200 / (p.ownHero.Hp + p.ownHero.armor);
            }
            // 对手血线安全
            if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy >= aggroboarder)
            {
                retval += 2 * (aggroboarder - p.enemyHero.Hp - p.enemyHero.armor - offset_enemy);
            }
            // 开始打脸
            else
            {
                retval += 4 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor - offset_enemy);
            }
            // 场攻+直伤大于对方生命，预计完成斩杀
            if (p.anzEnemyTaunt == 0 && p.calTotalAngr() + p.calDirectDmg(p.mana, false) >= p.enemyHero.Hp + p.enemyHero.armor)
            {
                retval += 2000;
            }
            return retval;
        }

        /// <summary>
        /// 获取使用地标的惩罚值
        /// </summary>
        /// <param name="m"></param>
        /// <param name="target"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getUseLocationPenality(Minion m, Minion target, Playfield p)
        {
            int penalty = 0; // 初始惩罚值为 0

            switch (m.handcard.card.nameCN.ToString())
            {
                case "树篱迷宫":
                    penalty = -47; // 优先级数值转为负值作为惩罚值
                    break;
                case "远足步道":
                    penalty = -49;
                    break;
                case "尤格萨隆的监狱":
                    penalty = -52;
                    break;
                case "惊险悬崖":
                    penalty = -68;
                    break;
                case "鹦鹉乐园":
                    penalty = -59;
                    break;
                case "大地之末号":
                    penalty = -59;
                    break;
                case "潮汐之地":
                    penalty = -50;
                    break;
                case "小玩物小屋":
                    penalty = -48;
                    break;
                default:
                    penalty = 0; // 如果卡牌名称不匹配，使用初始惩罚值
                    break;
            }

            return (int)penalty;
        }

        /// <summary>
        /// 获取使用泰坦技能的惩罚值
        /// </summary>
        /// <param name="m"></param>
        /// <param name="target"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getUseTitanAbilityPenality(Minion m, Minion target, Playfield p)
        {
            return 0;
        }

        /// <summary>
        /// 发现卡的价值
        /// </summary>
        /// <param name="card"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getDiscoverVal(CardDB.Card card, Playfield p)
        {
            //初始化Hsreplay数据
            Hsreplay hs = Hsreplay.Instance;

            // 从对应职业的数据列表中找到匹配的卡牌数据
            var cardStats = Hsreplay.AllCardStats.FirstOrDefault(c => c.DbfId == card.dbfId);

            if (cardStats != null)
            {
                Helpfunctions.Instance.logg("getDiscoverVal - 使用Hsreplay数据比对" + card.nameCN.ToString() + " => " + cardStats.WinrateWhenDrawn);
                ilog_0.Info("getDiscoverVal - 使用Hsreplay数据比对" + card.nameCN.ToString() + " => " + cardStats.WinrateWhenDrawn);
                // 返回 WinrateWhenDrawn 的整数部分
                return (int)cardStats.WinrateWhenDrawn;
            }

            // 如果找不到对应的卡牌数据，或者职业数据不存在，则返回默认值
            return 0;
        }

    }
}