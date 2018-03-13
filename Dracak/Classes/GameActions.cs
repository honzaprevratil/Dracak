using Dracak.Classes.Items;
using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public class GameActions
    {
        public void Move(MoveOptions Speed, int AdjacentLocationId)
        {
            /*SPEND TIME -- BASED ON MOVE OPTION*/
            switch (Speed)
            {
                case MoveOptions.Walk:
                    App.PlayerViewModel.Player.DoAction(3, 1);
                    break;

                case MoveOptions.March:
                    App.PlayerViewModel.Player.DoAction(2, 3);
                    break;

                case MoveOptions.Run:
                    App.PlayerViewModel.Player.DoAction(1, 6);
                    break;
            }
            App.PlayerViewModel.ReRenderBars();

            int locationId = App.LocationViewModel.CurrentLocation.AdjacentLocations[AdjacentLocationId].BindedId;
            App.LocationViewModel.ChangeLocation(locationId);
            App.PlayerViewModel.Player.InLocationId = locationId;
        }

        public void Search(SearchOptions Speed)
        {
            Random randInt = new Random();
            int dice = 0;

            /*GET DICE, SPEND TIME -- BASED ON SEARCH OPTION*/
            switch (Speed)
            {
                case SearchOptions.SlowSearch:
                    App.PlayerViewModel.Player.DoAction(3, 3);
                    dice = randInt.Next(6, 12);// 1k6 +5 BONUS FOR SLOW SEARCH
                    break;

                case SearchOptions.FastSearch:
                    App.PlayerViewModel.Player.DoAction(1, 1);
                    dice = randInt.Next(1, 7);// 1k6
                    break;
            }
            App.PlayerViewModel.ReRenderBars();

            /*TRY FIND ITEM*/
            int counter = 0;
            string foundItems = "";
            List<AItem> LocationItemList = App.LocationViewModel.CurrentLocation.ItemList.ToList();

            foreach (AItem item in LocationItemList)
            {
                if (TryFindItem(item, counter, dice))
                    foundItems += item.Name + ", ";
                counter++;
            }
            if (foundItems.Length > 0)
            {
                foundItems = foundItems.Substring(0, foundItems.Length - 2);
                App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.FastSearchText + " Našel jsi: " + foundItems+".";
            }
            else
                App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.FastSearchText + " Nic jsi neneašel.";

            App.LocationViewModel.UpdateLocationItemList();
        }
        public bool TryFindItem(AItem item, int itemIndex, int bonusChance = 0)
        {
            if (item.Visiblity == true)
                return true;

            if ((App.PlayerViewModel.Player.PrimaryStats.Inteligence + bonusChance) > item.FindChance)
            {
                item.Visiblity = true;
                App.LocationViewModel.CurrentLocation.ItemList[itemIndex] = item;
                return true;
            }
            return false;
        }

        public void PickUpItem(AItem pickedItem)
        {
            int itemIndex = App.LocationViewModel.CurrentLocation.ItemList.IndexOf(pickedItem);

            /* ADD ITEM TO LIST */
            bool DeleteFromDb = false;

            foreach (AItem playersItem in App.PlayerViewModel.Player.Inventory.ItemList)
            {
                if (playersItem.Name == pickedItem.Name)
                {
                    /* STACK CONSUMABLE ITEM */
                    if (playersItem is Consumable && pickedItem is Consumable)
                    {
                        (playersItem as Consumable).Amount += (pickedItem as Consumable).Amount;
                        DeleteFromDb = true;
                        App.PlayerViewModel.UpdateItem(playersItem);
                        App.PlayerViewModel.UpdatePlayer();
                        break;
                    }
                }
            }

            if (DeleteFromDb)
            {
                App.LocationViewModel.DeleteItemFromLocation(itemIndex);// deletes DB
            }
            else
            {
                App.PlayerViewModel.Player.Inventory.ItemList.Add(pickedItem);
                App.LocationViewModel.PickUpItemFromLocation(itemIndex);// updates DB
            }

            /* REMOVES ITEM FROM LIST */
            App.LocationViewModel.CurrentLocation.ItemList.RemoveAt(itemIndex);
        }
        public void DropItem(AItem droppedItem)
        {
            int itemIndex = App.PlayerViewModel.Player.Inventory.ItemList.IndexOf(droppedItem);

            App.LocationViewModel.CurrentLocation.ItemList.Add(droppedItem);
            App.LocationViewModel.UpdateLocationItemList();
            App.PlayerViewModel.Player.Inventory.ItemList.RemoveAt(itemIndex);
            App.PlayerViewModel.UpdatePlayer();
        }
        public void EatConsumable(Consumable food)
        {
            App.PlayerViewModel.Player.Eat(food);
            if (food.Amount > 1)
            {
                App.PlayerViewModel.UpdateItem(food);
            } else
            {
                App.PlayerViewModel.Player.Inventory.ItemList.Remove(food);
            }
            App.PlayerViewModel.ReRenderBars();
        }
        
        public void EquipItem (AItem WeaponOrArmor)
        {
            WeaponOrArmor.InventoryId = 1;

            if (WeaponOrArmor is Weapon)
            {
                Weapon OldWeapon = App.PlayerViewModel.Player.Inventory.UsingWeapon;
                OldWeapon.InventoryId = 9999;
                App.PlayerViewModel.UpdateItem(OldWeapon);

                App.PlayerViewModel.Player.Inventory.UsingWeapon = (Weapon)WeaponOrArmor;
            }
            else if (WeaponOrArmor is Armor)
            {
                Armor OldArmor = App.PlayerViewModel.Player.Inventory.UsingArmor;
                OldArmor.InventoryId = 9999;
                App.PlayerViewModel.UpdateItem(OldArmor);

                App.PlayerViewModel.Player.Inventory.UsingArmor = (Armor)WeaponOrArmor;
            }

            App.PlayerViewModel.UpdateItem(WeaponOrArmor);
        }

        public void Sleep(int hours)
        {
            App.PlayerViewModel.Player.Sleep(hours);
            App.PlayerViewModel.ReRenderBars();
            App.SlowWriter.StoryFull = "Hodil sis šlofíka na " + hours + (hours < 5 ? "hodiny." : "hodin.") + App.PlayerViewModel.GetTextLivingStatus();
        }
    }
}
