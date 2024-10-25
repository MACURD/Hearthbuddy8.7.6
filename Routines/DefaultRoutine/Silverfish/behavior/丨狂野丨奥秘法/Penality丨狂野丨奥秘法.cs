using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    //每个策略的 Penality{策略名}文件里面放 三个函数：打牌评分，随从进攻评分；英雄进攻评分 以及他们需要用到的private函数
    //这三个函数用于单动作评估，如果返回值超过500，则被剪枝，不列入候选动作
    public partial class Behavior丨狂野丨奥秘法 : Behavior
    {
        public override int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            return getComboPenality(card, target, p, nowHandcard);
        }

        public override int getAttackWithMininonPenality(Minion m, Playfield p, Minion target) // 越小越好
        {
            int ret = 0;
            //Todo: 这里可以限制不要打休眠的随从 if(target.)
            if (target.untouchable)
            {
                return 100000;
            }
            //ret += getAttackSecretPenality(m, p, target); // 进攻奥秘惩罚, 因为防奥秘涉及多个动作，放在场面评估函数最后牌序里
            //switch (m.name)
            //{
            //  亡语抽牌随从攻击 死亡导致爆牌的惩罚
            //}

            return ret;
        }
        //英雄攻击惩罚
        public override int getAttackWithHeroPenality(Minion target, Playfield p) // 奥秘法没有英雄带刀进攻
        {
            return 0;
        }

        /*
         * 
        ///secret strategys pala
        /// -Attack lowest enemy. If you can’t, use noncombat means to kill it. 
        /// -attack with something able to withstand 2 damage. 
        /// -Then play something that had low health to begin with to dodge Repentance. 
        /// 
        ///secret strategys hunter
        /// - kill enemys with your minions with 2 or less heal.
        ///  - Use the smallest minion available for the first attack 
        ///  - Then smack them in the face with whatever’s left. 
        ///  - If nothing triggered until then, it’s a Snipe, so throw something in front of it that won’t die or is expendable.
        /// 
        ///secret strategys mage
        /// - Play a small minion to trigger Mirror Entity.
        /// Then attack the mage directly with the smallest minion on your side. 
        /// If nothing triggered by that point, it’s either Spellbender or Counterspell, so hold your spells until you can (and have to!) deal with either. 
         */
        private int getSecretPenality(Playfield p)  // 牌序和防奥秘的影响
        {
            if (p.enemySecretCount == 0)
                return 0;
            return 0;             
        }

        //getSecretPenality是整个牌面层面的防奥秘；以下若干函数都是单一动作层面的防奥秘评估
        public override int getSecretPen_CharIsAttacked(Playfield p, SecretItem si, Minion attacker, Minion defender) //攻击触发防奥秘，注意每个策略要重载此函数，主流程中会计算到
        {
            return 0;
        }

        public override int getSecretPen_HeroGotDmg(Playfield p, SecretItem si, bool own, int dmg) //英雄受伤，防冰箱奥秘
        {
            int pen = 0;
            // 冰箱，尽量打到 1 血
            if (si.canBe_iceblock)
            {
                // 破冰时刻
                if (p.enemyHero.Hp - dmg < 0 && p.enemyHero.Hp > 0)
                {
                    pen -= 100 / (p.enemyHero.Hp);
                }
            }
            return pen;
        }

        public override int getSecretPen_MinionIsPlayed(Playfield p, SecretItem si, Minion playedMinion)// 打出随从防奥秘
        {
            int pen = 0;
            // 打出随从的基本惩罚
            if (si.canBe_snipe) // 防奥秘，猎人狙击
            {
                // 3费及以下可以接受
                pen -= 9;
                pen += playedMinion.handcard.card.cost * 3;
                // 喜闻乐见喜闻乐见
                if (playedMinion.handcard.card.cost <= 1) pen -= 10;
            }
            return pen;
        }

        public override int getSecretPen_SpellIsPlayed(Playfield p, SecretItem si, Minion target, CardDB.Card c) //打出法术
        {
            int pen = 0;
            // 打出法术基本惩罚
            if (si.canBe_counterspell)
            {
                // 2费不亏
                pen -= 10;
                pen += c.cost * 5;
                // 硬币破法反，喜闻乐见喜闻乐见
                if (c.cost == 0) pen -= 100;
            }
            return pen;
        }
    }
}
