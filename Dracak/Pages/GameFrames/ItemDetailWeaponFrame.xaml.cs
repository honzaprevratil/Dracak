﻿using Dracak.Classes;
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
        private AItem viewedItem;
        public ItemDetailWeaponFrame(AItem item)
        {
            InitializeComponent();
            viewedItem = item;

            Name.Content = item.Name;
            Speed.Content = item.Speed;
            Description.Text = item.Description;

            /* RARITY COLORS */
            var bc = new BrushConverter();
            if (item.FindChance <= 7)// 0 - 7
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF888888");//WHITE - COMMON
                Type.Content = "Normální";
            }
            else if (item.FindChance <= 14)// 8 - 14 
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF0743AC");//BLUE - RARE
                Type.Content = "Vzácn" + ((item is Weapon) ? "á" : "é");
            }
            else if (item.FindChance <= 20)// 15 - 20
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF911CC7");//PURPLE - EPIC
                Type.Content = "Epick" + ((item is Weapon) ? "á" : "é");
            }
            else// 21 - 26
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FFC9470B");//ORANGE - LEGENDARY
                Type.Content = "Legendární";
            }


            if (item is Weapon)
            {
                switch (((Weapon)item).Type)
                {
                    case WeaponType.Melee:
                        Type.Content += " zbraň na blízko";
                        break;
                    case WeaponType.Ranged:
                        Type.Content += " střelná zbraň";
                        break;
                }

                DamageLabel.Content = "Poškození";
                Damage.Content = ((Weapon)item).Damage;
            }
            else if (item is Armor)
            {
                Type.Content += " brnění";

                DamageLabel.Content = "Absorbce";
                Damage.Content = ((Armor)item).Defense;
            }

            if (item.InventoryId == 1)
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
            if (viewedItem is Weapon)
            {

            }
            else if (viewedItem is Armor)
            {

            }
        }

        private void Click_Throw(object sender, RoutedEventArgs e)
        {

        }
    }
}
