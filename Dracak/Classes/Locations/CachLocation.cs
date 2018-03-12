using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dracak.Classes.Locations
{
    public class CachLocation : INotifyPropertyChanged
    {
        /*
         * CACHED LOCATION
         * 
         * DBhelper - for comunication with DB
         * DBlocation dict - id translator
         * 
         * current location - hold current location data
         * 
         * header           - for view
         * background image - for view
         * filter           - for view
         * story            - for view
         */

        /* DB HELPER */
        public DBhelper DBhelper;
        /* DB LOCATIONS DICT */
        public Dictionary<int, string> LocationsDict = new Dictionary<int, string>();

        public CachLocation()
        {
            DBhelper = new DBhelper();
            LocationsDict = DBhelper.GetLocationDict();
            this.CurrentLocation = DBhelper.GetWithChildrenById<Location>(1);
        }

        /* CURRENT LOCATION */
        private Location location;
        public Location CurrentLocation
        {
            get { return location; }
            set
            {
                location = value;

                locationName = value.Name;
                OnPropertyChanged("CurrentLocationName");

                locationImage = new BitmapImage(new Uri(value.Image, UriKind.Relative));
                OnPropertyChanged("CurrentLocationImage");

                var bc = new BrushConverter();
                locationFilter = (Brush)bc.ConvertFrom(value.Filter);
                OnPropertyChanged("CurrentLocationFilter");

                CurrentLocationStory = value.Description;
            }
        }

        /* LOCATION HEADER */
        private string locationName;
        public string CurrentLocationName { get { return locationName; } }
        
        /* LOCATION BACKGROUND IMAGE */
        private BitmapImage locationImage;
        public BitmapImage CurrentLocationImage { get { return locationImage; } }

        /* LOCATION FILTER */
        private Brush locationFilter;
        public Brush CurrentLocationFilter { get { return locationFilter; } }

        /* LOCATION STORY BOARD */
        private string locationStory;
        public string CurrentLocationStory {
            get
            {
                return locationStory;
            }
            set
            {
                locationStory = value;
                App.SlowWriter.StoryFull = value;
                App.SlowWriter.StartWriting();
            }
        }


        /* EVENT AND FUNCTION FOR EVENT */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
