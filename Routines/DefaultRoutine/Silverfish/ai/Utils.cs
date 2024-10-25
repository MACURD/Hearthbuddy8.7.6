using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    class Utils
    {
        public static PenalityManager penman = PenalityManager.Instance;
        public static bool isOwnLowest(Minion mnn, Playfield p)
        {
            bool ret = true;
            int val = getValueOfMinion(mnn);
            foreach (Minion m in p.ownMinions)
            {
                if (!m.Ready) continue;
                if (getValueOfMinion(m) < val) ret = false;
            }
            return ret;
        }

        public static bool isOwnLowestInHand(Minion mnn, Playfield p)
        {
            bool ret = true;
            Minion m = new Minion();
            int val = getValueOfMinion(mnn);
            foreach (Handmanager.Handcard card in p.owncards)
            {
                if (card.card.type != CardDB.cardtype.MOB) continue;
                CardDB.Card c = card.card;
                m.Hp = c.Health;
                m.maxHp = c.Health;
                m.Angr = c.Attack;
                m.taunt = c.tank;
                m.name = c.nameEN;
                m.nameCN = c.nameCN;
                if (getValueOfMinion(m) < val) ret = false;
            }
            return ret;
        }

        public static bool isOwnHighAngr(Minion mnn, Playfield p)
        {
            bool ret = true;
            int val = mnn.Angr;
            foreach (Minion m in p.ownMinions)
            {
                if (!m.Ready) continue;
                if (m.Angr > val && m.Hp <= 3) ret = false;
            }
            return ret;
        }


        public static int getValueOfEnemyMinion(Minion m)
        {
            int ret = 0;
            ret += m.Hp;
            if (m.taunt) ret -= 2;
            return ret;
        }

        public static bool isEnemyLowest(Minion mnn, Playfield p)
        {
            bool ret = true;
            List<Minion> litt = p.GetAttackTargets(true, false);
            int val = getValueOfEnemyMinion(mnn);
            foreach (Minion m in p.enemyMinions)
            {
                if (litt.Find(x => x.entitiyID == m.entitiyID) == null) continue;
                if (getValueOfEnemyMinion(m) < val) ret = false;
            }
            return ret;
        }

        public static bool hasMinionsWithLowHeal(Playfield p)
        {
            bool ret = false;
            foreach (Minion m in p.ownMinions)
            {
                if (m.Hp <= 2 && (m.Ready || penman.priorityDatabase.ContainsKey(m.name))) ret = true;
            }
            return ret;
        }

        public static bool hasMinionsWithLowHeal2(Playfield p)
        {
            bool ret = false;
            foreach (Minion m in p.ownMinions)
            {
                if (m.Hp <= 3 && (m.Ready || penman.priorityDatabase.ContainsKey(m.name))) ret = true;
            }
            return ret;
        }

        public static int guessTotalSpellDamage(Playfield p, CardDB.cardNameEN name, bool ownplay)
        {
            int dmg = 0;
            if (penman.DamageTargetDatabase.ContainsKey(name)) dmg = penman.DamageTargetDatabase[name];
            else if (penman.DamageTargetSpecialDatabase.ContainsKey(name)) dmg = penman.DamageTargetSpecialDatabase[name];
            else if (penman.DamageRandomDatabase.ContainsKey(name)) dmg = penman.DamageRandomDatabase[name];
            else if (penman.DamageHeroDatabase.ContainsKey(name)) dmg = penman.DamageHeroDatabase[name];
            else if (penman.DamageAllDatabase.ContainsKey(name)) dmg = (p.ownMinions.Count * penman.DamageAllDatabase[name] + p.enemyMinions.Count * penman.DamageAllDatabase[name]) * 7 / 10;
            else if (penman.DamageAllEnemysDatabase.ContainsKey(name)) dmg = p.enemyMinions.Count * penman.DamageAllEnemysDatabase[name] * 7 / 10;
            else if (p.anzOwnAuchenaiSoulpriest >= 1)
            {
                if (penman.HealAllDatabase.ContainsKey(name)) dmg = (p.ownMinions.Count * penman.HealAllDatabase[name] + p.enemyMinions.Count * penman.HealAllDatabase[name]) * 7 / 10;
                else if (penman.HealTargetDatabase.ContainsKey(name)) dmg = Math.Min(penman.HealTargetDatabase[name], 29);
            }

            if (dmg != 0) dmg = (ownplay) ? p.getSpellDamageDamage(dmg) : p.getEnemySpellDamageDamage(dmg);
            return dmg;
        }

        public static int getValueOfMinion(Minion m)
        {
            int ret = 0;
            ret += 2 * m.Angr + m.Hp;
            if (m.taunt) ret += 2;
            if (penman.priorityDatabase.ContainsKey(m.name)) ret += 20 + penman.priorityDatabase[m.name];
            return ret;
        }
    }
}
