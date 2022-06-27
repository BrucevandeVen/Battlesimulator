using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleSimulator
{
    class Speler
    {
        // Fields
        private string naam;
        private int hitpoints;
        private int armor;

        // Properties
        // { get; set; } werkt niet, health wordt niet juist weergegeven
        public string Naam
        {
            get { return naam; }
            private set { naam = value; }
        }

        public int Hitpoints
        {
            get { return hitpoints; }
            private set { hitpoints = value; }
        }

        public int Armor
        {
            get { return armor; }
            private set { armor = value; }
        }

        // ctors
        public Speler(int hitpoints) : this(hitpoints, "Unknown") // If no name is given, name = Unknown
        {
            this.Hitpoints = hitpoints;
            this.armor = 0;
        }

        public Speler(int hitpoints, string naam)
        {
            this.Naam = naam;
            this.Hitpoints = hitpoints;
            this.armor = 0;
        }

        // methodes
        public void TakeDamage(int damage)
        {
            if(this.armor < damage) // If damage > armor, hitpoints - (damage - armor) = hitpoints
            {
                damage -= this.armor;
                this.hitpoints -= damage;
                this.armor = 0;
            }
            if(this.armor >= damage) // If armor >= damage, damage can be taken of armor only
            {
                this.armor -= damage;
            }
            if(this.hitpoints < 1) // Hitpoints can't be less then 0 and player is dead when hitpoints = 0, so armor is also 0
            {
                this.hitpoints = 0;
                this.armor = 0;
            }
            if(this.armor < 1) // Armor can't be less than 0
            {
                this.armor = 0;
            }
        }

        public int DealDamage()
        {
            Random attackPower = new Random();
            return attackPower.Next(0, 31); // Returns random value 0 - 30
        }

        public int DrinkPotion()
        {
            Random healPower = new Random();
            return healPower.Next(10, 21); // Returns random value 10 - 20
        }

        public void Heal(int healPower)
        {
            this.hitpoints += healPower;

            if(this.hitpoints > 100) // Hitpoints can't be over 100
            {
                this.hitpoints = 100;
            }
        }

        public int GainArmor(int takenDamage)
        {
            if (this.hitpoints == 0)
            {
                return this.armor = 0;
            }
            if (this.hitpoints == 1 && this.armor == 0)
            {
                return this.armor += 50;                        // Does not stack
            }
            if (this.hitpoints == 13 || takenDamage == 13)
            {
                if (takenDamage <= 10)
                {
                    return this.armor += 23;                    // 13 + 10
                }
                if (takenDamage >= 20 && takenDamage <= 25)
                {
                    return this.armor += 18;                    // 13 + 5
                }
                else
                {
                    return this.armor += 13;                    
                }
            }
            if (this.hitpoints <= 30 && takenDamage <= 10)
            {
                if (this.hitpoints == 13)
                {
                    return this.armor += 23;                   // 13 + 10
                }
                else
                {
                    return this.armor += 10;
                }
            }
            if (takenDamage >= 20 && takenDamage <= 25)
            {
                if (this.hitpoints == 13)
                {
                    return this.armor += 18;                   // 13 + 5
                }
                else
                {
                    return this.armor += 5;
                }         
            }
            return 0;
        }
    }
}
