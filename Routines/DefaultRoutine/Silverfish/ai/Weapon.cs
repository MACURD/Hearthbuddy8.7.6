using Triton.Game.Mapping;

namespace HREngine.Bots
{
    using System;
    using System.Collections.Generic;
    
    public class Weapon
    {
        public int pID = 0;
        public CardDB.cardNameEN name = CardDB.cardNameEN.unknown;
        public CardDB.Card card;
        public int numAttacksThisTurn = 0;
        public bool immuneWhileAttacking = false;
        
        public int Angr = 0;
        public int Durability = 0;
        
        public bool windfury = false;
        public bool immune = false;
        public bool lifesteal = false;
        public bool poisonous = false;
        public bool cantAttackHeroes = false;

        public int scriptNum1 = 0;

        // 荣耀击杀
        private bool _honorableKill = false;
        /// <summary>
        /// 荣耀击杀
        /// </summary>
        public bool HonorableKill
        {
            get { return _honorableKill; }
            set { _honorableKill = value; }
        }

        //超杀
        private bool _overkill = false;
        /// <summary>
        /// 超杀
        /// </summary>
        public bool Overkill
        {
            get { return _overkill; }
            set { _overkill = value; }
        }

        /// <summary>
        /// 法术迸发
        /// </summary>
        public bool Spellburst
        {
            get { return _spellburst; }
            set { _spellburst = value; }
        }
        private bool _spellburst = false;

        public Weapon()
        {
            this.card = CardDB.Instance.unknownCard;
        }

        public Weapon(Weapon w)
        {
            this.name = w.name;
            this.card = w.card;
            this.numAttacksThisTurn = w.numAttacksThisTurn;
            this.immuneWhileAttacking = w.immuneWhileAttacking;

            this.Angr = w.Angr;
            this.Durability = w.Durability;

            this.windfury = w.windfury;
            this.immune = w.immune;
            this.lifesteal = w.lifesteal;
            this.poisonous = w.poisonous;
            this.cantAttackHeroes = w.cantAttackHeroes;
            this.scriptNum1 = w.scriptNum1;
            this.Spellburst = w.Spellburst;//法术迸发
        }

        public bool isEqual(Weapon w)
        {
            if (this.Angr != w.Angr) return false;
            if (this.Durability != w.Durability) return false;
            if (this.poisonous != w.poisonous) return false;
            if (this.lifesteal != w.lifesteal) return false;
            if (this.name != w.name) return false;
            if (this.Spellburst != w.Spellburst) return false;//法术迸发
            if (this.scriptNum1 != w.scriptNum1) return false;
            return true;
        }

        public void equip(CardDB.Card c)
        {
            this.name = c.nameEN;
            this.card = c;
            this.numAttacksThisTurn = 0;
            this.immuneWhileAttacking = c.immuneWhileAttacking;

            this.Angr = c.Attack;
            this.Durability = c.Durability;

            this.windfury = c.windfury;
            this.immune = false;
            this.lifesteal = c.lifesteal;
            this.poisonous = c.poisonous;
            this.cantAttackHeroes = (c.nameEN == CardDB.cardNameEN.foolsbane) ? true : false;
            this.scriptNum1 = 0;
            this.Spellburst = c.Spellburst;//法术迸发
        }

        public string weaponToString()
        {
            return this.Angr + " " + this.Durability + " " + this.name + " " + this.card.cardIDenum + " " + (this.poisonous ? 1 : 0) + " " + (this.lifesteal ? 1 : 0) + " " + this.scriptNum1;
        }

        // 记录武器的附魔卡牌
        public List<miniEnch> enchants = new List<miniEnch>();
    }
}