using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

//cardids of duplicate + avenge
//nozdormu (for computing time :D)
//faehrtenlesen (tracking)
// lehrensucher cho
//scharmuetzel kills all :D
namespace HREngine.Bots
{
    public enum actionEnum
    {
        endturn = 0, //结束此回合
        playcard, //出一张卡牌，出的牌在card中
        attackWithHero, //英雄进行攻击，target作为目标，own通常是我方英雄
        useHeroPower, //使用英雄技能，target作为目标 (英雄技能无攻击目标的英雄则为null，如Mage)
        attackWithMinion, //随从进行攻击，target作为目标，own是用于攻击的随从
        trade, //交易
        useLocation, //使用地标
        useTitanAbility, //使用泰坦技能
        forge, //锻造
    }
    
    public class Action
    {
        // 用于记录操作的类型。
        public actionEnum actionType; 
        // 记录出的牌，在attackWithHero、attackWithMinion、useLocation操作中为null
        public Handmanager.Handcard card; 
        // 用于记录位置，如actionType为playcard时就会有内容，
        // 比如出一个随从就会标记出放置的位置（最左边为0），
        // 如果是出一张法术牌的话，具体内容没有研究，应该是有赋值的
        public int place;
        // 我方随从（英雄）
        public Minion own;
        // 攻击目标
        public Minion target;
        // 用于抉择牌选择
        // 如果是未修改过的兄弟，抉择牌的编号分别是（0：中间 1：左边 2：右边）
        public int druidchoice;
        // 惩罚值，对于这个操作给出多少的惩罚
        // 值越大越不推荐这样下，如果值为负数则是非常推荐。
        public int penalty;
        // 记录的应该是此回合中的第几步操作，初始值为-1 (不确定)
        public int turn = -1;
        // 我方随从（英雄）的血量
        public int prevHpOwn = -1;
        // 攻击目标的血量
        public int prevHpTarget = -1;
        // 使用泰坦技能的编号
        public int titanAbilityNO;

        public Action(actionEnum type, Handmanager.Handcard hc, Minion ownM, int place, Minion targetM, int pen, int choice)
        {
            this.actionType = type;
            this.card = hc;
            this.own = ownM;
            this.place = place;
            this.target = targetM;
            this.penalty = pen;
            this.druidchoice = choice;
            if (ownM != null) prevHpOwn = ownM.Hp;
            if (targetM != null) prevHpTarget = targetM.Hp;
        }

        public Action(actionEnum type, Handmanager.Handcard hc, Minion ownM, int place, Minion targetM, int pen, int choice, int abilityNO)
        {
            this.actionType = type;
            this.card = hc;
            this.own = ownM;
            this.place = place;
            this.target = targetM;
            this.penalty = pen;
            this.druidchoice = choice;
            if (ownM != null) prevHpOwn = ownM.Hp;
            if (targetM != null) prevHpTarget = targetM.Hp;
            this.titanAbilityNO = abilityNO;
        }
        
        public Action(Action a)
        {
            this.actionType = a.actionType;
            this.card = a.card;
            this.place = a.place;
            this.own = a.own;
            this.target = a.target;
            this.druidchoice = a.druidchoice;
            this.penalty = a.penalty;
            this.prevHpOwn = a.prevHpOwn;
            this.prevHpTarget = a.prevHpTarget;
            this.titanAbilityNO = a.titanAbilityNO;
        }

