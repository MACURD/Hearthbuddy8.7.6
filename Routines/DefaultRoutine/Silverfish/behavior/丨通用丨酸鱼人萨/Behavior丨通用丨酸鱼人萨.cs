using System.Collections.Generic;
using System;

namespace HREngine.Bots
{
    public partial class Behavior丨通用丨酸鱼人萨 : Behavior
    {
        public override string BehaviorName() { return "丨通用丨酸鱼人萨"; }

        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            // 无法选中
            if (target != null && target.untouchable)
            {
                return 100000;
            }
            if (card.Secret) return -1;
            // 初始惩罚值
            int pen = 0;
            pen = 0;
            // pen += getSpeicalCardVal(card);
            int murlocCount = 0;                //鱼人数量
            foreach (Minion m in p.ownMinions)
            {
                if (m.handcard.card.race == CardDB.Race.MURLOC || m.handcard.card.race == CardDB.Race.ALL) murlocCount++;
            }

            //foreach (Minion m in p.enemyMinions)
            //{
            //    if (m.handcard.card.chnName == CardDB.cardNameCN.末日预言者 && card.type == CardDB.cardtype.MOB && !card.Rush && !card.Charge)
            //        pen += card.Attack * 4 + card.Health * 4 + 10; 
            //}

            int bonus = 4;
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.鱼人招潮者:
                    if (p.ownMaxMana == 1) pen -= 3;
                    break;
                case CardDB.cardNameCN.衰变飞弹:
                    pen += 3 * 5;
                    break;
                case CardDB.cardNameCN.孵化池觅食者:
                case CardDB.cardNameCN.鱼人木乃伊:
                    pen--;
                    break;
                case CardDB.cardNameCN.鱼人宝宝:
                    if (p.ownMaxMana == 1)
                    {
                        bool dontPlay = false;
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.nameCN == CardDB.cardNameCN.下水道渔人 && p.enemyMinions.Count == 0)
                            {
                                dontPlay = true;
                                pen += 30;
                                break;
                            }
                            if (hc.card.nameCN == CardDB.cardNameCN.甜水鱼人斥候)
                            {
                                dontPlay = true;
                                pen += 30;
                                break;
                            }
                            if (hc.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔)
                            {
                                dontPlay = true;
                                pen += 40;
                                break;
                            }
                        }
                        if(dontPlay)
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if ( (hc.card.nameCN == CardDB.cardNameCN.南海岸酋长 || hc.card.nameCN == CardDB.cardNameCN.石塘猎人 || hc.card.nameCN == CardDB.cardNameCN.甜水鱼人佣兵) && p.enemyMinions.Count > 0 && p.mana >= 2)
                            {
                                pen -= 50;
                                break;
                            }
                        }
                    }
                    break;
                case CardDB.cardNameCN.南海岸酋长:
                    if (p.owncards.Count == 1 && p.ownMaxMana > 5 && p.ownMinions.Count < 3 && p.enemyHero.Hp > 10) pen += 50;
                    if (murlocCount == 0)
                    {
                        if (p.enemyHero.Hp < 5) pen += bonus * 10;
                        pen += bonus;
                    }
                    break;
                case CardDB.cardNameCN.下水道渔人:
                    //pen += bonus * 2;
                    if (p.ownMaxMana <= 2 && p.enemyMinions.Count == 0) pen -= bonus * 2;
                    if (p.enemyMinions.Count > 1 && p.ownMaxMana < 3 && p.ownMinions.Count == 0) pen += bonus;
                    if (p.owncards.Count == 1 && p.ownMaxMana > 5 && p.ownMinions.Count < 3 && p.enemyHero.Hp > 10) pen += 50;
                    break;
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                    if (p.enemyMinions.Count > 1 && p.ownMaxMana < 3 && p.ownMinions.Count == 0) pen += bonus;
                    if (p.owncards.Count == 1 && p.ownMaxMana > 5 && p.ownMinions.Count < 3 && p.enemyHero.Hp > 10) pen += 50;
                    if (p.mana == 2)
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            // 手里有毒鳍鱼人，不要下
                            if (hc.card.nameCN == CardDB.cardNameCN.毒鳍鱼人)
                            {
                                return 50;
                            }
                        }
                    // TODO 巨大劣势时留牌等待毒鳍
                    pen += bonus * 2;
                    break;
                case CardDB.cardNameCN.幸运币:
                    break;
                case CardDB.cardNameCN.石塘猎人:
                    if (p.ownMaxMana == 0 && target == null) pen += bonus * 4;
                    if (target == null || murlocCount == 0) pen += bonus;
                    break;
                case CardDB.cardNameCN.冰钓术:
                    break;
                case CardDB.cardNameCN.蛮爪洞穴:
                    if (p.owncards.Count < 4) return -40;
                    return -10;
                    break;
                case CardDB.cardNameCN.寒光先知:
                    if (murlocCount == 0) pen += 10;
                    if (murlocCount < 3) pen += 5;
                    break;
                case CardDB.cardNameCN.深海融合怪:   //酸小明
                    if (murlocCount == 0) return 50;                           //没有鱼人时暂时不下待优化 酸小明
                    if (p.ownMaxMana == 0 && target == null) pen += bonus * 4;
                    if (target == null || murlocCount == 0) pen += bonus;
                    break;
                case CardDB.cardNameCN.小丑鱼:   //酸小明
                    if (p.owncards.Count >= 3) return -50;// 手牌数大于等于3
                    break;
                case CardDB.cardNameCN.鳄鱼人掠夺者:   //酸小明
                    if (p.owncards.Count < 4) pen += bonus * 4;// 手牌数小于4
                    break;
                case CardDB.cardNameCN.迷潮挖掘者:   //酸小明
                    if (p.ownMinions.Count <= 5) return -12;            //确保有位置下2个
                    pen--;
                    break;
                case CardDB.cardNameCN.甜水鱼人佣兵:
                    if (murlocCount == 0) pen += bonus;
                    break;
                case CardDB.cardNameCN.厨师曲奇:
                    if (murlocCount == 0) pen -= bonus;
                    break;
                case CardDB.cardNameCN.暗鳞先知:
                    pen += bonus * 2;
                    break;
                case CardDB.cardNameCN.鱼人领军:
                    // 哪个混蛋设置了按鱼人数量修正？暂时先这样修正了
                    //if (murlocCount == 0) pen -= 50;
                    if (murlocCount > 1) pen += 20;
                    pen += bonus * 3;
                    break;
                case CardDB.cardNameCN.芬利莫格顿爵士:
                    if (p.enemyHero.Hp <= 2) pen -= 100;
                    if (p.enemyHero.Hp <= 1) pen -= 300;
                    break;
                case CardDB.cardNameCN.毒鳍鱼人:
                    if (p.owncards.Count == 1 && p.ownMaxMana > 5 && murlocCount < 3 && p.enemyHero.Hp > 10) pen += 50;
                    if (target == null || target.poisonous) pen += 10;
                    if (target != null)
                    {
                        if (target.handcard.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔 && p.enemyMinions.Count > 0) { pen -= 50; pen -= p.mana; }
                        else pen += 5;
                    }
                    // 对手空场别下
                    if (p.enemyMinions.Count == 0)
                    {
                        pen += 10;
                        //if (target != null && target.Hp == 1) pen += 2;
                    }
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        // 手里有火焰术士，不要下
                        if (hc.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔)
                        {
                            return 50;
                        }
                        if (hc.card.race == CardDB.Race.MURLOC && hc.manacost <= p.mana && hc.card.nameCN != CardDB.cardNameCN.毒鳍鱼人)
                        {
                            pen += 10;
                        }
                    }
                    break;
                case CardDB.cardNameCN.鱼勇可贾:
                    if (p.ownMinions.Count == 0) pen += 1000;
                    pen += 5;
                    break;
                case CardDB.cardNameCN.鱼人恩典:
                    if (p.ownMinions.Count == 0) pen += 1000;
                    pen -= 5;
                    break;
                case CardDB.cardNameCN.寒光智者:
                    if (p.owncards.Count > 3) pen += bonus * 2;
                    break;
                case CardDB.cardNameCN.甜水鱼人斥候:
                    if (p.enemyMinions.Count > 1 && p.ownMaxMana < 3 && p.ownMinions.Count == 0) pen += bonus * 3;
                    if (p.ownMaxMana <= 2 && p.enemyMinions.Count == 0) pen -= bonus * 3;
                    break;
                case CardDB.cardNameCN.鱼人大厨:
                    if (p.owncards.Count < 3) pen += bonus * 4;
                    break;
                case CardDB.cardNameCN.图腾召唤:
                    //pen += bonus ;
                    if (p.ownMinions.Count < 2) pen += bonus * 2;
                    break;
            }
            return pen;
        }

        PenalityManager penman = PenalityManager.Instance;
        // 核心，场面值
        public override float getPlayfieldValue(Playfield p)
        {
            if (p.value > -20000)
            {
                return p.value;
            }
            float retval = 0;
            retval += getGeneralVal(p);
            // 危险血线
            int hpboarder = 10;
            // 抢脸血线
            int aggroboarder = 15;
            retval += getHpValue(p, hpboarder, aggroboarder);
            // 对手基本随从交换模拟
            retval += enemyTurnPen(p);
            // 出牌序列数量
            int count = p.playactions.Count;
            int ownActCount = 0;
            // 对手空场
            //if (p.enemyMinions.Count == 0) retval += 4;

            // 排序问题！！！！
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i];
                ownActCount++;
                switch (a.actionType)
                {
                    // 英雄攻击
                    case actionEnum.attackWithHero:
                    case actionEnum.attackWithMinion:
                        if (a.target != null && a.target.handcard.card.nameCN == CardDB.cardNameCN.前沿哨所)
                        {
                            retval -= i * 5;
                        }
                        continue;
                    case actionEnum.useHeroPower:
                        foreach(Minion m in p.ownMinions)
                        {
                            if(m.handcard.card.nameCN == CardDB.cardNameCN.鱼人骑士)
                            {
                                retval -= i;
                            }
                        }
                        break;
                    case actionEnum.playcard:
                        break;
                    default:
                        continue;
                }
                switch (a.card.card.nameCN)
                {
                    case CardDB.cardNameCN.生命分流:
                        if(p.owncards.Count <= 3)
                        foreach(Minion m in p.ownMinions)
                        {
                            if(m.handcard.card.nameCN == CardDB.cardNameCN.贪婪的书虫)
                            {
                                retval -= 100;
                            }
                        }
                        retval -= i * 4;
                        if (p.enemyHero.Hp < 5) retval += 10;
                        break;
                    case CardDB.cardNameCN.小丑鱼:
                    case CardDB.cardNameCN.火焰术士弗洛格尔:
                        retval -= i * 5;
                        break;
                    case CardDB.cardNameCN.芬利莫格顿爵士:
                        retval -= i;
                        break;
                    case CardDB.cardNameCN.甜水鱼人斥候:
                    case CardDB.cardNameCN.鱼人招潮者:
                    case CardDB.cardNameCN.迷潮挖掘者:
                    case CardDB.cardNameCN.下水道渔人:
                    case CardDB.cardNameCN.毒鳍鱼人:
                        retval -= i * 2;
                        break;
                    case CardDB.cardNameCN.冰钓术:
                    case CardDB.cardNameCN.鳄鱼人掠夺者:
                        if (i == 0) retval += 6;
                        retval -= i * 3;
                        break;
                    //case CardDB.cardNameCN.南海岸酋长:
                    //    retval += i * 2;
                    //    break;
                }
            }
          
            retval += getSecretPenality(p); // 奥秘的影响
            //p.value = retval;
            return retval;
        }

        // 发现卡的价值
        public override int getDiscoverVal(CardDB.Card card, Playfield p)
        {
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.老瞎眼:
                    int cnt = 0;
                    foreach(Minion m in p.ownMinions)
                    {
                        if(m.handcard.card.race == CardDB.Race.MURLOC || m.handcard.card.race == CardDB.Race.ALL)
                        {
                            cnt++;
                        }
                    }
                    if (p.enemyHero.Hp <= cnt + 2) return 200;
                    return 20;
                case CardDB.cardNameCN.莫戈尔莫戈尔格:
                case CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameCN.下水道渔人:
                    return 20;
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (hc.card.nameCN == CardDB.cardNameCN.毒鳍鱼人) return 100;
                    }
                    return 30;

                case CardDB.cardNameCN.石塘猎人:
                case CardDB.cardNameCN.鱼人宝宝:
                case CardDB.cardNameCN.鱼人投手:
                case CardDB.cardNameCN.斯卡基尔:
                case CardDB.cardNameCN.孵化池觅食者:
                    return 10;
                case CardDB.cardNameCN.寒光先知:
                    return 5;

                case CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameCN.邪鳍导航员:
                case CardDB.cardNameCN.鱼人领军:
                    return p.ownMinions.Count * 10;
                case CardDB.cardNameCN.毒鳍鱼人:
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.handcard.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔 && !m.poisonous) return 100;
                    }
                    foreach(Handmanager.Handcard hc in p.owncards)
                    {
                        if(hc.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔) return 100;
                    }
                    return 5;
                case CardDB.cardNameCN.鱼人魔术师:
                case CardDB.cardNameCN.沙鳞灵魂行者:
                case CardDB.cardNameCN.鱼人大厨:
                    return p.owncards.Count < 3 ? 30 : 10;
                case CardDB.cardNameCN.红鳃锋颚战士:
                case CardDB.cardNameCN.芬利莫格顿爵士:
                case CardDB.cardNameCN.鱼人猎潮者:
                case CardDB.cardNameCN.蓝鳃战士:
                case CardDB.cardNameCN.鱼人骑士:
                case CardDB.cardNameCN.奖品商贩:
                case CardDB.cardNameCN.鱼人木乃伊:
                case CardDB.cardNameCN.鱼人招潮者:
                case CardDB.cardNameCN.鱼人袭击者:
                case CardDB.cardNameCN.甜水鱼人佣兵:
                case CardDB.cardNameCN.深海融合怪:
                case CardDB.cardNameCN.南海岸酋长:
                case CardDB.cardNameCN.小丑鱼:
                case CardDB.cardNameCN.鳄鱼人掠夺者:
                case CardDB.cardNameCN.迷潮挖掘者:
                    return 5;
                case CardDB.cardNameCN.争强好胜:
                case CardDB.cardNameCN.永不屈服:
                case CardDB.cardNameCN.古神在上:
                    return 10;
                case CardDB.cardNameCN.隐秘的智慧:
                case CardDB.cardNameCN.救赎:
                case CardDB.cardNameCN.复仇:
                case CardDB.cardNameCN.自动防御矩阵:
                case CardDB.cardNameCN.崇高牺牲:
                    return 5;
                case CardDB.cardNameCN.嗜血:
                case CardDB.cardNameCN.鱼勇可贾:
                    return 2;

            }
            return 0;
        }


        // 敌方随从价值 主要等于 （HP + Angr） * 3 + 5
        public override int getEnemyMinionValue(Minion m, Playfield p)
        {
            bool dieNextTurn = false;
            foreach (Minion mm in p.enemyMinions)
            {
                if (mm.handcard.card.nameCN == CardDB.cardNameCN.末日预言者 )
                {
                    dieNextTurn = true;
                    break;
                }
            }
            foreach (CardDB.cardIDEnum s in p.ownSecretsIDList)
            {
                if (s == CardDB.cardIDEnum.EX1_610 || s == CardDB.cardIDEnum.VAN_EX1_610)
                {
                    if (m.Hp <= 2)
                    {
                        dieNextTurn = true;
                        break;
                    }
                }
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            if (m.Hp <= 0) return 0;
            int bonus = 3;
            int retval = 5;

            bool canBe_flameward = false;
            foreach (SecretItem si in p.enemySecretList)  //Todo: 是否要判断己方回合还是敌方回合？？？
            {
                if (si.canBe_flameward || si.canBe_explosive) { canBe_flameward = true; break; }
            }
            if (canBe_flameward)
            {
                retval += m.Angr + m.Hp + bonus * 3;
            }

            if (m.Angr > 0 || p.enemyHeroStartClass == TAG_CLASS.PALADIN || p.enemyHeroStartClass == TAG_CLASS.PRIEST)
                retval += m.Hp * bonus;
            // 法强相当于 1 点属性
            retval += m.spellpower * bonus;
            if (!m.frozen )
            {
                retval += m.Angr * bonus;
                if(p.ownHero.Hp < 15 || p.ownHero.Hp <= m.Angr * 2) retval += m.Angr * bonus ;
                if (m.windfury) retval += m.Angr * 2;
            }
            if (m.silenced) return retval;

            if (m.taunt) retval += bonus * 3;
            if (m.divineshild) retval += m.Angr * 2;
            if (m.divineshild && m.taunt) retval += 5;
            if (m.stealth) retval += 2;

            // 剧毒价值 2 点属性
            if (m.poisonous && m.Angr < 3)
            {
                retval += bonus * 2 ;
            }
            // 吸血很致命
            if (m.lifesteal) retval += m.Angr * 6;
            
            switch (m.handcard.card.nameCN){
                case CardDB.cardNameCN.聒噪怪:
                    if (m.Spellburst) retval += bonus * 20;
                    break;
                case CardDB.cardNameCN.对空奥术法师:
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                case CardDB.cardNameCN.黑眼:
                    retval += bonus * 20;
                    break;
                // 解不掉游戏结束
                case CardDB.cardNameCN.魅影歹徒:
                case CardDB.cardNameCN.灵魂窃贼:
                case CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameCN.原野联络人:
                case CardDB.cardNameCN.狂欢报幕员:
                case CardDB.cardNameCN.巫师学徒:
                case CardDB.cardNameCN.塔姆辛罗姆:
                case CardDB.cardNameCN.导师火心:
                case CardDB.cardNameCN.伊纳拉碎雷:
                case CardDB.cardNameCN.暗影珠宝师汉纳尔:
                case CardDB.cardNameCN.伦萨克大王:
                case CardDB.cardNameCN.洛卡拉:
                case CardDB.cardNameCN.布莱恩铜须:
                case CardDB.cardNameCN.观星者露娜:
                case CardDB.cardNameCN.大法师瓦格斯:
                case CardDB.cardNameCN.火妖:
                case CardDB.cardNameCN.下水道渔人:
                case CardDB.cardNameCN.空中炮艇:
                case CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameCN.团伙核心:
                case CardDB.cardNameCN.巡游领队:
                case CardDB.cardNameCN.科卡尔驯犬者:
                case CardDB.cardNameCN.火舌图腾:
                    retval += bonus * 8;
                    break;
                case CardDB.cardNameCN.末日预言者:
                    //foreach(Minion mm in p.ownMinions)
                    //{
                    //    retval += (mm.Hp + mm.Angr) * 5;
                    //}
                    break;
                // 不解巨大劣势
                case CardDB.cardNameCN.法师猎手:
                case CardDB.cardNameCN.萨特监工:
                case CardDB.cardNameCN.甩笔侏儒:
                case CardDB.cardNameCN.精英牛头人酋长金属之神:
                case CardDB.cardNameCN.前沿哨所:
                case CardDB.cardNameCN.莫尔杉哨所:
                case CardDB.cardNameCN.凯瑞尔罗姆:
                case CardDB.cardNameCN.鱼人领军:
                case CardDB.cardNameCN.南海船长:
                case CardDB.cardNameCN.坎雷萨德埃伯洛克:
                case CardDB.cardNameCN.人偶大师多里安:
                case CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameCN.灭龙弩炮:
                case CardDB.cardNameCN.神秘女猎手:
                case CardDB.cardNameCN.鲨鳍后援:
                case CardDB.cardNameCN.怪盗图腾:
                case CardDB.cardNameCN.矮人神射手:
                case CardDB.cardNameCN.任务达人:
                case CardDB.cardNameCN.贪婪的书虫:
                case CardDB.cardNameCN.战马训练师:
                case CardDB.cardNameCN.相位追猎者:
                case CardDB.cardNameCN.鱼人宝宝车队:
                case CardDB.cardNameCN.科多兽骑手:
                case CardDB.cardNameCN.奥秘守护者:
                case CardDB.cardNameCN.获救的流民:
                case CardDB.cardNameCN.白银之手新兵:
                case CardDB.cardNameCN.低阶侍从:
                    retval += bonus * 3;
                    break;
                // 算有点用
                case CardDB.cardNameCN.战斗邪犬:
                case CardDB.cardNameCN.饥饿的秃鹫:
                case CardDB.cardNameCN.法力浮龙:
                case CardDB.cardNameCN.加基森拍卖师:
                case CardDB.cardNameCN.飞刀杂耍者:
                case CardDB.cardNameCN.锈水海盗:
                case CardDB.cardNameCN.大法师安东尼达斯:
                    retval += bonus * 2;
                    break;
                case CardDB.cardNameCN.疯狂的科学家:
                    retval -= bonus * 4;
                    break;
                //case CardDB.cardNameCN.力量图腾:
                //    retval -= 10;
                //    break;
            }
            return retval;
        }
        // 我方随从价值 主要等于 （HP + Angr） * 4 + 6
        public override int getMyMinionValue(Minion m, Playfield p)
        {
            bool dieNextTurn = false;
            foreach (Minion mm in p.enemyMinions)
            {
                if (mm.handcard.card.nameCN == CardDB.cardNameCN.末日预言者)
                {
                    dieNextTurn = true;
                    break;
                }
                if ( (mm.handcard.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔 || mm.handcard.card.nameCN == CardDB.cardNameCN.对空奥术法师) && p.enemyAnzCards > 2)
                {
                    if (mm.poisonous)
                        dieNextTurn = true;
                    if (m.Hp <= 2) dieNextTurn = true;
                    break;
                }
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            int retval = 0;
            int bonus = 4;
            if (m.Hp <= 0) return 0;
            retval += m.Hp * bonus;
            retval += m.Angr * bonus;
            if (m.Hp <= 1) retval -= (m.Angr - 1) * (bonus - 1 );
            // 高攻低血是垃圾
            if (m.Angr > m.Hp + 4) retval -= (m.Angr - m.Hp) * (bonus - 1);
            // 风怒价值
            if ((!m.playedThisTurn || m.rush == 1 || m.charge == 1) && m.windfury) retval += m.Angr;
            // 圣盾价值
            if (m.divineshild) retval += m.Angr * 3;
            // 潜行价值
            if (m.stealth) retval += m.Angr / 2 + 1;
            // 吸血
            if (m.lifesteal) retval += m.Angr / 2 + 1;
            // 圣盾嘲讽
            if (m.divineshild && m.taunt) retval += 4;
            // 鱼人种族价值
            if (m.handcard.card.race == CardDB.Race.MURLOC || m.handcard.card.race == CardDB.Race.ALL)
            {
                retval += bonus + 6;
            }
            // 剧毒价值 2 点属性
            if (m.poisonous && m.Angr < 3)
            {
                retval += bonus * 2;
            }
            if (m.reborn)
            {
                retval += bonus;
            }
            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                    retval += bonus * 2;
                    break;
                case CardDB.cardNameCN.孵化池觅食者:
                    retval += bonus;
                    break;
                case CardDB.cardNameCN.暗鳞先知:
                    retval += bonus * 2;
                    break;
                case CardDB.cardNameCN.鱼人领军:
                    retval += bonus * 3;
                    break;
                case CardDB.cardNameCN.下水道渔人:
                case CardDB.cardNameCN.鱼人招潮者:
                    retval += bonus;
                    break;
                case CardDB.cardNameCN.甜水鱼人斥候:
                    retval += bonus * 3;
                    if (m.Hp < 2) retval -= bonus * 2;
                    break;
                case CardDB.cardNameCN.贪婪的书虫:
                    if (p.owncards.Count < 3)
                        retval += bonus * (3 - p.owncards.Count);
                    break;
            }
            return retval;
        }

        public override int getSirFinleyPriority(List<Handmanager.Handcard> discoverCards)
        {
            return -1; //comment out or remove this to set manual priority
            int sirFinleyChoice = -1;
            int tmp = int.MinValue;
            for (int i = 0; i < discoverCards.Count; i++)
            {
                CardDB.cardNameEN name = discoverCards[i].card.nameEN;
                if (SirFinleyPriorityList.ContainsKey(name) && SirFinleyPriorityList[name] > tmp)
                {
                    tmp = SirFinleyPriorityList[name];
                    sirFinleyChoice = i;
                }
            }
            return sirFinleyChoice;
        }

        public override int getSirFinleyPriority(CardDB.Card card)
        {
            return SirFinleyPriorityList[card.nameEN];
            //switch (card.name)
            //{
            //    case CardDB.cardName.lesserheal: return 1;
            //    case CardDB.cardName.shapeshift: return 6;
            //    case CardDB.cardName.fireblast:
            //        if (p.enemyHero.Hp <= 1) return 100;
            //        return 7;
            //    case CardDB.cardName.armorup: return 0;
            //    case CardDB.cardName.lifetap: return 9;
            //    case CardDB.cardName.demonclaws:
            //        if (p.enemyHero.Hp <= 1) return 100;
            //        return 10;
            //    case CardDB.cardName.daggermastery:
            //        if (p.enemyHero.Hp <= 1) return 100;
            //        return 5;
            //    case CardDB.cardName.reinforce: return 4;
            //    case CardDB.cardName.totemiccall: return 2;
            //    case CardDB.cardName.steadyshot: return 8;
            //}
            //return -1;
        }

        private Dictionary<CardDB.cardNameEN, int> SirFinleyPriorityList = new Dictionary<CardDB.cardNameEN, int>
        {
            //{HeroPowerName, Priority}, where 0-9 = manual priority
            { CardDB.cardNameEN.lesserheal, 1 }, 
            { CardDB.cardNameEN.shapeshift, 6 },
            { CardDB.cardNameEN.fireblast, 7 },
            { CardDB.cardNameEN.armorup, 0 },
            { CardDB.cardNameEN.lifetap, 9 },
            { CardDB.cardNameEN.demonclaws, 10 },
            { CardDB.cardNameEN.daggermastery, 5 },
            { CardDB.cardNameEN.reinforce, 4 },
            { CardDB.cardNameEN.totemiccall, 2 },
            { CardDB.cardNameEN.steadyshot, 11 }
        };


        public override int getHpValue(Playfield p, int hpboarder, int aggroboarder)
        {
            int offset_enemy = 0;
            if(p.anzEnemyTaunt == 0)
            {
                foreach (Minion m in p.ownMinions)
                {
                    if (m.Ready && !m.cantAttackHeroes && !m.frozen && !(m.rush > 0 && m.playedThisTurn))
                    {
                        offset_enemy -= m.Angr;
                    }
                }
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.manacost > p.mana) continue;
                    if (hc.card.Charge)
                    {
                        offset_enemy -= hc.card.Attack + hc.addattack;
                    }
                    switch (hc.card.nameCN)
                    {
                        case CardDB.cardNameCN.南海岸酋长:
                            offset_enemy -= 2;
                            break;
                    }
                }
            }
            int retval = 0;
            // 血线安全
            if (p.ownHero.Hp + p.ownHero.armor > hpboarder)
            {
                retval += (5 + p.ownHero.Hp + p.ownHero.armor - hpboarder);
            }
            // 快死了
            else
            {
                //if (p.nextTurnWin()) retval -= (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
                retval -= 5 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
            }
            if (p.ownHero.Hp + p.ownHero.armor < 10 && p.ownHero.Hp + p.ownHero.armor > 0)
            {
                retval -= 200 / (p.ownHero.Hp + p.ownHero.armor);
            }
            // 对手血线安全
            if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy >= aggroboarder)
            {
                retval += 2 * (aggroboarder - p.enemyHero.Hp - p.enemyHero.armor - offset_enemy);
            }
            // 开始打脸
            else
            {
                retval += 4 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor - offset_enemy);
            }
            // 进入斩杀线
            if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy <= 5 && p.enemyHero.Hp + p.enemyHero.armor + offset_enemy > 0)
            {
                retval += 300 / (p.enemyHero.Hp + p.enemyHero.armor - offset_enemy);
            }
            // 预计完成斩杀
            if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy <= 0)
            {
                retval += 2000;
            }
            return retval;
        }


        // 简易模拟对手回合随从交换，粗糙处理，认为对手从大到小依次攻击我方随从
        public override int enemyTurnPen(Playfield p)
        {
            int pen = 0;
            List<Minion> enemyMinions = new List<Minion>(p.enemyMinions.ToArray());
            List<Minion> ownMinions = new List<Minion>(p.ownMinions.ToArray());
            enemyMinions.Sort((a, b) => -(a.poisonous ? 10000 : a.Angr + (a.untouchable ? -100 : 0)).CompareTo(b.poisonous ? 10000 : b.Angr + (b.untouchable ? -100 : 0)));
            ownMinions.Sort((a, b) => -(getMyMinionValue(a, p) + (a.Hp > 5 ? -100 : 0)).CompareTo(getMyMinionValue(b, p)) + (b.Hp > 5 ? -100 : 0));
            int minCnt = enemyMinions.Count > ownMinions.Count ? ownMinions.Count : enemyMinions.Count;
            for (int i = 0; i < minCnt; i++)
            {
                // 对手可以进行交换
                if ((enemyMinions[i].Angr >= ownMinions[i].Hp || enemyMinions[i].poisonous) && !ownMinions[i].divineshild && !ownMinions[i].stealth)
                {
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
                    if (ownMinions[i].handcard.card.nameCN == CardDB.cardNameCN.火焰术士弗洛格尔) pen -= 100;
                    if (ownMinions[i].handcard.card.nameCN == CardDB.cardNameCN.下水道渔人) pen -= 10;
                    // 对手愿意交换
                    if (enemyVal1 - enemyVal2 < myVal)
                    {
                        pen -= myVal;
                        pen += enemyVal1 - enemyVal2;
                    }
                }
            }
            return pen;
        }

        // 打出随从
        public override int getSecretPen_MinionIsPlayed(Playfield p, SecretItem si, Minion playedMinion)
        {
            int pen = 0;
            // 打出随从的基本惩罚
            if (si.canBe_snipe)
            {
                // 3费及以下可以接受
                pen -= 3;
                pen += playedMinion.handcard.card.cost;
                // 冲锋有可能冲不出哦
                if (playedMinion.handcard.card.Charge && (playedMinion.handcard.addHp + playedMinion.handcard.card.Health) <= 4)
                    pen += 5;
                // 亡语抵消
                if (playedMinion.handcard.card.deathrattle || playedMinion.handcard.card.reborn)
                {
                    pen -= 2;
                }
                switch (playedMinion.handcard.card.nameCN)
                {
                    case CardDB.cardNameCN.孵化池觅食者:
                    case CardDB.cardNameCN.鱼人木乃伊:
                    case CardDB.cardNameCN.鱼人宝宝:
                    case CardDB.cardNameCN.芬利莫格顿爵士:
                        pen -= 20;
                        break;
                    case CardDB.cardNameCN.甜水鱼人斥候:
                    case CardDB.cardNameCN.下水道渔人:
                    case CardDB.cardNameCN.火焰术士弗洛格尔:
                    case CardDB.cardNameCN.鱼人领军:
                    case CardDB.cardNameCN.暗鳞先知:
                        pen += 10;
                        break;
                }
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_snipe = false;
                }
            }
            return pen;
        }

    }

   

}


