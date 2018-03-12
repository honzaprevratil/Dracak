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
    /// Interakční logika pro GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
        }
        public GamePage BindData()
        {
            App.SlowWriter.Target = StoryBlock;
            /* LOCATION NAME */
            Binding BindingName = new Binding("CurrentLocationName")
            {
                Source = App.LocationViewModel
            };
            StoryHeader.SetBinding(ContentControl.ContentProperty, BindingName);
            /* LOCATION IMAGE */
            Binding BindingImage = new Binding("CurrentLocationImage")
            {
                Source = App.LocationViewModel
            };
            StoryImage.SetBinding(Image.SourceProperty, BindingImage);
            /* LOCATION FILTER */
            Binding BindingFilter = new Binding("CurrentLocationFilter")
            {
                Source = App.LocationViewModel
            };
            Filter.SetBinding(BackgroundProperty, BindingFilter);


            /* PLAYER'S HEALTH BAR */
            Binding BindingHealth = new Binding("HealthBarValue")
            {
                Source = App.PlayerViewModel
            };
            HealthBar.SetBinding(ProgressBar.ValueProperty, BindingHealth);
            /* PLAYER'S ENERGY BAR */
            Binding BindingEnergy = new Binding("EnergyBarValue")
            {
                Source = App.PlayerViewModel
            };
            EnergyBar.SetBinding(ProgressBar.ValueProperty, BindingEnergy);
            /* PLAYER'S HUNGER BAR */
            Binding BindingHunger = new Binding("HungerBarValue")
            {
                Source = App.PlayerViewModel
            };
            HungerBar.SetBinding(ProgressBar.ValueProperty, BindingHunger);
            /* PLAYER'S THIRST BAR */
            Binding BindingThirst = new Binding("ThirstBarValue")
            {
                Source = App.PlayerViewModel
            };
            ThirstBar.SetBinding(ProgressBar.ValueProperty, BindingThirst);
            return this;
        }
        public GamePage StartWriting()
        {
            App.SlowWriter.StartWriting();
            return this;
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
