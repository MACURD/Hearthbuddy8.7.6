using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    //每个策略的 Penality{策略名}文件里面放 三个函数：打牌评分，随从进攻评分；英雄进攻评分 以及他们需要用到的private函数
    //这三个函数用于单动作评估，如果返回值超过500，则被剪枝，不列入候选动作
    public partial class Behavior丨标准丨酸快攻德 : Behavior
    {
        public override int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            switch (card.nameCN)
            {
                //buff类
                case CardDB.cardNameCN.野性印记:
                case CardDB.cardNameCN.尖壳印记:
                case CardDB.cardNameCN.萌物来袭:
                    if (target != null && !target.own) return 1000;
                    break;
                //伤害类
                case CardDB.cardNameCN.瞄准射击:
                case CardDB.cardNameCN.快速射击:
                case CardDB.cardNameCN.自然死亡:
                case CardDB.cardNameCN.缠人根须:
                    if (target != null && target.own) return 1000;
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
                if (!target.isHero)
                    pen += 7;
            }
            return pen;
        }

        //英雄攻击惩罚
        public override int getAttackWithHeroPenality(Minion target, Playfield p) // 奥秘法没有英雄带刀进攻
        {
            if (target.untouchable)
                return 1000;
            int pen = -5;
            if (p.enemyHero.Hp + p.enemyHero.armor <= 15 || p.ownMaxMana > 7)
            {
                if (!target.isHero)
                    pen += 7;
            }
            if (p.ownMaxMana < 5 && (target.isHero || target.Hp != p.ownHero.Angr) && p.ownWeapon.Durability == 1)
            {
                pen += 5;
                //留刀
            }
            return pen;
        }

        private int getSecretPenality(Playfield p)  // 牌序和防奥秘的影响
        {
            // 设置奥秘
            bool set_explosive = false;
            foreach (CardDB.cardIDEnum s in p.ownSecretsIDList)
            {
                if (s == CardDB.cardIDEnum.EX1_610 || s == CardDB.cardIDEnum.VAN_EX1_610)
                {
                    set_explosive = true;
                }
            }
            if (set_explosive && p.enemyMinions.Count >= 2)
            {
                for (int i = 0; i < p.playactions.Count; i++)
                {
                    Action a = p.playactions[i];
                    // 设置爆炸陷阱的情况下不下怪
                    if (a.actionType == actionEnum.playcard && a.card.card.type == CardDB.cardtype.MOB && !a.card.card.Charge && !a.card.card.Stealth)
                    {
                        return -15000; // 不可接受，抛弃本牌面以及子牌面
                    }
                    // 不打对手随从
                    if (a.actionType == actionEnum.playcard && (a.card.card.nameCN == CardDB.cardNameCN.杀戮命令 || a.card.card.nameCN == CardDB.cardNameCN.奥术射击) && a.target != null && a.target != p.enemyHero && a.target.Hp <= 2)
                    {
                        return -15000; // 不可接受，抛弃本牌面以及子牌面
                    }
                    if (a.actionType == actionEnum.attackWithHero || a.actionType == actionEnum.attackWithMinion)
                    {
                        if (a.target != null && a.target.Hp < 3) return -15000;
                    }
                }
            }

            if (p.enemySecretCount == 0)
                return 0;

            int pen = 0;

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
