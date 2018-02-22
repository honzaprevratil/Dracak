using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Locations
{
    public class GameLocations
    {
        public Location[] Locations = new Location[10];
        public Dictionary<int, string> LocationsDict = new Dictionary<int, string>();
        public int ActualId { get; set; } = 1;

        public void FillLocations()
        {
            LocationsDict.Add(1, "Severní pláž");
            Locations[1] = new Location {
                Id = 1,
                Name = "Severní pláž",
                Image = @"/Dracak;component/Images/beach1.jpg",
                Filter = "#4CA49C15",
                Description = "Stojíš na severní pláži. Všude kolem Tebe písek, palmy, kameny a moře. Nehostinné podmínky ostrova na Tebe volají ze všech stran. Co se rozhodneš udělat?",
                AdjacentLocations = new int[]{2,3},
                FastSearchText = "Rychle jsi prohledal Severní pláž",
                SlowSearchText = "Důkladně prohledáváš Severní pláž",
            };

            LocationsDict.Add(2, "Severní moře");
            Locations[2] = new Location
            {
                Id = 2,
                Name = "Severní moře",
                Image = @"/Dracak;component/Images/castaway.jpg",
                Filter = "#4CA49C15",
                Description = "Rozhodl ses plavit mo",
                AdjacentLocations = new int[] { 1 },
                FastSearchText = "Rychle jsi prohledal Severní moře",
                SlowSearchText = "Důkladně prohledáváš Severní moře",
            };

            LocationsDict.Add(3, "Severní les");

        }
    }
}
