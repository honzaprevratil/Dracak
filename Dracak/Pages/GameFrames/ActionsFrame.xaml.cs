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
    public partial class ActionsFrame : Page
    {
        public ActionsFrame()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.PlayerViewModel.Player.InFight)
                this.NavigationService.Navigate(new FightFrame().LoadEnemy());
            if (App.PlayerViewModel.Player.StatsPoints > 0)
            {
                SkillPointsNotification.Visibility = Visibility.Visible;
                SkillPointsNotification.Content = "+" + App.PlayerViewModel.Player.StatsPoints;
            }
            else
                SkillPointsNotification.Visibility = Visibility.Hidden;
        }
        private void Click_Move(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MoveFrame());
        }

        private void Click_Inventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InventorySummaryFrame());
        }

        private void Click_Search(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SearchFrame());
        }

        private void Click_Character(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CharacterFrame());
        }

        private void Click_PickUp(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ItemListFrame("Sebrat"));
        }

        private void Click_Sleep(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SleepFrame());
        }
    }
}
