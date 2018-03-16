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
    public class LocationViewModel : INotifyPropertyChanged
    {
        /*
         * CACHED LOCATION
         * 
         * DBhelper - for comunication with DB
         * LocationsDict - id translator
         * 
         * CurrentLocation - hold current location data
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

        public LocationViewModel()
        {
            DBhelper = new DBhelper();
            DBhelper.FillAllDefaultData();
            LocationsDict = DBhelper.GetLocationDict();
            this.CurrentLocation = DBhelper.GetLocationById(App.PlayerViewModel.Player.InLocationId);
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

                App.SlowWriter.StoryFull = value.Description + App.PlayerViewModel.GetTextLivingStatus();
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
        public Brush CurrentLocationFilter { get { return locationFilter; } set { locationFilter = value; } }

        public void ChangeLocation (int locationId)
        {
            DBhelper.UpdateList(CurrentLocation.ItemList);
            this.CurrentLocation = DBhelper.GetLocationById(locationId);
        }
        public void UpdateLocationItemList()
        {
            DBhelper.UpdateList(CurrentLocation.ItemList);
        }

        public void PickUpItemFromLocation(int itemIndex)
        {
            var item = CurrentLocation.ItemList[itemIndex];
            item.InventoryId = 9999;
            item.LocationId = 9999;
            DBhelper.UpdateItem(item);
        }
        public void DeleteItemFromLocation(int itemIndex)
        {
            var item = CurrentLocation.ItemList[itemIndex];
            DBhelper.DeleteItem(item);
        }


        /* EVENT AND FUNCTION FOR EVENT */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
