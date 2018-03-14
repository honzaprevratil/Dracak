using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Items
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
        public override string GetName()
        {
            if (InventoryId == 1)
            {
                return "*"+ Name + "*";
            }
            //return Name + " [ " + Damage + "/" + Speed + " ]";
            return Name;
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
