using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
#if APPLICATION_MODE
    using RoutineHelper;
#endif

namespace HREngine.Bots
{
    public partial class MiniSimulator
    {
        //#####################################################################################################################
        private int maxdeep = 6;
        private int maxwide = 10;
        private int totalboards = 0; // 0表示没有限制每一层的大小
        private bool usePenalityManager = true;
        private bool useCutingTargets = true;
        private bool dontRecalc = true;
        private bool useLethalCheck = false;
        private bool useComparison = true;

        private bool printNormalstuff = false;
        public bool print = false; //单线程多线程，Todo:借用这个变量来调试actions

        List<Playfield> posmoves = new List<Playfield>(7000);
        List<Playfield> bestoldDuplicates = new List<Playfield>(7000);
        List<Playfield> twoturnfields = new List<Playfield>(500);

        List<List<Playfield>> threadresults = new List<List<Playfield>>(64);
        private int dirtyTwoTurnSim = 256;

        public Action bestmove = null;
        public float bestmoveValue = 0;
        private float bestoldval = -20000000;
        public Playfield bestboard = new Playfield();

        public Behavior botBase = null;
        private int calculated = 0;
        private bool enoughCalculations = false;

        private bool isLethalCheck = false;
        private bool simulateSecondTurn = false;
        private bool playaround = false;
        private int playaroundprob = 50;
        private int playaroundprob2 = 80;

        private static readonly object threadnumberLocker = new object();
        private int threadnumberGlobal = 0;

        Movegenerator movegen = Movegenerator.Instance;

        PenalityManager pen = PenalityManager.Instance;

        public MiniSimulator()
        {
        }
        public MiniSimulator(int deep, int wide, int ttlboards)
        {
            this.maxdeep = deep;
            this.maxwide = wide;
            this.totalboards = ttlboards;
        }

        public void updateParams(int deep, int wide, int ttlboards)
        {
            this.maxdeep = deep;
            this.maxwide = wide;
            this.totalboards = ttlboards;
        }

        public void setPrintingstuff(bool sp)
        {
            this.printNormalstuff = sp;
        }

        public void setSecondTurnSimu(bool sts, int amount)
        {
            //this.simulateSecondTurn = sts;
            this.dirtyTwoTurnSim = amount;
        }

        public int getSecondTurnSimu()
        {
            return this.dirtyTwoTurnSim;
        }

        public void setPlayAround(bool spa, int pprob, int pprob2)
        {
            this.playaround = spa;
            this.playaroundprob = pprob;
            this.playaroundprob2 = pprob2;
        }

        private void addToPosmoves(Playfield pf)
        {
            if (pf.ownHero.Hp <= 0) return;
            this.posmoves.Add(pf);
            if (this.totalboards >= 1)
            {
                this.calculated++;
            }
        }

