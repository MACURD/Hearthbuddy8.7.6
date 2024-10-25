using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    //每个策略的 Penality{策略名}文件里面放 三个函数：打牌评分，随从进攻评分；英雄进攻评分 以及他们需要用到的private函数
    //这三个函数用于单动作评估，如果返回值超过500，则被剪枝，不列入候选动作
    public partial class Behavior丨通用丨暗牧 : Behavior
    {
        public override int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.防护改装师:
                    if (target != null && target.own && target.Hp == 1) return 1000;
                    break;
                case CardDB.cardNameCN.瞄准射击:
                    if (target != null && target.own ) return 1000;
                    break;
                case CardDB.cardNameCN.梦境:
                    if (target != null && target.own && target.charge == 0) return 1000;
                    break;
                case CardDB.cardNameCN.梦魇:
                    if (target != null && !target.own) return 1000;
                    break;
                case CardDB.cardNameCN.萌物来袭:
                    if (!target.own) return 1000;
                    break;
                case CardDB.cardNameCN.黑铁矮人:
                case CardDB.cardNameCN.叫嚣的中士:
                    if (p.ownMinions.Count > 0 && target != null && !target.own) return 1000;
                    break;
                case CardDB.cardNameCN.杀戮命令:
                case CardDB.cardNameCN.奥术射击:
                case CardDB.cardNameCN.猎人印记:
                    if (target.own) return 1000;
                    break;
                case CardDB.cardNameCN.铁喙猫头鹰:
                    if(p.enemyMinions.Count>0 && target != null && target.own && !target.frozen && !(target.Angr < target.handcard.card.Attack))
                    {
                        return 1000;
                    }
                    break;
            }
            return getComboPenality(card, target, p, nowHandcard);
        }

        public override int getAttackWithMininonPenality(Minion m, Playfield p, Minion target)
        {
            if (!m.silenced && m.handcard.card.CantAttack || target.untouchable)
                return 1000;
            // 保留，别送
            int pen = -10;
            if (p.enemyHero.Hp + p.enemyHero.armor <= 15 && p.enemyHero.Hp + p.enemyHero.armor > 0 || p.ownMaxMana > 7 && p.enemyHero.Hp + p.enemyHero.armor > 0)
            {
                if(target.isHero)
                    pen -= 50;
            }
            return pen;
        }

        //英雄攻击惩罚
        public override int getAttackWithHeroPenality(Minion target, Playfield p) // 奥秘法没有英雄带刀进攻
        {
            if (target.untouchable)
                return 1000;
            int pen = -10;
            if (p.enemyHero.Hp + p.enemyHero.armor <= 15 && p.enemyHero.Hp + p.enemyHero.armor > 0 || p.ownMaxMana > 7 && p.enemyHero.Hp + p.enemyHero.armor > 0)
            {
                if (target.isHero)
                    pen -= 50;
            }
            return pen;
        }

        private int getSecretPenality(Playfield p)  // 牌序和防奥秘的影响
        {
            // 硬币不用来跳一些卡
            int flag = 0;
            for (int i = 0; i < p.playactions.Count; i++)
            {
                Action a = p.playactions[i];
                if (a.actionType == actionEnum.playcard)
                {
                    if (a.card.card.nameCN == CardDB.cardNameCN.幸运币) flag |= 1;
                    if (a.card.card.nameCN == CardDB.cardNameCN.空降歹徒) flag |= 2;
                }
            }
            if (flag == 3) return -15000;
            
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
                        if (a.actionType == actionEnum.playcard && a.card.card.type == CardDB.cardtype.MOB)
                        {
                            playCardBefore = true;
                        }
                    }
                    if (playCardBefore) return -15000;
                    // 用攻击力最高的先攻击
                    if (p.playactions[first_attack_hero].own.Hp < 4)
                    {
                        pen += p.playactions[first_attack_hero].own.Angr * 5;
                    }
                    pen += 20;
                    for (int i = first_attack_hero + 1; i < p.playactions.Count; i++)
                    {
                        Action a = p.playactions[i];
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
                    }else
                    {
                        //此后不应该存在攻击力更高的生物攻击
                        for (int i = first_attack_hero + 1; i < p.playactions.Count; i++)
                        {
                            Action a = p.playactions[i];
                            if (a.actionType == actionEnum.attackWithMinion)
                            {
                                if (a.own.Hp <= 3 && a.own.Angr > p.playactions[first_attack_hero].own.Angr)
                                    return -15000;
                            }
                        }
                    }
                }
            }
           
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
