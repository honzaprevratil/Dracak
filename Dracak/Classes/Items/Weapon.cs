using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Items
{
    class Weapon : Tool
    {
        public int Speed { get; set; }
        public int Damage { get; set; }
        public WeaponType Type { get; set; }
    }

    enum WeaponType
    {
        Melee, Ranged
    }
}