        /// <summary>
        /// 打印当前动作的详细信息到日志或缓冲区。
        /// </summary>
        /// <param name="tobuffer">是否将输出写入缓冲区（true）或直接输出到日志（false）。</param>
        public void print(bool tobuffer = false)
        {
            // 判断是否需要打印下一步动作，且该动作存在惩罚值
            if (printUtils.printNextMove && this.penalty != 0 && !(this.penalty == -printUtils.enfaceReward && this.actionType == actionEnum.attackWithMinion))
            {
                StringBuilder str = new StringBuilder("", 100);

                // 根据动作类型拼接相应的描述信息
                switch (this.actionType)
                {
                    case actionEnum.endturn:
                        str.Append("回合结束");
                        break;
                    case actionEnum.playcard:
                        str.Append("打出 " + (this.card != null && this.card.card != null ? this.card.card.chnInfo() : "无"));
                        str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                        break;
                    case actionEnum.attackWithHero:
                        str.Append("让英雄攻击 " + (this.target != null ? this.target.info() : "空"));
                        break;
                    case actionEnum.useHeroPower:
                        str.Append("使用英雄技能");
                        str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                        break;
                    case actionEnum.attackWithMinion:
                        str.Append("使用随从 " + this.own.info());
                        str.Append(" 攻击 " + (this.target != null ? this.target.info() : "空"));
                        break;
                    case actionEnum.trade:
                        str.Append("使用随从 " + this.own.info() + " 交易");
                        break;
                    case actionEnum.useLocation:
                        str.Append("使用地标 " + this.own.info());
                        str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                        break;
                    case actionEnum.useTitanAbility:
                        str.Append("使用泰坦技能 ");

                        string suffix = "";
                        switch (this.own.handcard.card.cardIDenum.ToString())
                        {
                            case "TTN_092":
                            case "TTN_858":
                            case "TTN_862":
                                if (this.titanAbilityNO == 1) suffix = "t1";
                                else if (this.titanAbilityNO == 2) suffix = "t2";
                                else if (this.titanAbilityNO == 3) suffix = "t3";
                                break;

                            case "TTN_075":
                            case "TTN_800":
                            case "TTN_415":
                            case "TTN_429":
                            case "YOG_516":
                            case "TTN_903":
                            case "TTN_721":
                                if (this.titanAbilityNO == 1) suffix = "t";
                                else if (this.titanAbilityNO == 2) suffix = "t2";
                                else if (this.titanAbilityNO == 3) suffix = "t3";
                                break;

                            case "TTN_737":
                                if (this.titanAbilityNO == 1) suffix = "t";
                                else if (this.titanAbilityNO == 2) suffix = "t1";
                                else if (this.titanAbilityNO == 3) suffix = "t3";
                                break;

                            case "TTN_960":
                                if (this.titanAbilityNO == 1) suffix = "t2";
                                else if (this.titanAbilityNO == 2) suffix = "t3";
                                else if (this.titanAbilityNO == 3) suffix = "t4";
                                break;

                            default:
                                if (this.titanAbilityNO == 1) suffix = "t1";
                                else if (this.titanAbilityNO == 2) suffix = "t2";
                                else if (this.titanAbilityNO == 3) suffix = "t3";
                                break;
                        }
                        CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(this.own.handcard.card.cardIDenum.ToString() + suffix));
                        str.Append(card.nameCN.ToString());
                        str.Append(" 目标 " + (this.target != null && this.target.handcard != null ? this.target.handcard.card.nameCN.ToString() : "无"));
                        break;
                    case actionEnum.forge:
                        str.Append("使用随从 " + this.own.info() + " 锻造");
                        break;
                }
                str.Append("，当前受到 ").Append(this.penalty).Append(" 点惩罚！");
                Helpfunctions.Instance.ErrorLog(str.ToString());
            }

            Helpfunctions help = Helpfunctions.Instance;

            // 如果是将输出写入缓冲区
            if (tobuffer)
            {
                string actionStr = this.GetActionString();
                help.writeToBuffer(actionStr);
                return;
            }

            // 直接输出到日志
            string logStr = this.GetActionString();
            help.logg(logStr);
        }

