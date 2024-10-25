using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

#if APPLICATION_MODE
    using RoutineHelper;
#endif

namespace HREngine.Bots
{
    public class Ai
    {

        public int maxdeep = 12;
        public int maxwide = 3000;
        public int maxCal = 60;
        //public int playaroundprob = 40;
        public int playaroundprob2 = 80;
        public int maxNumberOfThreads = 100; //don't change manually, because it changes automatically

        private bool usePenalityManager = true;
        private bool useCutingTargets = true;
        private bool dontRecalc = true;
        private bool useLethalCheck = false;  // 原本是true，现在我们应该能做到正常计算，斩杀的场景下得分最高，所以无需计算多次
        private bool useComparison = true;
        public Playfield bestplay = new Playfield();


        public int lethalMissing = 30; //RR

        public MiniSimulator mainTurnSimulator;
 		public List<EnemyTurnSimulator> enemyTurnSim = new List<EnemyTurnSimulator>();
        public List<MiniSimulatorNextTurn> nextTurnSimulator = new List<MiniSimulatorNextTurn>();
        public List<EnemyTurnSimulator> enemySecondTurnSim = new List<EnemyTurnSimulator>();

        public string currentCalculatedBoard = "1";

        PenalityManager penman = PenalityManager.Instance;
        Settings settings = Settings.Instance;

        List<Playfield> posmoves = new List<Playfield>(7000);

        Hrtprozis hp = Hrtprozis.Instance;
        Handmanager hm = Handmanager.Instance;
        Helpfunctions help = Helpfunctions.Instance;

        public Action bestmove = null;
        public float bestmoveValue = 0;
        public Playfield nextMoveGuess = new Playfield();
        public Behavior botBase = null;

        public List<Action> bestActions = new List<Action>();

        public bool secondturnsim = false;
        //public int secondTurnAmount = 256;
        public bool playaround = false;

        private static Ai instance;

        public static Ai Instance
        {
            get
            {
                return instance ?? (instance = new Ai());
            }
        }

        private Ai()
        {
            this.nextMoveGuess = new Playfield { mana = -100 };

            this.mainTurnSimulator = new MiniSimulator(maxdeep, maxwide, 0); // 0 for unlimited 对每层搜索数量没有限制
            this.mainTurnSimulator.setPrintingstuff(true);

            for (int i = 0; i < maxNumberOfThreads; i++)
            {
                this.nextTurnSimulator.Add(new MiniSimulatorNextTurn());
                this.enemyTurnSim.Add(new EnemyTurnSimulator());
                this.enemySecondTurnSim.Add(new EnemyTurnSimulator());

                this.nextTurnSimulator[i].thread = i;
                this.enemyTurnSim[i].thread = i;
                this.enemySecondTurnSim[i].thread = i;
            }
        }

        public void setMaxWide(int mw)
        {
            this.maxwide = mw;
            if (maxwide <= 0) this.maxwide = 3000;            
            if (maxwide <= 100) this.maxwide = 100;
            this.mainTurnSimulator.updateParams(maxdeep, maxwide, 0);
        }


        public void setMaxDeep(int mw)
        {
            this.maxdeep = mw;
            if (maxdeep <= 0) this.maxdeep = 12;
            if (maxdeep <= 2) this.maxdeep = 2;
            this.mainTurnSimulator.updateParams(maxdeep, maxwide, 0);
        }

        public void setMaxCal(int mw)
        {
            this.maxCal = mw;
            if (maxCal <= 0) this.maxCal = 60;
            if (maxCal <= 1) this.maxCal = 1;
        }

        public void setTwoTurnSimulation(bool stts, int amount)
        {
            this.mainTurnSimulator.setSecondTurnSimu(stts, amount);
            this.secondturnsim = stts;
            settings.secondTurnAmount = amount;
        }

        public void updateTwoTurnSim()
        {
            this.mainTurnSimulator.setSecondTurnSimu(settings.simulateEnemysTurn, settings.secondTurnAmount);
        }