        public float doallmoves(Playfield playf)
        {
            print = playf.print;
            this.isLethalCheck = playf.isLethalCheck; // 是否做斩杀检验
            enoughCalculations = false; //计算是否足够，深度大于最大深度，计算场面数大于最大宽度时为true
            botBase = Ai.Instance.botBase;  // 用哪个策略，策略文件
            this.posmoves.Clear();
            this.twoturnfields.Clear();
            this.addToPosmoves(playf);  //将当前场面加入到状态队列
            bool havedonesomething = true;  //是否无步骤可出，例如：法力值不够出任何牌，随从全部已经攻击
            List<Playfield> temp = new List<Playfield>(); //这一回合的状态队列
            int deep = 0;
            this.calculated = 0;
            Playfield bestold = null;
            bestoldval = -20000000;  //最小的val，用于标记是否已经计算过场面val
            while (havedonesomething)
            {
                // 每次循环是同一回合的一步，每多一步，deep加1
                GC.Collect();
                temp.Clear();
                temp.AddRange(this.posmoves);

                this.posmoves.Clear();
                havedonesomething = false;
                threadnumberGlobal = 0;

                if (print) startEnemyTurnSimThread(temp, 0, temp.Count);
                else
                {
                    Parallel.ForEach(Partitioner.Create(0, temp.Count),  // 多线程计算
                         range =>
                         {
                             startEnemyTurnSimThread(temp, range.Item1, range.Item2); //生成下一层，赋值temp[i].nextPlayfields
                         });
                }
                int idx = 0;
                int best_idx = 0;

                foreach (Playfield p in temp)  // 到这里nextPlayfields 哪些牌面计算好了（动作也都模拟做完了），value还没算，evaluatePenality已经计算好了。
                {
                    idx++;
                    float pVal = botBase.getPlayfieldValue(p);
                    p.value = pVal;
                    if (pVal > bestoldval)  // 找最优得分场面
                    {
                        bestoldval = pVal;
                        bestold = p;
                        bestoldDuplicates.Clear();
                        best_idx = idx;
                        // TODO 记录树层
                    }
                    else if (pVal == bestoldval) bestoldDuplicates.Add(p);  //如果val相等则加入bestoldDuplicates，方便之后调用

                    if (Settings.Instance.test)  // 重点调试，打印每一个step里面 每一个action带来的牌面，以及得分
                    {
                        Helpfunctions.Instance.logg(string.Format("树层:{0}，牌面{0}-{1}: actions {2}, 得分:{3}", deep, idx, p.playactions.Count, pVal));
                        p.printActions();
                        Helpfunctions.Instance.logg("");

#if APPLICATION_MODE
                        (Application.Current.MainWindow as MainWindow).UpdateActionViewTree(deep, idx, pVal, p.playactions, p);
#endif

                    }
                }
                // TODO 仅考虑得分最高的 60 种情况，将其他情况直接剪枝
                temp.Sort((a, b) => -a.value.CompareTo(b.value));
                if (this.calculated > Ai.Instance.maxwide)
                {
                    // 加快计算
                    temp = temp.Take(Ai.Instance.maxCal / 6).ToList();
                }
                else if (this.calculated > Ai.Instance.maxwide * 2 / 3)
                {
                    // 加快计算
                    temp = temp.Take(Ai.Instance.maxCal / 2).ToList();
                }
                else
                {
                    temp = temp.Take(Ai.Instance.maxCal).ToList();
                }
                temp.ForEach(p =>
                {
                    if (this.calculated > this.totalboards)
                    {
                        Helpfunctions.Instance.logg(string.Format("触发剪枝,deep={0},已计算={1}>阈值{2},牌面{3}的所有子孙牌面被抛弃", deep + 1, calculated, totalboards, idx));
                    }
                    else
                    {
                        // 计算量
                        if (this.totalboards > 0) this.calculated += p.nextPlayfields.Count;
                        this.posmoves.AddRange(p.nextPlayfields); //将下一层牌面进入队列，赋值posmoves下一层牌面
                        p.nextPlayfields.Clear();
                    }
                });

                if (best_idx > 0 && Settings.Instance.test)
                {
                    Helpfunctions.Instance.logg(string.Format("树第{0}层全局最优牌面下标:{1},得分:{2}", deep, best_idx, bestoldval));
                    Helpfunctions.Instance.logg("");
                }

                if (bestoldval >= 10000 && (Hrtprozis.Instance.enemySecretCount == 0 || Hrtprozis.Instance.enemyHeroStartClass != TAG_CLASS.MAGE)) this.posmoves.Clear();
                //如果bestval大于等于10000则意味着兄弟计算出当前场面可以斩杀
                //对应的策略中的代码是 if (p.enemyHero.Hp <= 0) retval = 10000;

                if (this.posmoves.Count > 0) havedonesomething = true; //如果还有其他状态可以模拟，则继续运算

                int num_before_cut = posmoves.Count;
                cuttingposibilities(isLethalCheck); //去除重复的PlayField  这里面有排序，以及对posmoves的赋值
                Helpfunctions.Instance.logg("cut to len 去重从" + num_before_cut + " 到 " + this.posmoves.Count);
                deep++;
                temp.Clear();

                if (this.calculated > this.totalboards) enoughCalculations = true;
                if (deep >= this.maxdeep) enoughCalculations = true;
            }

            if (this.dirtyTwoTurnSim > 0 && !twoturnfields.Contains(bestold)) twoturnfields.Add(bestold);
            this.posmoves.Clear();
            this.posmoves.Add(bestold);
            this.posmoves.AddRange(bestoldDuplicates);

            // search the best play...........................................................
            //do dirtytwoturnsim first :D
            if (!isLethalCheck && bestoldval < 10000) doDirtyTwoTurnsim();  // 搜索2个回合

            if (posmoves.Count >= 1)  //所有场面根据得分排序，高分排在前面，优先搜索
            {
                posmoves.Sort((a, b) => botBase.getPlayfieldValue(b).CompareTo(botBase.getPlayfieldValue(a)));
                Playfield bestplay = posmoves[0];
                float bestval = botBase.getPlayfieldValue(bestplay);
                int pcount = posmoves.Count;
                for (int i = 1; i < pcount; i++)
                {
                    float val = botBase.getPlayfieldValue(posmoves[i]);
                    if (bestval > val) break;
                    if (posmoves[i].cardsPlayedThisTurn > bestplay.cardsPlayedThisTurn) continue;
                    else if (posmoves[i].cardsPlayedThisTurn == bestplay.cardsPlayedThisTurn)
                    {
                        if (bestplay.optionsPlayedThisTurn > posmoves[i].optionsPlayedThisTurn) continue;
                        else if (bestplay.optionsPlayedThisTurn == posmoves[i].optionsPlayedThisTurn && bestplay.enemyHero.Hp <= posmoves[i].enemyHero.Hp) continue;

                    }
                    bestplay = posmoves[i];
                    bestval = val;
                }
                this.bestmove = bestplay.getNextAction();
                this.bestmoveValue = bestval;
                this.bestboard = new Playfield(bestplay);
                this.bestboard.guessingHeroHP = bestplay.guessingHeroHP;
                this.bestboard.value = bestplay.value;
                this.bestboard.hashcode = bestplay.hashcode;
                bestoldDuplicates.Clear();
                return bestval;  // 正常退出值
            }
            this.bestmove = null;
            this.bestmoveValue = -2000000;
            this.bestboard = playf;

            return -2000000; //异常退出值
        }


