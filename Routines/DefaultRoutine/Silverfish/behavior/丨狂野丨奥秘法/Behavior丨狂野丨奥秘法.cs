using System.Collections.Generic;
using System;
using System.Linq;

namespace HREngine.Bots
{

    public partial class Behavior丨狂野丨奥秘法 : Behavior
    {
        public override string BehaviorName() { return "丨狂野丨奥秘法"; }


        PenalityManager penman = PenalityManager.Instance;

        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard) //每个策略自己用到的卡牌放在这里面,这是单动作评估里调用，动作实施前
        {
            int pen = 0;
            bool zeroSecret = p.nextSecretThisTurnCost0; //手牌中下个奥秘0费出，注意：有可能手牌没有奥秘，只是因为下了肯瑞托法师等此值为true
            bool hasSecretHand = false;  //手牌是否有奥秘
            bool hasSecretHead = (p.ownSecretsIDList.Count >= 1); //头上有奥秘
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.Secret)
                {
                    hasSecretHand = true;
                    break;
                }
            }

            bool needSecret = false;
            foreach (Handmanager.Handcard hc in p.owncards)  // 暗金教水晶侍女；云雾王子；麦迪文男仆；隐秘咒术师；造谣食尸鬼
            {
                if (hc.card.nameEN == CardDB.cardNameEN.kabalcrystalrunner || hc.card.nameEN == CardDB.cardNameEN.cloudprince
                    || hc.card.nameEN == CardDB.cardNameEN.medivhsvalet || hc.card.nameEN == CardDB.cardNameEN.occultconjurer || hc.card.nameEN == CardDB.cardNameEN.scuttlebuttghoul)
                {
                    needSecret = true;
                    break;
                }
            }

            bool useCoinTurnOne = false; // 一上来跳硬币
            if (p.playactions.Count >= 1)
            {
                if (p.playactions[0].card != null)
                    if (p.playactions[0].card.card.nameEN == CardDB.cardNameEN.thecoin && p.ownMaxMana == 1)
                        useCoinTurnOne = true;
            }

            switch (card.nameEN)
            {
                case CardDB.cardNameEN.fireblast: // 英雄技能
                    if (target.own) //打自己随从或者头，提前剪枝
                        return 15000;
                    if (target.isHero) //打敌方头可以
                        return 0;
                    if (target.Hp == 1) //消灭一个随从，很不错
                        return -3;
                    return 15;  //避免点一些没用的随从，Todo:要考虑和火焰打3的配合，待优化

                //暗金教侍从（121 奥秘0费）
                case CardDB.cardNameEN.kaballackey:
                    if (!hasSecretHand && p.ownMaxMana <= 8) return 50;//没有奥秘不要下
                    if (zeroSecret) return 500;//有0费奥秘不要下
                    return -7;//有奥秘

                //匿名线人（223 奥秘0费）
                case CardDB.cardNameEN.anonymousinformant:
				    if (!hasSecretHand && p.ownMaxMana <= 8) return 0;//没有奥秘也可以下
                    if (zeroSecret) return 500;//有0费奥秘不要下
                    return -7;//有奥秘

                //模糊的陌生人（223 奥秘0费）
                case CardDB.cardNameEN.sketchystranger:
                    return -5;

                //健谈的调酒师
                case CardDB.cardNameEN.chattybartender:
                    pen = 0;
                    if (p.enemyMinions.Count > 0)
                    {
                        int kill_num = 0;
                        foreach (Minion mi in p.enemyMinions)
                        {
                            if (mi.Hp <= 2)                    //对方随从生命值小于 2
                            {
                                kill_num++;
                                pen -= mi.Angr * mi.Angr; // 惩罚值和攻击力成正比

                            }
                        }
                        pen -= kill_num * 5 + p.enemyMinions.Count;  //每能炸一个随从，加1，炸死一个，加5
                    }
                    else pen = 5;
                    return pen;

                //豪宅管家俄里翁
                case CardDB.cardNameEN.orionmansionmanager:
                    if (!hasSecretHead) return 50;
                    if (p.enemyMinions.Count > 2) return 0;
                    return -20;

                case CardDB.cardNameEN.kirintormage://肯瑞托法师
                    if (zeroSecret && hasSecretHand) return 500; //有0费奥秘在手，先下奥秘再说。 注意要判断hasSecretHand，以防没法2个连下等
                    if (!hasSecretHand)
                    {
                        if (p.ownMaxMana <= 5) return 20;//没有奥秘不要下
                        else return 0;// 费用高的时候该下就下了  因为案例：缺奥秘425 更新费用阈值从8->5,如果后续遇到反例，可以考虑引入对方血量这一限制
                    }
                    return -8;//有奥秘

                case CardDB.cardNameEN.aluneth: //艾露尼斯
                    if (p.owncards.Count >= 5) return 500;
                    else return -30;

                //麦迪文的男仆
                case CardDB.cardNameEN.medivhsvalet:
                    if (!hasSecretHead) return 50;
                    if (p.enemyHero.Hp <= 20)
                    {
                        if (target.isHero && !target.own) return -5;
                    }
                    return 0;
                //隐秘咒术师
                case CardDB.cardNameEN.occultconjurer:
                    if (!hasSecretHead) return 50;
                    if (p.ownMinions.Count <= 5) return -12;  //确保有位置下2个44
                    return 0;
                //造谣食尸鬼
                case CardDB.cardNameEN.scuttlebuttghoul:
                    if (!hasSecretHead) return 50;
                    if (p.ownMinions.Count <= 5) return -12;  //确保有位置下2个44
                    return 0;
                //云雾王子
                case CardDB.cardNameEN.cloudprince:
                    if (!hasSecretHead) return 60;
                    if (target.isHero && !target.own && p.enemyHero.Hp <= 15) return -20; //打敌方头，且敌方血量少
                    if (target.isHero && !target.own) return -6;
                    return 0;
                //暗月先知赛格
                case CardDB.cardNameEN.saygeseerofdarkmoon:
                    int ownCardsNum = p.owncards.Count; // 当前手牌数
                    if (ownCardsNum >= 6) // 手牌数大于等于6，则不抽
                    {
                        return 20;
                    }
                    int ownDeckSize = p.ownDeckSize; // 牌库剩余量
                    int drawCardsNum = 1; // 暗月先知抽牌数
                    int ownHeroHP = p.ownHero.Hp; // 我方英雄血量
                    int enemyHeroHP = p.enemyHero.Hp; // 敌方英雄血量
                    foreach (Handmanager.Handcard hc in p.owncards) // 获取暗月先知抽牌数
                    {
                        if (hc.card.nameEN == CardDB.cardNameEN.saygeseerofdarkmoon)
                        {
                            //Helpfunctions.Instance.ErrorLog("打出赛格抽牌数：" + hc.SCRIPT_DATA_NUM_1);
                            drawCardsNum = hc.SCRIPT_DATA_NUM_1;
                            break;
                        }
                    }
                    if (drawCardsNum > ownDeckSize) // 抽牌数大于牌库数
                    {
                        int tiredDmg1 = 0; // 获取抽牌造成的疲劳伤害总和
                        int maxTiredDmg = 0; // 抽牌后的下回合疲劳伤害
                        for (int i = 1; i <= drawCardsNum - ownDeckSize + 1; i++) // 我方下回合不会疲劳死
                        {
                            tiredDmg1 += i;
                            maxTiredDmg = i;
                        }
                        if (enemyHeroHP <= 12 && ownHeroHP - tiredDmg1 > 0 && p.ownMaxMana >= 10) // 敌方HP不高于12，我方下回合不会疲劳死，法力值大于等于10点，则抽牌GTMD
                            return -20;
                        if (enemyHeroHP <= 6 && ownHeroHP - tiredDmg1 + maxTiredDmg > 0 && p.ownMaxMana >= 10) // 敌方HP不高于6，我方当前回合不会疲劳死，法力值大于等于10点，则抽牌GTMD
                            return -20;
                        else return 20;
                    }
                    return -(6 - p.owncards.Count) * (6 - p.owncards.Count) * 4; // 二次函数，牌越少越要出
                //低调的游客
                case CardDB.cardNameEN.inconspicuousrider:
                    if (p.ownSecretsIDList.Count >= 5) return 20;  //确保头上或者牌库中还有不同类型奥秘可以挂
                    if (needSecret) return -8;
                    if (!hasSecretHead) return -8;  //头上没有优先级更高
                    return -5;
                // 部落特工
                case CardDB.cardNameEN.hordeoperative: //根据敌人头上奥秘的数量，数量越多，越适合出
                    if (p.enemySecretCount == 0) return 5;
                    if (p.enemySecretCount == 1) return -5;
                    if (p.enemySecretCount == 2) return -15;
                    if (p.enemySecretCount >= 3) return -20;
                    break;
                // 军情七处渗透者
                case CardDB.cardNameEN.si7infiltrator: //根据敌人头上有无奥秘
                    if (p.enemySecretCount >= 1) return -20;
                    if (p.enemyHero.cardClass == TAG_CLASS.MAGE || p.enemyHero.cardClass == TAG_CLASS.HUNTER
                      || p.enemyHero.cardClass == TAG_CLASS.PALADIN)   // 法师，猎人，圣骑士容易头上有奥秘
                    {
                        if (p.enemySecretCount == 0)
                            return 5;  //晚点出，等对方头上有奥秘
                    }
                    return 0;
                case CardDB.cardNameEN.arcaneflakmage://对空奥术法师
                    int secret_count = 0;//当前mana可以出的secret
                    int available_mana = p.mana - card.cost;  //要扣掉奥术法师的cost
                    foreach (Handmanager.Handcard hc in p.owncards)
                    {
                        if (hc.card.Secret && available_mana >= hc.getManaCost(p))  //奥秘费用会变，不能用card.cost
                                                                                    //注意这段代码可以正确处理0和3两种奥秘费用，如果奥秘费用多样化，则需要对费用从小到大排序
                        {
                            if (!p.ownSecretsIDList.Contains(hc.card.cardIDenum))
                            // fix伏笔，虽然手牌有奥秘，但和头上挂的一样，出了对空后，发现没法出奥秘，尴尬
                            {
                                available_mana -= hc.manacost;
                                secret_count++;
                            }
                        }
                    }
                    if (secret_count == 0)  //裸下
                    {
                        if (p.enemyMinions.Count >= 1 || p.enemyMaxMana >= 4)  //且对面有人或者费足够，送死
                            pen = 15;
                        else
                        {
                            if (useCoinTurnOne) // 开头跳费出 很傻
                                pen = 20;
                            else
                                pen = 8; //对面空场且费少，可能存活
                        }
                    }
                    else // 可以触发
                    {
                        pen = -p.enemyMinions.Count * 2 * secret_count;
                        foreach (Minion mi in p.enemyMinions)
                        {
                            if (mi.Hp <= 2 * secret_count)
                                pen -= 4; // 每致死一位敌方随从，额外4分
                        }
                    }
                    return pen;
                //疯狂的科学家
                case CardDB.cardNameEN.madscientist:
                    if (!hasSecretHead) return -3;
                    return 0;
                //盛装歌手
                case CardDB.cardNameEN.costumedsinger:
                    if (!hasSecretHead) return -15;
                    return 0;
                //秘法学家
                case CardDB.cardNameEN.arcanologist:
                    if (!hasSecretHand) return -5;
                    return 0;
                //远古谜团
                case CardDB.cardNameEN.ancientmysteries:
                    if (!hasSecretHand) return -4;
                    if (zeroSecret) return 10;
                    return 0;
                case CardDB.cardNameEN.kabalcrystalrunner://水晶侍女
                    if (nowHandcard.getManaCost(p) >= 5) return 10;
                    if (nowHandcard.getManaCost(p) >= 4) return 5;
                    if (nowHandcard.getManaCost(p) >= 3) return -2;
                    if (nowHandcard.getManaCost(p) >= 2) return -12;  
                    if (nowHandcard.getManaCost(p) >= 1) return -15;  
                    return 0;
                case CardDB.cardNameEN.contractconjurer://契约咒术师
                    if (p.ownSecretsIDList.Count >= 4) return -15;
                    if (p.ownSecretsIDList.Count >= 3) return -12;
                    if (p.ownSecretsIDList.Count >= 2) return -2;
                    if (p.ownSecretsIDList.Count >= 1) return 10;
                    return 0;
                //非公平游戏
                case CardDB.cardNameEN.riggedfairegame:
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -8;
                    if (p.ownWeapon.name == CardDB.cardNameEN.aluneth)
                    {
                        if (hasSecretHead) return 50; // 不缺牌，不缺奥秘
                        if (needSecret) pen -= 3;
                    }
                    else
                    {//没有装备刀的情况 
                        if (p.owncards.Count >= 7) return 15;
                        if (p.owncards.Count <= 5 || p.enemyMaxMana <= 2) pen -= 10; //缺牌或者早期回合
                        if (p.enemyMinions.Count == 0) pen -= 8;  //对方场上没有随从，更容易触发
                        return pen;
                    }
                    return pen;
                //火焰结界
                case CardDB.cardNameEN.flameward:
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -6;
                    if (needSecret) pen -= 5;
                    if (p.enemyMinions.Count > 0)
                    {
                        int kill_num = 0;
                        foreach (Minion mi in p.enemyMinions)
                        {
                            //对方随从生命值小于 3
                            if (mi.Hp <= 3)
                            {
                                if (mi.name != CardDB.cardNameEN.madscientist)
                                {
                                    kill_num++;
                                    pen -= mi.Angr * mi.Angr; // 惩罚值和攻击力成正比
                                }
                                else
                                    pen += 5;// 不仅仅不加分，要扣5分，因为对方多了个奥秘
                            }
                        }
                        pen -= kill_num * 5 + p.enemyMinions.Count;  //每能炸一个随从，加1，炸死一个，加5
                    }
                    return pen;
                case CardDB.cardNameEN.iceblock://寒冰屏障
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -8;
                    if (p.ownHero.Hp <= 12) //有被斩杀风险
                        pen -= 30 + (12 - p.ownHero.Hp) * (12 - p.ownHero.Hp);
                    if (needSecret)
                        pen -= 4;   //优先级低于其他奥秘
                    return pen;

                case CardDB.cardNameEN.objection://异议
                    pen = -5;
                    if (p.nextSecretThisTurnCost0)
                        pen -= 8;
                    if (needSecret)
                        pen -= 9;
                    return pen;

                case CardDB.cardNameEN.explosiverunes://爆炸符文
                    pen = -5;
                    if (p.nextSecretThisTurnCost0)
                        pen -= 8;
                    if (needSecret)
                        pen -= 9;
                    if (p.enemyMinions.Count == 0 || p.enemyHero.Hp < 15)
                        pen -= 10;
                    return pen;
                case CardDB.cardNameEN.counterspell://法术反制
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -6;
                    if (p.enemyHeroName == HeroEnum.priest && p.enemyMaxMana >= 4) pen -= 10;
                    if (p.enemyHeroName == HeroEnum.warlock && p.enemyMaxMana >= 4) pen -= 10;
                    if (p.enemyHeroName == HeroEnum.mage && p.enemyMaxMana >= 3) pen -= 5;
                    if (p.enemyMaxMana >= 7) pen -= 6;
                    if (p.ownMinions.Count >= 3) pen -= 10;
                    if (needSecret) pen -= 5;
                    return pen;

                case CardDB.cardNameEN.duplicate://复制
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -7;
                    if (p.owncards.Count <= 3)
                        pen -= 4;
                    foreach (Minion m in p.ownMinions)  // 暗金教水晶侍女；云雾王子；隐秘咒术师；谣言食尸鬼
                    {
                        if (m.name == CardDB.cardNameEN.kabalcrystalrunner || m.name == CardDB.cardNameEN.cloudprince
                            || m.name == CardDB.cardNameEN.occultconjurer || m.name == CardDB.cardNameEN.scuttlebuttghoul || m.name == CardDB.cardNameEN.costumedsinger)
                        {
                            pen -= 7;
                            break;
                        }
                    }
                    if (needSecret)
                    {
                        if (p.owncards.Count >= 7) pen -= 2;
                        else pen -= 5;
                    }
                    else
                    {
                        if (p.owncards.Count >= 7) return 20;
                    }
                    return pen;
                case CardDB.cardNameEN.oasisally://绿洲联盟 
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -8;
                    if (needSecret)
                    {
                        if (p.ownMinions.Count >= 1)  //看是否有触发的机会
                            pen -= 7;
                        else
                            pen -= 6;
                    }
                    else
                    {
                        if (p.ownMinions.Count >= 1)
                            pen -= 5;
                        else
                            pen -= 3;
                    }
                    return pen;
                case CardDB.cardNameEN.netherwindportal://虚空之风传送门
                    pen = 0;
                    if (p.nextSecretThisTurnCost0)
                        pen = -8;
                    if (p.enemyHeroName == HeroEnum.priest && p.ownMaxMana <= 3) pen -= 10;
                    if (p.enemyHeroName == HeroEnum.warlock && p.ownMaxMana <= 3) pen -= 10;
                    if (p.enemyHeroName == HeroEnum.mage && p.ownMaxMana <= 3) pen -= 10;
                    if (needSecret)
                        pen -= 5;
                    return pen;
                case CardDB.cardNameEN.fireball://火球术
                    if (target.isHero && !target.own) //打敌方头
                    {
                        if (p.enemyHero.Hp <= 12)
                            return -10;
                        if (p.enemyHero.Hp > 20)  //不要太早打头，出其他牌
                            return 10;
                        return -2;  //一般用于斩杀，不用太早打敌方头
                    }
                    if (target.name == CardDB.cardNameEN.flamewaker) //打火妖没问题
                        return -20;
                    if (!target.isHero) // 说明是随从
                    {
                        if (p.enemyHero.Hp <= 15) // 血量低，不应该打随从
                            return 15;
                        pen = target.Hp - 6;  // Todo: 要考虑法术加成
                        if (pen > 0)
                            return 0;  //打中的随从血量高，还ok
                        else
                            return -pen * 3; //血量低于6，说明伤害有浪费，要惩罚
                    }
                    return 2;

                case CardDB.cardNameEN.forgottentorch://老旧的火把
                    if (target.isHero && !target.own)
                        if (p.enemyHero.Hp <= 10)  //敌方快挂了
                            return -8;
                        else
                            return -3;    //这个值要比一些奥秘值低
                    return 1;

                case CardDB.cardNameEN.roaringtorch://炽热的火把
                    if (target.isHero && !target.own) return -6;
                    return 2;

                case CardDB.cardNameEN.flare: //照明弹
                    pen = 0;
                    foreach (Minion mn in p.ownMinions) if (mn.stealth) pen++;
                    foreach (Minion mn in p.enemyMinions) if (mn.stealth) pen--;
                    if (p.enemySecretCount > 0)
                    {
                        bool canPlayMinion = false;
                        bool canPlaySpell = false;
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.nameEN == CardDB.cardNameEN.flare) continue;
                            if (hc.manacost <= p.mana - 2)
                            {
                                if (!canPlayMinion && hc.card.type == CardDB.cardtype.MOB)
                                {

                                    int tmp = p.getSecretTriggersByType(0, true, false, target);
                                    if (tmp > 0) pen -= tmp * 50;
                                    canPlayMinion = true;
                                    continue;
                                }
                                if (!canPlaySpell && hc.card.type == CardDB.cardtype.SPELL)
                                {
                                    int tmp = p.getSecretTriggersByType(1, true, false, target);
                                    if (tmp > 0) pen -= tmp * 50;
                                    canPlaySpell = true;
                                    continue;
                                }
                            }
                        }
                        pen -= p.enemySecretCount * 5;
                        if (p.playactions.Count == 0) pen -= 5;
                    }
                    else
                    {
                        switch (p.enemyHeroStartClass)
                        {//敌方类型
                            case TAG_CLASS.MAGE: pen += 5; break;
                            case TAG_CLASS.PALADIN: pen += 5; break;
                            case TAG_CLASS.HUNTER: pen += 5; break;
                        }
                    }
                    return pen;
                case CardDB.cardNameEN.eaterofsecrets: //奥秘吞噬者
                    pen = 0;
                    if (p.enemySecretCount >= 1)
                    {
                        pen -= p.enemySecretCount * 50;
                    }
                    else
                    {
                        switch (p.enemyHeroStartClass)
                        {
                            case TAG_CLASS.MAGE: pen += 50; break;  // 法师一定要等
                            case TAG_CLASS.PALADIN: pen += 5; break; //圣骑士可能没奥秘
                            case TAG_CLASS.HUNTER: pen += 5; break;  //猎人也可能不是奥秘猎
                        }
                    }
                    return pen;
                case CardDB.cardNameEN.kezanmystic: //科赞秘术师
                    pen = 0;
                    if (p.enemySecretCount == 1 && p.playactions.Count == 0) pen -= 50;
                    return pen;
            }  // switch 法师结束
            return 0;
        }

        // 发现卡的价值
        public override int getDiscoverVal(CardDB.Card card, Playfield p)
        {
            switch (card.nameCN)
            {
                case CardDB.cardNameCN.爆炸陷阱:
                    return 1 + p.enemyMinions.Count * 10;

                case CardDB.cardNameCN.毒蛇陷阱:
                    if (p.ownMinions.Count > 0 && p.enemyMinions.Count > 0) return 18;
                    return 7;
                case CardDB.cardNameCN.冰冻陷阱:
                    if (p.enemyMinions.Count == 1)
                    {
                        if (p.enemyMinions[0].Hp > 4) return 30;
                    }
                    return 7;
                case CardDB.cardNameCN.打开兽笼:
                    if (p.enemyHeroStartClass == TAG_CLASS.MAGE && p.enemyMinions.Count == 0) return 20;
                    if (p.ownMinions.Count > 3) return 20;
                    return 5;
                case CardDB.cardNameCN.集群战术:
                    if (p.ownMinions.Count > 0 && p.enemyMinions.Count > 0) return 19;
                    return 8;


                case CardDB.cardNameCN.争强好胜:
                    if (p.ownMinions.Count > 2 && p.enemyMinions.Count < 2) return 18;  //我方大于2随从对方小于2随从时选
                    return 4;
                case CardDB.cardNameCN.永不屈服:
                    if (p.ownMinions.Count > 2 && p.enemyMinions.Count < 2) return 18;  //我方大于2随从对方小于2随从时选
                    return 4;

                case CardDB.cardNameCN.邪恶计谋:
                    {
                        if (p.owncards.Count >= 6) // 手牌数大于等于6，则不抽
                        {
                            return 3;
                        }
                        return 18;
                    }


                case CardDB.cardNameCN.毒镖陷阱:
                    {
                        if (p.enemyMinions.Count > 1) return 9;  //对方大于1随从时选
                    }
                    return 4;

                case CardDB.cardNameCN.古神在上:
                case CardDB.cardNameCN.伏击:
                case CardDB.cardNameCN.偷天换日:
                case CardDB.cardNameCN.豹子戏法:
                    return 10;
                case CardDB.cardNameCN.隐秘的智慧:
                case CardDB.cardNameCN.救赎:
                case CardDB.cardNameCN.复仇:
                case CardDB.cardNameCN.自动防御矩阵:
                case CardDB.cardNameCN.崇高牺牲:
                case CardDB.cardNameCN.误导:
                case CardDB.cardNameCN.忏悔:
                case CardDB.cardNameCN.捕鼠陷阱:
                case CardDB.cardNameCN.叛变:
                case CardDB.cardNameCN.暗影克隆:
                case CardDB.cardNameCN.诈死:
                case CardDB.cardNameCN.军备宝箱:
                case CardDB.cardNameCN.眼镜蛇陷阱:
                case CardDB.cardNameCN.游荡怪物:
                case CardDB.cardNameCN.捕熊陷阱:
                    return 5;
                case CardDB.cardNameCN.抄袭:
                case CardDB.cardNameCN.闪避:
                    return 3;

            }
            return 0;
        }


        public override float getPlayfieldValue(Playfield p)
        {
            if (p.value > -200000) return p.value;
            //p.value的初始值是Int32.MinValue，也就是-2147483648
            //意思就是防止多次运算，这个场面的val如果大于-2000000(已经计算过val)则直接返回  Todo:待Fix 不应该出现这种情况
            //if (p.value >= -2000000) return p.value; 有些动作序列不同的相同场面，得分是不同的
            int retval = 0;
            int hpboarder = 10; //已方血量临界值
            int aggroboarder = 11; //敌方血量临界值
            retval += p.ownMaxMana * 20 - p.enemyMaxMana * 20;  // 水晶价值
            retval += (p.enemyHeroAblility.manacost - p.ownHeroAblility.manacost) * 4; //双方英雄技能耗费费用价值
            if (p.ownHeroPowerAllowedQuantity != p.enemyHeroPowerAllowedQuantity) //双方英雄技能可以使用次数价值
            {
                if (p.ownHeroPowerAllowedQuantity > p.enemyHeroPowerAllowedQuantity) retval += 3;
                else retval -= 3;
            }

            retval += p.ownSecretsIDList.Count * 14 - p.enemySecretCount * 40;
            // 奥秘数量价值，不对称是因为前者可以通过自己出奥秘牌，后者需要干掉敌方奥秘
            // 14: case: test_fireball.txt  455 vs 322游客+1个奥秘 得分12是均衡态

            //敌方法强价值
            if (p.enemyHeroName == HeroEnum.mage || p.enemyHeroName == HeroEnum.druid) retval -= 2 * p.enemyspellpower;

            if (p.ownHero.Hp + p.ownHero.armor > hpboarder)
            {
                retval += 2 * (p.ownHero.Hp + p.ownHero.armor);
            }
            else  // 血量低于临界值要惩罚，如果下回合能赢，惩罚和临界值的差距，否则惩罚平方
            {
                if (p.nextTurnWin()) retval -= (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
                else retval -= 2 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
            }

            if (p.enemyHero.Hp + p.enemyHero.armor > aggroboarder)
            {
                retval -= 2 * (p.enemyHero.Hp + p.enemyHero.armor); //  是正常的2倍，更强调打脸
            }
            else
            {
                retval += 5 * (aggroboarder + 1 - p.enemyHero.Hp - p.enemyHero.armor);  // 敌方快挂了，加分
            }

            //if (p.ownWeapon.Angr > 0) 奥秘法没有进攻武器
            //{
            //    if (p.ownWeapon.Angr > 1) retval += p.ownWeapon.Angr * p.ownWeapon.Durability;
            //    else retval += p.ownWeapon.Angr * p.ownWeapon.Durability + 1;
            //}

            if (!p.enemyHero.frozen)
            {
                retval -= p.enemyWeapon.Durability * p.enemyWeapon.Angr;
            }
            else
            {
                if (p.enemyWeapon.Durability >= 1)
                {
                    retval += 12; //敌方英雄被冻的价值
                }
            }

            //RR card draw value depending on the turn and distance to lethal
            //RR if lethal is close, carddraw value is increased
            if (p.lethalMissing() <= 5) //RR
            {
                retval += p.owncarddraw * 100; // 如果对面差5以内斩杀，显著提高抽牌价值
            }
            if (p.ownMaxMana < 4)  //早期回合 3费以内抽一张牌+2价值
            {
                retval += p.owncarddraw * 2;
            }
            else  //后期回合，抽牌价值*5
            {
                retval += p.owncarddraw * 5;
            }

            //奥秘法不会帮助对方抽牌
            //if (p.owncarddraw + 1 >= p.enemycarddraw) retval -= p.enemycarddraw * 7;
            //else retval -= (p.owncarddraw + 1) * 7 + (p.enemycarddraw - p.owncarddraw - 1) * 12;

            //int owntaunt = 0;
            int readycount = 0;
            int ownMinionsCount = 0;
            foreach (Minion m in p.ownMinions)
            {
                retval += 5;
                retval += m.Hp * 2;
                retval += m.Angr * 2;
                //retval += m.handcard.card.rarity;
                //if (!m.playedThisTurn && m.windfury) retval += m.Angr; 奥秘法没有风怒随从
                //if (m.divineshild) retval += 1; 没有圣盾
                //if (m.stealth) retval += 1; 没有潜行
                //if (m.handcard.card.isSpecialMinion && !m.silenced)  奥秘法已方没有特殊要照顾的随从
                //{
                //    retval += 1;
                //    if (!m.taunt && m.stealth) retval += 20;
                //}
                //else
                //{
                //    if (m.Angr <= 2 && m.Hp <= 2 && !m.divineshild) retval -= 5;
                //}
                //if (!m.taunt && m.stealth && penman.specialMinions.ContainsKey(m.name)) retval += 20;
                //if (m.poisonous) retval += 1; //没有剧毒
                //if (m.lifesteal) retval += m.Angr/2; //没有吸血
                //if (m.divineshild && m.taunt) retval += 4; //没有圣盾
                //if (m.taunt && m.handcard.card.name == CardDB.cardName.frog) owntaunt++; //没有嘲讽
                //if (m.handcard.card.isToken && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
                //if (!penman.specialMinions.ContainsKey(m.name) && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
                //if (p.ownMinions.Count > 2 && (m.handcard.card.name == CardDB.cardName.direwolfalpha  给类似恐狼先锋这种加攻生物加价值
                //|| m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.stormwindchampion || m.handcard.card.name == CardDB.cardName.raidleader)) retval += 10;
                //if (m.handcard.card.name == CardDB.cardName.bloodmagethalnos) retval += 10; 血法师额外赋值，没带
                //if (m.handcard.card.name == CardDB.cardName.nerubianegg) //蛛魔之卵
                //{
                //    if (m.Angr >= 1) retval += 2;
                //    if ((!m.taunt && m.Angr == 0) && (m.divineshild || m.maxHp > 2)) retval -= 10;
                //}
                if (m.Ready) readycount++;
                if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
                //retval += m.synergy; //种族和职业匹配度
            }
            //retval += p.anzOgOwnCThunAngrBonus; //克苏恩价值
            //retval += p.anzOwnExtraAngrHp - p.anzEnemyExtraAngrHp; //加给手牌的buff

            /*if (p.enemyMinions.Count >= 0)
            {
                int anz = p.enemyMinions.Count;
                if (owntaunt == 0) retval -= 10 * anz;
                retval += owntaunt * 10 - 11 * anz;
            }*/
            bool useAbili = false; //是否使用过英雄技能
            int usecoin = 0;  //是否使用过硬币
            int count = p.playactions.Count;
            int ownActCount = 0; //已方动作数量
            // 排序问题
            for (int i = 0; i < count; i++)
            {
                Action a = p.playactions[i];
                ownActCount++;
                switch (a.actionType)
                {
                    // 英雄攻击
                    case actionEnum.attackWithHero:
                        continue; //继续for循环
                    case actionEnum.useHeroPower:
                        useAbili = true;
                        continue;
                    case actionEnum.playcard:
                        break;  //跳出去，处理打牌场景
                    default:
                        continue;
                }
                // retval -= getComboPenality(a.card.card, a.target, p); 
                // 不能这么做，因为这里动作已经做完了，还是要在动作做之前打分评估，否则比如不知道对方头上多少奥秘
                // 场面因为我的动作发生了什么变化

                if (i == 0) // 第一个动作
                {
                    switch (a.card.card.nameEN)
                    {
                        case CardDB.cardNameEN.arcaneflakmage: //对空奥术法师
                            if (p.enemySecretCount > 0 && p.enemyHeroName == HeroEnum.mage)
                                retval -= 1;  // 扣1分，别扣太多，别不出对空奥术了，稍微防下爆炸符文
                            break;
                        case CardDB.cardNameEN.madscientist: //疯狂的科学家
                            if (p.enemySecretCount > 0 && p.enemyHeroName == HeroEnum.mage)
                                retval += 1;  // 给1分，也许可以干掉爆炸符文  
                            break;
                        case CardDB.cardNameEN.eaterofsecrets: //奥秘吞噬者 一定要第一个出
                            retval += 300; // 注意，牌面打分时，动作已经做完，所以不能限制对方头上有奥秘
                            break;
                    }
                }
                else if (i == count - 1) // 最后打出的牌
                {
                    switch (a.card.card.nameEN)
                    {
                        case CardDB.cardNameEN.thecoin:// CardDB.cardNameCN.幸运币
                            retval -= 10;
                            break;
                    }
                }
                switch (a.card.card.nameEN)
                {
                    case CardDB.cardNameEN.thecoin: // 幸运币
                        usecoin++;
                        break;
                }
                if (a.target == null) continue;
            }
            retval -= p.evaluatePenality; //每个动作的价值存在这里面（执行前评估），去重会用到这个值来选择同类场景（因为牌序不同得分不同），一些sim里也会复制
            //这行要放在较后的位置，因为牌序还会更新这个值

            if (usecoin > 0)
            {
                if (useAbili && p.ownMaxMana <= 2) retval -= 40; // 不要在2费之前用硬币用英雄技能
                retval -= 5 * p.manaTurnEnd; //不要浪费硬币，还有剩余法力值没必要用硬币
                if (p.manaTurnEnd + usecoin > 10) retval -= 5 * usecoin; //不浪费硬币
            }
            if (p.manaTurnEnd >= p.ownHeroAblility.manacost && !useAbili && p.ownAbilityReady) ///费用充足，不要不用技能就结束回合
            {
                retval -= 10;
            }

            int mobsInHand = 0; //手牌中随从数量
            int bigMobsInHand = 0;//手牌中攻击力大于3的随从数量
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    mobsInHand++;
                    if (hc.card.Attack + hc.addattack >= 3) bigMobsInHand++;
                    retval += hc.addattack + hc.addHp; //  手牌被buff的情况
                }
            }

            if (ownMinionsCount - p.enemyMinions.Count >= 4 && bigMobsInHand >= 1)
            {
                retval += bigMobsInHand * 25;  //已方场面优势，留一个大随从在手 有奖励：注意 这类综合衡量牌面的，放这里编写策略
            }


            //bool hasTank = false;
            foreach (Minion m in p.enemyMinions)
            {
                retval -= this.getEnemyMinionValue(m, p);
                //hasTank = hasTank || m.taunt;
            }
            retval -= p.enemyMinions.Count * 2;
            /*foreach (SecretItem si in p.enemySecretList)
            {
                if (readycount >= 1 && !hasTank && si.canbeTriggeredWithAttackingHero)
                {
                    retval -= 100;
                }
                if (readycount >= 1 && p.enemyMinions.Count >= 1 && si.canbeTriggeredWithAttackingMinion)
                {
                    retval -= 100;
                }
                if (si.canbeTriggeredWithPlayingMinion && mobsInHand >= 1)
                {
                    retval -= 25;
                }
            }*/

            retval -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2 避免浪费伤害
            //retval -= p.lostWeaponDamage; //避免乱切刀浪费

            retval -= getSecretPenality(p); // 奥秘的影响

            if (p.enemyHero.Hp <= 0) //斩杀价值
            {
                retval += 10000;
                if (retval < 10000) retval = 10000;
            }

            if (p.enemyHero.Hp >= 1 && p.guessingHeroHP <= 0) // 被斩杀
            {
                if (p.turnCounter < 2) retval += p.owncarddraw * 100;
                retval -= 1000;
            }
            if (p.ownHero.Hp <= 0) retval -= 10000;  //被斩杀

            p.value = retval;
            return retval;
        }



        public override int getEnemyMinionValue(Minion m, Playfield p)
        {
            int retval = 5; //初始值
            if (m.Angr == 0)
                retval = -2; //0攻怪，没有初始5分，负分开始
            else if (m.Angr <= 2 && !m.divineshild && !m.taunt) //进攻小，且非圣盾 非嘲讽
            {
                if (m.Hp <= 3)
                    retval = 0;  // <=23身材的没有初始5分，以防火焰总点23变22拿奖励
            }

            retval += m.Hp * 2;
            if (!m.frozen && !(m.cantAttack && m.name != CardDB.cardNameEN.argentwatchman))
            {
                retval += m.Angr * 2;
                if (m.windfury) retval += m.Angr * 2;
                if (m.Angr >= 5) retval += 10;  //改成攻击力为5，一些奥秘法对手的 44不用着急解
                if (m.Angr >= 7) retval += 50;
            }

            if (m.taunt) retval += 5; //嘲讽
            if (m.divineshild) retval += m.Angr; //圣盾
            if (m.divineshild && m.taunt) retval += 5; //圣盾且嘲讽
            if (m.stealth) retval += 1; //潜行

            if (m.poisonous) //剧毒
            {
                retval += 4;
                if (p.ownMinions.Count < p.enemyMinions.Count) retval += 10;
            }
            if (m.lifesteal) retval += m.Angr; //吸血

            if (m.handcard.card.targetPriority >= 1 && !m.silenced)
            {
                retval += m.handcard.card.targetPriority; //目标优先级，也在penman字典里面
            }
            if (m.name == CardDB.cardNameEN.nerubianegg && m.Angr <= 3 && !m.taunt) retval = 0; //蛛魔之卵

            switch (m.handcard.card.nameCN)
            {
				case CardDB.cardNameCN.安保自动机:
				case CardDB.cardNameCN.机械跃迁者:
				case CardDB.cardNameCN.健谈的调酒师:
				case CardDB.cardNameCN.豪宅管家俄里翁:
				case CardDB.cardNameCN.盛装歌手:
				case CardDB.cardNameCN.盗版之王托尼:
                    retval += 100;
                    break;
			}
            //这段用于处理对局中地方关键随从赋权
            switch (m.handcard.card.nameEN)
            {
                // 解不掉游戏结束
                case CardDB.cardNameEN.flamewaker: // CardDB.cardNameCN.火妖
                    retval += 100;
                    break;
                case CardDB.cardNameEN.blademastersamuro: // 钻石卡 剑圣萨穆罗
                    retval += 40;
                    break;
                //不解巨大劣势
                case CardDB.cardNameEN.tamsinroame: // CardDB.cardNameCN.塔姆辛罗姆
                case CardDB.cardNameEN.inarastormcrash: // CardDB.cardNameCN.伊纳拉碎雷
                case CardDB.cardNameEN.shadowjewelerhanar: // CardDB.cardNameCN.暗影珠宝师汉纳尔
                case CardDB.cardNameEN.overlordrunthak: // CardDB.cardNameCN.伦萨克大王
                case CardDB.cardNameEN.rokara: // CardDB.cardNameCN.洛卡拉
                case CardDB.cardNameEN.firemancerflurgl: // CardDB.cardNameCN.火焰术士弗洛格尔 鱼人萨
                case CardDB.cardNameEN.brannbronzebeard: // CardDB.cardNameCN.布莱恩铜须
                case CardDB.cardNameEN.stargazerluna: // CardDB.cardNameCN.观星者露娜   火妖
                case CardDB.cardNameEN.archmagevargoth:// CardDB.cardNameCN.大法师瓦格斯  大哥牧
                case CardDB.cardNameEN.underbellyangler:// CardDB.cardNameCN.下水道渔人  鱼人萨
                case CardDB.cardNameEN.arcaneflakmage: // CardDB.cardNameCN.对空奥术法师: 奥秘法
                case CardDB.cardNameEN.skybarge: // CardDB.cardNameCN.空中炮艇:
                case CardDB.cardNameEN.shipscannon: //CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameEN.sorcerersapprentice://CardDB.cardNameCN.巫师学徒:
                case CardDB.cardNameEN.etcgodofmetal://CardDB.cardNameCN.精英牛头人酋长金属之神:
                case CardDB.cardNameEN.paradeleader://CardDB.cardNameCN.巡游领队:
                case CardDB.cardNameEN.playmaker://CardDB.cardNameCN.团伙核心:
                case CardDB.cardNameEN.carielroame://CardDB.cardNameCN.凯瑞尔罗姆:
                case CardDB.cardNameEN.murlocwarleader://CardDB.cardNameCN.鱼人领军:
                case CardDB.cardNameEN.southseacaptain://CardDB.cardNameCN.南海船长:
                case CardDB.cardNameEN.doomsayer://CardDB.cardNameCN.末日预言者:
                case CardDB.cardNameEN.kanrethadebonlocke://CardDB.cardNameCN.坎雷萨德埃伯洛克:
                case CardDB.cardNameEN.dollmasterdorian://CardDB.cardNameCN.人偶大师多里安:
                case CardDB.cardNameEN.lushwaterscout://CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameEN.grimscaleoracle://CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameEN.dragonbane://CardDB.cardNameCN.灭龙弩炮:
                case CardDB.cardNameEN.cloakedhuntress://CardDB.cardNameCN.神秘女猎手:
                case CardDB.cardNameEN.eviltotem://CardDB.cardNameCN.怪盗图腾:
                case CardDB.cardNameEN.dwarvensharpshooter://CardDB.cardNameCN.矮人神射手:
                case CardDB.cardNameEN.questingadventurer://CardDB.cardNameCN.任务达人:
                case CardDB.cardNameEN.voraciousreader://CardDB.cardNameCN.贪婪的书虫:
                case CardDB.cardNameEN.phasestalker://CardDB.cardNameCN.相位追猎者:
                case CardDB.cardNameEN.tinyfinscaravan://CardDB.cardNameCN.鱼人宝宝车队:
                case CardDB.cardNameEN.kodorider://CardDB.cardNameCN.科多兽骑手:
                case CardDB.cardNameEN.archmageantonidas://CardDB.cardNameCN.大法师安东尼达斯:
                    retval += 30;
                    break;
                // 算有点用
                case CardDB.cardNameEN.secretkeeper://CardDB.cardNameCN.奥秘守护者:
                case CardDB.cardNameEN.warhorsetrainer://CardDB.cardNameCN.战马训练师:
                case CardDB.cardNameEN.kolkarpackrunner://CardDB.cardNameCN.科卡尔驯犬者:
                case CardDB.cardNameEN.sharkfinfan://CardDB.cardNameCN.鲨鳍后援:
                case CardDB.cardNameEN.starvingbuzzard://CardDB.cardNameCN.饥饿的秃鹫:
                case CardDB.cardNameEN.manawyrm://CardDB.cardNameCN.法力浮龙:
                case CardDB.cardNameEN.gadgetzanauctioneer://CardDB.cardNameCN.加基森拍卖师:
                case CardDB.cardNameEN.knifejuggler://CardDB.cardNameCN.飞刀杂耍者:
                    retval += 10;
                    break;
                // 带点异能随手解一下吧
                case CardDB.cardNameEN.lowlysquire://CardDB.cardNameCN.低阶侍从:
                case CardDB.cardNameEN.buccaneer://CardDB.cardNameCN.锈水海盗:
                    retval += 3;
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

        private Dictionary<CardDB.cardNameEN, int> SirFinleyPriorityList = new Dictionary<CardDB.cardNameEN, int>
        {
            //{HeroPowerName, Priority}, where 0-9 = manual priority
            { CardDB.cardNameEN.lesserheal, 0 },
            { CardDB.cardNameEN.shapeshift, 6 },
            { CardDB.cardNameEN.fireblast, 0 },
            { CardDB.cardNameEN.totemiccall, 1 },
            { CardDB.cardNameEN.lifetap, 9 },
            { CardDB.cardNameEN.daggermastery, 5 },
            { CardDB.cardNameEN.reinforce, 4 },
            { CardDB.cardNameEN.armorup, 2 },
            { CardDB.cardNameEN.steadyshot, 8 }
        };
    }


}