        public void setPlayAround()
        {
            this.mainTurnSimulator.setPlayAround(settings.playaround, settings.playaroundprob, settings.playaroundprob2);
        }

        /// <summary>
        /// test，调试的时候为true,不过发现下面没用:(
        /// isLethalCheck，斩杀判定，当为true时，程序接下来只会模拟一些有可能造成伤害的操作，来判定是否会斩杀，如果不能造成斩杀，
        /// 则再次进行isLethalCheck = false的运算。如果看到兄弟错斩的情况，就可以从这里入手来找解决的办法，具体解决办法在PenalityManager.cs处说明
        /// </summary>
        /// <param name="test"></param>
        /// <param name="isLethalCheck"></param>
        private void doallmoves(bool test, bool isLethalCheck)
        {
            if (isLethalCheck)
                help.logg("斩杀检验开始:");
            //set maxwide to the value for the first-turn-sim.   wide：搜索广度
            foreach (EnemyTurnSimulator ets in enemyTurnSim)
            {
                ets.setMaxwide(true);
            }
            foreach (EnemyTurnSimulator ets in enemySecondTurnSim)
            {
                ets.setMaxwide(true);
            }

            this.posmoves[0].isLethalCheck = isLethalCheck;
            this.mainTurnSimulator.doallmoves(this.posmoves[0]); //核心计算

            bestplay = this.mainTurnSimulator.bestboard;  // 找到最优场面
            float bestval = this.mainTurnSimulator.bestmoveValue; //最优场面得分

            help.loggonoff(true);
            help.logg("-------------------------------------");
            if (bestplay.ruleWeight != 0) help.logg("ruleWeight " + bestplay.ruleWeight * -1);
            if (settings.printRules > 0)
            {
                String[] rulesStr = bestplay.rulesUsed.Split('@');
                foreach (string rs in rulesStr)
                {
                    if (rs == "") continue;
                    help.logg("rule: " + rs);
                }
            }
            help.logg("value of best board " + bestval);

#if APPLICATION_MODE
            (Application.Current.MainWindow as MainWindow).BestScore = bestval;
#endif

            this.bestActions.Clear();  // 初始化，清除上次回合的最优操作  这是List<Action>
            this.bestmove = null;

            ActionNormalizer an = new ActionNormalizer(); //对操作重排使得其先打出AOE伤害，在_setting.txt中设定，默认关闭，实际使用效果并不好
            if (settings.adjustActions > 0) an.adjustActions(bestplay, isLethalCheck);
            Helpfunctions.Instance.logg("bestplay.playactions ################################# START");
            foreach (Action a in bestplay.playactions)
            {
                this.bestActions.Add(new Action(a));
                a.print();
            }
            Helpfunctions.Instance.logg("bestplay.playactions ################################# END");

#if APPLICATION_MODE
            (Application.Current.MainWindow as MainWindow).UpdateBestActionList(bestplay.playactions);
#endif

            if (this.bestActions.Count >= 1)
            {
                this.bestmove = this.bestActions[0];
                this.bestActions.RemoveAt(0);
            }
            this.bestmoveValue = bestval;  

            if (bestmove != null && bestmove.actionType != actionEnum.endturn) // save the guessed move, so we doesnt need to recalc!
            {
                this.nextMoveGuess = new Playfield();

                this.nextMoveGuess.doAction(bestmove);
            }
            else
            {
                nextMoveGuess.mana = -100;
            }

            if (isLethalCheck)
            {
                this.lethalMissing = bestplay.enemyHero.armor + bestplay.enemyHero.Hp;//RR
                help.logg("斩杀检验结束：还差伤害:" + this.lethalMissing);
            }
        }
        
