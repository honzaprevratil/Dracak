using Dracak.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Filter { get; set; }
        public string Description { get; set; }
        public string FastSearchText { get; set; }
        public string SlowSearchText { get; set; }
        public int[] AdjacentLocations { get; set; } = { 1, 2 };
        //public List<AItem> ItemList { get; set; } = new List<AItem>();
        //public List<Material> MineableItemList { get; set; } = new List<Material>();
        //public Creature Enemy { get; set; }
    }
}
