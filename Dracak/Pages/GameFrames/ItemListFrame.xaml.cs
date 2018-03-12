using Dracak.Classes;
using Dracak.Classes.Creatures;
using Dracak.Classes.Items;
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
        private List<AItem> ItemList;

        public ItemListFrame(List<AItem> itemList, string header)
        {
            InitializeComponent();
            OnPage = 1;
            ItemCounter = 0;
            buttons = new Button[] { But1, But2, But3, But4, But5, But6, But7, But8, But9, But10 };

            ItemList = MakeVisibleItemList(itemList);
            ItemListHeader.Content = header;
            HeaderText = header;

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
                PageStatus.Content = OnPage.ToString() + "/" + MaxPage.ToString();

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
                App.GameActions.PickUpItem(ItemList[itemIndex]);

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
                if (ItemList[itemIndex] is Weapon)
                {
                    this.NavigationService.Navigate(new ItemDetailWeaponFrame((Weapon)ItemList[itemIndex]));
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
