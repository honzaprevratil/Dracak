using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.Locations;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    public class Location : ATable
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Filter { get; set; }
        public string Description { get; set; }
        public string FastSearchText { get; set; }
        public string SlowSearchText { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LocationBind> AdjacentLocations { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<AItem> ItemList { get; set; }

        [OneToOne]
        public Enemy Enemy { get; set; }
    }
}
