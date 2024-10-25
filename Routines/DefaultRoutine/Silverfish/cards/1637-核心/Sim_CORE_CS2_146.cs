using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
	//随从 中立 费用：1 攻击力：2 生命值：1
	//Southsea Deckhand
	//南海船工
	//Has <b>Charge</b> while you have a weapon equipped.
	//如果你装备着武器，本随从拥有<b>冲锋</b>。
	class Sim_CORE_CS2_146 : SimTemplate
	{
        public override void onAuraStarts(Playfield p, Minion m)
        {
            // 检查是否满足“冲锋”的条件
            if ((m.own && p.ownWeapon.Durability >= 1) || (!m.own && p.enemyWeapon.Durability >= 1))
            {
                m.charge = 1; // 给予该随从“冲锋”效果
            }
        }

        public override void onAuraEnds(Playfield p, Minion m)
        {
            // 当光环效果结束或条件不满足时，移除“冲锋”效果
            m.charge = 0;
        }

        public override void onTurnEndsTrigger(Playfield p, Minion m, bool turnEndOfOwner)
        {
            // 在回合结束时重新检查“冲锋”效果
            if (turnEndOfOwner == m.own)
            {
                if ((m.own && p.ownWeapon.Durability >= 1) || (!m.own && p.enemyWeapon.Durability >= 1))
                {
                    m.charge = 1; // 依然满足条件，确保“冲锋”效果仍然存在
                }
                else
                {
                    m.charge = 0; // 不满足条件，移除“冲锋”效果
                }
            }
        }
    }
}
