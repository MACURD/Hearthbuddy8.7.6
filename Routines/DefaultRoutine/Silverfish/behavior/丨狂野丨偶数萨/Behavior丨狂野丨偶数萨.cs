using System.Collections.Generic;
using System;

namespace HREngine.Bots
{
    public partial class Behavior丨狂野丨偶数萨 : Behavior
    {
        private int bonus_enemy = 4;
        private int bonus_mine = 4;

        public override string BehaviorName() { return "丨狂野丨偶数萨"; }
        PenalityManager penman = PenalityManager.Instance;

        public override int getComboPenality(CardDB.Card card, Minion target, Playfield p, Handmanager.Handcard nowHandcard)
        {
            // 无法选中
            if (target != null && target.untouchable)
            {
                return 100000;
            }
            // 初始惩罚值
            int pen = 0;


            switch (card.nameCN)
            {
                case CardDB.cardNameCN.图腾之力:
                case CardDB.cardNameCN.图腾潮涌:
                // 检查是否有远古图腾在手牌中，如果有，则给予额外的评分。
                foreach (Handmanager.Handcard hhc in p.owncards)
                {
                   if (hhc.card.nameCN == CardDB.cardNameCN.远古图腾)
                   {
                        pen += 10;
                        break;
                   }
                }
                // 统计我方场上图腾的数量。
                int totemCount = 0;
                foreach (Minion m in p.ownMinions)
                {
                    if (m.handcard.card.race == CardDB.Race.TOTEM)
                {
                    totemCount += 1;
                }
                }
               // 根据场上图腾数量调整评分。
               if (totemCount > 2)
                    pen -= 20;
               else
                    pen += 20;
               break;
               case CardDB.cardNameCN.远古图腾:
               break;
               case CardDB.cardNameCN.即兴演奏:
               // 目标不为空或目标或是我方随从
               if (target == null || !target.own)
                    return 100000;
               // 统计敌方场上生命值小于等于1的随从数量，同时减去这些随从攻击力的平方值。
               int kill_num = 0;
               if (p.enemyMinions.Count > 0)
                    foreach (Minion mi in p.enemyMinions)
                    {
                        if (mi.Hp <= 1)                   
                        {
                        kill_num++;
                        pen -= mi.Angr * mi.Angr;
                        }
                    }
                // 如果满足特定条件，调整 kill_num 的值以影响对即将被消灭的敌方随从的评分。
                if (kill_num < 3 && p.enemyHeroName == HeroEnum.thief && p.enemyMaxMana < 3) 
                // 遇到贼开局留了即兴演奏3费之前应该再等他至少铺场2个怪才使用。
                    kill_num -= 3;
                // 统计我方场上生命值小于等于1的随从数量，同时增加这些随从攻击力的平方值。
                if (p.ownMinions.Count > 0)
                    foreach (Minion mi in p.ownMinions)
                    {
                        if (mi.Hp <= 1)                    
                        {
                        kill_num--;
                        pen += mi.Angr * mi.Angr; 
                        }
                    }
                // 根据 kill_num 的值调整 pen 的分数，并乘以一个固定的数值（5）。
                pen -= kill_num * 5;
                break;
                case CardDB.cardNameCN.可靠陪伴:
                if(target != null && target.handcard.card.race == CardDB.Race.TOTEM)
                    pen -= 20;
                break;
                case CardDB.cardNameCN.深海融合怪:
                if(target != null && target.handcard.card.race == CardDB.Race.TOTEM)
                    pen -= 10;
                break;
                case CardDB.cardNameCN.火舌图腾:
                break;
                case CardDB.cardNameCN.石雕凿刀:
                    if (p.ownWeapon.Durability > 0)
                        return 100000;
                    //1费先摇技能再2费提刀
                    if(p.anzUsedOwnHeroPower == 0) 
                        pen += 20;
                    pen -= 100;
                break;
                case CardDB.cardNameCN.连环爆裂:
                    if (target == null || target.own)
                        return 100000;
                break;
                case CardDB.cardNameCN.针岩图腾:
                break;
                case CardDB.cardNameCN.风怒:
                    if(target == null || !target.own || target.windfury)
                        return 100000;
                    int maxAttack = 0;
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.windfury)
                            continue;
                        if(m.Angr > maxAttack)
                            maxAttack = m.Angr;
                    }
                    if(maxAttack > 0 && target.Angr >= maxAttack)
                        pen -= 2 * bonus_mine;
                    else
                        pen += 100000;
                break;
                case CardDB.cardNameCN.驻锚图腾:
                break;
                case CardDB.cardNameCN.分裂战斧:
                    if (p.ownWeapon.Durability > 0)
                        return 100000;
                    if (p.ownMinions.Count > 2 && p.ownMinions.Count < 5)
                        pen -= 100;
                    if (p.ownMinions.Count < 2)
                        pen += 100;
                break;
                case CardDB.cardNameCN.衰变:
                    int allAttack = 0;
                    foreach (Minion m in p.enemyMinions)
                    {
                        allAttack += m.Angr;
                    }
                    if(allAttack >= 6)
                        pen -= 5 * bonus_mine;
                    else
                        pen += 10;
                break;
                case CardDB.cardNameCN.巨型图腾埃索尔:
                if (p.ownMinions.Count > 2  &&  p.enemyMinions.Count < 2)
                        pen -= 1000;
                break;
                case CardDB.cardNameCN.锻石师:
                    pen -= 2000;
                break;
                case CardDB.cardNameCN.吉恩格雷迈恩:
                break;
                case CardDB.cardNameCN.深渊魔物:
                case CardDB.cardNameCN.图腾巨像:
                    if (nowHandcard.getManaCost(p) <= 0) pen -= bonus_mine * 5;
                    if (nowHandcard.getManaCost(p) <= 2) pen -= bonus_mine * 3;
                    if (nowHandcard.getManaCost(p) >= 5) pen += bonus_mine * 5;
                break;
                case CardDB.cardNameCN.奇利亚斯豪华版3000型:
                    if (p.ownMinions.Count >= 3) pen -= 30;
                    if (nowHandcard.getManaCost(p) <= 0) pen -= 60;
                    if (nowHandcard.getManaCost(p) <= 1) pen -= 40;
                    if (nowHandcard.getManaCost(p) <= 3) pen -= 10;
                    if (nowHandcard.getManaCost(p) > 3) pen += 10;
                break;

                }
            return pen;
        }

        // 发现卡的价值
        public override int getDiscoverVal(CardDB.Card card, Playfield p)
        {
           switch (card.nameCN)
            {
                
                case CardDB.cardNameCN.图腾巨像:
                    return 200;
                case CardDB.cardNameCN.图腾巨怪:
                    return 150;
                case CardDB.cardNameCN.针岩图腾:
                    return 100;
                case CardDB.cardNameCN.巨型图腾埃索尔:
                    return 95;
                case CardDB.cardNameCN.法力之潮图腾:
                    return 90;
                case CardDB.cardNameCN.驻锚图腾:
                case CardDB.cardNameCN.火舌图腾:
                    return 80;
                case CardDB.cardNameCN.马戏团融合怪:
                    return 70;
                case CardDB.cardNameCN.图腾魔像:
                case CardDB.cardNameCN.毒蛇守卫:
                case CardDB.cardNameCN.立体声图腾:
                case CardDB.cardNameCN.梦魇融合怪:
                    return 60;
                case CardDB.cardNameCN.蛮鱼图腾:
                case CardDB.cardNameCN.点唱机图腾:
                case CardDB.cardNameCN.派对图腾:
                    return 50;
                case CardDB.cardNameCN.活力图腾:
                case CardDB.cardNameCN.戏法图腾:
                case CardDB.cardNameCN.怪盗图腾:
                case CardDB.cardNameCN.融合独奏团:
                    return 30;
                case CardDB.cardNameCN.罪碑图腾:
                case CardDB.cardNameCN.永恒融合怪:
                case CardDB.cardNameCN.错误产物:
                    return 20;

            }
			if (card.race == CardDB.Race.TOTEM)    //类型等于海盗
            {
                return 3;
            }
            return 0;
        }
        

        // 核心，场面值
        public override float getPlayfieldValue(Playfield p)
        {
            if (p.value > -200000) return p.value;
            float retval = 0;
            retval += getGeneralVal(p);
            retval += p.owncarddraw * 5;
            retval -= p.lostWeaponDamage;
            // 危险血线
            int hpboarder = 5;
            // 不考虑法强了
            if (p.enemyHeroName == HeroEnum.mage) retval += 2 * p.enemyspellpower;
            // 抢脸血线
            int aggroboarder = 20;
            retval += getHpValue(p, hpboarder, aggroboarder);
            // 出牌序列数量
            int count = p.playactions.Count;
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
                        continue;
                    case actionEnum.useHeroPower:
                    case actionEnum.playcard:
                        break;
                    default:
                        continue;
                }
                switch (a.card.card.nameCN)
                {
                    // 排序优先
                    case CardDB.cardNameCN.幸运币:
                        retval -= 6 * i;
                        break;
                    case CardDB.cardNameCN.锻石师:
                        retval -= 4 * i;
                        break;
                    case CardDB.cardNameCN.驻锚图腾:
                        retval -= 3 * i;
                        break;
                    case CardDB.cardNameCN.火舌图腾:
                        retval -= 2 * i;
                        break;
                }
            }
            // 对手基本随从交换模拟
            //retval += enemyTurnPen(p);
            retval -= p.lostDamage;
            retval += getSecretPenality(p); // 奥秘的影响
            retval -= p.enemyWeapon.Angr * 3 + p.enemyWeapon.Durability * 3;

            // 特殊：优势防亵渎
            if (retval > 50 && p.enemyHeroStartClass == TAG_CLASS.WARLOCK && p.enemyMinions.Count == 0 && p.ownMinions.Count > 2)
            {
                bool found = false;
                // 从 2 开始防，术士自带一堆 1
                for (int i = 1; i <= 10; i++)
                {
                    found = false;
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.Hp == i)
                        {
                            retval -= 2 * (i - 1);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        if (i == 1 ) retval += 10;
                        if (i == 2 ) retval += 10;
                        break;
                    }
                }
            }
            // 特殊处理一费留硬币等炮
            if (Hrtprozis.Instance.gTurn == 2 )
            {
                int flag = 0;
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.nameCN == CardDB.cardNameCN.驻锚图腾) flag |= 1;
                    if (hc.card.nameCN == CardDB.cardNameCN.幸运币) flag |= 2;
                }
                if (flag == 3) retval += 200;
            }
            //p.value = retval;
            return retval;
        }

        
        // 敌方随从价值 主要等于（HP + Angr） * 4 + 4
        public override int getEnemyMinionValue(Minion m, Playfield p)
        {
            bool dieNextTurn = false;
            foreach (Minion mm in p.enemyMinions)
            {
                if (mm.handcard.card.nameCN == CardDB.cardNameCN.末日预言者)
                {
                    dieNextTurn = true;
                    break;
                }
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            if (m.Hp <= 0) return 0;
            int retval = 4;
            if (m.Angr > 0 || p.enemyHeroStartClass == TAG_CLASS.PALADIN || p.enemyHeroStartClass == TAG_CLASS.PRIEST)
                retval += m.Hp * 4;
            retval += m.spellpower * 2;
            if (!m.frozen && (!m.cantAttack || m.handcard.card.nameCN == CardDB.cardNameCN.邪刃豹))
            {
                retval += m.Angr * 4;
                if (m.Angr > 5) retval += 10;
                if (m.windfury) retval += m.Angr * 2;
            }
            if (m.silenced) return retval;

            if (m.taunt) retval += 2;
            if (m.divineshild) retval += m.Angr * 2;
            if (m.divineshild && m.taunt) retval += 5;
            if (m.stealth) retval += 2;

            // 鱼人
            if (m.handcard.card.race == CardDB.Race.MURLOC) retval += bonus_enemy * 4;

            // 剧毒价值两点属性
            if (m.poisonous)
            {
                retval += 8;
            }
            if (m.lifesteal) retval += m.Angr * bonus_enemy * 4;

            int bonus = 4;
            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.巫师学徒:
                case CardDB.cardNameCN.肢体商贩:
                case CardDB.cardNameCN.巨型图腾埃索尔:
                case CardDB.cardNameCN.驻锚图腾:
                case CardDB.cardNameCN.刺豚拳手:
                case CardDB.cardNameCN.空中飞爪:
                case CardDB.cardNameCN.金翼巨龙:
                case CardDB.cardNameCN.安保自动机:
                case CardDB.cardNameCN.机械跃迁者:
                case CardDB.cardNameCN.火焰术士弗洛格尔:
                case CardDB.cardNameCN.对空奥术法师:
                case CardDB.cardNameCN.前沿哨所:
                case CardDB.cardNameCN.战场军官:
                case CardDB.cardNameCN.伯尔纳锤喙:
                case CardDB.cardNameCN.甜水鱼人斥候:
                case CardDB.cardNameCN.塔姆辛罗姆:
                case CardDB.cardNameCN.暗影珠宝师汉纳尔:
                case CardDB.cardNameCN.伦萨克大王:
                case CardDB.cardNameCN.布莱恩铜须:
                case CardDB.cardNameCN.观星者露娜:
                case CardDB.cardNameCN.大法师瓦格斯:
                case CardDB.cardNameCN.火妖:
                case CardDB.cardNameCN.下水道渔人:
                case CardDB.cardNameCN.空中炮艇:
                case CardDB.cardNameCN.船载火炮:
                case CardDB.cardNameCN.火舌图腾:
                case CardDB.cardNameCN.末日预言者:
                case CardDB.cardNameCN.莫尔杉哨所:
                case CardDB.cardNameCN.鱼人领军:
                case CardDB.cardNameCN.南海船长:
                case CardDB.cardNameCN.灭龙弩炮:
                case CardDB.cardNameCN.战马训练师:
                case CardDB.cardNameCN.加基森拍卖师:
                case CardDB.cardNameCN.健谈的调酒师:
                case CardDB.cardNameCN.豪宅管家俄里翁:
                case CardDB.cardNameCN.小鬼骑士:
                case CardDB.cardNameCN.针岩图腾:
                case CardDB.cardNameCN.伴唱机:
                case CardDB.cardNameCN.空气之怒图腾:
                case CardDB.cardNameCN.战场通灵师:
                case CardDB.cardNameCN.纸艺天使:
                case CardDB.cardNameCN.纳亚克海克森:
                case CardDB.cardNameCN.深岩之洲晶簇:
                    retval += 150;
                    break;
              
                // 不解巨大劣势
                case CardDB.cardNameCN.安娜科德拉:
                case CardDB.cardNameCN.农夫:
                case CardDB.cardNameCN.旗标骷髅:
                case CardDB.cardNameCN.尼鲁巴蛛网领主:
                case CardDB.cardNameCN.凯瑞尔罗姆:
                case CardDB.cardNameCN.暗鳞先知:
                case CardDB.cardNameCN.鲨鳍后援:
                case CardDB.cardNameCN.相位追猎者:
                case CardDB.cardNameCN.鱼人宝宝车队:
                case CardDB.cardNameCN.饥饿的秃鹫:
                case CardDB.cardNameCN.锈水海盗:
                case CardDB.cardNameCN.盛装歌手:
                case CardDB.cardNameCN.玛克扎尔的小鬼:
                case CardDB.cardNameCN.发明机器人:
                case CardDB.cardNameCN.宝藏经销商:
                case CardDB.cardNameCN.随船外科医师:
                case CardDB.cardNameCN.玩具船:
                    retval += 50;
                    break;
                // 算有点用
                case CardDB.cardNameCN.贪婪的书虫:
                case CardDB.cardNameCN.治疗图腾:
                case CardDB.cardNameCN.力量图腾:
                case CardDB.cardNameCN.神秘女猎手:
                case CardDB.cardNameCN.矮人神射手:
                case CardDB.cardNameCN.低阶侍从:
                case CardDB.cardNameCN.战斗邪犬:
                case CardDB.cardNameCN.法力浮龙:
                case CardDB.cardNameCN.飞刀杂耍者:
                    retval += 15;
                    break;
            }
            return retval;
        }

        // 我方随从价值，大致等于主要等于 （HP + Angr） * 4 + 4
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
            }
            if (m.destroyOnEnemyTurnEnd || m.destroyOnEnemyTurnStart || m.destroyOnOwnTurnEnd || m.destroyOnOwnTurnStart) dieNextTurn = true;
            if (dieNextTurn)
            {
                return -1;
            }
            if (m.Hp <= 0) return 0;
            int retval = 4;
            if (m.Hp <= 0) return 0;
            retval += m.Hp * 4;
            retval += m.Angr * 4;
            if (m.Hp <= 1) retval -= (m.Angr - 1) * 3;
            if (m.Angr <= 1)
            {
                if (m.Hp > 3)
                {
                    retval -= (m.Hp - 3) * 4;
                }
            }
            // 高攻低血是垃圾
            if (m.Angr > m.Hp + 4) retval -= (m.Angr - m.Hp) * 3;

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

            int bonus = 4;
            switch (m.handcard.card.nameCN)
            {
                case CardDB.cardNameCN.驻锚图腾:
                    retval += bonus_mine * 3;
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
        }

        private Dictionary<CardDB.cardNameEN, int> SirFinleyPriorityList = new Dictionary<CardDB.cardNameEN, int>
        {
            //{HeroPowerName, Priority}, where 0-9 = manual priority
            { CardDB.cardNameEN.lesserheal, 0 }, 
            { CardDB.cardNameEN.shapeshift, 6 },
            { CardDB.cardNameEN.fireblast, 7 },
            { CardDB.cardNameEN.totemiccall, 1 },
            { CardDB.cardNameEN.lifetap, 9 },
            { CardDB.cardNameEN.daggermastery, 5 },
            { CardDB.cardNameEN.reinforce, 4 },
            { CardDB.cardNameEN.armorup, 2 },
            { CardDB.cardNameEN.steadyshot, 8 }
        };


        public override int getHpValue(Playfield p, int hpboarder, int aggroboarder)
        {
            int offset_enemy = 0;
            int offset_mine = p.calEnemyTotalAngr() + Hrtprozis.Instance.enemyDirectDmg;

            int retval = 0;
            // 血线安全
            if (p.ownHero.Hp + p.ownHero.armor - offset_mine > hpboarder)
            {
                retval += (5 + p.ownHero.Hp + p.ownHero.armor - offset_mine - hpboarder) ;
            }
            // 快死了
            else if (p.ownHero.Hp + p.ownHero.armor - offset_mine > 0)
            {
                //if (p.nextTurnWin()) retval -= (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor);
                retval -= 4 * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor + offset_mine) * (hpboarder + 1 - p.ownHero.Hp - p.ownHero.armor + offset_mine);
            }
            else
            {
                retval -= 3 * (hpboarder + 1) * (hpboarder + 1) + 100;
            }
            if (p.ownHero.Hp + p.ownHero.armor - offset_mine < 6 && p.ownHero.Hp + p.ownHero.armor - offset_mine > 0)
            {
                retval -= 80 / (p.ownHero.Hp + p.ownHero.armor - offset_mine);
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
            // if (p.enemyHero.Hp + p.enemyHero.armor + offset_enemy <= 5 && p.enemyHero.Hp + p.enemyHero.armor + offset_enemy > 0)
            // {
            //     retval += 300 / (p.enemyHero.Hp + p.enemyHero.armor - offset_enemy);
            // }
            // 场攻+直伤大于对方生命，预计完成斩杀
            if (p.anzEnemyTaunt == 0 && p.calTotalAngr() + p.calDirectDmg(p.mana, false) >= p.enemyHero.Hp + p.enemyHero.armor)
            {
                retval += 2000;
            }
            // 下回合斩杀本回合打脸
            if (p.calDirectDmg(p.ownMaxMana + 1, false, true) >= p.enemyHero.Hp + p.enemyHero.armor)
            {
                retval += 100;
            }
            return retval;
        }


        /// <summary>
        /// 攻击触发的奥秘惩罚
        /// </summary>
        /// <param name="si"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public override int getSecretPen_CharIsAttacked(Playfield p, SecretItem si, Minion attacker, Minion defender)
        {
            if (attacker.isHero) return 0;
            int pen = 0;
            // 攻击的基本惩罚
            if (si.canBe_explosive && !defender.isHero)
            {
                pen -= 20;
                //pen += (attacker.Hp + attacker.Angr);
                foreach (SecretItem sii in p.enemySecretList)
                {
                    sii.canBe_explosive = false;
                }
            }
            return pen;
        }

    }
}
