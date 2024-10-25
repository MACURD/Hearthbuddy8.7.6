using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
    class Sim_VAC_442 : SimTemplate // Light the Candle Elemental
    {

        // <b>战吼：</b> 造成@点伤害<i>（每有一个你使用过元素牌的连续的回合，伤害都会提升）</i>。
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            int damage = Hrtprozis.Instance.ownConsecutiveElementalTurns; // 根据连续使用元素牌的回合数确定伤害值
            if (target != null)
            {
                p.minionGetDamageOrHeal(target, damage + 1); // 对目标造成相应的伤害
            }
        }

        public override PlayReq[] GetPlayReqs()
        {
            return new PlayReq[] {
                new PlayReq(CardDB.ErrorType2.REQ_TARGET_TO_PLAY),        // 需要选择目标
                new PlayReq(CardDB.ErrorType2.REQ_ENEMY_TARGET)    // 目标必须是敌方
            };
        }
    }
}
