using Dracak.Classes.AItems;
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
        public Player (int strngth = 5, int stamina = 5, int dextirity = 5, int inteligence = 5, int swiftness = 5)
            : base(new PrimaryStats(strngth, stamina, dextirity, inteligence, swiftness), new Inventory())
        {

        }
        public Player() : base()
        {

        }
        public void DefaultData(int statsPoints = 5)
        {
            RefreshMaximumValues();
            currentHunger = SetAndReturnMaxHunger();
            currentThirst = SetAndReturnMaxThirst();
            currentEnergy = SetAndReturnMaxEnergy();
            CurrentHealth = SetAndReturnMaxHealth();
            StatsPoints = statsPoints;
        }

        public int InLocationId { get; set; }
        public bool InFight { get; set; }
        public int StatsPoints { get; set; }

        /* H U N G E R */
        /* CURRENT HUNGER */
        public double CurrentHunger { get; set; }
        private double currentHunger
        {
            get
            {
                return CurrentHunger;
            }
            set
            {
                CurrentHunger = value;
                if (CurrentHunger < 0)
                {
                    this.TakeDamage(CurrentHunger / 3, false);
                }
                CurrentHunger = CurrentHunger > 0 ? value : 0;
                CurrentHunger = CurrentHunger < MaxHunger ? CurrentHunger : MaxHunger;
            }
        }
        /* MAX HUNGER*/
        public double MaxHunger { get; set; }
        private double SetAndReturnMaxHunger()
        {
            return MaxHunger = PrimaryStats.Stamina;
        }

        /* T H I R S T */
        /* CURRENT THIRST */
        public double CurrentThirst { get; set; }
        private double currentThirst
        {
            get
            {
                return CurrentThirst;
            }
            set
            {
                CurrentThirst = value;
                if (CurrentThirst < 0)
                {
                    this.TakeDamage(CurrentThirst / 2, false);
                }
                CurrentThirst = CurrentThirst > 0 ? value : 0;
                CurrentThirst = CurrentThirst < MaxThirst ? CurrentThirst : MaxThirst;
            }
        }
        /* MAX THIRST*/
        public double MaxThirst { get; set; }
        private double SetAndReturnMaxThirst()
        {
            return MaxThirst = 2 * PrimaryStats.Stamina / 3;
        }

        /* E N E R G Y */
        /* CURRENT ENERGY */
        public double CurrentEnergy { get; set; }
        private double currentEnergy
        {
            get
            {
                return CurrentEnergy;
            }
            set
            {
                CurrentEnergy = value;
                if (CurrentEnergy < 0)
                {
                    this.TakeDamage(CurrentEnergy / 1.5, false);
                }
                CurrentEnergy = CurrentEnergy > 0 ? value : 0;
                CurrentEnergy = CurrentEnergy < MaxEnergy ? CurrentEnergy : MaxEnergy;
            }
        }
        /* MAX ENERGY*/
        public double MaxEnergy { get; set; }
        private double SetAndReturnMaxEnergy()
        {
            return MaxEnergy = 3 * PrimaryStats.Strength;
        }

        public void DoAction(int ActionCost, int EnergyCost)
        {
            currentThirst -= GetTrueActionCost(ActionCost);
            currentHunger -= GetTrueActionCost(ActionCost);
            currentEnergy -= EnergyCost;
        }
        public double GetTrueActionCost(int ActionCost)
        {
            // max swiftness - 15 ==> all actions cost 50% less
            // about -3% drom duration foreach point
            return ActionCost * (1 - (double)PrimaryStats.Swiftness / 30);
        }
        public void Sleep(int hours)
        {
            switch (hours)
            {
                case 2:
                    currentEnergy += MaxEnergy * 0.20;
                    currentHealth += MaxHealth * (0.10 * (currentHunger > 0 ? 1 : 0) + 0.10 * (currentThirst > 0 ? 1 : 0));
                    break;

                case 4:
                    currentEnergy += MaxEnergy * 0.45;
                    currentHealth += MaxHealth * (0.225 * (currentHunger > 0 ? 1 : 0) + 0.225 * (currentThirst > 0 ? 1 : 0));
                    break;

                case 6:
                    currentEnergy += MaxEnergy * 0.75;
                    currentHealth += MaxHealth * (0.375 * (currentHunger > 0 ? 1 : 0) + 0.375 * (currentThirst > 0 ? 1 : 0));
                    break;

                case 8:
                    currentEnergy = MaxEnergy;
                    currentHealth += MaxHealth * (0.5 * (currentHunger > 0 ? 1 : 0) + 0.5 * (currentThirst > 0 ? 1 : 0));
                    break;
            }
            DoAction(hours / 2, 0);
        }
        public void Eat(double hungerGain = 0, double thirstGain = 0, double healthGain = 0, double energyGain = 0)
        {
            currentHealth += healthGain;
            currentThirst += thirstGain;
            currentHunger += hungerGain;
            currentEnergy += energyGain;
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
