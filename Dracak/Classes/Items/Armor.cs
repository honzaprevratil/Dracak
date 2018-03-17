using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.AItems
{
    public class Armor : AItem
    {
        public int Defense { get; set; }

        public Armor(string name, int defense, int speed, string description, int findChance, bool visiblity, int locationId, int inventoryId)
            : base(name, speed, description, findChance, visiblity, locationId, inventoryId)
        {
            this.Defense = defense;
        }
        public Armor()
        {

        }
        public override string GetInventoryName()
        {
            if (InventoryId == 1)
            {
                return "*" + Name + "*";
            }
            return Name;
        }
        public override string GetWriterName()
        {
            return Name + " (" + Defense + "/" + Speed + ")";
        }
        public string GetStats()
        {
            return Defense + "/" + Speed;
        }
    }
}
