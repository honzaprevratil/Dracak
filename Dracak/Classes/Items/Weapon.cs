using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.AItems
{
    public class Weapon : AItem
    {
        public int Damage { get; set; }
        public WeaponType Type { get; set; }

        public Weapon (string name, int damage, int speed, WeaponType weaponType, string description, int findChance, bool visiblity, int locationId, int inventoryId)
            : base(name, speed, description, findChance, visiblity, locationId, inventoryId)
        {
            this.Damage = damage;
            this.Type = weaponType;
        }
        public Weapon ()
        {

        }
        public override string GetInventoryName()
        {
            if (InventoryId == 1)
            {
                return "*"+ Name + "*";
            }
            return Name;
        }
        public override string GetWriterName()
        {
            return Name + " (" + Damage + "/" + Speed + ")";
        }
        public string GetStats()
        {
            return Damage + "/" + Speed;
        }
    }

    public enum WeaponType
    {
        Melee, Ranged
    }
}
