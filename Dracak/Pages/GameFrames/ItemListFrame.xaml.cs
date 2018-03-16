using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.AItems;
using Dracak.Classes.Locations;
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
    public partial class ItemListFrame : Page
    {
        private int OnPage;
        private int MaxPage;
        private int ItemCounter;

        private string HeaderText;
        private Button[] buttons;
        private Label[] labels;
        private List<AItem> ItemList;

        public ItemListFrame(string header)
        {
            InitializeComponent();
            buttons = new Button[] { But1, But2, But3, But4, But5, But6, But7, But8, But9, But10 };
            labels = new Label[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
            ItemListHeader.Content = header;
            HeaderText = header;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (HeaderText == "Sebrat")
            {
                ItemList = MakeVisibleItemList(App.LocationViewModel.CurrentLocation.ItemList);
            }
            else if (HeaderText == "Předměty")
            {
                ItemList = MakeVisibleItemList(App.PlayerViewModel.Player.Inventory.ItemList);
            }

            OnPage = 1;
            ItemCounter = 0;
            RenderButtons();
        }
        private List<AItem> MakeVisibleItemList(List<AItem> MixedVisiblityList)
        {
            List<AItem> VisibleItems = new List<AItem>();
            foreach (AItem item in MixedVisiblityList)
            {
                if (item.Visiblity)
                {
                    VisibleItems.Add(item);
                }
            }
            return VisibleItems;
        }
        private void RenderButtons()
        {
            /* NO ITEMS FOUND */
            if (ItemList.Count == 0)
            {
                MaxPage = 1;
                NoItem.Visibility = Visibility.Visible;
                PageStatus.Visibility = Visibility.Hidden;
            }
            else
            {
                MaxPage = (int)Math.Ceiling((double)ItemList.Count / 10);
                NoItem.Visibility = Visibility.Hidden;
                PageStatus.Visibility = Visibility.Visible;
            }

            if (MaxPage < OnPage)
            {
                Swap_Page();
            } else
            {
                /* NEXT BUTTON VISIBLITY*/
                ButNext.Visibility = ItemList.Count > 10 ? Visibility.Visible : Visibility.Hidden;
                PageStatus.Content = "stránka\n     " + OnPage.ToString() + "/" + MaxPage.ToString();

                for (int butCtr = 0; butCtr <= 9; butCtr++)
                {
                    if (ItemList.Count > ItemCounter)
                    {
                        try
                        {
                            (buttons[butCtr].Content as Label).Content = ItemList[ItemCounter].GetName();
                        }
                        catch
                        {
                            buttons[butCtr].Visibility = Visibility.Hidden;
                            break;
                        }
                        /* RARITY COLOR */
                        labels[butCtr].Foreground = App.GameActions.ItemActions.GetRarityBrush(ItemList[ItemCounter]);

                        /* SET TOOLTIP*/
                        if (ItemList[ItemCounter] is Weapon)
                        {
                            buttons[butCtr].ToolTip = "Poškození: " + ((Weapon)ItemList[ItemCounter]).Damage + "\nRychlost: " + ItemList[ItemCounter].Speed; // set description
                        }
                        else if (ItemList[ItemCounter] is Armor)
                        {
                            buttons[butCtr].ToolTip = "Absorbce: " + ((Armor)ItemList[ItemCounter]).Defense + "\nRychlost: " + ItemList[ItemCounter].Speed; // set description
                        }
                        else if (ItemList[ItemCounter] is Consumable)
                        {
                            buttons[butCtr].ToolTip = "Množství: " + ((Consumable)ItemList[ItemCounter]).Amount; // set description
                        }

                        buttons[butCtr].CommandParameter = ItemCounter;
                        buttons[butCtr].Visibility = Visibility.Visible;
                        ItemCounter++;
                    }
                    else
                        buttons[butCtr].Visibility = Visibility.Hidden;
                }
            }
        }
        private void ItemClick(int buttonIndex)
        {
            var param = buttons[buttonIndex].CommandParameter;
            int.TryParse(param.ToString(), out int itemIndex);

            if (HeaderText == "Sebrat")
            {
                App.GameActions.ItemActions.PickUp(ItemList[itemIndex]);

                if (MaxPage == OnPage)
                {
                    if (ItemList.Count % 10 == 0 || ItemList.Count % 10 == 1)
                        ItemCounter = ItemCounter - 10;
                    else
                        ItemCounter = ItemCounter - (ItemList.Count % 10);
                }
                else
                {
                    ItemCounter = ItemCounter - 10;
                }

                ItemList.RemoveAt(itemIndex);

                RenderButtons();
            }
            else if (HeaderText == "Předměty")
            {
                if (ItemList[itemIndex] is Weapon || ItemList[itemIndex] is Armor)
                {
                    this.NavigationService.Navigate(new ItemWeaponArmorFrame(ItemList[itemIndex]));
                }
                else if (ItemList[itemIndex] is Consumable)
                {
                    this.NavigationService.Navigate(new ItemConsumableFrame((Consumable)ItemList[itemIndex]));
                }
            }
        }
        private void Swap_Page()
        {
            if (OnPage < MaxPage)
            {
                OnPage++;
            } else
            {
                OnPage = 1;
                ItemCounter = 0;
            }
            RenderButtons();
        }
        private void Click_NextPage(object sender, RoutedEventArgs e)
        {
            Swap_Page();
        }
        private void Click_GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        /* TO DO COMMAND PATERN */
        private void Click_Item1(object sender, RoutedEventArgs e)
        {
            ItemClick(0);
        }
        private void Click_Item2(object sender, RoutedEventArgs e)
        {
            ItemClick(1);
        }
        private void Click_Item3(object sender, RoutedEventArgs e)
        {
            ItemClick(2);
        }
        private void Click_Item4(object sender, RoutedEventArgs e)
        {
            ItemClick(3);
        }
        private void Click_Item5(object sender, RoutedEventArgs e)
        {
            ItemClick(4);
        }
        private void Click_Item6(object sender, RoutedEventArgs e)
        {
            ItemClick(5);
        }
        private void Click_Item7(object sender, RoutedEventArgs e)
        {
            ItemClick(6);
        }
        private void Click_Item8(object sender, RoutedEventArgs e)
        {
            ItemClick(7);
        }
        private void Click_Item9(object sender, RoutedEventArgs e)
        {
            ItemClick(8);
        }
        private void Click_Item10(object sender, RoutedEventArgs e)
        {
            ItemClick(9);
        }
    }
}
