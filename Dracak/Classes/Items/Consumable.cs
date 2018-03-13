using Dracak.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    public class Consumable : AItem
    {
        public int Amount { get; set; }
        public bool CanBeEaten { get; set; }

        public double HealthGain { get; set; }
        public double HungerGain { get; set; }
        public double ThirstGain { get; set; }
        public double EnergyGain { get; set; }

        public Consumable(string name, int amount, bool eatable, int speed, string description, int findChance, bool visiblity, int locationId, int inventoryId, double healthGain = 0, double hungerGain = 0, double thirstGain = 0, double energyGain = 0) 
            : base(name, speed, description, findChance, visiblity, locationId, inventoryId)
        {
            this.Amount = amount;
            this.CanBeEaten = eatable;

            this.HealthGain = healthGain;
            this.HungerGain = hungerGain;
            this.ThirstGain = thirstGain;
            this.EnergyGain = energyGain;
        }

        public Consumable()
        {

        }
        public override string GetName()
        {
            return Name + " [ " + Amount + " ]";
        }
    }
}
