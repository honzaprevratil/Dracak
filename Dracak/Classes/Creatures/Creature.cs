using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.Items;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    public class Creature : ATable
    {
        public Creature(PrimaryStats primaryStats, Inventory inventory)
        {
            this.PrimaryStats = primaryStats;
            this.Inventory = inventory;

            currentHealth = SetAndReturnMaxHealth();
        }

        [ForeignKey(typeof(Location))]
        public int LocationId { get; set; }

        private PrimaryStats primaryStats;

        [OneToOne]
        public PrimaryStats PrimaryStats
        {
            get
            {
                return primaryStats;
            }
            set
            {
                primaryStats = value;
                SetAndReturnMaxHealth();
            }
        }
        /* MAX HEALTH */
        private double maxHealth;
        public double MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }
        public double SetAndReturnMaxHealth()
        {
            return maxHealth = 5 * PrimaryStats.Stamina;
        }

        /* CURRENT HEALTH + IS ALIVE */
        private double currentHealth;
        public bool IsAlive;
        public double CurrentHealth
        {
            get
            {
                return currentHealth;
            } set
            {
                currentHealth = currentHealth > 0 ? value : 0;
                currentHealth = currentHealth < MaxHealth ? currentHealth : MaxHealth;

                IsAlive = currentHealth == 0 ? true : false;
            }
        }

        /* INVENTORY */
        private Inventory inventory;
        [OneToOne]
        public Inventory Inventory
        {
            set
            {
                inventory = value;
            }
            get
            {
                return inventory;
            }
        }

        /* BATTLE NUMBERS & DAMAGE */
        public double GetBattleSpeed()  
        {
            return PrimaryStats.Swiftness + Inventory.UsingArmor.Speed + Inventory.UsingWeapon.Speed;
        }
        public double GetDamage()
        {
            Random randInt = new Random();
            return PrimaryStats.Strength + Inventory.UsingWeapon.Damage + randInt.Next(1,7);
        }
        public string GetStringDamage()
        {
            int DamageLow = (PrimaryStats.Strength + Inventory.UsingWeapon.Damage);
            return DamageLow.ToString() + " - " + (DamageLow + 6).ToString() + " (~" + (DamageLow + 3).ToString() + ")";
        }
        public double GetDefense()
        {
            return Inventory.UsingArmor.Defense + PrimaryStats.Dextirity;
        }
        public void TakeDamage(double Damage, bool UseArmor)
        {
            Damage = Damage > 0 ? Damage : (-1) * Damage;

            if (UseArmor)
            {
                double Absorption = GetDefense();
                if (Damage > Absorption)
                {
                    CurrentHealth -= Damage - Absorption;
                }
            } else
            {
                CurrentHealth -= Damage;
            }
        }
    }
}