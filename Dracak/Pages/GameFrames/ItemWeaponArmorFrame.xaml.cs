using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.AItems;
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
    public partial class ItemWeaponArmorFrame : Page
    {
        private AItem viewedItem;
        public ItemWeaponArmorFrame(AItem item)
        {
            InitializeComponent();
            viewedItem = item;
        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void Click_Equip(object sender, RoutedEventArgs e)
        { 
            App.GameActions.ItemActions.Equip(viewedItem);
            Equiped.Visibility = Visibility.Visible;
            NotEquiped.Visibility = Visibility.Hidden;
            Throw.Visibility = Visibility.Hidden;
        }

        private void Click_Throw(object sender, RoutedEventArgs e)
        {
            App.GameActions.ItemActions.Drop(viewedItem);
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Content = viewedItem.Name;
            Speed.Content = viewedItem.Speed;
            Description.Text = viewedItem.Description;

            /* RARITY COLORS */
            Brush RarityColorBrush = App.GameActions.ItemActions.GetRarityBrush(viewedItem, Type);
            Name.Foreground = RarityColorBrush;
            Type.Foreground = RarityColorBrush;
            if (viewedItem.FindChance > 7 && viewedItem.FindChance < 21)
                Type.Content += ((viewedItem is Weapon) ? "á" : "é");

            if (viewedItem is Weapon)
            {
                switch (((Weapon)viewedItem).Type)
                {
                    case WeaponType.Melee:
                        Type.Content += " zbraň na blízko";
                        break;
                    case WeaponType.Ranged:
                        Type.Content += " střelná zbraň";
                        break;
                }

                NotEquiped.ToolTip = "Aktuální zbraň: " + App.PlayerViewModel.Player.Inventory.UsingWeapon.Name + "\nPoškození: " + App.PlayerViewModel.Player.Inventory.UsingWeapon.Damage + "\nRychlost: " + App.PlayerViewModel.Player.Inventory.UsingWeapon.Speed;
                DamageLabel.Content = "Poškození";
                Damage.Content = ((Weapon)viewedItem).Damage;
            }
            else if (viewedItem is Armor)
            {
                Type.Content += " brnění";

                NotEquiped.ToolTip = "Aktuální brnění: " + App.PlayerViewModel.Player.Inventory.UsingArmor.Name + "\nPoškození: " + App.PlayerViewModel.Player.Inventory.UsingArmor.Defense + "\nRychlost: " + App.PlayerViewModel.Player.Inventory.UsingWeapon.Speed;
                DamageLabel.Content = "Absorbce";
                Damage.Content = ((Armor)viewedItem).Defense;
            }

            if (viewedItem.InventoryId == 1)
            {
                Equiped.Visibility = Visibility.Visible;
                NotEquiped.Visibility = Visibility.Hidden;
                Throw.Visibility = Visibility.Hidden;
            }
            else
            {
                Equiped.Visibility = Visibility.Hidden;
                NotEquiped.Visibility = Visibility.Visible;
                Throw.Visibility = Visibility.Visible;
            }
        }
    }
}