        private void startEnemyTurnSimThread(List<Playfield> source, int startIndex, int endIndex)
        {
            int threadnumber = 0;
            lock (threadnumberLocker)
            {
                threadnumber = threadnumberGlobal++;
                System.Threading.Monitor.Pulse(threadnumberLocker);
            }
            if (threadnumber > Ai.Instance.maxNumberOfThreads - 2)
            {
                threadnumber = Ai.Instance.maxNumberOfThreads - 2;
                Helpfunctions.Instance.ErrorLog("You need more threads!");
                return;
            }

            int berserk = Settings.Instance.berserkIfCanFinishNextTour;
            int printRules = Settings.Instance.printRules;
            for (int i = startIndex; i < endIndex; i++)  // 不同的index对应同一步的不同选择到达的不同牌面
            {
                Playfield p = source[i];
                float pv = botBase.getPlayfieldValue(p);
                if (pv < -10000) // 场面得分过低，弃掉这个场面以及这个场面的子序列，提前剪枝
                    continue;
                if (!enoughCalculations)
                {
                    //gernerate actions and play them!
                    //从这里进入调用getMoveList函数，这个函数返回的是兄弟下一步所有可以做的操作
                    //如果调试到这里，输出actions，发现没有你想要的Action就可以跟进去看看
                    //一般来说，法力值不够出的牌会直接省略，如果正常操作能够出的牌没出，
                    //则说明，惩罚值大于500，操作直接被省略
                    //建议直接跟进去看寻找原因，可以加深理解
                    List<Action> actions = movegen.getMoveList(p, usePenalityManager, useCutingTargets, true); // 这里面会算每个action的惩罚值

                    //读取到actions后接下来对每个步骤进行模拟
                    //从而得到操作之后的场面并且计算val值
                    if (printRules > 0) p.endTurnState = new Playfield(p);
                    // 全局集合，记录已经模拟过的泰坦技能动作
                    var usedTitanSkills = "";
                    foreach (Action a in actions)  // 到这步 a.penalty已经计算好了
                    {
                        // 检查是否为泰坦技能动作，并检查是否已经模拟过
                        if (a.actionType == actionEnum.useTitanAbility)
                        {
                            string titanSkillKey = a.own.entitiyID + "-" + a.titanAbilityNO;
                            if (usedTitanSkills.Length == 0)
                            {
                                usedTitanSkills = titanSkillKey;
                            }
                            else
                            {
                                if (usedTitanSkills != titanSkillKey)
                                {
                                    continue; // 已使用其他技能
                                }
                            }
                        }
                        if (a.actionType == actionEnum.useLocation)
                        {
                            if (a.own != null)
                            {
                                bool hasSameEntitiyID = p.playactions.Any(temp => temp.own != null && temp.own.entitiyID == a.own.entitiyID);
                                if (hasSameEntitiyID)
                                {
                                    continue; //当前地标已经模拟使用
                                }
                            }
                        }
                        Playfield pf = new Playfield(p);
                        pf.doAction(a);
                        if (pf.ownHero.Hp > 0 && pf.evaluatePenality < 500) p.nextPlayfields.Add(pf);
                    }
                }

                if (this.isLethalCheck)
                {
                    if (berserk > 0) // 可以斩杀
                    {
                        p.endTurn();  //结束回合，接下来模拟敌方下一回合的操作
                        if (p.enemyHero.Hp > 0)
                        {
                            bool needETS = true;
                            //如果对面没有嘲讽且我方随从全部可以攻击则进行不模拟
                            if (p.anzEnemyTaunt < 1) foreach (Minion m in p.ownMinions) { if (m.Ready) { needETS = false; break; } }
                            else
                            {
                                if (p.anzOwnTaunt < 1) foreach (Minion m in p.ownMinions) { if (m.Ready) { needETS = false; break; } }
                            }
                            //从这里进入模拟敌方下一回合的操作
                            if (needETS) Ai.Instance.enemyTurnSim[threadnumber].simulateEnemysTurn(p, this.simulateSecondTurn, playaround, false, playaroundprob, playaroundprob2);
                        }
                    }

                    p.complete = true;

                }
                else
                {
                    p.endTurn();
                    if (p.enemyHero.Hp > 0)
                    {
                        Ai.Instance.enemyTurnSim[threadnumber].simulateEnemysTurn(p, this.simulateSecondTurn, playaround, false, playaroundprob, playaroundprob2);
                        if (p.value <= -10000)
                        {
                            bool secondChance = false;
                            foreach (Action a in p.playactions)
                            {
                                if (a.actionType == actionEnum.playcard)
                                {
                                    //这里判定一下是否出了战吼相关的牌，如果有的话 val += 1500
                                    if (pen.cardDrawBattleCryDatabase.ContainsKey(a.card.card.nameEN)) secondChance = true;
                                }
                            }
                            if (secondChance) p.value += 1500;
                        }
                    }
                    p.complete = true;
                }
            }
        }

