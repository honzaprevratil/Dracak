using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Creatures
{
    public class PrimaryStats : ATable
    {
        [ForeignKey(typeof(Creature))]
        public int CreatureId { get; set; }

        public int Strength { get; set; }
        public int Stamina { get; set; }
        public int Dextirity { get; set; }
        public int Inteligence { get; set; }
        public int Swiftness { get; set; }

        public PrimaryStats(int strngth, int stamina, int dextirity, int inteligence, int swiftness)
        {
            this.Strength = strngth;
            this.Stamina = stamina;
            this.Dextirity = dextirity;
            this.Inteligence = inteligence;
            this.Swiftness = swiftness;
        }
        public PrimaryStats()
        {

        }
    }
}
