using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    // 本类重点调试：用于跟进特定deep idx的牌面打分计算问题，基于deep 和 idx 设置断点
    public partial class MiniSimulator
    {
        public void debugCal(Playfield root, Playfield p, int deep, int idx)  //重点调试
            // root:初始牌面；p: 当前要调试的牌面; deep:当前深度; idx:当前深度的同一层的编号
        {
            return; // 调试时注释掉此行
            // if (deep == 2 && idx == 1) // 调试用代码，或者比较多个牌面，就设置条件断点
            // {
            //     //重新构造下p，方便知道P.evaluatePenality的计算过程
            //     Playfield tmp = new Playfield(root); //初始化 = 根节点
            //     tmp.evaluatePenality = 0;
            //     tmp.isOwnTurn = true; // 这步不能少
            //     for (int i = 0; i < p.playactions.Count; i++)
            //     {
            //         Action a = p.playactions[i];
            //         int ap = 0;
            //         //注意：为了还原计算，a.target要复原，因为target可能已经被扣血，影响打分计算
            //         Minion tar = null;
            //         if (a.target != null)
            //         {
            //             if (a.target.isHero)
            //             {
            //                 if (a.target.own)
            //                     tar = tmp.ownHero;
            //                 else
            //                     tar = tmp.enemyHero;
            //             }
            //             else
            //             {
            //                 //奥秘法的目标不会是己方随从，所以这里无需增加ownMinions判断，但是其他策略需要增加
            //                 for (int j = 0; j < tmp.enemyMinions.Count; j++)
            //                 {
            //                     if (a.target.entitiyID == tmp.enemyMinions[j].entitiyID)
            //                     {
            //                         tar = tmp.enemyMinions[j];
            //                         break;
            //                     }
            //                 }
            //                 for(int j = 0; j < tmp.ownMinions.Count; j++)
            //                 {
            //                     if (a.target.entitiyID == tmp.ownMinions[j].entitiyID)
            //                     {
            //                         tar = tmp.enemyMinions[j];
            //                         break;
            //                     }
            //                 }
            //             }
            //             if(tar == null)
            //                 Helpfunctions.Instance.logg("错误！tar==null,但应该是" + a.target.info());
            //         }
            //         switch (a.actionType)
            //         {
            //             case actionEnum.playcard:
            //                 if (tar != null && tar.untouchable) { ap = 1000; break; }
            //                 ap = PenalityManager.Instance.getPlayCardPenality(a.card.card, tar, tmp);  
            //                 break;
            //             case actionEnum.attackWithMinion:
            //                 if (tar != null && tar.untouchable) { ap = 1000; break; }
            //                 ap = PenalityManager.Instance.getAttackWithMininonPenality(a.own, tmp, tar);
            //                 break;
            //             case actionEnum.attackWithHero:
            //                 if (tar != null && tar.untouchable) { ap = 1000; break; }
            //                 ap = PenalityManager.Instance.getAttackWithHeroPenality(tar, tmp);
            //                 break;
            //             case actionEnum.useHeroPower:
            //                 if (tar != null && tar.untouchable) { ap = 1000; break; }
            //                 ap = PenalityManager.Instance.getPlayCardPenality(tmp.ownHeroAblility.card, tar, tmp);
            //                 break;
            //         }
            //         a.penalty = ap;
            //         tmp.doAction(p.playactions[i]); // 一步一步变成当前p，并更新tmp.evaluatePenality
            //     }
            //     if (!tmp.isEqual(p, false))
            //     {
            //         //出错了，应该一致
            //     }
            //     tmp.manaTurnEnd = p.manaTurnEnd; // 调试发现上述路径下来，manaTurnEnd没有更新
            //     float tmpvalue = botBase.getPlayfieldValue(tmp);
            //     /*
            //      * 打分过程：getMoveList(Movegenerator.cs)中给每个候选动作打分（做该动作前），记录在p.evaluatePenality里面，做完动作后，得到的牌面，进行牌面打分
            //      */
            // }
        }
        
    }
}
