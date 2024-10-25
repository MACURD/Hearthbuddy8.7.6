using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
	//随从 潜行者 费用：1 攻击力：2 生命值：4
	//Brain Masseuse
	//心灵按摩师
	//[x]Whenever this minion takesdamage, also deal thatamount to your hero.
	//每当本随从受到伤害，对你的英雄造成等量的伤害。
	class Sim_VAC_512 : SimTemplate
	{
        public override void onMinionGotDmgTrigger(Playfield p, Minion triggerEffectMinion, int anzOwnMinionsGotDmg, int anzEnemyMinionsGotDmg, int anzOwnHeroGotDmg, int anzEnemyHeroGotDmg)
        {
            // 检查是否是心灵按摩师受到了伤害
            if (triggerEffectMinion.Hp > 0 && !triggerEffectMinion.silenced)
            {
                int dmgTaken = triggerEffectMinion.maxHp - triggerEffectMinion.Hp; // 计算随从受到的伤害

                if (dmgTaken > 0)
                {
                    // 对己方或敌方英雄造成等量的伤害
                    p.minionGetDamageOrHeal(triggerEffectMinion.own ? p.ownHero : p.enemyHero, dmgTaken);
                }
            }
        }
    }
}
