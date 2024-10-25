using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    //每个策略的 Penality{策略名}文件里面放 三个函数：打牌评分，随从进攻评分；英雄进攻评分 以及他们需要用到的private函数
    //这三个函数用于单动作评估，如果返回值超过500，则被剪枝，不列入候选动作
    public partial class Behavior丨通用丨酸鱼人萨 : Behavior
    {
        public override int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.鱼人杀手蟹:
                    if (target.own && (target.Hp > 1 || target.Angr > 1 )) return 1000;
                    break;
                    case CardDB.cardNameCN.深海融合怪:
                    if (target != null&&target.handcard.card.race != CardDB.Race.MURLOC) return 1000;      //避免加给图腾（待优化  酸小明9.7）
                    break;
                case CardDB.cardNameCN.火焰冲击:
                    if (target.own) return 1000;
                    break;
                case CardDB.cardNameCN.南海岸酋长:
                    if (target!=null&&target.own) return 1000;
                    break;
                case CardDB.cardNameCN.毒鳍鱼人:
                    // 有火焰术士必定加给火焰术士
                    foreach(Minion m in p.ownMinions)
                    {
                        if(m.handcard.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔 && !m.poisonous && target!=null && target.entitiyID != m.entitiyID)
                        {
                            return 1000;
                        }
                        if (target != null && target.poisonous )
                        {
                            foreach(Minion mmm in p.ownMinions)
                            {
                                if (!mmm.poisonous)
                                {
                                    return 1000;
                                }
                            }
                        }
                    }
                    break;
            }
            return getComboPenality(card, target, p, nowHandcard);
        }

        public override int getAttackWithMininonPenality(Minion m, Playfield p, Minion target)
        {
            int pen = -10;
            if (p.enemyHero.untouchable || p.enemyHero.Hp <= 0)
            {
                if (!target.isHero)
                {
                    pen -= 30;
                }
            }
            if (!target.isHero)
                switch (m.handcard.card.nameCN)
                {
                    case CardDB.cardNameCN.甜水鱼人斥候:
                    case CardDB.cardNameCN.下水道渔人:
                    case CardDB.cardNameCN.鱼人领军:
                    case CardDB.cardNameCN.火焰术士弗洛格尔:
                        pen += 8;
                        break;
                }
            return pen;
        }

        //英雄攻击惩罚
        public override int getAttackWithHeroPenality(Minion target, Playfield p) // 奥秘法没有英雄带刀进攻
        {
            int pen = -10;
            // 谁莫名其妙给我设置了 16 点杀怪奖励？
            //if (target.Hp <= p.ownHero.Angr && !target.divineshild)
            //{
            //    pen += 16;
            //}
            // TMD 不知道在哪里写了个溢出伤害超过2给了20点惩罚，修正
            //if (p.ownHero.Angr >= target.Hp + 2)
            //{
            //    pen -= 20;
            //}
            return pen;
        }

        private int getSecretPenality(Playfield p)  // 牌序和防奥秘的影响
        {
            int bonus = 4;
            if (p.enemySecretCount == 0)
                return 0;
            int pen = 0;
            bool canBe_flameward = false;
            foreach (SecretItem si in p.enemySecretList)  //Todo: 是否要判断己方回合还是敌方回合？？？
            {
                if (si.canBe_flameward) { canBe_flameward = true; break; }
            }
            if (canBe_flameward)
            {
                
                // 如果存在<=3的随从，用其中攻击最大的（会死的里面最大的）；否则无所谓
                // 奖励这种情况1分就好

                //先攻击随从，再攻击英雄，再出牌 这个顺序最适合奥秘法，因为没有buff手牌
                int first_attack_hero = -2;
                for (int i = 0; i < p.playactions.Count; i++)
                {
                    Action a = p.playactions[i];
                    if (a.actionType == actionEnum.attackWithMinion && a.target.isHero) // 随从攻击敌方英雄
                    {
                        first_attack_hero = i;
                        pen += 3;
                        break;
                    }
                }
                if (first_attack_hero >= 0) // 存在随从攻击英雄
                {
                    bool playCardBefore = false;
                    //如果此前出牌了，扣分，容易被炸
                    for (int i = 0; i < first_attack_hero; i++)
                    {
                        Action a = p.playactions[i];
                        if (a.actionType == actionEnum.playcard)
                        {
                            playCardBefore = true;
                        }else if(a.actionType == actionEnum.useHeroPower && (p.ownHeroAblility.card.cardIDenum == CardDB.cardIDEnum.HERO_02bp || p.ownHeroAblility.card.cardIDenum == CardDB.cardIDEnum.HERO_04bp) )
                        {
                            playCardBefore = true;
                        }
                        if (a.actionType == actionEnum.playcard && (a.card.card.nameCN == CardDB.cardNameCN.鱼人恩典 || a.card.card.nameCN == CardDB.cardNameCN.鱼勇可贾))
                        {
                            int savedMinion = 0;
                            foreach(Minion m in p.ownMinions)
                            {
                                if(m.Hp >= 4 )
                                {
                                    savedMinion++;
                                }else
                                {
                                    savedMinion--;
                                }
                            }
                            if(savedMinion > 0) playCardBefore = false;
                            break;
                        }
                    }
                    if (playCardBefore) return -15000;
                    // 用攻击力最高的先攻击
                    if (p.playactions[first_attack_hero].own.Hp < 4)
                    {
                        pen += p.playactions[first_attack_hero].own.Angr * 5;
                    }
                    for (int i = first_attack_hero + 1; i < p.playactions.Count; i++)
                    {
                        Action a = p.playactions[i];
                        if (a.actionType == actionEnum.playcard)
                        {
                            if (a.card.card.nameCN == CardDB.cardNameCN.鱼人恩典 || a.card.card.nameCN == CardDB.cardNameCN.鱼勇可贾)
                                return -15000; // 不可接受，抛弃本牌面以及子牌面
                        }
                        if (a.actionType == actionEnum.attackWithMinion && !a.target.isHero && a.own.Hp < 3 && !a.own.divineshild)
                        {
                            return -15000; // 不可接受，抛弃本牌面以及子牌面
                        }
                    }
                    if (p.playactions[first_attack_hero].own.Hp > 3)
                    {
                        //此后不应该存在hp<=3的随从
                        for (int i = first_attack_hero + 1; i < p.playactions.Count; i++)
                        {
                            Action a = p.playactions[i];
                            if (a.actionType == actionEnum.attackWithMinion)
                            {
                                if (a.own.Hp <= 3)
                                    return -15000;
                            }                           
                        }
                    }
                }
            }
            //bool canBe_vaporize = false;  // 防蒸发奥秘
            //foreach (SecretItem si in p.enemySecretList)
            //{
            //    if (si.canBe_vaporize) { canBe_vaporize = true; break; }
            //}
            //if (canBe_vaporize)  
            //{
            //    if (!target.own)
            //    {
            //        bool islow = isOwnLowest(m, p);
            //        if (!islow) pen += 10;
            //        else
            //        {
            //            if (getValueOfMinion(m) > 14) pen += 5;
            //        }
            //        if (p.enemyMinions.Count > 0) pen += 12;
            //    }
            //    return pen;
            //}

            //bool canBe_duplicate = false; // 防复制奥秘
            //foreach (SecretItem si in p.enemySecretList)
            //{
            //    if (si.canBe_duplicate) { canBe_duplicate = true; break; }
            //}
            //if (canBe_duplicate)
            //{
            //    pen = 1;
            //    if (target.Hp > m.Angr || target.divineshild) return 0;
            //    else
            //    {
            //        pen += target.handcard.manacost;
            //        if (target.handcard.card.battlecry && target.name != CardDB.cardName.kingmukla) pen += 1;
            //        return pen;
            //    }
            //}
            bool canBe_explosive = false;  //防止是猎人的爆炸陷阱
            foreach (SecretItem si in p.enemySecretList)
            {
                if (si.canBe_explosive) { canBe_explosive = true; break; }
            }
            if (canBe_explosive)
            {
                int first_attack_hero = -2;
                for (int i = 0; i < p.playactions.Count; i++)
                {
                    Action a = p.playactions[i];
                    if ((a.actionType == actionEnum.attackWithMinion || a.actionType == actionEnum.attackWithHero) && a.target.isHero) // 随从攻击敌方英雄
                    {
                        first_attack_hero = i;
                        if (p.ownHero.Hp <= 2)
                            pen -= 500;
                        break;
                    }
                    // Todo: 这里还要考虑法术伤害敌方英雄 待Fix
                }
                if (first_attack_hero >= 0) // 存在随从攻击英雄
                {
                    //如果此前出牌了，扣分，容易被炸
                    for (int i = 0; i < first_attack_hero; i++)
                    {
                        Action a = p.playactions[i];
                        if (a.actionType == actionEnum.playcard)
                        {
                            if (a.card.card.type == CardDB.cardtype.MOB) //出了随从
                                return -15000; // 不可接受，抛弃本牌面以及子牌面
                        }
                    }
                    for (int i = first_attack_hero + 1; i < p.playactions.Count; i++)
                    {
                        Action a = p.playactions[i];
                        if (a.actionType == actionEnum.attackWithMinion && !a.target.isHero && a.own.Hp < 3 && !a.own.divineshild)
                        {
                            return -15000; // 不可接受，抛弃本牌面以及子牌面
                        }
                    }
                    // 尽量少留 2 血以下生物
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.Hp < 3 && !m.divineshild)
                        {
                            pen -= m.Angr * 6;
                        }
                    }
                }
            }
            return pen;
        }
    }
}