        public void doDirtyTwoTurnsim()
        {
            if (this.dirtyTwoTurnSim == 0) return;
            this.posmoves.Clear();

            if (print) doDirtyTwoTurnsimThread(twoturnfields, 0, twoturnfields.Count);
            else
            {
                Parallel.ForEach(Partitioner.Create(0, this.twoturnfields.Count),
                          range =>
                          {
                              doDirtyTwoTurnsimThread(twoturnfields, range.Item1, range.Item2);
                          });
            }
            this.posmoves.AddRange(this.twoturnfields);
        }

        public void doDirtyTwoTurnsimThread(List<Playfield> source, int startIndex, int endIndex)
        {
            int threadnumber = Ai.Instance.maxNumberOfThreads - 2;
            if (endIndex < source.Count) threadnumber = startIndex / (endIndex - startIndex);
            //set maxwide of enemyturnsimulator's to second step (this value is higher than the maxwide in first step) 
            Ai.Instance.enemyTurnSim[threadnumber].setMaxwide(false);

            for (int i = startIndex; i < endIndex; i++)
            {
                Playfield p = source[i];
                if (p.guessingHeroHP >= 1)
                {
                    p.doDirtyTts = p.value;
                    p.complete = false;
                    p.value = int.MinValue;
                    p.bestEnemyPlay = null;
                    Ai.Instance.enemyTurnSim[threadnumber].simulateEnemysTurn(p, true, playaround, false, this.playaroundprob, this.playaroundprob2);
                }
                else
                {
                    //p.value = -10000;
                }
                botBase.getPlayfieldValue(p);
            }
        }

