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
    public partial class CharacterFrame : Page
    {
        public CharacterFrame()
        {
            InitializeComponent();

            /* STRENGTH */
            Strength.Content = App.PlayerViewModel.Player.PrimaryStats.Strength;
            if (App.PlayerViewModel.Player.PrimaryStats.Strength >= 15)
                StrengthUp.Visibility = Visibility.Hidden;

            /* STAMINA */
            Stamina.Content = App.PlayerViewModel.Player.PrimaryStats.Stamina;
            if (App.PlayerViewModel.Player.PrimaryStats.Stamina >= 15)
                StaminaUp.Visibility = Visibility.Hidden;

            /* INTELIGENCE */
            Inteligence.Content = App.PlayerViewModel.Player.PrimaryStats.Inteligence;
            if (App.PlayerViewModel.Player.PrimaryStats.Inteligence >= 15)
                InteligenceUp.Visibility = Visibility.Hidden;

            /* DEXTIRITY */
            Dextirity.Content = App.PlayerViewModel.Player.PrimaryStats.Dextirity;
            if (App.PlayerViewModel.Player.PrimaryStats.Dextirity >= 15)
                DextirityUp.Visibility = Visibility.Hidden;

            /* SWIFTNESS */
            Swiftness.Content = App.PlayerViewModel.Player.PrimaryStats.Swiftness;
            if (App.PlayerViewModel.Player.PrimaryStats.Swiftness >= 15)
                SwiftnessUp.Visibility = Visibility.Hidden;

            /* STAT POINTS */
            Points.Content = App.PlayerViewModel.Player.StatsPoints;
            if (App.PlayerViewModel.Player.StatsPoints == 0)
                HideAllUpButtons();
        }

        private void Click_StrengthUp(object sender, RoutedEventArgs e)
        {
            StatUp(Strength, StrengthUp);
        }

        private void Click_StaminaUp(object sender, RoutedEventArgs e)
        {
            StatUp(Stamina, StaminaUp);
        }

        private void Click_InteligenceUp(object sender, RoutedEventArgs e)
        {
            StatUp(Inteligence, InteligenceUp);
        }
        private void Click_DextirityUp(object sender, RoutedEventArgs e)
        {
            StatUp(Dextirity, DextirityUp);
        }
        private void Click_SwiftnessUp(object sender, RoutedEventArgs e)
        {
            StatUp(Swiftness, SwiftnessUp);
        }
        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            int Str = int.Parse(Strength.Content.ToString());
            int Sta = int.Parse(Stamina.Content.ToString());
            int Int = int.Parse(Inteligence.Content.ToString());
            int Dex = int.Parse(Dextirity.Content.ToString());
            int Swi = int.Parse(Swiftness.Content.ToString());

            App.PlayerViewModel.Player.ChangePrimaryStats(new PrimaryStats(Str, Sta, Dex, Int, Swi));
            App.PlayerViewModel.Player.StatsPoints = int.Parse(Points.Content.ToString());

            App.PlayerViewModel.ReRenderBars();
        }
        private void StatUp(Label statLabel, Button statUpButton)
        {
            int stat = int.Parse(statLabel.Content.ToString()) + 1;
            statLabel.Content = stat.ToString();
            statUpButton.Visibility = stat >= 15 ? Visibility.Hidden : Visibility.Visible;

            int points = int.Parse(Points.Content.ToString()) - 1;
            Points.Content = points.ToString();

            if (points == 0)
            {
                HideAllUpButtons();
            }
        }
        private void HideAllUpButtons()
        {
            StrengthUp.Visibility = Visibility.Hidden;
            StaminaUp.Visibility = Visibility.Hidden;
            InteligenceUp.Visibility = Visibility.Hidden;
            DextirityUp.Visibility = Visibility.Hidden;
            SwiftnessUp.Visibility = Visibility.Hidden;
        }
    }
}
