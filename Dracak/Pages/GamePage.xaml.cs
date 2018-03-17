using Dracak.Classes.Creatures;
using Dracak.Classes.DB;
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
using System.Windows.Threading;

namespace Dracak.Pages
{
    /// <summary>
    /// Interakční logika pro GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public GamePage()
        {
            InitializeComponent();
        }
        public GamePage BindData()
        {
            App.SlowWriter.Target = StoryBlock;
            /* LOCATION NAME */
            Binding BindingName = new Binding("CurrentLocationName") { Source = App.LocationViewModel };
            StoryHeader.SetBinding(ContentControl.ContentProperty, BindingName);
            
            /* LOCATION IMAGE */
            Binding BindingImage = new Binding("CurrentLocationImage") { Source = App.LocationViewModel };
            StoryImage.SetBinding(Image.SourceProperty, BindingImage);
            
            /* LOCATION FILTER */
            Binding BindingFilter = new Binding("CurrentLocationFilter") { Source = App.LocationViewModel };
            Filter.SetBinding(TextBlock.BackgroundProperty, BindingFilter);

            /* PLAYER'S HEALTH BAR */
            Binding BindingHealth = new Binding("HealthBarValue") { Source = App.PlayerViewModel }; // value bind
            Binding BindingBarsWidth = new Binding("HealthThirstHungerBarWidth") { Source = App.PlayerViewModel }; // bar width bind
            Binding BindingHealthText = new Binding("HealthBarString") { Source = App.PlayerViewModel }; // text value bind

            HealthBar.SetBinding(ProgressBar.ValueProperty, BindingHealth); // value bind
            HealthBar.SetBinding(ProgressBar.WidthProperty, BindingBarsWidth); // bar width bind - same for health, thirst and hunger
            HealthBarString.SetBinding(ContentControl.ContentProperty, BindingHealthText); // text value bind

            /* PLAYER'S ENERGY BAR */
            Binding BindingEnergy = new Binding("EnergyBarValue") { Source = App.PlayerViewModel }; // value bind
            Binding BindingEneryWidth = new Binding("EnergyBarWidth") { Source = App.PlayerViewModel };  // bar width bind
            Binding BindingEnergyText = new Binding("EnergyBarString") { Source = App.PlayerViewModel }; // text value bind

            EnergyBar.SetBinding(ProgressBar.ValueProperty, BindingEnergy); // value bind
            EnergyBar.SetBinding(ProgressBar.WidthProperty, BindingEneryWidth); // bar width bind 
            EnergyBarString.SetBinding(ContentControl.ContentProperty, BindingEnergyText); // text value bind

            /* PLAYER'S HUNGER BAR */
            Binding BindingHunger = new Binding("HungerBarValue") { Source = App.PlayerViewModel }; // value bind
            Binding BindingHungerText = new Binding("HungerBarString") { Source = App.PlayerViewModel }; // text value bind

            HungerBar.SetBinding(ProgressBar.ValueProperty, BindingHunger); // value bind
            HungerBar.SetBinding(ProgressBar.WidthProperty, BindingBarsWidth); // bar width bind - same for health, thirst and hunger
            HungerBarString.SetBinding(ContentControl.ContentProperty, BindingHungerText); // text value bind

            /* PLAYER'S THIRST BAR */
            Binding BindingThirst = new Binding("ThirstBarValue") { Source = App.PlayerViewModel }; // value bind
            Binding BindingThirstText = new Binding("ThirstBarString") { Source = App.PlayerViewModel }; // text value bind

            ThirstBar.SetBinding(ProgressBar.ValueProperty, BindingThirst);// value bind
            ThirstBar.SetBinding(ProgressBar.WidthProperty, BindingBarsWidth); // bar width bind - same for health, thirst and hunger
            ThirstBarString.SetBinding(ContentControl.ContentProperty, BindingThirstText); // text value bind

            /* CHECK ALIVE */
            timer.Interval = new TimeSpan(0, 0, 0, 0, 333);
            timer.Tick += (sender, args) => { CheckAlive(); };
            timer.Start();
            return this;
        }
        public GamePage StartWriting()
        {
            App.SlowWriter.StartWriting();
            return this;
        }

        private void CheckAlive()
        {
            if (!App.PlayerViewModel.Player.IsAlive)
            {
                App.PlayerViewModel.DBhelper.DeleteAllFromDB();
                this.NavigationService.Navigate(new DeathPage());
                App.PlayerViewModel.DBhelper.CloseConnection();
                App.LocationViewModel.DBhelper.CloseConnection();
                FileHelper fileHelper = new FileHelper();
                fileHelper.DeleteDB();
                timer.Stop();
            }
            if (App.PlayerViewModel.Player.InLocationId == 11)
            {
                this.NavigationService.Navigate(new EndPage());
            }
        }

        private void GameFrame_Initialized(object sender, EventArgs e)
        {
            GameFrame.NavigationService.Navigate(new ActionsFrame());
        }

        private void Click_Next(object sender, RoutedEventArgs e)
        {
            if (App.SlowWriter.IsWriting())
            {
                App.SlowWriter.StopWriting();
            }
        }

        private void Click_Menu(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/MenuPage.xaml", UriKind.Relative));
        }
    }
}