        public void doNextCalcedMove()
        {
            help.logg("noRecalcNeeded!!!-----------------------------------");

            this.bestmove = null;
            if (this.bestActions.Count >= 1)
            {
                this.bestmove = this.bestActions[0];
                this.bestActions.RemoveAt(0);
            }
            if (this.nextMoveGuess == null) this.nextMoveGuess = new Playfield();
            else Silverfish.Instance.updateCThunInfo(nextMoveGuess.anzOgOwnCThunAngrBonus, nextMoveGuess.anzOgOwnCThunHpBonus, nextMoveGuess.anzOgOwnCThunTaunt);

            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {
                //this.nextMoveGuess = new Playfield();
                Helpfunctions.Instance.logg("nmgsim-");
                try
                {
                    this.nextMoveGuess.doAction(bestmove);
                    this.bestmove = this.nextMoveGuess.playactions[this.nextMoveGuess.playactions.Count - 1];
                }
                catch (Exception ex)
                {
                    Helpfunctions.Instance.logg("Message ---");
                    Helpfunctions.Instance.logg("Message ---" + ex.Message);
                    Helpfunctions.Instance.logg("Source ---" + ex.Source);
                    Helpfunctions.Instance.logg("StackTrace ---" + ex.StackTrace);
                    Helpfunctions.Instance.logg("TargetSite ---\n{0}" + ex.TargetSite);

                }
                Helpfunctions.Instance.logg("nmgsime-");

            }
            else
            {
                //Helpfunctions.Instance.logg("nd trn");
                nextMoveGuess.mana = -100;
                int twilightelderBonus = 0;
                foreach (Minion m in this.nextMoveGuess.ownMinions)
                {
                    if (m.name == CardDB.cardNameEN.twilightelder && !m.silenced) twilightelderBonus++;
                }
                if (twilightelderBonus > 0)
                {
                    Silverfish.Instance.updateCThunInfo(nextMoveGuess.anzOgOwnCThunAngrBonus + twilightelderBonus, nextMoveGuess.anzOgOwnCThunHpBonus + twilightelderBonus, nextMoveGuess.anzOgOwnCThunTaunt);
                }
            }

        }

        public void dosomethingclever(Behavior bbase)
        {
            this.botBase = bbase;
            hp.updatePositions();

            posmoves.Clear();
            posmoves.Add(new Playfield());

            help.loggonoff(false);
            //do we need to recalc?
            help.logg("recalc-check###########");
            if (this.dontRecalc && posmoves[0].isEqual(this.nextMoveGuess, true))
            {
                doNextCalcedMove();
            }
            else
            {
                bestmoveValue = -1000000;
                DateTime strt = DateTime.Now;
                if (useLethalCheck)  //如果斩杀检验，默认不做斩杀检验
                {
                    strt = DateTime.Now;
                    doallmoves(false, true);
                    help.logg("calculated " + (DateTime.Now - strt).TotalSeconds);
                }

                if (bestmoveValue < 10000)  //如果没有斩杀，再试试
                {
                    posmoves.Clear();
                    posmoves.Add(new Playfield());
                    strt = DateTime.Now;
                    doallmoves(false, false);
                    help.logg("calculated " + (DateTime.Now - strt).TotalSeconds);

                }
            }
        }
        


