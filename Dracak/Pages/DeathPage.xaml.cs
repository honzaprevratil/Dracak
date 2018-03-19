using Dracak.Classes.DB;
using Dracak.Classes.Locations;
using Dracak.Pages;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class DeathPage : Page
    {
        public DeathPage()
        {
            InitializeComponent();
            gameVersion.Content = "verze 1.1.02 (19.03.18)";
        }

        private void Click_ExitGame(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }
    }
}
