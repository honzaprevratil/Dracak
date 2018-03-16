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
    public partial class FightFrame : Page
    {
        Enemy enemy;
        bool PlayerStarts = false;
        int EscapeAttempts = 0;
        Random randInt = new Random();

        public FightFrame()
        {
            InitializeComponent();
        }
        public FightFrame LoadEnemy()
        {
            enemy = App.LocationViewModel.CurrentLocation.Enemy;
            if ((App.PlayerViewModel.Player.Iniciative() + randInt.Next(1, 7)) > (enemy.Iniciative() + randInt.Next(1, 7)))
                PlayerStarts = true;

            App.SlowWriter.StoryFull = enemy.Story;

            EnemyName.Content = enemy.Name;
            Damage.Content = enemy.GetStringDamage();
            Defense.Content = enemy.Defense;
            Speed.Content = enemy.Speed;

            EnemyHealthBar.Maximum = enemy.MaxHealth;
            RenderEnemyHealthBar(); // enemy's render
            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update

            return this;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void RenderEnemyHealthBar()
        {
            EnemyHealthBarString.Content = enemy.CurrentHealth + " / " + enemy.MaxHealth;
            EnemyHealthBar.Value = enemy.CurrentHealth;
        }

        private void Click_Inventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InventorySummaryFrame());
        }

        private void Click_Attack(object sender, RoutedEventArgs e)
        {
            switch(App.GameActions.FightActions.FightOneRound(PlayerStarts))
            {
                case ActionResult.PlayerDied:
                    // GamePage.NavigationService.Navigate(new DeathPage());
                    // how to do this????
                    break;

                case ActionResult.EnemyDied:
                    this.NavigationService.Navigate(new ActionsFrame());
                    break;
            }
            RenderEnemyHealthBar(); // enemy's render
        }

        private void Click_RunAway(object sender, RoutedEventArgs e)
        {
            switch (App.GameActions.FightActions.TryEscape(EscapeAttempts))
            {
                case ActionResult.PlayerDied:
                    // Game.NavigationService.Navigate(new DeathPage());
                    // how to do this????
                    break;

                case ActionResult.SuccessfullyDone:
                    this.NavigationService.Navigate(new ActionsFrame());
                    break;

                default:
                    EscapeAttempts++;
                    break;
            }
        }
    }
}
