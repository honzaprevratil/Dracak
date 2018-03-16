using Dracak.Classes.Creatures;
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
    public partial class SearchFrame : Page
    {
        public SearchFrame()
        {
            InitializeComponent();
        }
        private void Click_Fast(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

            switch (App.GameActions.ItemActions.Search(SearchOptions.FastSearch))
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

        private void Click_Slow(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

            switch (App.GameActions.ItemActions.Search(SearchOptions.SlowSearch))
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
