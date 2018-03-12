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
    public partial class ItemDetailWeaponFrame : Page
    {
        public ItemDetailWeaponFrame(Weapon weapon)
        {
            InitializeComponent();

            Name.Content = weapon.Name;
            switch(weapon.Type)
            {
                case WeaponType.Melee:
                    Type.Content = "Zbraň na blízko";
                    break;
                case WeaponType.Ranged:
                    Type.Content = "Střelná zbraň";
                    break;
            }
            Damage.Content = weapon.Damage;
            Speed.Content = weapon.Speed;
            Rarity.Content = weapon.FindChance;

            if (weapon.InventoryId == 1)
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

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void Click_Equip(object sender, RoutedEventArgs e)
        {

        }

        private void Click_Throw(object sender, RoutedEventArgs e)
        {

        }
    }
}
