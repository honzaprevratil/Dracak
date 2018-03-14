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
        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Click_ItemsInventory(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ItemListFrame("Předměty"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /* USIN WEAPON BOUND */
            WeaponName.Content = App.PlayerViewModel.Player.Inventory.UsingWeapon.Name.ToString();
            WeaponName.Foreground = App.GameActions.GetRarityBrush(App.PlayerViewModel.Player.Inventory.UsingWeapon);
            WeaponDamage.Content = App.PlayerViewModel.Player.Inventory.UsingWeapon.GetStats();

            /* USIN ARMOR BOUND */
            ArmorName.Content = App.PlayerViewModel.Player.Inventory.UsingArmor.Name.ToString();
            ArmorName.Foreground = App.GameActions.GetRarityBrush(App.PlayerViewModel.Player.Inventory.UsingArmor);
            ArmorDefence.Content = App.PlayerViewModel.Player.Inventory.UsingArmor.GetStats();

            /* STATS BOUND */
            Damage.Content = App.PlayerViewModel.Player.GetStringDamage();
            Defense.Content = App.PlayerViewModel.Player.GetDefense();
            Speed.Content = App.PlayerViewModel.Player.GetBattleSpeed();
        }
    }
}