        // 剪枝，排序
        public void cuttingposibilities(bool isLethalCheck)
        {
            // take the x best values
            List<Playfield> temp = new List<Playfield>();
            Dictionary<Int64, Playfield> tempDict = new Dictionary<Int64, Playfield>();
            try
            {
                posmoves.ForEach(p => botBase.getPlayfieldValue(p));
                posmoves.Sort((a, b) => b.value.CompareTo(a.value));//want to keep the best
            }
            catch (Exception ex) // Todo:待fix，不应该有这个异常，是处理了奥数、疯狂科学家牌序后才有的
            {
                Helpfunctions.Instance.logg("cuttingposibilities异常:" + ex.Message);
            }
            //useComparison = false;// 暂时调试用，打出所有重复的牌面，重点调试，用于寻找为啥不是相同牌面里的最佳牌序
            if (this.useComparison)
            {
                int i = 0;
                int max = Math.Min(posmoves.Count, this.maxwide);

                Playfield p = null;
                for (i = 0; i < max; i++)
                {
                    p = posmoves[i];
                    Int64 hash = p.GetPHash();
                    p.hashcode = hash;
                    if (!tempDict.ContainsKey(hash)) tempDict.Add(hash, p);
                    else
                    {
                        float pvalue = botBase.getPlayfieldValue(p);
                        float tempv = botBase.getPlayfieldValue(tempDict[hash]);
                        if (pvalue > tempv) //如果场面一样，取得分高的
                        {
                            tempDict[hash] = p;
                        }
                    }
                }
                foreach (KeyValuePair<Int64, Playfield> d in tempDict)
                {
                    temp.Add(d.Value);
                }
            }
            else
            {
                temp.AddRange(posmoves);
            }
            posmoves.Clear();
            posmoves.AddRange(temp);




            //twoturnfields!
            if (this.dirtyTwoTurnSim == 0 || isLethalCheck) return;
            tempDict.Clear();
            temp.Clear();
            if (bestoldval >= 10000) return;
            foreach (Playfield p in twoturnfields) tempDict.Add(p.hashcode, p);
            posmoves.Sort((a, b) => botBase.getPlayfieldValue(b).CompareTo(botBase.getPlayfieldValue(a)));

            int maxTts = Math.Min(posmoves.Count, this.dirtyTwoTurnSim);
            for (int i = 0; i < maxTts; i++)
            {
                if (!tempDict.ContainsKey(posmoves[i].hashcode)) temp.Add(posmoves[i]);
            }
            twoturnfields.Sort((a, b) => botBase.getPlayfieldValue(b).CompareTo(botBase.getPlayfieldValue(a)));
            temp.AddRange(twoturnfields.GetRange(0, Math.Min(this.dirtyTwoTurnSim, twoturnfields.Count)));
            twoturnfields.Clear();
            twoturnfields.AddRange(temp);



        }

        public List<targett> cutAttackTargets(List<targett> oldlist, Playfield p, bool own)
        {
            List<targett> retvalues = new List<targett>();
            List<Minion> addedmins = new List<Minion>(8);

            bool priomins = false;
            List<targett> retvaluesPrio = new List<targett>();
            foreach (targett t in oldlist)
            {
                if ((own && t.target == 200) || (!own && t.target == 100))
                {
                    retvalues.Add(t);
                    continue;
                }
                if ((own && t.target >= 10 && t.target <= 19) || (!own && t.target >= 0 && t.target <= 9))
                {
                    Minion m = null;
                    if (own) m = p.enemyMinions[t.target - 10];
                    if (!own) m = p.ownMinions[t.target];


                    bool goingtoadd = true;
                    List<Minion> temp = new List<Minion>(addedmins);
                    bool isSpecial = m.handcard.card.isSpecialMinion;
                    foreach (Minion mnn in temp)
                    {
                        // special minions are allowed to attack in silended and unsilenced state!
                        //help.logg(mnn.silenced + " " + m.silenced + " " + mnn.name + " " + m.name + " " + penman.specialMinions.ContainsKey(m.name));

                        bool otherisSpecial = mnn.handcard.card.isSpecialMinion;

                        if ((!isSpecial || (isSpecial && m.silenced)) && (!otherisSpecial || (otherisSpecial && mnn.silenced))) // both are not special, if they are the same, dont add
                        {
                            if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous && mnn.lifesteal == m.lifesteal) goingtoadd = false;
                            continue;
                        }

                        if (isSpecial == otherisSpecial && !m.silenced && !mnn.silenced) // same are special
                        {
                            if (m.name != mnn.name) // different name -> take it
                            {
                                continue;
                            }

                            // 同名随从不重复添加
                            // same name -> test whether they are equal
                            if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous && mnn.lifesteal == m.lifesteal && mnn.Spellburst == m.Spellburst) goingtoadd = false;
                            continue;
                        }

                    }

                    if (goingtoadd)
                    {
                        addedmins.Add(m);
                        retvalues.Add(t);
                        //help.logg(m.name + " " + m.id +" is added to targetlist");
                    }
                    else
                    {
                        //help.logg(m.name + " is not needed to attack");
                        continue;
                    }

                }
            }
            //help.logg("end targetcutting");
            if (priomins) return retvaluesPrio;

            return retvalues;
        }

        public void printPosmoves()
        {
            int i = 0;
            foreach (Playfield p in this.posmoves)
            {
                p.printBoard();
                i++;
                if (i >= 200) break;
            }
        }

    }

}