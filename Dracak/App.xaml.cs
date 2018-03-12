using Dracak.Classes;
using Dracak.Classes.Creatures;
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
        private static GameActions _GameActions;

        public static GameActions GameActions
        {
            get
            {
                if (_GameActions == null)
                {
                    _GameActions = new GameActions();
                }
                return _GameActions;
            }
        }
        private static PlayerViewModel _PlayerViewModel;

        public static PlayerViewModel PlayerViewModel
        {
            get
            {
                if (_PlayerViewModel == null)
                {
                    _PlayerViewModel = new PlayerViewModel();
                }
                return _PlayerViewModel;
            }
        }
        private static LocationViewModel _LocationViewModel;

        public static LocationViewModel LocationViewModel
        {
            get
            {
                if (_LocationViewModel == null)
                {
                    _LocationViewModel = new LocationViewModel();
                }
                return _LocationViewModel;
            }
        }

        private static SlowWriter _SlowWriter;
        public static SlowWriter SlowWriter
        {
            get
            {
                if (_SlowWriter == null)
                {
                    _SlowWriter = new SlowWriter();
                }
                return _SlowWriter;
            }
        }
    }
}
