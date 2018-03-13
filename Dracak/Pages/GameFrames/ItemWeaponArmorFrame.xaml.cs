using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.Items;
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
            App.GameActions.EquipItem(viewedItem);
            Equiped.Visibility = Visibility.Visible;
            NotEquiped.Visibility = Visibility.Hidden;
            Throw.Visibility = Visibility.Hidden;
        }

        private void Click_Throw(object sender, RoutedEventArgs e)
        {
            App.GameActions.DropItem(viewedItem);
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Content = viewedItem.Name;
            Speed.Content = viewedItem.Speed;
            Description.Text = viewedItem.Description;

            /* RARITY COLORS */
            var bc = new BrushConverter();
            if (viewedItem.FindChance <= 7)// 0 - 7
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF888888");//WHITE - COMMON
                Type.Content = "Normální";
            }
            else if (viewedItem.FindChance <= 14)// 8 - 14 
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF0743AC");//BLUE - RARE
                Type.Content = "Vzácn" + ((viewedItem is Weapon) ? "á" : "é");
            }
            else if (viewedItem.FindChance <= 20)// 15 - 20
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF911CC7");//PURPLE - EPIC
                Type.Content = "Epick" + ((viewedItem is Weapon) ? "á" : "é");
            }
            else// 21 - 26
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FFC9470B");//ORANGE - LEGENDARY
                Type.Content = "Legendární";
            }


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

                DamageLabel.Content = "Poškození";
                Damage.Content = ((Weapon)viewedItem).Damage;
            }
            else if (viewedItem is Armor)
            {
                Type.Content += " brnění";

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
