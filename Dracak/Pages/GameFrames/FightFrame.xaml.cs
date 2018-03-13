using Dracak.Classes.Creatures;
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
    public partial class FightFrame : Page
    {
        Enemy enemy;
        public FightFrame()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            enemy = App.LocationViewModel.CurrentLocation.Enemy;

            App.SlowWriter.StoryFull = enemy.Story;

            EnemyName.Content = enemy.Name;
            Damage.Content = enemy.GetStringDamage();
            Defense.Content = enemy.Defense;
            Speed.Content = enemy.Speed;
            EnemyHealthBar.Maximum = enemy.MaxHealth;
            EnemyHealthBar.Value = enemy.CurrentHealth;
        }

        private void Click_Inventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InventorySummaryFrame());
        }
        
        private void Click_Attack(object sender, RoutedEventArgs e)
        {
            enemy.TakeDamage(App.PlayerViewModel.Player.DealDamage(), true);
            EnemyHealthBar.Value = enemy.CurrentHealth;

            if (enemy.CurrentHealth < 0)
            {
                this.NavigationService.Navigate(new ActionsFrame());

                enemy.IsAlive = false;

                App.PlayerViewModel.Player.StatsPoints += 1;
                App.PlayerViewModel.UpdatePlayer();
            }

            App.LocationViewModel.DBhelper.UpdateOne(enemy);
        }

        private void Click_RunAway(object sender, RoutedEventArgs e)
        {

        }
    }
}
