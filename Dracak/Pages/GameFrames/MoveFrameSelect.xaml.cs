﻿using Dracak.Classes.Creatures;
using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dracak.Pages
{
    /// <summary>
    /// Interakční logika pro Actions.xaml
    /// </summary>
    public partial class MoveFrameSelect : Page
    {
        MoveOptions SelectedMoveOption;

        public MoveFrameSelect(MoveOptions moveOptions)
        {
            InitializeComponent();
            SelectedMoveOption = moveOptions;

            Button[] buttons = new Button[] { But1, But2, But3, But4, But5 };

            for (int x = 0; x <= 4; x++)
            {
                if (App.LocationViewModel.CurrentLocation.AdjacentLocations.Count > x)
                {
                    int locationId = App.LocationViewModel.CurrentLocation.AdjacentLocations[x].BindedId;
                    (buttons[x].Content as Label).Content = App.LocationViewModel.LocationsDict[locationId];
                } else
                {
                    buttons[x].Visibility = Visibility.Hidden;
                }
            }
        }
        private void Click_Location(object sender, RoutedEventArgs e)
        {
            /* GET ID */
            var param = (sender as Button).CommandParameter;
            int.TryParse(param.ToString(), out int AdjacentLocationIndex);
            
            this.NavigationService.GoBack();
            this.NavigationService.GoBack();
            /* MOVE */

            switch (App.GameActions.Move(SelectedMoveOption, AdjacentLocationIndex))
            {
                case ActionResult.PlayerDied:
                    // Game.NavigationService.Navigate(new DeathPage());
                    // how to do this????
                    break;

                case ActionResult.PlayerAmbushed:
                    this.NavigationService.Navigate(new FightFrame().LoadEnemy());
                    break;
            }
        }
        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
