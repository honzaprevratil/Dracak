using Dracak.Classes.Items;
using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Creatures
{
    public class Player : Creature
    {
        public Player (int strngth = 4, int stamina = 4, int dextirity = 4, int inteligence = 4, int swiftness = 4, int statsPoints = 10)
            : base(new PrimaryStats(strngth, stamina, dextirity, inteligence, swiftness), new Inventory())
        {
            DefaultData(statsPoints);
        }
        public Player()
            : base(new PrimaryStats(4, 4, 4, 4, 4), new Inventory())
        {
            DefaultData();
        }
        private void DefaultData(int statsPoints = 10)
        {
            currentHunger = SetAndReturnMaxHunger();
            currentThirst = SetAndReturnMaxThirst();
            currentEnergy = SetAndReturnMaxEnergy();
            CurrentHealth = SetAndReturnMaxHealth();
            StatsPoints = statsPoints;
        }

        public int InLocationId { get; set; }
        public int StatsPoints { get; set; }

        /* H U N G E R */
        /* CURRENT HUNGER */
        private double currentHunger;
        public double CurrentHunger
        {
            get
            {
                return currentHunger;
            }
            set
            {
                currentHunger = value;
                if (currentHunger < 0)
                {
                    this.TakeDamage(currentHunger/3, false);
                }
                currentHunger = currentHunger > 0 ? value : 0;
                currentHunger = currentHunger < MaxHunger ? currentHunger : MaxHunger;
            }
        }
        /* MAX HUNGER*/
        private double maxHunger;
        public double MaxHunger
        {
            get
            {
                return maxHunger;
            }
        }
        private double SetAndReturnMaxHunger()
        {
            return maxHunger = PrimaryStats.Stamina;
        }

        /* T H I R S T */
        /* CURRENT THIRST */
        private double currentThirst;
        public double CurrentThirst
        {
            get
            {
                return currentThirst;
            }
            set
            {
                currentThirst = value;
                if (currentThirst < 0)
                {
                    this.TakeDamage(currentThirst/2, false);
                }
                currentThirst = currentThirst > 0 ? value : 0;
                currentThirst = currentThirst < MaxThirst ? currentThirst : MaxThirst;
            }
        }
        /* MAX THIRST*/
        private double maxThirst;
        public double MaxThirst
        {
            get
            {
                return maxThirst;
            }
        }
        private double SetAndReturnMaxThirst()
        {
            return maxThirst = 2 * PrimaryStats.Stamina / 3;
        }

        /* E N E R G Y */
        /* CURRENT ENERGY */
        private double currentEnergy;
        public double CurrentEnergy
        {
            get
            {
                return currentEnergy;
            }
            set
            {
                currentEnergy = value;
                if (currentEnergy < 0)
                {
                    this.TakeDamage(currentEnergy / 1.5, false);
                }
                currentEnergy = currentEnergy > 0 ? value : 0;
                currentEnergy = currentEnergy < MaxEnergy ? currentEnergy : MaxEnergy;
            }
        }
        /* MAX ENERGY*/
        private double maxEnergy;
        public double MaxEnergy
        {
            get
            {
                return maxEnergy;
            }
        }
        private double SetAndReturnMaxEnergy()
        {
            return maxEnergy = 2 * PrimaryStats.Stamina + PrimaryStats.Strength;
        }

        public void DoAction(int ActionCost, int EnergyCost)
        {
            // max swiftness - 15 ==> all actions cost 50% less
            double TrueActionCost = ActionCost - ((double)PrimaryStats.Swiftness / 30) * ActionCost;
            CurrentThirst -= TrueActionCost;
            CurrentHunger -= TrueActionCost;
            CurrentEnergy -= EnergyCost;
        }
        public void Sleep(int hours)
        {
            switch (hours)
            {
                case 2:
                    CurrentEnergy += MaxEnergy * 0.20;
                    break;

                case 4:
                    CurrentEnergy += MaxEnergy * 0.45;
                    break;

                case 6:
                    CurrentEnergy += MaxEnergy * 0.75;
                    break;

                case 8:
                    CurrentEnergy = MaxEnergy;
                    break;
            }
            DoAction(hours / 2, 0);
        }
        public void ChangePrimaryStats(PrimaryStats primaryStats)
        {
            this.PrimaryStats.Strength = primaryStats.Strength;
            this.PrimaryStats.Stamina = primaryStats.Stamina;
            this.PrimaryStats.Dextirity = primaryStats.Dextirity;
            this.PrimaryStats.Inteligence = primaryStats.Inteligence;
            this.PrimaryStats.Swiftness = primaryStats.Swiftness;

            RefreshMaximumValues();
        }
        public void RefreshMaximumValues()
        {
            SetAndReturnMaxHealth();
            SetAndReturnMaxHunger();
            SetAndReturnMaxThirst();
            SetAndReturnMaxEnergy();
        }
    }
}
