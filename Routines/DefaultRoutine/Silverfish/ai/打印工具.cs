namespace HREngine.Bots
{
    using System;
    using System.IO;
    public class printUtils
    {
        public static bool printResult = false;
        public static string outputFilePath = null;
        public static string detailFilePath = null;

        // 打印对局记录设置，不需要设置，会自动判断
        public static bool printRecord = false;
        // 对局记录路径，不需要设置，会自动生成
        public static string recordPath = null;
        // 对局记录回合数
        public static string recordRounds = null;

        public static bool printPentity = false;

        public static bool printNextMove = false;

        public static int enfaceReward = 0;

        public static bool sayHello = false;

        public static string emoteMode = "无";

        // type 取值 0 （打印下一步）， 1（打印自定义惩罚）
        public static void printDebuggerInfo(CardDB.Card card, string content, int pen, int type){
            if(pen == 0) return;
            if( printPentity && type == 1 ){
                Helpfunctions.Instance.ErrorLog(card.nameCN + content + pen+"");
            }
        }

        public static void printNowVal(){
            if(!printNextMove){
                return;
            }
            Playfield p = new Playfield(); 
            // 输出当前场面价值判定
            String enemyVal = "[敌方场面] ";
            String myVal    = "[我方场面] ";
            foreach (Minion m in p.enemyMinions)
            {
                enemyVal += m.handcard.card.nameCN + "(" + m.Angr + "/" + m.Hp + ") 威胁: " + Ai.Instance.botBase.getEnemyMinionValue(m, p) + "; ";
                //hasTank = hasTank || m.taunt;
            }
            foreach (Minion m in p.ownMinions)
            {
                myVal += m.handcard.card.nameCN + "(" + m.Angr + "/" + m.Hp +  ") 价值: " + Ai.Instance.botBase.getMyMinionValue(m, p) + "; ";
                //hasTank = hasTank || m.taunt;
            }
            Helpfunctions.Instance.logg("(猜测对手构筑为:" + p.enemyGuessDeck + " 套牌代码：" + Hrtprozis.Instance.enemyDeckCode + " 预计直伤： " + Hrtprozis.Instance.enemyDirectDmg + "， 加上场攻一共 " + (Hrtprozis.Instance.enemyDirectDmg + p.calEnemyTotalAngr()) + " )");

            Helpfunctions.Instance.ErrorLog(enemyVal);
            Helpfunctions.Instance.ErrorLog(myVal);
        }

        public static void Emote(string hello)
        {

        }
    }
}