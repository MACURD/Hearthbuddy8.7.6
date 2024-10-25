
using System;
using System.Collections.Generic;

namespace HREngine.Bots
{
    public abstract class Behavior
    {
        /// <summary>
        /// 场面值通用
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual float getGeneralVal(Playfield p)
        {
            // 场面值
            int retval = 0;

            // 斩杀了
            if (p.enemyHero.Hp <= 0)
            {
                retval += 10000;
                // 斩杀了的情况操作越少越好
                //retval += 1000000 / (p.playactions.Count + 1);
            }
            // 斩杀了
            if (p.ownHero.Hp <= 0)
            {
                retval -= 20000;
            }

            // --------------------------通用---------------------------------
            // 计算额外惩罚
            retval -= p.evaluatePenality;
            // 手牌价值（可能导致不出牌）
            // retval += p.owncards.Count * 5;
            // 最大法力水晶
            retval += p.ownMaxMana * 20 - p.enemyMaxMana * 20;
            // 英雄技能
            // retval += (p.enemyHeroAblility.manacost - p.ownHeroAblility.manacost) * 4;
            // if (p.ownHeroPowerAllowedQuantity != p.enemyHeroPowerAllowedQuantity)
            // {
            //     if(p.ownHeroPowerAllowedQuantity > p.enemyHeroPowerAllowedQuantity) retval += 3;
            //     else retval -= 3;
            // }
            // 对法师，需要考虑法强效果
            if (p.enemyHeroName == HeroEnum.mage) retval -= 4 * p.enemyspellpower;
            // 武器价值
            if (p.ownWeapon.Durability > 0)
            {
                if (p.ownWeapon.Angr > 1) retval += p.ownWeapon.Angr * p.ownWeapon.Durability * 2;
                else retval += (p.ownWeapon.Angr + 1) * p.ownWeapon.Durability * 2 - 3;
            }
            // 敌方武器价值
            if (!p.enemyHero.frozen)
            {
                retval -= p.enemyWeapon.Durability * p.enemyWeapon.Angr;
            }
            else
            {
                if (p.enemyWeapon.Durability >= 1)
                {
                    retval += 12;
                }
            }
            // 抽牌价值
            //RR card draw value depending on the turn and distance to lethal
            //RR if lethal is close, carddraw value is increased
            if (p.lethalMissing() <= 5) //RR
            {
                retval += p.owncarddraw * 100;
            }
            if (p.ownMaxMana < 4)
            {
                retval += p.owncarddraw * 2;
            }
            else
            {
                retval += p.owncarddraw * 5;
            }
            // 卡差
            retval -= p.enemycarddraw * 5;
            // else retval -= (p.owncarddraw + 1) * 7 + (p.enemycarddraw - p.owncarddraw - 1) * 12;
            // 计算我方随从价值
            int owntaunt = 0;
            // int readycount = 0;
            int ownMinionsCount = 0;
            foreach (Minion m in p.ownMinions)
            {
                retval += getMyMinionValue(m, p);
                if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
                if (m.taunt) owntaunt++;
                retval += m.synergy;
            }
            // 危险血量
            if (p.ownHero.Hp + p.ownHero.armor < 12)
            {
                if (owntaunt > 0)
                {
                    retval += 20;
                }
            }
            // 克苏恩计数器
            // retval += p.anzOgOwnCThunAngrBonus;
            retval += p.anzOwnExtraAngrHp * 2 - p.anzEnemyExtraAngrHp * 2;

            /*if (p.enemyMinions.Count >= 0)
            {
                int anz = p.enemyMinions.Count;
                if (owntaunt == 0) retval -= 10 * anz;
                retval += owntaunt * 10 - 11 * anz;
            }*/

            // 使用英雄技能
            bool useAbili = false;
            // 使用硬币
            int usecoin = 0;
            //soulfire etc
            // int deletecardsAtLast = 0;
            int wasCombo = 0;
            bool firstSpellToEnHero = false;
            // 出牌序列数量
            int count = p.playactions.Count;

            // FIXME 空过惩罚
            //if(count == 0)
            //{
            //    retval -= 5;
            //}

            int ownActCount = 0;
            // 排序问题！！！！
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i];
                ownActCount++;
                switch (a.actionType)
                {
                    // 英雄攻击
                    case actionEnum.attackWithHero:
                        // 抢脸血线
                        //if (p.enemyHero.Hp <= p.attackFaceHP) retval++;
                        // 武器解怪收益
                        //Helpfunctions.Instance.ErrorLog(a.target.name+"的生命值为"+a.target.Hp);
                        //if ((a.target.Hp == 0 || a.target.Hp == -1) && p.ownHero.Hp >= 15 && p.enemyHero.Hp >= 15)
                        //{
                        //    retval += 20;
                        //}
                        //if(a.target.isHero && p.enemyHero.Hp <= 10)
                        //{
                        //    retval += 10;
                        //}
                        // 小心剧毒
                        //if (a.target.poisonous && a.target.Hp <= 0)
                        //{
                        //    retval += 50;
                        //}
                        if (i == 0 && p.ownWeapon.Durability > 0)
                        {
                            if (p.ownWeapon.card.cardIDenum == CardDB.cardIDEnum.BT_018)
                            {
                                retval += 3;
                            }
                        }
                        // 战士
                        // if (p.ownHeroName == HeroEnum.warrior && useAbili) retval -= 1;
                        continue;
                    case actionEnum.useHeroPower:
                        useAbili = true;
                        continue;
                    case actionEnum.playcard:
                        break;
                    default:
                        continue;
                }
                switch (a.card.card.nameCN)
                {
                    // 排序优先
                    case CardDB.cardNameCN.雷霆绽放:
                        usecoin += 2;
                        if (i == count - 1) retval -= 100;
                        break;
                    case CardDB.cardNameCN.激活:
                    case CardDB.cardNameCN.幸运币:
                        usecoin++;
                        // 最后一张卡
                        if (i == count - 1) retval -= 10;
                        break;
                }
                // 连击
                if (a.card.card.Combo && i > 0) wasCombo++;
                if (a.target == null) continue;
                //save spell for all classes
                // 法术不打脸？
                if (a.card.card.type == CardDB.cardtype.SPELL && (a.target.isHero && !a.target.own))
                {
                    if (i == 0) firstSpellToEnHero = true;
                    retval -= 11;
                }
            }
            if (wasCombo > 0 && firstSpellToEnHero)
            {
                if (wasCombo + 1 == ownActCount) retval += 10;
            }
            if (usecoin > 0)
            {
                retval -= 10 * p.manaTurnEnd;
                if (p.manaTurnEnd + usecoin > 10) retval -= 10 * usecoin;
            }
            // 法力水晶还剩下 2 个并且还能用英雄技能
            if (p.manaTurnEnd >= p.ownHeroAblility.manacost && !useAbili)
            {
                switch (p.ownHeroAblility.card.nameEN)
                {
                    case CardDB.cardNameEN.heal: goto case CardDB.cardNameEN.lesserheal;
                    case CardDB.cardNameEN.lesserheal:
                        bool wereTarget = false;
                        if (p.ownHero.Hp < p.ownHero.maxHp) wereTarget = true;
                        if (!wereTarget)
                        {
                            foreach (Minion m in p.ownMinions)
                            {
                                if (m.wounded) { wereTarget = true; break; }
                            }
                        }
                        if (wereTarget && !(p.anzOwnAuchenaiSoulpriest > 0 || p.embracetheshadow > 0)) retval -= 10;
                        break;
                    case CardDB.cardNameEN.poisoneddaggers: goto case CardDB.cardNameEN.daggermastery;
                    case CardDB.cardNameEN.daggermastery:
                        if (!(p.ownWeapon.Durability > 1 || p.ownWeapon.Angr > 1)) retval -= 10;
                        break;
                    case CardDB.cardNameEN.totemicslam: goto case CardDB.cardNameEN.totemiccall;
                    case CardDB.cardNameEN.totemiccall:
                        if (p.ownMinions.Count < 7) retval -= 10;
                        else retval -= 3;
                        break;
                    case CardDB.cardNameEN.thetidalhand: goto case CardDB.cardNameEN.reinforce;
                    case CardDB.cardNameEN.thesilverhand: goto case CardDB.cardNameEN.reinforce;
                    case CardDB.cardNameEN.reinforce:
                        if (p.ownMinions.Count < 7) retval -= 10;
                        else retval -= 3;
                        break;
                    case CardDB.cardNameEN.soultap:
                        if (p.owncards.Count < 10 && p.ownDeckSize > 0)
                        {
                            retval -= 20;
                        }
                        break;
                    case CardDB.cardNameEN.lifetap:
                        if (p.owncards.Count < 10 && p.ownDeckSize > 0 && p.ownHero.Hp >= 20 && p.enemyMinions.Count == 0)
                        {
                            retval -= 10;
                        }
                        break;
                    default:
                        retval -= 10;
                        break;
                }
            }
            // if (usecoin && p.mana >= 1) retval -= 20;
            // 手里的随从
            int mobsInHand = 0;
            int bigMobsInHand = 0;
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    mobsInHand++;
                    if (hc.card.Attack + hc.addattack >= 3) bigMobsInHand++;
                    retval += hc.addattack + hc.addHp + hc.poweredUp;
                }
                // 回合结束时丢弃
                if(hc.enchs.Count > 0 && (hc.enchs.Contains(CardDB.cardIDEnum.BOT_568e) || hc.enchs.Contains(CardDB.cardIDEnum.YOD_027e) ) )
                {
                    retval -= 15;
                }
                // 幸运币留牌跳币策略
                if(hc.card.nameCN == CardDB.cardNameCN.幸运币 && p.mana == 0)
                {
                    Playfield nextTurn = new Playfield(p);
                    nextTurn.mana = p.ownMaxMana + 2;
                    if (nextTurn.mana > 10) nextTurn.mana = 10;
                    int maxPen = 0;
                    foreach (Handmanager.Handcard hhc in p.owncards)
                    {
                        // 下回合需要跳币用的牌
                        if(hhc.getManaCost(nextTurn) == nextTurn.mana && hhc.canplayCard(nextTurn, true))
                        {
                            int pen = this.getComboPenality(hhc.card, nextTurn.ownHero, nextTurn, hc);
                            if (pen < maxPen) maxPen = pen;
                        }
                    }
                    retval -= maxPen;
                }
            }

            if (ownMinionsCount - p.enemyMinions.Count >= 4 && bigMobsInHand >= 1)
            {
                retval += bigMobsInHand * 25;
            }

            // 敌方随从
            //bool hasTank = false;
            // 敌方已死，不考虑随从加权
            if(p.enemyHero.Hp > 0)
            {
                foreach (Minion m in p.enemyMinions)
                {
                    retval -= this.getEnemyMinionValue(m, p);
                    //hasTank = hasTank || m.taunt;
                }
            }            
            retval -= p.enemyMinions.Count * 2;
            // 敌方奥秘
            retval -= p.enemySecretCount;
            // 溢出伤害
            //retval -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2
            // 溢出武器伤害
            retval -= p.lostWeaponDamage;

            //if (p.ownMinions.Count == 0) retval -= 20;
            //if (p.enemyMinions.Count == 0) retval += 20;
            // 已斩杀
            if (p.enemyHero.Hp <= 0)
            {
                retval += 10000;
                if (retval < 10000) retval = 10000;
            }
            // 感觉要死
            if (p.enemyHero.Hp >= 1 && p.guessingHeroHP <= 0)
            {
                retval -= 1000;
            }
            // 确实无法斩杀，并且对面场攻已经斩杀我们了，苟命要紧，但是如果能神抽说不定还有机会
            if (p.calEnemyTotalAngr() >= p.ownHero.Hp + p.ownHero.armor && (p.calDirectDmg(p.mana, false) < p.enemyHero.Hp + p.enemyHero.armor) && ( p.anzEnemyTaunt > 0 || p.calTotalAngr() + p.calDirectDmg(p.mana, false) < p.enemyHero.Hp + p.enemyHero.armor ) )
            {
                int val = p.owncarddraw * p.mana * 500 - 3000;
                if (val > -1000) val = -1000;
                retval += val;
            }
            // 濒死
            if (p.ownHero.Hp <= 0) retval -= 20000;

            //retval += getCantAcceptPenality(p);

            return retval;
        }

        /// <summary>
        /// 场面值，决定AI如何判定最优解
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual float getPlayfieldValue(Playfield p)
        {
            return 0;
        }

        /// <summary>
        /// 敌我生命值的价值判定
        /// </summary>
        /// <param name="p">场面</param>
        /// <param name="hpboarder">我方危险血线</param>
        /// <param name="aggroboarder">敌方危险血线</param>
        /// <returns></returns>
        public virtual int getHpValue(Playfield p, int hpboarder, int aggroboarder)
        {
            int retval = 0;
            // 血线安全
            if (p.ownHero.Hp + p.ownHero.armor > hpboarder)
            {
                retval += (2 + p.ownHero.Hp + p.ownHero.armor - hpboarder ) / 2;
            }
            // 快死了
            else
            {
                retval -= 4 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
            }
            // 对手血线安全
            if (p.enemyHero.Hp + p.enemyHero.armor > aggroboarder)
            {
                retval += 3 - p.enemyHero.Hp - p.enemyHero.armor + aggroboarder;
            }
            // 开始打脸
            else if (p.enemyHero.Hp + p.enemyHero.armor <= aggroboarder && p.enemyHero.Hp + p.enemyHero.armor > aggroboarder / 3)
            {
                retval += 3 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor);
            }
            else
            {
                retval += 4 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor);
            }
            return retval;
        }


        public virtual int getEnemyMinionValue(Minion m, Playfield p)
        {
            return 0;
        }

        public virtual string BehaviorName()
        {
            return "None";
        }

        public virtual int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            return 0;
        }

        public virtual int getAttackWithHeroPenality(Minion target, Playfield p)
        {
            return 0;
        }

        public virtual int getAttackWithMininonPenality(Minion m, Playfield p, Minion target)
        {
            return 0;
        }

        public virtual int getSirFinleyPriority(List<Handmanager.Handcard> discoverCards)
        {
            return -1;
        }

        public virtual int getSirFinleyPriority(CardDB.Card card)
        {
            return -1;
        }



        public virtual int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)  
            // 每个策略写自己特定的，通用的放PenalityManager里面getSpecial...，所以要注意不要写重复了，避免双重惩罚
        {
            return 0;
        }

        public virtual int getMyMinionValue(Minion m, Playfield p)
        {
            int retval = 5;
            retval += m.Hp * 2;
            if(!m.cantAttack || !m.Ready || !m.frozen){
                retval += m.Angr * 2;
            }else {
                retval += m.Angr / 2;
            }
            // 风怒价值
            if ((!m.playedThisTurn || m.rush == 1 || m.charge == 1 )  && m.windfury) retval += m.Angr;
            // 圣盾价值
            if (m.divineshild) retval += m.Angr / 2 + 1;
            // 潜行价值
            if (m.stealth) retval += m.Angr / 3 + 1;
            // 吸血
            if (m.lifesteal) retval += m.Angr / 3 + 1;
            // 圣盾嘲讽
            if (m.divineshild && m.taunt) retval += 4;
            retval += m.synergy;
            return retval;
        }

        /// <summary>
        /// 攻击触发的奥秘惩罚
        /// </summary>
        /// <param name="si"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public virtual int getSecretPen_CharIsAttacked(Playfield p, SecretItem si, Minion attacker, Minion defender)
        {
            int pen = 0;
            // 攻击的基本惩罚
            if (si.canBe_noblesacrifice)
            {
                pen -= 10;
                pen += attacker.Hp ;
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_noblesacrifice = false;
                }
            }
            return pen;
        }

        // 英雄受到伤害
        public virtual int getSecretPen_HeroGotDmg(Playfield p, SecretItem si, bool own, int dmg)
        {
            int pen = 0;
            // 冰箱，尽量打到 1 血
            if (si.canBe_iceblock)
            {
                // 破冰时刻
                if(p.enemyHero.Hp - dmg < 0 && p.enemyHero.Hp > 0)
                {
                    pen -= 1000 / (p.enemyHero.Hp);
                }
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_iceblock = false;
                }
            }
            return pen;
        }

        // 打出随从
        public virtual int getSecretPen_MinionIsPlayed(Playfield p, SecretItem si, Minion playedMinion)
        {
            int pen = 0;
            // 打出随从的基本惩罚
            if (si.canBe_snipe)
            {
                // 3费及以下可以接受
                pen -= 3;
                pen += playedMinion.handcard.card.cost ;
                // 冲锋有可能冲不出哦
                if (playedMinion.handcard.card.Charge && (playedMinion.handcard.addHp+ playedMinion.handcard.card.Health ) <= 4 ) 
                    pen += 5;
                // 亡语抵消
                if (playedMinion.handcard.card.deathrattle || playedMinion.handcard.card.reborn)
                {
                    pen -= 2;
                }
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_snipe = false;
                }
            }
            return pen;
        }

        // 打出法术
        public virtual int getSecretPen_SpellIsPlayed(Playfield p, SecretItem si, Minion target, CardDB.Card c)
        {
            if(c.type != CardDB.cardtype.SPELL)
            {
                return 0;
            }
            int pen = 0;
            // 打出法术基本惩罚
            if (si.canBe_counterspell)
            {
                // 2费不亏
                pen -= 2;
                pen += c.cost * 2;
                // 硬币破法反，喜闻乐见喜闻乐见
                if (c.cost == 0 && p.ownMaxMana >= 3) pen -= 5;
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_counterspell = false;
                }
            }
            return pen;
        }

        /// <summary>
        /// 随从死亡
        /// </summary>
        /// <param name="si"></param>
        /// <param name="own"></param>
        /// <returns></returns>
        public virtual int getSecretPen_MinionDied(Playfield p, SecretItem si, bool own)
        {
            int pen = 0;
            return pen;
        }

        // 英雄技能
        public virtual int getSecretPen_HeroPowerUsed(Playfield p, SecretItem si)
        {
            int pen = 0;
            return pen;
        }

        public virtual int getDiscoverVal(CardDB.Card card, Playfield p)
        {
            return 0;
        }

        // 简易模拟对手回合随从交换，粗糙处理，认为对手从大到小依次攻击我方随从
        public virtual int enemyTurnPen(Playfield p)
        {
            int pen = 0;
            List<Minion> enemyMinions = new List<Minion>(p.enemyMinions.ToArray());
            List<Minion> ownMinions = new List<Minion>(p.ownMinions.ToArray());
            enemyMinions.Sort((a, b) => -( a.poisonous ? 10000 : a.Angr + (a.untouchable ? -100 : 0)).CompareTo(b.poisonous ? 10000 : b.Angr + (b.untouchable ? -100 : 0)));
            ownMinions.Sort((a, b) => -(getMyMinionValue(a,p) + (a.taunt ? 1000 : 0) + (a.Hp > 5 || a.untouchable || a.divineshild || a.stealth ? -100: 0 ) ).CompareTo(getMyMinionValue(b, p)) + (b.taunt ? 1000 : 0) + (b.Hp > 5 || b.untouchable || b.divineshild || b.stealth ? -100 : 0));
            int minCnt = enemyMinions.Count > ownMinions.Count ? ownMinions.Count : enemyMinions.Count;
            for(int i = 0; i < minCnt; i++)
            {
                // 对手可以进行交换
                if( (enemyMinions[i].Angr >= ownMinions[i].Hp || enemyMinions[i].poisonous ) )
                {
                    if(ownMinions[i].untouchable || enemyMinions[i].untouchable || ownMinions[i].divineshild || ownMinions[i].stealth)
                    {
                        continue;
                    }
                    // 攻击前
                    int enemyVal1 = getEnemyMinionValue(enemyMinions[i], p);
                    Minion afterAtk = new Minion(enemyMinions[i]);
                    if (!enemyMinions[i].divineshild)
                    {
                        afterAtk.Hp -= ownMinions[i].Angr;
                        if (ownMinions[i].poisonous) afterAtk.Hp = 0;
                    }
                    else
                    {
                        afterAtk.divineshild = false;
                    }
                    // 攻击后
                    int enemyVal2 = getEnemyMinionValue(afterAtk, p);
                    int myVal = getMyMinionValue(ownMinions[i], p);
                    if(ownMinions[i].handcard.card.nameCN == CardDB.cardNameCN.森金持盾卫士 && enemyMinions[i].handcard.card.nameCN == CardDB.cardNameCN.黑铁矮人)
                    {
                        int x = 10;
                    }
                    // 对手愿意交换
                    if(enemyVal1 - enemyVal2 < myVal)
                    {
                        pen -= myVal;
                        pen += enemyVal1 - enemyVal2;
                    }
                }
            }
            return pen;
        }

        // 无法接受的场面惩罚
        public virtual int getCantAcceptPenality(Playfield p)
        {
            // 下回合我方死亡
            if (p.enemyHero.Hp > 0)
            {
                // 计算敌方场攻
                int enemyAtk = p.enemyWeapon == null ? 0 : p.enemyWeapon.Angr;
                switch (p.enemyHeroStartClass)
                {
                    case TAG_CLASS.HUNTER: enemyAtk += 2;break;
                    case TAG_CLASS.ROGUE:
                    case TAG_CLASS.MAGE:
                    case TAG_CLASS.DRUID:
                    case TAG_CLASS.DEMONHUNTER: enemyAtk++;break;
                }
                if (p.ownHero.immune) return 0;
                bool found_flameward = false;
                bool found_explosive = false;

                foreach (CardDB.cardIDEnum s in p.ownSecretsIDList)
                {
                    switch (s)
                    {
                        case CardDB.cardIDEnum.VAN_EX1_295: return 0;
                        case CardDB.cardIDEnum.EX1_295: return 0;
                        case CardDB.cardIDEnum.EX1_289: enemyAtk -= 8; break;
                        case CardDB.cardIDEnum.VAN_EX1_289: enemyAtk -= 8; break;
                        case CardDB.cardIDEnum.ULD_239: found_flameward = true;break;
                        case CardDB.cardIDEnum.CORE_EX1_610: 
                        case CardDB.cardIDEnum.VAN_EX1_610: 
                        case CardDB.cardIDEnum.EX1_610: found_explosive = true; break;
                    }
                }
                foreach (Minion m in p.enemyMinions)
                {
                    if (m.Hp <= 0) continue;
                    if (found_flameward && m.Hp <= 3 && !m.divineshild) continue;
                    if (found_explosive && m.Hp <= 2 && !m.divineshild) continue;
                    enemyAtk += m.Angr;
                    if (m.windfury)
                    {
                        enemyAtk += m.Angr;
                    }
                }
                foreach (Minion m in p.ownMinions)
                {
                    if (m.Hp <= 0) continue;
                    if (m.taunt)
                    {
                        enemyAtk -= m.Hp;
                    }
                }
                // 并且我方(无冰箱)
                if ( enemyAtk >= p.ownHero.Hp + p.ownHero.armor)
                {
                    return -1000;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取使用地标的惩罚值
        /// </summary>
        /// <param name="m"></param>
        /// <param name="target"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual int getUseLocationPenality(Minion m, Minion target, Playfield p)
        {
            return 0;
        }

        /// <summary>
        /// 获取使用泰坦技能的惩罚值
        /// </summary>
        /// <param name="m"></param>
        /// <param name="target"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual int getUseTitanAbilityPenality(Minion m, Minion target, Playfield p)
        {
            return 0;
        }

        /// <summary>
        /// 特殊留牌规则
        /// </summary>
        /// <param name="cards">起手卡牌</param>
        public virtual void specialMulligan(List<HREngine.Bots.Mulligan.CardIDEntity> cards)
        {

        }

        /// <summary>
        /// 探底
        /// </summary>
        /// <param name="card"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual int getDredgeVal(CardDB.Card card, Playfield p)
        {
            return 0;
        }

    }

}