        public List<double> autoTester(bool printstuff, string data = "", int mode = 0) //测试用函数  返回值是每一步的计算时间
                                                                                        //mode: 0 先斩杀检验，再正常出牌 1：斩杀检验 2:正常
        {
            List<double> retval = new List<double>();
            double calcTime = 0;
            BoardTester bt = new BoardTester(data);
            if (!bt.datareaded) return retval;
            help.logg("打印初始局面：");
            //hp.printHero();
            //hp.printOwnMinions();
            //hp.printEnemyMinions();
            //hm.printcards();
            //calculate the stuff
            posmoves.Clear();
            Playfield pMain = new Playfield();  // 为什么new出来的有牌面信息，因为public Hrtprozis prozis = Hrtprozis.Instance存储了对局信息
            pMain.prozis.turnDeck = bt.od;

            pMain.printBoard();
            pMain.print = printstuff;
            posmoves.Add(pMain);
            //pMain.printBoard();
            //help.logg("ownminionscount " + posmoves[0].ownMinions.Count);
            //help.logg("owncardscount " + posmoves[0].owncards.Count);

            //foreach (var item in this.posmoves[0].owncards)
            //{
            //    help.logg("card " + item.card.name + " is playable :" + item.canplayCard(posmoves[0], true) + " cost/mana: " + item.manacost + "/" + posmoves[0].mana);
            //}
            //help.logg("ability " + posmoves[0].ownHeroAblility.card.name + " is playable :" + posmoves[0].ownHeroAblility.card.canplayCard(posmoves[0], 2, true) + " cost/mana: " + posmoves[0].ownHeroAblility.card.getManaCost(posmoves[0], 2) + "/" + posmoves[0].mana);

            DateTime strt = DateTime.Now;

            //默认不再做斩杀检验，斩杀检查,所有动作都是为了制造伤害，看能否致死
            //if (mode == 0 || mode == 1)
            //{
            //    doallmoves(false, true);
            //    calcTime = (DateTime.Now - strt).TotalSeconds;
            //    help.logg("calculated " + calcTime);
            //    retval.Add(calcTime);
            //}

            // 这里要补充一下，如果兄弟判定对面血量为负数(已斩杀)，则会将场面评分提升到10000以上。
            // 而这里的bestmoveValue > 5000，则说明场面已经大概可以斩杀了。
            // 但是为什么是五千不是一万？
            // 这个问题的原因在于兄弟是会对当前回合和我方下一回合进行模拟的，而默认的权重分配是0.5
            // 如果这个回合斩杀了，兄弟可能就不会对下个回合进行模拟，所以如果这个回合评分为10000 * 0.5 + 0 * 0.5 = 5000
            // 以上只是个人猜测，有少量的代码可证明，知道原因的可以在下面评论哦
            // FIXME 暂时不考虑斩杀，继续进行计算，获得最高的评分后行动！
            //if (bestmoveValue >= 5000)
            //{
            //    //可以斩杀
            //}
            //else //如果不足以斩杀
             
            if (mode == 0 || mode == 2)
            {
                posmoves.Clear();
                pMain = new Playfield();
                pMain.print = printstuff;
                posmoves.Add(pMain);
                strt = DateTime.Now;
                doallmoves(false, false);
                calcTime = (DateTime.Now - strt).TotalSeconds;
                help.logg("calculated " + calcTime);
                retval.Add(calcTime);
            }

            if (printstuff)
            {
                //this.mainTurnSimulator.printPosmoves();
                simmulateWholeTurn(bt);
                help.logg("calculated " + calcTime);
            }

            return retval;
        }

        public void simmulateWholeTurn(BoardTester bt)  // 这里影响输出.result文件
        {
#if APPLICATION_MODE
            (Application.Current.MainWindow as MainWindow).playfieldActionList.Clear();
#endif

            // 打印结果设置
            printUtils.printResult = true;
            help.logg("########################################################################################################");
            //输出场面
            Playfield p = new Playfield();
            p.prozis.turnDeck = bt.od;

#if APPLICATION_MODE
            (Application.Current.MainWindow as MainWindow).playfieldActionList.Add("回合开始", new Playfield(p));
#endif

            string normalInfo = "";
            String enemyVal = "[敌方场面] ";
            String myVal = "[我方场面] ";
            String handCard = "[我方手牌] ";

            normalInfo += "水晶： " + p.mana + " / " + p.ownMaxMana
                + " [我方英雄] " + p.ownHeroName + " （生命: " + p.ownHero.Hp + " + " + p.ownHero.armor + " 奥秘数: " + p.ownSecretsIDList.Count + " ) "
                + "[敌方英雄] " + p.enemyHeroName + " （生命: " + p.enemyHero.Hp + " + " + p.enemyHero.armor + " 奥秘数: " + p.enemySecretCount + (p.enemyHero.immune ? " 免疫" : "") + " ) "
                + "[任务] quests: " + p.ownQuest.Id + " " + p.ownQuest.questProgress + " " + p.ownQuest.maxProgress + " "
                + p.enemyQuest.Id + " " + p.enemyQuest.questProgress + " " + p.enemyQuest.maxProgress;
            foreach (Minion m in p.enemyMinions)
            {
                enemyVal += m.handcard.card.nameCN + " ( " + m.Angr + " / " + m.Hp + " ) " + (m.frozen ? "[冻结]" : "") + (!m.Ready || m.cantAttack ? "[无法攻击]" : "") + (m.windfury ? "[风怒]" : "") + (m.taunt ? "[嘲讽]" : "");
            }
            foreach (Minion m in p.ownMinions)
            {
                myVal += m.handcard.card.nameCN + " ( " + m.Angr + " / " + m.Hp + " ) " + (m.frozen ? "[冻结]" : "") + (!m.Ready || m.cantAttack ? "[无法攻击]" : "") + (m.windfury ? "[风怒]" : "") + (m.taunt ? "[嘲讽]" : "");
            }
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                handCard += hc.card.nameCN + " (费用: " + hc.manacost + " ; " + (hc.addattack + hc.card.Attack) + " / " + (hc.addHp + hc.card.Health) + " ) ";
            }

