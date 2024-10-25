using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
	//法术 中立 费用：1
	//Amulet of Passions
	//迷情护符
	//Take control of an enemy minion until the end of your turn.
	//直到你的回合结束，夺取一个敌方随从的控制权。
	class Sim_VAC_959t01t : SimTemplate
	{
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (target != null && !target.own)
            {
                // 夺取敌方随从的控制权，直到回合结束
                p.minionGetControlled(target, ownplay, true);
            }
        }

    }
}
