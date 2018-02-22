using Dracak.Pages;
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

namespace Dracak
{
    /// <summary>
    /// Interakční logika pro MainGame.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Click_NewGame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StoryPage());
        }

        private void Click_ContinueGame(object sender, RoutedEventArgs e)
        {

        }

        private void Click_ExitGame(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }
    }
}
