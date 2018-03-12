using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Locations
{
    public class LocationBind : ATable
    {
        [ForeignKey(typeof(Location))]     
        public int LocationId { get; set; }
        [ForeignKey(typeof(Location))]     
        public int BindedId { get; set; }

        public LocationBind() { }
        public LocationBind(int locationId, int bindedId)
        {
            this.LocationId = locationId;
            this.BindedId = bindedId;
        }
    }
}
