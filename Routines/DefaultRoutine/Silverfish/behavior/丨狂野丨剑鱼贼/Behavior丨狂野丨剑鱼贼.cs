using System.Collections.Generic;
using System;
using System.Linq;
using log4net;
using Triton.Common.LogUtilities;

namespace HREngine.Bots
{
    public partial class Behavior丨狂野丨剑鱼贼 : Behavior
    {
        private static readonly ILog ilog_0 = Logger.GetLoggerInstanceForType();

        private int bonus_enemy = 4;
        private int bonus_mine = 4;

        public override string BehaviorName()
        {
            return "丨狂野丨剑鱼贼";
        }

        PenalityManager penman = PenalityManager.Instance;

        /// <summary>
        /// 剑鱼贼的留牌策略
        /// </summary>
        /// <param name="cards">起手卡牌列表</param>
        public override void specialMulligan(List<Mulligan.CardIDEntity> cards)
        {
            // 初始化卡牌出现次数字典
            Dictionary<CardDB.cardNameCN, int> cardFlags = new Dictionary<CardDB.cardNameCN, int>()
            {
                {CardDB.cardNameCN.空降歹徒, 0},
                {CardDB.cardNameCN.宝藏经销商, 0},
                {CardDB.cardNameCN.旗标骷髅, 0},
                {CardDB.cardNameCN.鱼排斗士, 0},
                {CardDB.cardNameCN.玩具船, 0},
                {CardDB.cardNameCN.船载火炮, 0},
                {CardDB.cardNameCN.剑鱼, 0}
            };

            // 遍历手牌，记录每张卡牌的出现次数
            foreach (Mulligan.CardIDEntity card in cards)
            {
                CardDB.Card cardCN = CardDB.Instance.getCardDataFromID(card.id);
                if (cardFlags.ContainsKey(cardCN.nameCN))
                {
                    cardFlags[cardCN.nameCN]++;
                }
            }

            // 设置保留规则
            foreach (Mulligan.CardIDEntity card in cards)
            {
                CardDB.Card cardCN = CardDB.Instance.getCardDataFromID(card.id);

                if (cardFlags.ContainsKey(cardCN.nameCN))
                {
                    if (cardFlags[cardCN.nameCN] > 0)
                    {
                        if (cardCN.nameCN == CardDB.cardNameCN.空降歹徒 ||
                            cardCN.nameCN == CardDB.cardNameCN.宝藏经销商 ||
                            cardCN.nameCN == CardDB.cardNameCN.旗标骷髅)
                        {
                            card.holdByRule = 2; // 高优先级保留
                            card.holdReason = "符合规则而保留: 高优先级保留";
                        }
                        else
                        {
                            card.holdByRule = 1; // 中等优先级保留
                            card.holdReason = "符合规则而保留: 中等优先级保留";
                        }
                    }
                }
                else
                {
                    card.holdByRule = 0; // 其他卡牌不保留
                    card.holdReason = "符合规则而弃掉: 其他卡牌不保留";
                }
            }
        }



        /// <summary>
        /// 计算并返回给定卡牌在当前回合的组合技惩罚值。
        /// 该方法用于根据当前手牌、场上随从状态以及对手情况，评估某张卡牌的出牌优先级。
        /// 返回的惩罚值越高，表示该卡牌在当前回合出牌的优先级越低，反之亦然。
        /// </summary>
        /// <param name="card">当前需要评估的卡牌。</param>
        /// <param name="target">该卡牌可能指向的目标随从。</param>
        /// <param name="p">当前的游戏状态，包括己方和对手的场面信息。</param>
        /// <param name="nowHandcard">当前手牌信息，包含卡牌费用和其他属性。</param>
        /// <returns>返回一个整数，表示该卡牌的组合技惩罚值。值越大，优先级越低。</returns>
        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            // 无法选中目标时，返回极高的惩罚值，避免选择该目标
            if (target != null && target.untouchable)
            {
                return 100000;
            }

            // 初始惩罚值设为0
            int pen = 0;