            help.logg(normalInfo + (p.enemyGuessDeck == "" ? "": "(猜测对手构筑为:" + p.enemyGuessDeck + " 套牌代码：" + Hrtprozis.Instance.enemyDeckCode + " 预计直伤： " + Hrtprozis.Instance.enemyDirectDmg + " 加上场攻一共 "+ (Hrtprozis.Instance.enemyDirectDmg + p.calEnemyTotalAngr()) + " )") ) ;

            help.logg(enemyVal);
            help.logg(myVal);
            help.logg(handCard);
            help.logg("########################################################################################################");
            help.logg("simulate best board，最终结果如下：");
            help.logg("########################################################################################################");
            //this.bestboard.printActions();

            Playfield tempbestboard = new Playfield();
            //tempbestboard.printBoard();
            int step = 0;
            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {
                step++;
                help.logg("第" + step + "步:");
                bestmove.print();
                tempbestboard.doAction(bestmove);

#if APPLICATION_MODE
                (Application.Current.MainWindow as MainWindow).playfieldActionList.Add("第" + step + "步", new Playfield(tempbestboard));
#endif

                if (this.bestActions.Count == 0)
                {
                    help.ErrorLog("end turn");
                }
            }
            else
            {
                tempbestboard.mana = -100;
                help.ErrorLog("end turn");
            }
            
