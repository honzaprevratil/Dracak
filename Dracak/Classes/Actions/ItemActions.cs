using Dracak.Classes.AItems;
using Dracak.Classes.Creatures;
using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dracak.Classes
{
    public class ItemActions
    {
        /* 
         * ACTIONS FOR FIGHTS 
         */
        public ActionResult Search(SearchOptions Speed)
        {
            var locationVM = App.LocationViewModel;
            var playerVM = App.PlayerViewModel;
            
            Random randInt = new Random();
            int time = randInt.Next(0, 4);
            int dice = 0;

            /*GET DICE, SPEND TIME -- BASED ON SEARCH OPTION*/
            switch (Speed)
            {
                case SearchOptions.SlowSearch:
                    if (App.GameActions.FightActions.Ambush(playerVM.Player.GetTrueActionCost(3)) == ActionResult.PlayerAmbushed)
                    {
                        playerVM.Player.DoAction(time, time);
                        return ActionResult.PlayerAmbushed;
                    }
                    playerVM.Player.DoAction(3, 3);
                    dice = randInt.Next(6, 12);// 1k6 +5 BONUS FOR SLOW SEARCH
                    break;

                case SearchOptions.FastSearch:
                    if (App.GameActions.FightActions.Ambush(playerVM.Player.GetTrueActionCost(1)) == ActionResult.PlayerAmbushed)
                    {
                        playerVM.Player.DoAction((int)(time / 3), (int)(time / 3));
                        return ActionResult.PlayerAmbushed;
                    }
                    playerVM.Player.DoAction(1, 1);
                    dice = randInt.Next(1, 7);// 1k6
                    break;
            }
            playerVM.ReRenderBars();

            /*TRY FIND ITEM*/
            int counter = 0;
            string foundItems = "";
            List<AItem> LocationItemList = locationVM.CurrentLocation.ItemList.ToList();

            foreach (AItem item in LocationItemList)
            {
                if (TryFindItem(item, counter, dice))
                    foundItems += item.Name + ", ";
                counter++;
            }
            if (foundItems.Length > 0)
            {
                foundItems = foundItems.Substring(0, foundItems.Length - 2);
                if (Speed == SearchOptions.FastSearch)
                    App.SlowWriter.StoryFull = locationVM.CurrentLocation.FastSearchText + foundItems + ".";
                else
                    App.SlowWriter.StoryFull = locationVM.CurrentLocation.SlowSearchText + foundItems + ".";
            }
            else
                if (Speed == SearchOptions.FastSearch)
                App.SlowWriter.StoryFull = locationVM.CurrentLocation.FastSearchText + "velký prd.";
            else
                App.SlowWriter.StoryFull = locationVM.CurrentLocation.FastSearchText + "ztracenou lítost.";

            locationVM.UpdateLocationItemList();
            return ActionResult.SuccessfullyDone;
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

        public void PickUp(AItem pickedItem)
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
        public void Drop(AItem droppedItem)
        {
            int itemIndex = App.PlayerViewModel.Player.Inventory.ItemList.IndexOf(droppedItem);
            droppedItem.LocationId = 1000 + App.PlayerViewModel.Player.InLocationId;
            App.PlayerViewModel.DBhelper.UpdateItem(droppedItem);
            App.LocationViewModel.CurrentLocation.ItemList.Add(droppedItem);
            App.PlayerViewModel.Player.Inventory.ItemList.RemoveAt(itemIndex);
            //App.PlayerViewModel.UpdatePlayer();
        }
        public void Equip(AItem WeaponOrArmor)
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

            /* FREE HIT IF IN FIGHT*/
            if (App.PlayerViewModel.Player.InFight)
                App.GameActions.FightActions.EnemyAtack();

            App.PlayerViewModel.UpdateItem(WeaponOrArmor);
            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
        }

        public ActionResult EatConsumable(Consumable food)
        {
            var playerVM = App.PlayerViewModel;
            Random randInt = new Random();
            switch (food.Name)
            {
                case "Kokosy":
                    playerVM.Player.Eat(1, 2, 0, 0);
                    break;
                case "Banány":
                    playerVM.Player.Eat(3, 0, 0, 3);
                    break;

                case "Chaluhy":
                    playerVM.Player.Eat(0.5, 0, -1, 0);
                    break;
                case "Mrtvá ryba":
                    playerVM.Player.Eat(8, 0, -15, 0);
                    break;

                case "Brusinky":
                    playerVM.Player.Eat(0.5, 1, 0, 0);
                    break;
                case "Maliny":
                    playerVM.Player.Eat(2, 1, 0, 0);
                    break;
                case "Ostružiny":
                    playerVM.Player.Eat(4, 2, 1, 1);
                    break;
                case "Jahody":
                    playerVM.Player.Eat(4, 3, 10, 10);
                    break;
                case "Zelená houba":
                    if (randInt.Next(0, 2) == 1)
                        playerVM.Player.Eat(99, 99, 99, 99);
                    else
                        playerVM.Player.Eat(-99, -99, -99, -99);
                    break;

                case "Mochomůrky":
                    playerVM.Player.Eat(3, 0, -10, 0);
                    break;
                case "Hřiby":
                    playerVM.Player.Eat(3, 0, 0, 0);
                    break;
                case "Borůvky":
                    playerVM.Player.Eat(1, 2, 2, 0);
                    break;
                case "Chléb":
                    playerVM.Player.Eat(5, 0, 0, 4);
                    break;

                case "Jablko":
                    playerVM.Player.Eat(1, 0.5, 0, 0);
                    break;
                case "Zlaté Jablko":
                    playerVM.Player.Eat(2, 1, 3, 3);
                    break;
                default:
                    playerVM.Player.Eat(1, 1, 1, 1);
                    break;
            }

            /* FREE HIT IF IN FIGHT*/
            if (playerVM.Player.InFight)
            {
                if (App.GameActions.FightActions.EnemyAtack() == ActionResult.PlayerDied)
                    return ActionResult.PlayerDied;
            }

            if (food.Amount > 0)
                playerVM.UpdateItem(food);
            else
            {
                playerVM.Player.Inventory.ItemList.Remove(food);
                playerVM.DBhelper.DeleteItem(food);
            }
            playerVM.ReRenderBars(); // player's render + DB-update

            if (playerVM.Player.IsAlive)
                return ActionResult.SuccessfullyDone;
            else
                return ActionResult.PlayerDied;
        }

        /* RARITY COLORS */
        public Brush GetRarityBrush(AItem Item, Label TextRarityLabel = null)
        {
            var bc = new BrushConverter();
            if (Item.FindChance <= 7)// 0 - 7
            {
                if (TextRarityLabel != null)
                    TextRarityLabel.Content = "Normální";
                return (Brush)bc.ConvertFrom("#FF4F4F4F");//WHITE - COMMON
            }
            else if (Item.FindChance <= 14)// 8 - 14 
            {
                if (TextRarityLabel != null)
                    TextRarityLabel.Content = "Vzácn";
                return (Brush)bc.ConvertFrom("#FF0743AC");//BLUE - RARE
            }
            else if (Item.FindChance <= 20)// 15 - 20
            {
                if (TextRarityLabel != null)
                    TextRarityLabel.Content = "Epick";
                return (Brush)bc.ConvertFrom("#FF911CC7");//PURPLE - EPIC
            }
            else// 21 - 26
            {
                if (TextRarityLabel != null)
                    TextRarityLabel.Content = "Legendární";
                return (Brush)bc.ConvertFrom("#FFDE8300");//ORANGE - LEGENDARY
            }
        }
    }
}