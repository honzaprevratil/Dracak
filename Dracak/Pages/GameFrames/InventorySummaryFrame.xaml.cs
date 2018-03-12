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
    public partial class InventorySummaryFrame : Page
    {
        public InventorySummaryFrame()
        {
            InitializeComponent();

            /* Using items bound */
            WeaponName.Content = App.PlayerViewModel.Player.Inventory.UsingWeapon.Name.ToString();
            WeaponDamage.Content = App.PlayerViewModel.Player.Inventory.UsingWeapon.GetStats();
            ArmorName.Content = App.PlayerViewModel.Player.Inventory.UsingArmor.Name.ToString();
            ArmorDefence.Content = App.PlayerViewModel.Player.Inventory.UsingArmor.GetStats();

            Damage.Content = App.PlayerViewModel.Player.GetStringDamage();
            Defense.Content = App.PlayerViewModel.Player.GetDefense();
            Speed.Content = App.PlayerViewModel.Player.GetBattleSpeed().ToString();

        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Click_ItemsInventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ItemListFrame(App.PlayerViewModel.Player.Inventory.ItemList, "Předměty"));
        }
    }
}
