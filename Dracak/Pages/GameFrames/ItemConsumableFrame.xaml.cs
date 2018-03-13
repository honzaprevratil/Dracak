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
    public partial class ItemConsumableFrame : Page
    {
        private Consumable viewedItem;
        public ItemConsumableFrame(Consumable item)
        {
            InitializeComponent();
            viewedItem = item;

            Name.Content = item.Name;
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
                Type.Content = "Vzácn" + (item.CanBeEaten ? "á" : "ý");
            }
            else if (item.FindChance <= 20)// 15 - 20
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FF911CC7");//PURPLE - EPIC
                Type.Content = "Epick" + (item.CanBeEaten ? "á" : "ý");
            }
            else// 21 - 26
            {
                Type.Foreground = (Brush)bc.ConvertFrom("#FFC9470B");//ORANGE - LEGENDARY
                Type.Content = "Legendární";
            }

            if (item.CanBeEaten)
            {
                Type.Content += " potravina";
                Eat.Visibility = Visibility.Visible;
            }
            else
            {
                Type.Content += " materiál";
                Eat.Visibility = Visibility.Hidden;
            }

            Amount.Content = item.Amount;
        }

        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void Click_Eat(object sender, RoutedEventArgs e)
        {
            if (viewedItem.Amount <= 1)
            {
                this.NavigationService.GoBack();
            }
            viewedItem.Amount -= 1;
            Amount.Content = viewedItem.Amount;
            App.GameActions.EatConsumable(viewedItem);
        }
        private void Click_Throw(object sender, RoutedEventArgs e)
        {
            App.GameActions.DropItem(viewedItem);
            this.NavigationService.GoBack();
        }

    }
}
