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
        

        public Consumable(string name, int amount, bool eatable, int speed, string description, int findChance, bool visiblity, int locationId, int inventoryId) 
            : base(name, speed, description, findChance, visiblity, locationId, inventoryId)
        {
            this.Amount = amount;
            this.CanBeEaten = eatable;
        }

        public Consumable()
        {

        }
        public override string GetName()
        {
            //return Name + " [ " + Amount + " ]";
            return Name;
        }
    }
}
