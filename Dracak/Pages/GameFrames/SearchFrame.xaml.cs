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
            App.GameActions.Search(SearchOptions.FastSearch);
            this.NavigationService.GoBack();
        }

        private void Click_Slow(object sender, RoutedEventArgs e)
        {
            App.GameActions.Search(SearchOptions.SlowSearch);
            this.NavigationService.GoBack();
        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