            foreach (Action imove in this.bestActions)  // imove:每一步动作  bestmove + bestActions 才是整体，因为有个RemoveAt的操作
            {
                step++;
                help.logg("第" + step + "步:");
                if (imove != null && imove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
                {
                    imove.print();
                    tempbestboard.doAction(imove);
                }
                else
                {
                    tempbestboard.mana = -100;
                }
#if APPLICATION_MODE
                (Application.Current.MainWindow as MainWindow).playfieldActionList.Add("第" + step + "步", new Playfield(tempbestboard));
#endif
            }

#if APPLICATION_MODE
            (Application.Current.MainWindow as MainWindow).UpdatePlayfieldActionList();
#endif

            normalInfo = "水晶： " + tempbestboard.mana + " / " + tempbestboard.ownMaxMana
                + " [我方英雄] " + tempbestboard.ownHeroName + " （生命: " + tempbestboard.ownHero.Hp + " + " + tempbestboard.ownHero.armor + " 奥秘数: " + tempbestboard.ownSecretsIDList.Count + " ) "
                + "[敌方英雄] " + tempbestboard.enemyHeroName + " （生命: " + tempbestboard.enemyHero.Hp + " + " + tempbestboard.enemyHero.armor + " 奥秘数: " + tempbestboard.enemySecretCount + (tempbestboard.enemyHero.immune ? " 免疫" : "") + " ) "
                + "[任务] quests: " + tempbestboard.ownQuest.Id + " " + tempbestboard.ownQuest.questProgress + " " + tempbestboard.ownQuest.maxProgress + " "
                + tempbestboard.enemyQuest.Id + " " + tempbestboard.enemyQuest.questProgress + " " + tempbestboard.enemyQuest.maxProgress;
            enemyVal = "[敌方场面] ";
            myVal = "[我方场面] ";
            handCard = "[我方手牌] ";
            foreach (Minion m in tempbestboard.enemyMinions)
            {
                enemyVal += m.handcard.card.nameCN + " ( " + m.Angr + " / " + m.Hp + " ) " + (m.frozen ? "[冻结]" : "") + (!m.Ready || m.cantAttack ? "[无法攻击]" : "") + (m.reborn ? "[复生]" : "") + (m.divineshild ? "[圣盾]" : "") + (m.windfury ? "[风怒]" : "") + (m.taunt ? "[嘲讽]" : "");
            }
            foreach (Minion m in tempbestboard.ownMinions)
            {
                myVal += m.handcard.card.nameCN + " ( " + m.Angr + " / " + m.Hp + " ) " + (m.frozen ? "[冻结]" : "") + (!m.Ready || m.cantAttack ? "[无法攻击]" : "") + (m.reborn ? "[复生]" : "") + (m.divineshild ? "[圣盾]" : "") + (m.windfury ? "[风怒]" : "") + (m.taunt ? "[嘲讽]" : "");
            }
            foreach (Handmanager.Handcard hc in tempbestboard.owncards)
            {
                handCard += hc.card.nameCN + " (费用: " + hc.manacost + " ; " + (hc.addattack + hc.card.Attack) + " / " + (hc.addHp + hc.card.Health) + " ) ";
            }

            help.logg("########################################################################################################");
            help.logg("最终场面：");
            help.logg(normalInfo);
            help.logg(enemyVal);
            help.logg(myVal);
            help.logg(handCard);
            help.logg("########################################################################################################");

            // 打印结果设置
            printUtils.printResult = false;

            help.logg("----------------------------");
        }

        public void simmulateWholeTurnandPrint()
        {
            help.ErrorLog("###################################");
            help.ErrorLog("what would silverfish do?---------");
            help.ErrorLog("###################################");
            if (this.bestmoveValue >= 10000) help.ErrorLog("DETECTED LETHAL ###################################");
            //this.bestboard.printActions();

            Playfield tempbestboard = new Playfield();

            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {

                tempbestboard.doAction(bestmove);
                tempbestboard.printActionforDummies(tempbestboard.playactions[tempbestboard.playactions.Count - 1]);

                if (this.bestActions.Count == 0)
                {
                    help.ErrorLog("end turn");
                }
            }
            else
            {
                tempbestboard.mana = -100;
                help.ErrorLog("end turn");
            }


            foreach (Action bestmovee in this.bestActions)
            {

                if (bestmovee != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
                {
                    //bestmovee.print();
                    tempbestboard.doAction(bestmovee);
                    tempbestboard.printActionforDummies(tempbestboard.playactions[tempbestboard.playactions.Count - 1]);

                }
                else
                {
                    tempbestboard.mana = -100;
                    help.ErrorLog("end turn");
                }
            }
        }

        public void updateEntitiy(int old, int newone)
        {
            Helpfunctions.Instance.logg("entityupdate! " + old + " to " + newone);
            if (this.nextMoveGuess != null)
            {
                foreach (Minion m in this.nextMoveGuess.ownMinions)
                {
                    if (m.entitiyID == old) m.entitiyID = newone;
                }
                foreach (Minion m in this.nextMoveGuess.enemyMinions)
                {
                    if (m.entitiyID == old) m.entitiyID = newone;
                }
            }
            foreach (Action a in this.bestActions)
            {
                if (a.own != null && a.own.entitiyID == old) a.own.entitiyID = newone;
                if (a.target != null && a.target.entitiyID == old) a.target.entitiyID = newone;
                if (a.card != null && a.card.entity == old) a.card.entity = newone;
            }

        }

    }


}