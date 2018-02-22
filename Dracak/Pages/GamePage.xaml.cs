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
        SlowWriter writer = new SlowWriter();
        public GamePage()
        {
            InitializeComponent();

            writer.Target = StoryBlock;
            writer.StoryFull = "Stojíš na severní pláži. Všude kolem Tebe písek, palmy, kameny a moře. Nehostinné podmínky ostrova na Tebe volají ze všech stran. Co se rozhodneš udělat?";
            writer.StartWriting();
        }

        private void GameFrame_Initialized(object sender, EventArgs e)
        {
            GameFrame.NavigationService.Navigate(new Actions());
        }

        private void Click_Next(object sender, RoutedEventArgs e)
        {
            if (writer.IsWriting())
            {
                writer.StopWriting();
            }
        }

        private void Click_Menu(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/MenuPage.xaml", UriKind.Relative));
        }
    }
}
