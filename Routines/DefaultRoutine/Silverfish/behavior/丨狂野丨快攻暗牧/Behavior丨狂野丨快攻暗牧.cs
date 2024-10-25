using System.Collections.Generic;
using System;
using Logger = Triton.Common.LogUtilities.Logger;
using log4net;
using System.Linq;

namespace HREngine.Bots
{
    public partial class Behavior丨狂野丨快攻暗牧 : Behavior
    {
        private int bonus_enemy = 4;
        private int bonus_mine = 4;

        public override string BehaviorName() { return "丨狂野丨快攻暗牧"; }
        PenalityManager penman = PenalityManager.Instance;

        //改于2024.9.9  by血色

        // 存储海盗卡牌的集合
        HashSet<CardDB.Card> pirateCards = new HashSet<CardDB.Card>()
        {
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.TOY_518),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.VAC_512),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CFM_637),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CORE_WON_065),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.WON_065),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.DRG_056),
            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.DED_513),
        };

        //文本输出
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        /// <summary>
        /// 快攻暗牧的留牌策略
        /// </summary>
        /// <param name="cards">起手卡牌列表</param>
        public override void specialMulligan(List<Mulligan.CardIDEntity> cards)
        {
            int flag1 = 0;//宝藏经销商
            int flag2 = 0;//心灵按摩师
            int flag3 = 0;//虚触侍从
            int flag4 = 0;//随船外科医师
            int flag5 = 0;//空降歹徒
            int flag6 = 0;//纸艺天使
            int flag7 = 0;//狂暴邪翼蝠
            int flag8 = 0;//针灸
            foreach (Mulligan.CardIDEntity card in cards)
            {
                CardDB.Card cardCN = CardDB.Instance.getCardDataFromID(card.id);
                if (cardCN.nameCN == CardDB.cardNameCN.宝藏经销商)
                {
                    flag1 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.心灵按摩师)
                {
                    flag2 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.虚触侍从)
                {
                    flag3 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.随船外科医师)
                {
                    flag4 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.空降歹徒)
                {
                    flag5 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.纸艺天使)
                {
                    flag6 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.狂暴邪翼蝠)
                {
                    flag7 += 1;
                }
                if (cardCN.nameCN == CardDB.cardNameCN.针灸)
                {
                    flag8 += 1;
                }
            }

            foreach (Mulligan.CardIDEntity card in cards)
            {
                CardDB.Card cardCN = CardDB.Instance.getCardDataFromID(card.id);

                if (cardCN.nameCN == CardDB.cardNameCN.宝藏经销商)
                {
                    if (cards.Count == 3 && flag4 == 0)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先手一个没有随船外科医师留一张宝藏经销商";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡宝藏经销商";
                            }
                        }
                    }
                    else if (cards.Count > 3 && flag2 + flag3 + flag4 == 1)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手除了宝藏经销商只有1张能用下的1费，留一张宝藏经销商";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡宝藏经销商";
                            }
                        }
                    }
                    else if (cards.Count > 3 && flag4 >= 1 && flag2 + flag3 >= 1)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "后手有随船外科医师和1张能用的1费，不留宝藏经销商";
                    }
                    else if (cards.Count > 3 && flag4 == 0 && flag2 == 0)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手没随船外科医师和心灵按摩师，宝藏经销商全留";
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }

                }

                if (cardCN.nameCN == CardDB.cardNameCN.心灵按摩师)
                {
                    if (cards.Count == 3 && flag1 + flag4 == 0)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先手没宝藏经销商和随船外科医师留1张心灵按摩师";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡心灵按摩师";
                            }
                        }
                    }
                    else if (cards.Count > 3 && flag1 == 0 && flag4 == 0)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手没随船外科医师和宝藏经销商，心灵按摩师全留";
                    }
                    else if (cards.Count > 3 && flag1 + flag4 == 1)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手有随船外科医师和宝藏经销商中间的一张，心灵按摩师留一张";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡心灵按摩师";
                            }
                        }
                    }
                    else if (cards.Count > 3 && flag1 + flag4 >= 2)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "后手有随船外科医师和宝藏经销商中间的两张，心灵按摩师不留";
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.随船外科医师)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先后手都留两张随船外科医师";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.空降歹徒)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先后手留2张空降歹徒";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.虚触侍从)
                {
                    if (cards.Count == 3 && flag1 + flag2 + flag4 >= 1)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先手有1费能下的海盗，留一张虚触侍从";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡虚触侍从";
                            }
                        }
                    }
                    else if (cards.Count > 3 && flag4 >= 1)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手有随船外科医师，留一张虚触侍从";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡虚触侍从";
                            }
                        }
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.纸艺天使)
                {
                    if (cards.Count == 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "先手不留纸艺天使";
                    }
                    else if (cards.Count > 3 && flag1 + flag2 + flag4 == 0 && flag5 < 2)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "后手没1张能用的1费海盗，空降歹徒低于两张，留1张纸艺天使保底";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡纸艺天使";
                            }
                        }
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.针灸)
                {
                    if (cards.Count >= 3 && flag7 == 2)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先后手有2张狂暴邪翼蝠留1张针灸";
                        foreach (Mulligan.CardIDEntity tmp in cards)
                        {
                            if (tmp.entitiy == card.entitiy) continue;
                            if (tmp.id == card.id)
                            {
                                tmp.holdByRule = -2;
                                tmp.holdReason = "按规则丢弃第二张卡针灸";
                            }
                        }
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.狂暴邪翼蝠)
                {
                    if (cards.Count >= 3 && flag8 >= 1 && flag7 == 2)
                    {
                        card.holdByRule = 2;
                        card.holdReason = "先后手有2张狂暴邪翼蝠和1张针灸留2张狂暴邪翼蝠";
                    }
                    else
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不符合特殊规则不留";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.亡者复生)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留亡者复生";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.暗影投弹手)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留暗影投弹手";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.海盗帕奇斯)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留海盗帕奇斯";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.精神灼烧)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留精神灼烧";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.心灵震爆)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留心灵震爆";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.暮光欺诈者)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留暮光欺诈者";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.迪菲亚麻风侏儒)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留迪菲亚麻风侏儒";
                    }
                }

                if (cardCN.nameCN == CardDB.cardNameCN.黑暗主教本尼迪塔斯)
                {
                    if (cards.Count >= 3)
                    {
                        card.holdByRule = -2;
                        card.holdReason = "不留黑暗主教本尼迪塔斯";
                    }
                }
            }
        }

        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            // 无法选中(值越大越不会打出)
            if (target != null && target.untouchable)
            {
                return 100000;
            }
            // 初始惩罚值（负为优先打出该牌，正为低优先打出该牌）
            int pen = 0;
            //一费检查手牌有没有船载火炮、幸运币、海盗，此处为没有海盗，返回值1000不打出此combo。
            if (Hrtprozis.Instance.gTurn <= 2 && card.race == CardDB.Race.PIRATE && p.enemyMinions.Count == 0)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.nameCN == CardDB.cardNameCN.船载火炮)
                    {
                        foreach (Handmanager.Handcard hhc in p.owncards)
                        {
                            if (hhc.card.nameCN == CardDB.cardNameCN.幸运币 || hhc.getManaCost(p) == 1 && hhc.card.race != CardDB.Race.PIRATE && hhc.card.type == CardDB.cardtype.MOB)
                            {
                                return 1000;
                            }
                        }
                    }
                }
            }
            //如果是海盗并且随从有船载火炮 增加基础推荐出牌。
            if (card.race == CardDB.Race.PIRATE)
            {
                foreach (Minion m in p.ownMinions)
                {
                    if (m.handcard.card.nameCN == CardDB.cardNameCN.船载火炮)
                    {
                        pen -= 100;
                    }
                }
            }

            int 一费有用随从 = 0;//1费随从手牌数量           
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.nameCN == CardDB.cardNameCN.宝藏经销商
                    || hc.card.nameCN == CardDB.cardNameCN.心灵按摩师
                    || hc.card.nameCN == CardDB.cardNameCN.暗影投弹手
                    || hc.card.nameCN == CardDB.cardNameCN.虚触侍从
                    || hc.card.nameCN == CardDB.cardNameCN.随船外科医师)
                    一费有用随从++;
            }

            int 暗影法术牌 = 0;//暗影法术牌手牌数量           
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.nameCN == CardDB.cardNameCN.亡者复生
                    || hc.card.nameCN == CardDB.cardNameCN.精神灼烧
                    || hc.card.nameCN == CardDB.cardNameCN.针灸
                    || hc.card.nameCN == CardDB.cardNameCN.心灵震爆)
                    暗影法术牌++;
            }

            int 空降歹徒数量 = 0; // 用于统计空降歹徒的数量
            // 遍历手牌，统计空降歹徒数量
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.nameCN == CardDB.cardNameCN.空降歹徒)
                {
                    空降歹徒数量++;
                }
            }

            bool 幸运币 = false;   // 是否有幸运币
            bool 一费的狂暴邪翼蝠 = false; // 是否有1费的狂暴邪翼蝠
            bool 随船外科医师 = false;   // 是否有随船外科医师
            bool 宝藏经销商 = false;   // 是否有宝藏经销商
            bool 心灵按摩师 = false;   // 是否有心灵按摩师
            bool 海盗帕奇斯 = false;   // 是否有海盗帕奇斯
            bool 空降歹徒 = false;   // 是否有空降歹徒
            bool 纸艺天使 = false;   // 是否有纸艺天使
            bool 狂暴邪翼蝠 = false;   // 是否有狂暴邪翼蝠
            bool 暗影投弹手 = false;   // 是否有暗影投弹手
            bool 虚触侍从 = false;   // 是否有虚触侍从
            bool 亡者复生 = false;   // 是否有亡者复生  

            // 遍历手牌
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                // 检查是否有幸运币
                if (hc.card.nameCN == CardDB.cardNameCN.幸运币)
                {
                    幸运币 = true;
                }

                // 检查是否有1费的狂暴邪翼蝠
                if (hc.card.nameCN == CardDB.cardNameCN.狂暴邪翼蝠 && hc.manacost == 1)
                {
                    一费的狂暴邪翼蝠 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.随船外科医师)
                {
                    随船外科医师 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.狂暴邪翼蝠)
                {
                    狂暴邪翼蝠 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.宝藏经销商)
                {
                    宝藏经销商 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.心灵按摩师)
                {
                    心灵按摩师 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.海盗帕奇斯)
                {
                    海盗帕奇斯 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.空降歹徒)
                {
                    空降歹徒 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.纸艺天使)
                {
                    纸艺天使 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.暗影投弹手)
                {
                    暗影投弹手 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.虚触侍从)
                {
                    虚触侍从 = true;
                }

                if (hc.card.nameCN == CardDB.cardNameCN.虚触侍从)
                {
                    亡者复生 = true;
                }
            }

            // 判断是否为英雄技能
            if (card.type == CardDB.cardtype.HEROPWR)
            {
                // 判断目标是否为海盗帕奇斯
                if (target != null && target.handcard.card.nameCN == CardDB.cardNameCN.海盗帕奇斯)
                {
                    // 判断玩家是否试图使用自己的英雄技能对海盗帕奇斯造成伤害
                    if (target.own) // target.own 判断目标是否是玩家自己控制的随从
                    {
                        // 如果手牌有亡者复生且坟墓里没有随从
                        if (p.getCorpseCount() == 0 && 亡者复生)
                        {
                            pen = 500; // 设置一个较大的惩罚值，禁止使用  修复墓地没怪发癫杀自己怪用亡者复生的伏笔
                        }
                    }
                }
            }

            //此处为单卡描述
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.亡者复生:
                    if (p.getCorpseCount() < 2) // 如果墓地里的尸体小于2 建议不出牌。 
                    {
                        pen += 500;
                    }
                    if (p.owncards.Count < 2 && p.getCorpseCount() >= 1)   // 手牌数量太少了可以推荐出牌
                    {
                        pen -= 10;
                    }
                    break;
                case CardDB.cardNameCN.暗影投弹手:
                    break;
                case CardDB.cardNameCN.心灵按摩师:
                    pen -= 2;
                    break;
                case CardDB.cardNameCN.海盗帕奇斯:
                    pen += 1;
                    break;
                case CardDB.cardNameCN.精神灼烧:
                    if (target != null && target.Hp <= 2 || !target.own)       //对方随从生命值小于 2
                    {
                        pen -= 20;
                    }
                    break;
                case CardDB.cardNameCN.随船外科医师:
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (hc.card.race == CardDB.Race.PIRATE || hc.card.nameCN == CardDB.cardNameCN.错误产物 || hc.card.nameCN == CardDB.cardNameCN.口渴的流浪者 || hc.card.nameCN == CardDB.cardNameCN.虚触侍从)
                        {

                            pen -= 35;

                        }
                    }
                    pen -= 2;
                    break;
                case CardDB.cardNameCN.伴唱机:
                    if (p.mana < 4)
                    {
                        pen += 10;
                    }
                    if (p.anzOwnAuchenaiSoulpriest > 0 && p.mana > 4)
                    {
                        pen -= 20;
                    }
                    break;
                case CardDB.cardNameCN.奇利亚斯豪华版3000型:
                    if (p.ownMinions.Count >= 3) pen -= 30;
                    if (nowHandcard.getManaCost(p) <= 0) pen -= 60;
                    if (nowHandcard.getManaCost(p) <= 1) pen -= 40;
                    if (nowHandcard.getManaCost(p) <= 3) pen -= 10;
                    if (nowHandcard.getManaCost(p) > 3) pen += 10;
                    break;
                case CardDB.cardNameCN.狂暴邪翼蝠:
                    if (nowHandcard.getManaCost(p) <= 0) pen -= 10;
                    if (nowHandcard.getManaCost(p) >= 2) pen += 10;
                    break;
                case CardDB.cardNameCN.口渴的流浪者:
                    if (nowHandcard.getManaCost(p) <= 0) pen -= 60;
                    if (nowHandcard.getManaCost(p) <= 1) pen -= 40;
                    if (nowHandcard.getManaCost(p) <= 3) pen -= 10;
                    if (nowHandcard.getManaCost(p) > 3) pen += 10;
                    break;
                case CardDB.cardNameCN.黑暗主教本尼迪塔斯:
                case CardDB.cardNameCN.心灵尖刺:
                    if (target != null)
                    {
                        if (target.own)
                        {
                            return 1000; // 规定不以己方为目标
                        }
                        else
                        {
                            pen -= 3;
                        }
                    }
                    break;
                case CardDB.cardNameCN.心灵震爆:
                    if (p.enemyHero.immune) return 1000;    //对面免疫时不打。
                                                            //对面使用脱罪力证不打。(不成功)
                                                            //如果对手没有嘲讽随从，然后计算你的总攻击力加上你可以对敌方英雄造成的伤害，看是否足够来击败对手的英雄。
                    if (p.anzEnemyTaunt == 0 && p.calTotalAngr() + p.calDirectDmg(p.mana, false) >= p.enemyHero.Hp + p.enemyHero.armor)
                    {
                        return -20;
                    }
                    if (p.owncards.FindAll(x => x.card.nameCN == CardDB.cardNameCN.心灵震爆).Count >= 3 && p.anzEnemyTaunt == 0 && p.calTotalAngr() + p.calDirectDmg(p.mana, false) >= p.enemyHero.Hp + p.enemyHero.armor)
                    {
                        return -20;
                    }
                    if (p.ownWeapon.Durability == 0) //首先检查己方武器的耐久度是否为0
                    {
                        if (p.enemySecretCount == 0) //进一步检查对手是否没有奥秘
                            foreach (Handmanager.Handcard hc in p.owncards)
                            {
                                if (hc.card.nameCN == CardDB.cardNameCN.暮光欺诈者) return 0;
                            }
                        pen += 50;
                        if (p.ownAbilityReady) return 200;//少生孩子多射箭
                        // 手里有别牌就藏着
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.getManaCost(p) <= nowHandcard.getManaCost(p) && hc.card.nameCN != CardDB.cardNameCN.心灵震爆) return 200;
                        }
                    }
                    else
                        pen += 10;
                    break;
                case CardDB.cardNameCN.迪菲亚麻风侏儒:
                    if (暗影法术牌 <= 0) pen += 20;
                    else pen -= 5;
                    break;
                case CardDB.cardNameCN.虚触侍从:
                    int ownAtk = 0;
                    int enemyAtk = 0;
                    foreach (var item in p.ownMinions)
                    {
                        ownAtk += item.Angr;
                    }
                    foreach (var item in p.enemyMinions)
                    {
                        enemyAtk += item.Angr;
                    }

                    if (ownAtk >= enemyAtk)
                    {
                        pen -= 5;
                    }
                    else
                    {
                        pen += 3;
                    }

                    // 如果手牌有纸艺天使，费用小于等于2，场上友方随从是3个以下，以打满费用为主
                    if (p.owncards.Any(hand => hand.card.nameCN == CardDB.cardNameCN.纸艺天使) &&
                        p.mana == 2 && p.ownMinions.Count < 3)
                    {
                        pen += 10;
                    }
                    break;
                case CardDB.cardNameCN.空降歹徒:
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if ((hc.card.race == CardDB.Race.PIRATE || pirateCards.Contains(card)) && hc.card.nameCN != CardDB.cardNameCN.空降歹徒)
                        {
                            return 1000; // 如果手牌中有其他海盗，禁止使用空降歹徒
                        }
                    }
                    pen += bonus_mine * 4; // 增加惩罚值，避免在不合适时机打出
                    break;
                case CardDB.cardNameCN.赎罪教堂:
                    if (p.ownMinions.Select(temp => temp.handcard.card.type != CardDB.cardtype.LOCATION).ToList().Count > 0 &&
                        p.mana >= 3 && p.owncards.Count <= 3)
                    {
                        pen -= 100;
                    }
                    break;
            }

            if (Hrtprozis.Instance.gTurn == 2
                 && 幸运币
                 && 宝藏经销商
                 && 心灵按摩师
                 && !随船外科医师)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.宝藏经销商:
                        pen -= 25;
                        break;
                    case CardDB.cardNameCN.幸运币:
                        pen -= 5;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 2
                && 幸运币
                && 宝藏经销商
                && 暗影投弹手
                && !狂暴邪翼蝠
                && !心灵按摩师
                && !随船外科医师)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.宝藏经销商:
                        pen -= 25;
                        break;
                    case CardDB.cardNameCN.幸运币:
                        pen -= 40;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 1
                 && 宝藏经销商
                 && (暗影投弹手 || 虚触侍从)
                 && !心灵按摩师
                 && !海盗帕奇斯
                 && !随船外科医师)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.宝藏经销商:
                        pen -= 25;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 2
                && 幸运币
                && 一费有用随从 == 0
                && 海盗帕奇斯
                && 纸艺天使
                && !空降歹徒)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.纸艺天使:
                        pen -= 10;
                        break;
                    case CardDB.cardNameCN.幸运币:
                        pen -= 5;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 2
                 && 幸运币
                 && 一费有用随从 >= 2)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.幸运币:
                        pen -= 5;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 2
                && 幸运币
                && 一费有用随从 == 1
                && 一费的狂暴邪翼蝠
                 )
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.幸运币:
                        pen -= 5;
                        break;
                }
            }

            if (Hrtprozis.Instance.gTurn == 2
                && 幸运币
                && 一费有用随从 == 0
                && p.enemyHero.Hp <= (p.enemyHero.maxHp - 3)
                && 一费的狂暴邪翼蝠)
            {
                switch (card.nameCN)
                {
                    case CardDB.cardNameCN.幸运币:
                        pen -= 5;
                        break;
                }
            }

            return pen;
        }

        /// <summary>
        /// 核心，场面值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override float getPlayfieldValue(Playfield p)
        {
            // 如果场上的评分值大于-200000，则返回该值
            if (p.value > -200000) return p.value;

            float retval = 0; // 初始化返回值

            // 加上一般的场面价值
            retval += getGeneralVal(p);

            // 自己的抽牌数量，每张牌价值5分
            retval += p.owncarddraw * 5;

            // 危险血量线
            int hpboarder = 3;

            // 不考虑法术伤害加成
            if (p.enemyHeroName == HeroEnum.mage) retval += 2 * p.enemyspellpower;

            // 攻击血量线
            int aggroboarder = 20;

            // 加上血量值
            retval += getHpValue(p, hpboarder, aggroboarder);

            // 出牌的动作数量
            int count = p.playactions.Count;
            int ownActCount = 0; // 自己的动作计数
            bool useAb = false; // 是否使用了英雄技能
            bool attacted = false; // 是否已进行攻击

            // 遍历所有的动作
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i]; // 当前动作
                ownActCount++; // 计数自己的动作数量

                // 根据不同动作类型调整评分
                switch (a.actionType)
                {
                    case actionEnum.useLocation:
                        retval -= i * 10;
                        continue;

                    case actionEnum.trade:
                        retval -= 20; // 交换行动减分
                        continue;

                    // 英雄或随从攻击
                    case actionEnum.attackWithMinion:
                    case actionEnum.attackWithHero:
                        if (a.target != null && a.target.isHero)
                        {
                            attacted = true; // 如果攻击了英雄，标记为已攻击
                        }
                        if (a.actionType == actionEnum.attackWithMinion)
                        {
                            int atk = a.own.Angr > 0 ? a.own.Angr + p.anzOldWoman : a.own.Angr;
                            retval += atk * 10;
                        }
                        continue;

                    // 使用英雄技能
                    case actionEnum.useHeroPower:
                        useAb = true; // 使用了英雄技能
                        break;


                    //在这里加出牌顺序
                    case actionEnum.playcard:

                        // 判断具体的卡牌，并根据出牌顺序调整评分  减分早下  加分晚下 分数别太极端 会出毛病
                        switch (a.card.card.nameCN)
                        {
                            case CardDB.cardNameCN.幸运币:
                                retval -= i * 10;
                                break;
                            case CardDB.cardNameCN.暮光欺诈者:
                                retval += i * 15;
                                break;
                            case CardDB.cardNameCN.赎罪教堂:
                                retval -= (i * 11 + 5);
                                break;
                        }
                        break;

                    default:
                        continue;
                }

                // 如果出牌是海盗或“虚触侍从”
                if (a.card.card.race == CardDB.Race.PIRATE || a.card.card.nameCN == CardDB.cardNameCN.虚触侍从)
                {
                    // 检查己方随从是否有“船载火炮”
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.handcard.card.nameCN == CardDB.cardNameCN.船载火炮)
                        {
                            retval += 10 - i * 3; // 根据出牌顺序加分
                            break;
                        }
                    }
                }
            }

            // 对手基本随从交换模拟
            retval -= p.lostDamage;
            retval += getSecretPenality(p); // 奥秘的影响
            retval -= p.enemyWeapon.Angr * 3 + p.enemyWeapon.Durability * 3; // 对方武器影响

            // 留着技能下回合使用的情况
            if (p.ownMaxMana < 2 && p.ownHeroPowerCostLessOnce <= -99)
            {
                if (!useAb && p.enemyMinions.Count == 0)
                {
                    retval += 20;
                }
            }

            // 针对术士职业的特殊防“亵渎”
            if (retval > 50 && p.enemyHeroStartClass == TAG_CLASS.WARLOCK && p.enemyMinions.Count == 0 && p.ownMinions.Count > 2)
            {
                bool found = false;

                // 防止“亵渎”，从2血开始计算
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

            // “心灵震爆”闲置时优先出
            if (p.owncards.Count <= 4)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.nameCN == CardDB.cardNameCN.心灵震爆 && hc.getManaCost(p) <= p.mana)
                    {
                        retval -= 50;
                    }
                }
            }

            // 如果不攻击就能击杀敌方英雄，额外加分
            if (!attacted && p.enemyHero.Hp <= 0) retval += 10000;

            // 返回计算后的场面价值
            return retval;
        }

        /// <summary>
        /// 发现卡的价值
        /// </summary>
        /// <param name="card"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override int getDiscoverVal(CardDB.Card card, Playfield p)
        {
            switch (card.nameCN)
            {
                //法术（if条件成功了！）
                case CardDB.cardNameCN.心灵震爆:
                    if (p.enemyHero.Hp <= 5 + p.calTotalAngr()) return 100;
                    return 15;
                case CardDB.cardNameCN.精神灼烧:
                    if (p.enemyHero.Hp >= 3 + p.calTotalAngr() && p.ownMinions.Count <= p.enemyMinions.Count) return 20;
                    return 10;
                case CardDB.cardNameCN.亡者复生:
                    return 5;

                //随从（龙）
                case CardDB.cardNameCN.礼盒雏龙:
                    return 30;
                case CardDB.cardNameCN.暮光雏龙:
                case CardDB.cardNameCN.随船外科医师:
                case CardDB.cardNameCN.错误产物:
                case CardDB.cardNameCN.精灵龙:
                    return 20;
                case CardDB.cardNameCN.光明之翼:
                case CardDB.cardNameCN.星光雏龙:
                case CardDB.cardNameCN.深蓝系咒师:
                case CardDB.cardNameCN.碧蓝幼龙:
                    return 15;
                case CardDB.cardNameCN.黏土巢母:
                case CardDB.cardNameCN.骸骨巨龙:
                    return 10;

            }
            if (card.race == CardDB.Race.DRAGON)
            {
                return 3;
            }
            return 0;

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

            int bonus = 4;
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
                case CardDB.cardNameCN.空气之怒图腾:
                case CardDB.cardNameCN.战场通灵师:
                case CardDB.cardNameCN.纸艺天使:
                case CardDB.cardNameCN.纳亚克海克森:
                case CardDB.cardNameCN.粗暴的猢狲:
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
            return -1; //comment out or remove this to set manual priority
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
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_explosive = false;
                }
            }
            return pen;
        }
    }
}