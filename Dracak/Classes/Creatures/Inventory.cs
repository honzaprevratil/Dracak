using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Items
{
    public class Inventory : ATable
    {
        [ForeignKey(typeof(Creature))]
        public int CreatureId { get; set; }

        [OneToOne]
        public Weapon UsingWeapon { get; set; }

        [OneToOne]
        public Armor UsingArmor { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<AItem> ItemList { get; set; } = new List<AItem>();

        public Inventory()
        {

        }
    }
}
