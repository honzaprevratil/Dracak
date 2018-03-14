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
        bool PlayerStarts = false;
        int EscapeAttempts = 0;
        Random randInt = new Random();

        public FightFrame()
        {
            InitializeComponent();
            enemy = App.LocationViewModel.CurrentLocation.Enemy;
            if ((App.PlayerViewModel.Player.Iniciative() + randInt.Next(1, 7)) > (enemy.Iniciative() + randInt.Next(1, 7)))
                PlayerStarts = true;

            App.SlowWriter.StoryFull = enemy.Story;

            EnemyName.Content = enemy.Name;
            Damage.Content = enemy.GetStringDamage();
            Defense.Content = enemy.Defense;
            Speed.Content = enemy.Speed;

            EnemyHealthBar.Maximum = enemy.MaxHealth;
            EnemyHealthBar.Value = enemy.CurrentHealth;
            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Click_Inventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InventorySummaryFrame());
        }

        private void Click_Attack(object sender, RoutedEventArgs e)
        {
            App.GameActions.FightOneRound(PlayerStarts);

            /* ENEMY KILLED */
            if (enemy.CurrentHealth < 0)
            {
                this.NavigationService.Navigate(new ActionsFrame());
            }

            EnemyHealthBar.Value = enemy.CurrentHealth; // enemy's render
        }

        private void Click_RunAway(object sender, RoutedEventArgs e)
        {
            bool escaped = App.GameActions.TryEscape(EscapeAttempts);
            if (escaped)
            {
                this.NavigationService.Navigate(new ActionsFrame());
            }
            else
            {
                EscapeAttempts++;
            }
        }
    }
}
