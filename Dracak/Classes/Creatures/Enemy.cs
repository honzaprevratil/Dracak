using Dracak.Classes.Battle;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Creatures
{
    public class Enemy : ATable, IBattleMethods
    {
        public Enemy()
        {

        }
        public Enemy(string name, int health, int damage, int speed, int defense, int fightChance, string story, string deathStory)
        {
            this.Name = name;
            this.CurrentHealth = health;
            this.MaxHealth = health;
            this.Damage = damage;
            this.Speed = speed;
            this.Defense = defense;
            this.Aggressivity = fightChance;
            this.Story = story;
            this.DeathStory = deathStory;
        }

        [ForeignKey(typeof(Location))]
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string Story { get; set; }
        public string DeathStory { get; set; }

        public double CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public bool IsAlive { get; set; } = true;

        public int Damage { get; set; }
        public int Speed { get; set; }
        public int Inteligence { get; set; }
        public int Defense { get; set; }
        public int Aggressivity { get; set; }

        public double DealDamage()
        {
            Random randInt = new Random();
            return Damage + randInt.Next(1, 7);
        }
        public double GetBattleSpeed()
        {
            return Speed;
        }
        public string GetStringDamage()
        {
            return Damage.ToString() + " - " + (Damage + 6).ToString() + " [~" + (Damage + 3).ToString() + "]";
        }
        public double GetDefense()
        {
            return Defense;
        }
        public void TakeDamage(double DamageDealt, bool UseArmor)
        {
            DamageDealt = DamageDealt > 0 ? DamageDealt : (-1) * DamageDealt;

            if (UseArmor)
            {
                if (DamageDealt > Defense)
                    CurrentHealth -= DamageDealt - Defense;
                else
                    CurrentHealth -= 1;
            }
            else
                CurrentHealth -= DamageDealt;
        }

        public int Iniciative()
        {
            return Inteligence;
        }
    }
}
