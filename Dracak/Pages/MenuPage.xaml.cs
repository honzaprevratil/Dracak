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
                MessageBoxResult result = MessageBox.Show("To start a new game, current saves will be deleted. Do wish to continue?", "Delete old progress", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
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
                        result = MessageBox.Show("To start a new game, the game must be restarted. Do wish to restart now?", "Restart game", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            this.NavigationService.Navigate(new GamePage().BindData().StartWriting());
        }

        private void Click_ExitGame(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }
    }
}
