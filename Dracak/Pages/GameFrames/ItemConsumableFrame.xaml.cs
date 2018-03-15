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
            Brush RarityColorBrush = App.GameActions.GetRarityBrush(viewedItem, Type);
            Name.Foreground = RarityColorBrush;
            Type.Foreground = RarityColorBrush;
            if (viewedItem.FindChance > 7 && viewedItem.FindChance < 21)
                Type.Content += ((viewedItem.CanBeEaten) ? "á" : "ý");

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