        /// <summary>
        /// 根据动作类型获取动作的描述字符串。
        /// </summary>
        /// <returns>动作描述字符串。</returns>
        private string GetActionString()
        {
            StringBuilder str = new StringBuilder("", 100);

            switch (this.actionType)
            {
                case actionEnum.endturn:
                    str.Append("回合结束");
                    break;
                case actionEnum.playcard:
                    str.Append("打出 " + (this.card != null && this.card.card != null ? this.card.card.chnInfo() : "无"));
                    str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                    break;
                case actionEnum.attackWithHero:
                    str.Append("让英雄攻击 " + (this.target != null ? this.target.info() : "空"));
                    break;
                case actionEnum.useHeroPower:
                    str.Append("使用英雄技能");
                    str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                    break;
                case actionEnum.attackWithMinion:
                    str.Append("使用随从 " + this.own.info());
                    str.Append(" 攻击 " + (this.target != null ? this.target.info() : "空"));
                    break;
                case actionEnum.trade:
                    str.Append("使用随从 " + this.own.info() + " 交易");
                    break;
                case actionEnum.useLocation:
                    str.Append("使用地标 " + this.own.info());
                    str.Append(" 目标 " + (this.target != null ? this.target.info() : "空"));
                    break;
                case actionEnum.useTitanAbility:
                    str.Append("使用泰坦技能 ");

                    string suffix = "";
                    switch (this.own.handcard.card.cardIDenum.ToString())
                    {
                        case "TTN_092":
                        case "TTN_858":
                        case "TTN_862":
                            if (this.titanAbilityNO == 1) suffix = "t1";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_075":
                        case "TTN_800":
                        case "TTN_415":
                        case "TTN_429":
                        case "YOG_516":
                        case "TTN_903":
                        case "TTN_721":
                            if (this.titanAbilityNO == 1) suffix = "t";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_737":
                            if (this.titanAbilityNO == 1) suffix = "t";
                            else if (this.titanAbilityNO == 2) suffix = "t1";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_960":
                            if (this.titanAbilityNO == 1) suffix = "t2";
                            else if (this.titanAbilityNO == 2) suffix = "t3";
                            else if (this.titanAbilityNO == 3) suffix = "t4";
                            break;

                        default:
                            if (this.titanAbilityNO == 1) suffix = "t1";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;
                    }
                    CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(this.own.handcard.card.cardIDenum.ToString() + suffix));
                    str.Append(card.nameCN.ToString());
                    str.Append(" 目标 " + (this.target != null && this.target.handcard != null ? this.target.handcard.card.nameCN.ToString() : "无"));
                    break;
                case actionEnum.forge:
                    str.Append("使用随从 " + this.own.info() + " 锻造");
                    break;
            }
            return str.ToString();
        }

        /// <summary>
        /// 根据当前动作生成详细的字符串描述信息。
        /// </summary>
        /// <returns>返回描述当前动作的字符串。</returns>
        public string printString()
        {
            StringBuilder retval = new StringBuilder(); // 使用StringBuilder提高字符串拼接效率

            // 根据动作类型拼接相应的描述信息
            switch (this.actionType)
            {
                case actionEnum.playcard:
                    retval.Append("打出 ").Append(this.card != null && this.card.card != null ? this.card.card.chnInfo() : "无");
                    if (this.target != null)
                    {
                        retval.Append(" 目标 ").Append(this.target != null ? this.target.info() : "空");
                    }
                    if (this.place >= 0)
                    {
                        retval.Append(" 位置 ").Append(this.place);
                    }
                    if (this.druidchoice >= 1)
                    {
                        retval.Append(" 抉择 ").Append(this.druidchoice);
                    }
                    break;

                case actionEnum.attackWithMinion:
                    retval.Append("使用随从 ").Append(this.own.info())
                          .Append(" 攻击 ").Append(this.target != null ? this.target.info() : "空");
                    break;

                case actionEnum.attackWithHero:
                    retval.Append("让英雄攻击 ").Append(this.target != null ? this.target.info() : "空");
                    break;

                case actionEnum.useHeroPower:
                    retval.Append("使用英雄技能 ");
                    if (this.target != null)
                    {
                        retval.Append(" 目标 ").Append(this.target != null ? this.target.info() : "空");
                    }
                    break;

                case actionEnum.trade:
                    retval.Append("使用随从 " + this.own.info() + " 交易");
                    break;

                case actionEnum.useLocation:
                    retval.Append("使用地标 ").Append(this.own.info());
                    if (this.target != null)
                    {
                        retval.Append(" 目标 ").Append(this.target != null ? this.target.info() : "空");
                    }
                    break;

                case actionEnum.useTitanAbility:
                    retval.Append("使用泰坦技能 ");

                    string suffix = "";
                    switch (this.own.handcard.card.cardIDenum.ToString())
                    {
                        case "TTN_092":
                        case "TTN_858":
                        case "TTN_862":
                            if (this.titanAbilityNO == 1) suffix = "t1";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_075":
                        case "TTN_800":
                        case "TTN_415":
                        case "TTN_429":
                        case "YOG_516":
                        case "TTN_903":
                        case "TTN_721":
                            if (this.titanAbilityNO == 1) suffix = "t";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_737":
                            if (this.titanAbilityNO == 1) suffix = "t";
                            else if (this.titanAbilityNO == 2) suffix = "t1";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;

                        case "TTN_960":
                            if (this.titanAbilityNO == 1) suffix = "t2";
                            else if (this.titanAbilityNO == 2) suffix = "t3";
                            else if (this.titanAbilityNO == 3) suffix = "t4";
                            break;

                        default:
                            if (this.titanAbilityNO == 1) suffix = "t1";
                            else if (this.titanAbilityNO == 2) suffix = "t2";
                            else if (this.titanAbilityNO == 3) suffix = "t3";
                            break;
                    }
                    CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.Instance.cardIdstringToEnum(this.own.handcard.card.cardIDenum.ToString() + suffix));
                    retval.Append(card.nameCN.ToString());
                    retval.Append(" 目标 " + (this.target != null && this.target.handcard != null ? this.target.handcard.card.nameCN.ToString() : "无"));
                    break;

                case actionEnum.forge:
                    retval.Append("使用随从 " + this.own.info() + " 锻造");
                    break;

            }

            return retval.ToString();
        }


    }


}