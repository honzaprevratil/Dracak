using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Dracak
{
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static GameLocations _GameLocations;

        public static GameLocations GameLocations
        {
            get
            {
                if (_GameLocations == null)
                {
                    _GameLocations = new GameLocations();
                }
                return _GameLocations;
            }
        }
    }
}
