using Dracak.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SearchText { get; set; }
        public int[] AdjacentLocations { get; set; } = { 1, 2 };
        public List<AItem> ItemList { get; set; } = new List<AItem>();
        public List<Material> MineableItemList { get; set; } = new List<Material>();
        public Creature Enemy { get; set; }
    }
}
