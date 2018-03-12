using Dracak.Classes.Items;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public abstract class AItem : ATable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Speed { get; set; }
        public int FindChance { get; set; }
        public bool Visiblity { get; set; }

        [ForeignKey(typeof(Location))]     
        public int LocationId { get; set; }
        
        [ForeignKey(typeof(Inventory))]
        public int InventoryId { get; set; }


        public AItem(string name, int speed, string description, int findChance, bool visiblity, int locationId, int inventoryId)
        {
            this.Name = name;
            this.Speed = speed;
            this.Description = description;
            this.FindChance = findChance;
            this.Visiblity = visiblity;
            this.LocationId = locationId;
            this.InventoryId = inventoryId;
        }
        public AItem()
        {

        }
        public abstract string GetName();
    }
}
