using Dracak.Classes.Creatures;
using Dracak.Classes.AItems;
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
    public class GameActions
    {
        public bool Move(MoveOptions Speed, int AdjacentLocationId)
        {
            Player player = App.PlayerViewModel.Player;
            Random randInt = new Random();
            int time = randInt.Next(0, 4);

            /*SPEND TIME -- BASED ON MOVE OPTION*/
            switch (Speed)
            {
                case MoveOptions.Walk:
                    if (Ambush(player.GetTrueActionCost(3)))
                    {
                        player.DoAction(time, (int)(time / 3));
                        return true;
                    }
                    player.DoAction(3, 1);
                    break;

                case MoveOptions.March:
                    if (Ambush(player.GetTrueActionCost(2)))
                    {
                        player.DoAction((int)(time / 1.5), time);
                        return true;
                    }
                    player.DoAction(2, 3);
                    break;

                case MoveOptions.Run:
                    if (Ambush(player.GetTrueActionCost(1)))
                    {
                        player.DoAction((int)(time / 3), time * 2);
                        return true;
                    }
                    player.DoAction(1, 6);
                    break;
            }

            int locationId = App.LocationViewModel.CurrentLocation.AdjacentLocations[AdjacentLocationId].BindedId;
            App.LocationViewModel.ChangeLocation(locationId);
            player.InLocationId = locationId;

            if (Ambush(player.GetTrueActionCost(0)))
            {
                return true;
            }

            App.PlayerViewModel.ReRenderBars();
            return false;
        }

        public bool Search(SearchOptions Speed)
        {
            Player player = App.PlayerViewModel.Player;
            Random randInt = new Random();
            int time = randInt.Next(0, 4);
            int dice = 0;

            /*GET DICE, SPEND TIME -- BASED ON SEARCH OPTION*/
            switch (Speed)
            {
                case SearchOptions.SlowSearch:
                    if (Ambush(player.GetTrueActionCost(3)))
                    {
                        player.DoAction(time, time);
                        return true;
                    }
                    player.DoAction(3, 3);
                    dice = randInt.Next(6, 12);// 1k6 +5 BONUS FOR SLOW SEARCH
                    break;

                case SearchOptions.FastSearch:
                    if (Ambush(player.GetTrueActionCost(1)))
                    {
                        player.DoAction((int)(time / 3), (int)(time / 3));
                        return true;
                    }
                    player.DoAction(1, 1);
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
                if (Speed == SearchOptions.FastSearch)
                    App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.FastSearchText + foundItems + ".";
                else
                    App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.SlowSearchText + foundItems + ".";
            }
            else
                if (Speed == SearchOptions.FastSearch)
                    App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.FastSearchText + "velký prd.";
                else
                    App.SlowWriter.StoryFull = App.LocationViewModel.CurrentLocation.FastSearchText + "ztracenou lítost.";


            App.LocationViewModel.UpdateLocationItemList();
            return false;
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
            droppedItem.LocationId = 1000+App.PlayerViewModel.Player.InLocationId;
            App.PlayerViewModel.DBhelper.UpdateItem(droppedItem);
            App.LocationViewModel.CurrentLocation.ItemList.Add(droppedItem);
            App.PlayerViewModel.Player.Inventory.ItemList.RemoveAt(itemIndex);
            //App.PlayerViewModel.UpdatePlayer();
        }
        public void EatConsumable(Consumable food)
        {
            switch (food.Name)
            {
                case "Jablko":
                    App.PlayerViewModel.Player.Eat(1, 0.5, 0, 0);
                    break;
                case "Zlaté Jablko":
                    App.PlayerViewModel.Player.Eat(2, 1, 3, 3);
                    break;
                default:
                    App.PlayerViewModel.Player.Eat(1, 1, 1, 1);
                    break;
            }

            /* FREE HIT IF IN FIGHT*/
            if (App.PlayerViewModel.Player.InFight)
                EnemyAtack();

            if (food.Amount > 0)
            {
                App.PlayerViewModel.UpdateItem(food);
            } else
            {
                App.PlayerViewModel.Player.Inventory.ItemList.Remove(food);
                App.PlayerViewModel.DBhelper.DeleteItem(food);
            }
            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
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

            /* FREE HIT IF IN FIGHT*/
            if (App.PlayerViewModel.Player.InFight)
                EnemyAtack();

            App.PlayerViewModel.UpdateItem(WeaponOrArmor);
            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
        }

        public void Sleep(int hours)
        {
            App.PlayerViewModel.Player.Sleep(hours);
            App.PlayerViewModel.ReRenderBars();
            App.SlowWriter.StoryFull = "Hodil sis šlofíka na " + hours + (hours < 5 ? "hodiny." : "hodin.") + App.PlayerViewModel.GetTextLivingStatus();
        }



        /* FIGHTS */
        /* FIGHTS */
        /* FIGHTS */
        public bool Ambush(double hours)
        {
            if (App.LocationViewModel.CurrentLocation.Enemy is null)
                return false;
            if (!App.LocationViewModel.CurrentLocation.Enemy.IsAlive || App.LocationViewModel.CurrentLocation.Enemy.CurrentHealth <= 0)
                return false;

            Random randInt = new Random();
            int rage = App.LocationViewModel.CurrentLocation.Enemy.Aggressivity;
            int dex = App.PlayerViewModel.Player.PrimaryStats.Dextirity;

            if ((rage + randInt.Next(1, 6) + hours) > (dex + randInt.Next(1, 6)))
            {
                App.PlayerViewModel.Player.InFight = true;
                App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
                return true;
            }
            else
                return false;
        }
        public bool TryEscape(int Attempt)
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var player = App.PlayerViewModel.Player;
            enemy.Aggressivity -= 1;

            if (Attempt >= 3)
            {
                enemy.Aggressivity -= 1;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars(); // player's DB-update
                return true;
            }

            Random randInt = new Random();

            if (player.GetBattleSpeed() + randInt.Next(1, 7) > enemy.GetBattleSpeed() + randInt.Next(1, 7))
            {
                enemy.Aggressivity -= 1;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars();// player's DB-update
                return true;
            }

            App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
            EnemyAtack();
            return false;
        }

        public void FightOneRound(bool playerStarts)
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var player = App.PlayerViewModel.Player;
            double SpeedDifference = enemy.GetBattleSpeed() - App.PlayerViewModel.Player.GetBattleSpeed();

            if (playerStarts)
            {
                PlayerAtack();
                if (enemy.CurrentHealth > 0) // IsAlive
                    EnemyAtack();
            }
            else
            {
                EnemyAtack();
                if (player.IsAlive)
                    PlayerAtack();
            }

            /* ENEMY KILLED */
            if (enemy.CurrentHealth < 0)
            {
                App.SlowWriter.StoryFull = enemy.DeathStory; // story write

                player.InFight = false;
                player.StatsPoints += 1; // skill point for kill
                App.LocationViewModel.DBhelper.DeleteOne(enemy); // enemy's DB-update
                App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
            }
        }

        public void EnemyAtack()
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var player = App.PlayerViewModel.Player;
            double SpeedDifference = enemy.GetBattleSpeed() - App.PlayerViewModel.Player.GetBattleSpeed();

            /* BONUS DAMAGE FOR SPEED */
            if (SpeedDifference >= 5)
                player.TakeDamage(enemy.DealDamage() + 3, true);
            else
                player.TakeDamage(enemy.DealDamage(), true);

            /* SECOND STRIKE */
            if (SpeedDifference >= 10)
                player.TakeDamage(enemy.DealDamage(), true);

            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
        }
        public void PlayerAtack()
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var player = App.PlayerViewModel.Player;
            double SpeedDifference = enemy.GetBattleSpeed() - App.PlayerViewModel.Player.GetBattleSpeed();

            /* BONUS DAMAGE FOR SPEED */
            if (SpeedDifference <= -5)
                enemy.TakeDamage(player.DealDamage() + 3, true);
            else
                enemy.TakeDamage(player.DealDamage(), true);

            /* SECOND STRIKE */
            if (SpeedDifference <= -10)
                enemy.TakeDamage(player.DealDamage(), true);

            App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
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
