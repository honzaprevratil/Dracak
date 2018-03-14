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
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Click_NewGame(object sender, RoutedEventArgs e)
        {
            FileHelper fileHelper = new FileHelper();

            /* EXISTS SAVE? */
            if (File.Exists(fileHelper.GetDBfilePath()))
            {
                MessageBoxResult result = MessageBox.Show("Abyste mohl začít novou hru, aktuální postup hry musí být smazán." + "\n\n" + "Přejete si smazat herní postup?" + "\n\n" + "VAROVÁNÍ: Herní progres bude smazán TRVALE!", "Začít novou hru", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                /* WANT TO DELETE SAVE? */
                if (result == MessageBoxResult.Yes)
                {
                    bool DBDeletingDone = fileHelper.DeleteDB();

                    if (DBDeletingDone)
                    {
                        this.NavigationService.Navigate(new StoryPage().StartWriting());
                    }
                    else
                    {
                        /* CAN NOT DELETE, WISH TO RESTART? */
                        result = MessageBox.Show("Abyste mohl začít novou hru, hru je nejprve nutno restartovat." + "\n\n" + "Přeje si restarotvat hru?", "Restartovat hru", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                            Application.Current.Shutdown();
                    }
                }
            }
            else
                this.NavigationService.Navigate(new StoryPage().StartWriting());
        }

        private void Click_ContinueGame(object sender, RoutedEventArgs e)
        {
            FileHelper fileHelper = new FileHelper();

            /* EXISTS SAVE? */
            if (File.Exists(fileHelper.GetDBfilePath()))
            {
                this.NavigationService.Navigate(new GamePage().BindData().StartWriting());
            } 
            else
            {
                var result = MessageBox.Show("Abyste mohl pokračovat musíte nejprve začít novou hru."+"\n\n"+"Chcete začít novou hru?", "Začít novou hru", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                    this.NavigationService.Navigate(new StoryPage().StartWriting());
            }
        }

        private void Click_ExitGame(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }
    }
}