            // 一费检查：如果当前是前两回合且手牌有船载火炮和幸运币或其他一费非海盗随从时，不打出海盗
            if (Hrtprozis.Instance.gTurn <= 2 && card.race == CardDB.Race.PIRATE && p.enemyMinions.Count == 0)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.nameCN == CardDB.cardNameCN.船载火炮)
                    {
                        foreach (Handmanager.Handcard hhc in p.owncards)
                        {
                            if (hhc.card.nameCN == CardDB.cardNameCN.幸运币 || (hhc.getManaCost(p) == 1 && hhc.card.race != CardDB.Race.PIRATE && hhc.card.type == CardDB.cardtype.MOB))
                            {
                                return 1000; // 返回高惩罚值，避免打出该combo
                            }
                        }
                    }
                }
            }

            // 如果卡牌是海盗且场上有船载火炮，降低惩罚值，优先出牌
            if (card.race == CardDB.Race.PIRATE)
            {
                foreach (Minion m in p.ownMinions)
                {
                    if (m.handcard.card.nameCN == CardDB.cardNameCN.船载火炮)
                    {
                        pen -= 100; // 降低惩罚值
                    }
                }
            }

            // 根据不同卡牌设定个性化惩罚策略
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.南海船工:
                    // 没有特殊处理，保留默认惩罚值
                    break;

                case CardDB.cardNameCN.宝藏经销商:
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (hc.card.race == CardDB.Race.PIRATE)
                        {
                            pen -= 30; // 如果手牌有海盗，降低惩罚值
                        }
                    }
                    pen -= 30; // 默认降低惩罚值
                    break;

                case CardDB.cardNameCN.旗标骷髅:
                    if (p.ownWeapon.Durability > 0)
                        pen -= 5; // 如果武器耐久度大于0，降低惩罚值
                    break;

                case CardDB.cardNameCN.海盗帕奇斯:
                    // 没有特殊处理，保留默认惩罚值
                    break;

                case CardDB.cardNameCN.秘密通道:
                    if (p.owncards.Count >= 5 || p.ownMaxMana <= 4 || p.mana <= 3)
                    {
                        return 10; // 如果手牌数大于等于5或法力上限小于3或剩余法力值小于等于2，增加惩罚值，不打出
                    }

                    if (p.ownHero.Angr < 3 && p.ownMaxMana >= 5) // 如果手中没有强力武器，且费用大于等于5
                    {
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.nameCN == CardDB.cardNameCN.剑鱼)
                            {
                                return 10; // 如果有这些卡牌，适度增加惩罚值
                            }
                        }
                        return -30; // 否则，降低惩罚值，优先使用秘密通道寻找武器
                    }
                    return 0; // 保持默认惩罚值
                case CardDB.cardNameCN.鱼排斗士:
                    if (p.ownMaxMana <= 1 && target.Hp == 1)
                        return -200; // 如果只有1费，且目标生命值为1，极大降低惩罚值，优先打出

                    if (target.Hp == 1)
                        return -10; // 如果目标生命值为1，适度降低惩罚值

                    return 0; // 否则，保持默认惩罚值
                case CardDB.cardNameCN.换挡漂移:
                    int index = 0;
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (index == 2)
                            break; // 只处理手牌中的前两张

                        if (hc.card.nameCN == CardDB.cardNameCN.剑鱼)
                            return 1000; // 如果有剑鱼，返回极高惩罚值，不打出换挡漂移

                        if (hc.card.nameCN == CardDB.cardNameCN.海盗帕奇斯 && index < 3)
                            pen -= 30; // 如果有海盗帕奇斯，降低惩罚值

                        if (hc.card.nameCN == CardDB.cardNameCN.空降歹徒 && index < 3)
                            pen += 35; // 如果有空降歹徒，增加惩罚值

                        index += 1;
                    }
                    break;

                case CardDB.cardNameCN.玩具船:
                    // 规则1: 如果场上已经有旗标骷髅，并且存在武器，最优先出牌
                    bool hasBannerSkeletonOnBoard = false;
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.handcard.card.nameCN == CardDB.cardNameCN.旗标骷髅 && p.ownWeapon.Durability > 0)
                        {
                            hasBannerSkeletonOnBoard = true;
                            break;
                        }
                    }

                    if (hasBannerSkeletonOnBoard)
                    {
                        pen -= 200; // 极大降低惩罚值，最优先打出玩具船
                    }
                    else
                    {
                        // 规则2: 如果手牌费用合适，出完玩具船后还能出2张或更多海盗，优先出牌
                        int remainingManaAfterToyBoat = p.mana - nowHandcard.getManaCost(p);
                        int affordablePirates = 0;
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.race == CardDB.Race.PIRATE && hc.getManaCost(p) <= remainingManaAfterToyBoat)
                            {
                                remainingManaAfterToyBoat -= hc.getManaCost(p);
                                affordablePirates++;
                                if (affordablePirates >= 2) break;
                            }
                        }

                        if (affordablePirates >= 2)
                        {
                            pen -= 100; // 降低惩罚值，优先打出玩具船
                        }
                    }
                    break;
                case CardDB.cardNameCN.空降歹徒:
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (hc.card.race == CardDB.Race.PIRATE)
                        {
                            return 1000; // 如果手牌中有1费海盗，禁止使用空降歹徒
                        }
                    }
                    pen += bonus_mine * 4; // 增加惩罚值，避免在不合适时机打出
                    break;
                case CardDB.cardNameCN.船载火炮:
                    // 如果是第二回合，且手中有1费海盗，极大降低惩罚值，优先打出
                    if (Hrtprozis.Instance.gTurn == 2)
                    {
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.getManaCost(p) == 1 && hc.card.race == CardDB.Race.PIRATE)
                            {
                                pen -= 100;
                            }
                        }
                    }
                    pen += bonus_mine * 3; // 增加惩罚值，避免不必要的打出

                    // 如果费用小于等于2且敌方没有随从，降低惩罚值，优先打出海盗
                    if (p.ownMaxMana <= 2 && p.enemyMinionStartCount == 0)
                    {
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.race == CardDB.Race.PIRATE)
                            {
                                pen -= bonus_mine * 5;
                            }
                        }
                    }

                    if (p.enemyMinions.Count > 1 && p.ownMaxMana < 3 && p.ownMinions.Count == 0)
                        pen += bonus_mine; // 如果敌方随从较多且我方无随从，增加惩罚值
                    break;
                case CardDB.cardNameCN.剑鱼:
                    if (p.ownWeapon.Angr > 2)
                        return 100000; // 如果武器攻击力大于2，极大增加惩罚值，不打出剑鱼

                    return -2000; // 否则，极大降低惩罚值，优先打出剑鱼
                case CardDB.cardNameCN.恐怖海盗:
                    if (nowHandcard.getManaCost(p) <= 0)
                        pen -= 10; // 如果卡牌费用为0，适度降低惩罚值

                    if (nowHandcard.getManaCost(p) >= 3)
                        pen += 10; // 如果卡牌费用大于等于3，适度增加惩罚值
                    break;
                case CardDB.cardNameCN.奇利亚斯豪华版3000型:
                    if (p.ownMinions.Count >= 3)
                        pen -= 50; // 如果场上有3个及以上随从，极大降低惩罚值

                    if (nowHandcard.getManaCost(p) <= 0)
                        pen -= 60; // 如果卡牌费用为0，极大降低惩罚值

                    if (nowHandcard.getManaCost(p) <= 1)
                        pen -= 40; // 如果卡牌费用为1，适度降低惩罚值

                    if (nowHandcard.getManaCost(p) <= 3)
                        pen -= 10; // 如果卡牌费用为3，适度降低惩罚值

                    if (nowHandcard.getManaCost(p) > 3)
                        pen += 10; // 如果卡牌费用大于3，适度增加惩罚值
                    break;
                case CardDB.cardNameCN.奖品掠夺者:
                    // 如果本回合没有打出其他牌，则惩罚值增加
                    if (p.cardsPlayedThisTurn == 0)
                    {
                        pen += 10;
                    }
                    break;
                case CardDB.cardNameCN.匕首精通:
                    if (p.ownWeapon.Angr > 1)
                        return 100000; // 如果当前武器的攻击力大于1，极大增加惩罚值，不打出匕首精通

                    if (p.ownWeapon.Angr == 1 && p.ownWeapon.Durability == 1)
                        return 8; // 如果当前武器攻击力为1且耐久度为1，适度增加惩罚值，避免浪费

                    return 10; // 在其他情况下，适度增加惩罚值
                case CardDB.cardNameCN.悦耳嘻哈:
                    if (p.ownWeapon.Durability > 1)
                    {
                        pen -= (p.ownWeapon.Durability * bonus_mine + p.ownWeapon.Angr + bonus_mine / 3); // 如果当前有武器，降低惩罚值，优先使用悦耳嘻哈
                    }
                    else
                    {
                        pen += 50; // 如果当前没有武器，增加惩罚值，降低使用优先级
                    }
                    break;
            }
            return pen; // 返回最终计算的惩罚值
        }

        /// <summary>
        /// 探底卡的价值
        /// </summary>
        /// <param name="card"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getDredgeVal(CardDB.Card card, Playfield p)
        {
            switch (card.nameCN)
            {
                // 高优先级 - 抽到胜率60%以上，且是海盗卡牌
                case CardDB.cardNameCN.旗标骷髅:
                    return 20;

                // 中等优先级 - 抽到胜率55%到60%之间，或者重要的海盗卡牌
                case CardDB.cardNameCN.宝藏经销商:
                case CardDB.cardNameCN.空降歹徒:
                case CardDB.cardNameCN.恐怖海盗:
                case CardDB.cardNameCN.鱼排斗士:
                    return 15;

                // 低等优先级 - 海盗种族，剑鱼探底加攻
                case CardDB.cardNameCN.南海船工:
                case CardDB.cardNameCN.奖品掠夺者:
                    return 10;
                case CardDB.cardNameCN.海盗帕奇斯:
                    return 6;

                // 次级优先级 - 抽到胜率高，但不是海盗种族的卡牌
                case CardDB.cardNameCN.剑鱼:
                case CardDB.cardNameCN.秘密通道:
                case CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameCN.玩具船:
                case CardDB.cardNameCN.冷血:
                case CardDB.cardNameCN.奇利亚斯豪华版3000型:
                    return 5;

                // 未特别指定的卡牌，根据WinrateWhenDrawn计算优先级
                default:
                    // 获取当前卡的职业
                    string className = card.Class.ToString().ToUpper();

                    //初始化Hsreplay数据
                    Hsreplay hs = Hsreplay.Instance;

                    // 从对应职业的数据列表中找到匹配的卡牌数据
                    var cardStats = Hsreplay.AllCardStats.FirstOrDefault(c => c.DbfId == card.dbfId);

                    if (cardStats != null)
                    {
                        Helpfunctions.Instance.logg("getDredgeVal - 使用Hsreplay数据比对" + card.nameCN.ToString() + " => " + cardStats.WinrateWhenDrawn);
                        ilog_0.Info("getDredgeVal - 使用Hsreplay数据比对" + card.nameCN.ToString() + " => " + cardStats.WinrateWhenDrawn);
                        // 返回计算优先级
                        return (int)(20 * cardStats.WinrateWhenDrawn / 100);
                    }

                    // 如果找不到对应的卡牌数据，或者职业数据不存在，则返回最低优先级
                    return 0;
            }
        }

        /// <summary>
        /// 核心，场面值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override float getPlayfieldValue(Playfield p)
        {
            if (p.value > -200000) return p.value;
            float retval = 0;
            retval += getGeneralVal(p);
            retval += p.owncarddraw * 5;
            retval -= p.lostWeaponDamage;
            // 危险血线
            int hpboarder = 5;
            // 不考虑法强了
            if (p.enemyHeroName == HeroEnum.mage) retval += 2 * p.enemyspellpower;
            // 抢脸血线
            int aggroboarder = 20;
            retval += getHpValue(p, hpboarder, aggroboarder);
            // 出牌序列数量
            int count = p.playactions.Count;
            int ownActCount = 0;
            // 排序问题！！！！
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i];
                ownActCount++;
                switch (a.actionType)
                {
                    // 英雄攻击
                    case actionEnum.attackWithHero:
                        retval -= i;
                        continue;
                    case actionEnum.useHeroPower:
                    case actionEnum.playcard:
                        break;
                    default:
                        continue;
                }
                if (a.card.card.race == CardDB.Race.PIRATE)
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.handcard.card.nameCN == CardDB.cardNameCN.船载火炮)
                        {
                            retval += 10 - i * 3;
                            break;
                        }
                    }
                switch (a.card.card.nameCN)
                {
                    // 排序优先
                    case CardDB.cardNameCN.幸运币:
                        retval -= 6 * i;
                        break;
                    case CardDB.cardNameCN.秘密通道:
                        retval -= 6 * i;
                        break;
                    case CardDB.cardNameCN.船载火炮:
                        retval -= 3 * i;
                        break;
                    case CardDB.cardNameCN.玩具船:
                        retval -= 3 * i;
                        break;
                    case CardDB.cardNameCN.宝藏经销商:
                        retval -= 2 * i;
                        break;
                    case CardDB.cardNameCN.旗标骷髅:
                        retval -= 2 * i;
                        break;

                }
            }

            // 对手基本随从交换模拟
            retval -= p.lostDamage;
            retval += getSecretPenality(p); // 奥秘的影响
            retval -= p.enemyWeapon.Angr * 3 + p.enemyWeapon.Durability * 3;


            // 特殊：优势防亵渎
            if (retval > 50 && p.enemyHeroStartClass == TAG_CLASS.WARLOCK && p.enemyMinions.Count == 0 && p.ownMinions.Count > 2)
            {
                bool found = false;
                // 从 2 开始防，术士自带一堆 1
                for (int i = 1; i <= 10; i++)
                {
                    found = false;
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.Hp == i)
                        {
                            retval -= 2 * (i - 1);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        if (i == 1) retval += 10;
                        if (i == 2) retval += 10;
                        break;
                    }
                }
            }


            return retval;
        }



        /// <summary>
        /// 敌方随从价值 主要等于（HP + Angr） * 4
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
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
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            if (m.Hp <= 0) return 0;
            int retval = 0;
            if (m.Angr > 0 || m.taunt || m.handcard.card.race == CardDB.Race.TOTEM || p.enemyHeroStartClass == TAG_CLASS.PALADIN || p.enemyHeroStartClass == TAG_CLASS.PRIEST)
                retval += m.Hp * 4;
            retval += m.spellpower * 2;
            retval += m.Hp * m.Angr / 2;
            if (!m.frozen && (!m.cantAttack || m.handcard.card.nameCN == CardDB.cardNameCN.邪刃豹))
            {
                retval += m.Angr * 4;
                if (m.Angr > 5) retval += 10;
                if (m.windfury) retval += m.Angr * 2;
            }
            if (m.silenced) return retval;

            if (m.taunt) retval += 2;
            if (m.divineshild) retval += m.Angr * 2;
            if (m.divineshild && m.taunt) retval += 5;
            if (m.stealth) retval += 2;

            // 鱼人
            if (m.handcard.card.race == CardDB.Race.MURLOC) retval += bonus_enemy * 4;

            // 剧毒价值两点属性
            if (m.poisonous)
            {
                retval += 8;
            }
            if (m.lifesteal) retval += m.Angr * bonus_enemy * 4;

            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.巫师学徒:
                case CardDB.cardNameCN.肢体商贩:
                case CardDB.cardNameCN.巨型图腾埃索尔:
                case CardDB.cardNameCN.驻锚图腾:
                case CardDB.cardNameCN.刺豚拳手:
                case CardDB.cardNameCN.空中飞爪:
                case CardDB.cardNameCN.金翼巨龙:
                case CardDB.cardNameCN.安保自动机:
                case CardDB.cardNameCN.机械跃迁者:
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                case CardDB.cardNameCN.对空奥术法师:
                case CardDB.cardNameCN.前沿哨所:
                case CardDB.cardNameCN.战场军官:
                case CardDB.cardNameCN.伯尔纳锤喙:
                case CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameCN.塔姆辛罗姆:
                case CardDB.cardNameCN.暗影珠宝师汉纳尔:
                case CardDB.cardNameCN.伦萨克大王:
                case CardDB.cardNameCN.布莱恩铜须:
                case CardDB.cardNameCN.观星者露娜:
                case CardDB.cardNameCN.大法师瓦格斯:
                case CardDB.cardNameCN.火妖:
                case CardDB.cardNameCN.下水道渔人:
                case CardDB.cardNameCN.空中炮艇:
                case CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameCN.火舌图腾:
                case CardDB.cardNameCN.末日预言者:
                case CardDB.cardNameCN.莫尔杉哨所:
                case CardDB.cardNameCN.鱼人领军:
                case CardDB.cardNameCN.南海船长:
                case CardDB.cardNameCN.灭龙弩炮:
                case CardDB.cardNameCN.战马训练师:
                case CardDB.cardNameCN.加基森拍卖师:
                case CardDB.cardNameCN.健谈的调酒师:
                case CardDB.cardNameCN.豪宅管家俄里翁:
                case CardDB.cardNameCN.小鬼骑士:
                case CardDB.cardNameCN.针岩图腾:
                case CardDB.cardNameCN.伴唱机:
                    retval += 150;
                    break;

                // 不解巨大劣势
                case CardDB.cardNameCN.安娜科德拉:
                case CardDB.cardNameCN.农夫:
                case CardDB.cardNameCN.旗标骷髅:
                case CardDB.cardNameCN.尼鲁巴蛛网领主:
                case CardDB.cardNameCN.凯瑞尔罗姆:
                case CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameCN.鲨鳍后援:
                case CardDB.cardNameCN.相位追猎者:
                case CardDB.cardNameCN.鱼人宝宝车队:
                case CardDB.cardNameCN.饥饿的秃鹫:
                case CardDB.cardNameCN.锈水海盗:
                case CardDB.cardNameCN.盛装歌手:
                case CardDB.cardNameCN.玛克扎尔的小鬼:
                case CardDB.cardNameCN.发明机器人:
                case CardDB.cardNameCN.宝藏经销商:
                case CardDB.cardNameCN.随船外科医师:
                case CardDB.cardNameCN.玩具船:
                    retval += 50;
                    break;
                // 算有点用
                case CardDB.cardNameCN.贪婪的书虫:
                case CardDB.cardNameCN.治疗图腾:
                case CardDB.cardNameCN.力量图腾:
                case CardDB.cardNameCN.神秘女猎手:
                case CardDB.cardNameCN.矮人神射手:
                case CardDB.cardNameCN.低阶侍从:
                case CardDB.cardNameCN.战斗邪犬:
                case CardDB.cardNameCN.法力浮龙:
                case CardDB.cardNameCN.飞刀杂耍者:
                    retval += 15;
                    break;
            }
            return retval;
        }

        /// <summary>
        /// 我方随从价值，大致等于主要等于 （HP + Angr） * 4 
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
            if (m.Hp <= 0) return 0;
            int retval = 5;
            if (m.Hp <= 0) return 0;
            retval += m.Hp * 4;
            retval += m.Angr * 4;
            retval += m.Hp * m.Angr / 2;
            // 高攻低血是垃圾
            if (m.Angr > m.Hp + 4) retval -= (m.Angr - m.Hp) * 3;

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

            int bonus = 4;
            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.虚触侍从:
                    retval += 2 * bonus_mine;
                    break;
                case CardDB.cardNameCN.船载火炮:
                    retval += 3 * bonus_mine;
                    break;
                case CardDB.cardNameCN.随船外科医师:
                    retval += 1 * bonus_mine;
                    break;
            }
            return retval;
        }

        public override int getSirFinleyPriority(List<Handmanager.Handcard> discoverCards)
        {
            return -1;
        }

        public override int getSirFinleyPriority(CardDB.Card card)
        {
            return SirFinleyPriorityList[card.nameEN];
        }

        private Dictionary<CardDB.cardNameEN, int> SirFinleyPriorityList = new Dictionary<CardDB.cardNameEN, int>
        {
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

        /// <summary>
        /// 敌我生命值的价值判定
        /// </summary>
        /// <param name="p">场面</param>
        /// <param name="hpboarder">我方危险血线</param>
        /// <param name="aggroboarder">敌方危险血线</param>
        public override int getHpValue(Playfield p, int hpboarder, int aggroboarder)
        {
            int offset_enemy = 0;
            int offset_mine = p.calEnemyTotalAngr() + Hrtprozis.Instance.enemyDirectDmg;

            int retval = 0;
            // 血线安全
            if (p.ownHero.Hp + p.ownHero.armor - offset_mine > hpboarder)
            {
                retval += (5 + p.ownHero.Hp + p.ownHero.armor - offset_mine - hpboarder) * 3 / 2;
            }
            // 快死了
            else if (p.ownHero.Hp + p.ownHero.armor - offset_mine > 0)
            {
                retval -= 4 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor + offset_mine) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor + offset_mine);
            }
            else
            {
                retval -= 3 * (hpboarder + 1) * (hpboarder + 1) + 100;
            }
            if (p.ownHero.Hp + p.ownHero.armor - offset_mine < 6 && p.ownHero.Hp + p.ownHero.armor - offset_mine > 0)
            {
                retval -= 80 / (p.ownHero.Hp + p.ownHero.armor - offset_mine);
            }
            // 对手血线安全
            if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy >= aggroboarder)
            {
                retval += 3 * (aggroboarder - p.enemyHero.Hp - p.enemyHero.armor - offset_enemy);
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
            // 下回合斩杀本回合打脸
            if (p.calDirectDmg(p.ownMaxMana + 1, false, true) >= p.enemyHero.Hp + p.enemyHero.armor)
            {
                retval += 100;
            }
            return retval;
        }

        /// <summary>
        /// 攻击触发的奥秘惩罚
        /// </summary>
        /// <param name="si"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public override int getSecretPen_CharIsAttacked(Playfield p, SecretItem si, Minion attacker, Minion defender)
        {
            if (attacker.isHero) return 0;
            int pen = 0;
            // 攻击的基本惩罚
            if (si.canBe_explosive && !defender.isHero)
            {
                pen -= 20;
                //pen += (attacker.Hp + attacker.Angr);
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_explosive = false;
                }
            }
            return pen;
        }
    }
}

