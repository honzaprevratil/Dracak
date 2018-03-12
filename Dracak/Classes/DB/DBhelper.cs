using Dracak.Classes.Creatures;
using Dracak.Classes.Items;
using Dracak.Pages;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dracak.Classes.Locations
{
    public class DBhelper
    {
        /* DATABASE WORKER
         * work with database
         * fills start data
         * creates tables, insert, update, select */

        private SQLiteConnection db;

        public DBhelper()
        {
            Connect();

            var AllLocations = this.GetAllWithChildren<Location>();

            if (AllLocations.Count == 0)
            {
                this.FillPlayerToDB();
                this.FillLocationsToDB();
            }
        }
        public void Connect()
        {
            db = new SQLiteConnection("database.db");

            try
            {
                /*LOACTIONS TABLES*/
                db.CreateTable<Location>();
                db.CreateTable<LocationBind>();

                /*ITEMS TABLES*/
                db.CreateTable<Armor>();
                db.CreateTable<Consumable>();
                db.CreateTable<Weapon>();
                db.CreateTable<AItem>();

                /* CREATURES TABLES */
                db.CreateTable<Creature>();
                db.CreateTable<PrimaryStats>();
                db.CreateTable<Inventory>();
                db.CreateTable<Player>();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Database loading failed. Try to repair the file or remove it.", "Database load error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
        public void CloseConnection()
        {
            db.Close();
        }
        public void InsertWithChildren<T>(T table) where T : ATable, new()
        {
            db.InsertWithChildren(table, true);
        }
        public List<T> GetAllWithChildren<T>() where T : ATable, new()
        {
            return db.GetAllWithChildren<T>().ToList();
        }
        public Location GetLocationById(int id = 1)
        {
            Location location = GetWithChildrenById<Location>(id);
            location.ItemList = GetItemListByOwnerId(1000+id);
            return location;
        }
        public Player GetPlayer()
        {
            Player Player = GetWithChildrenById<Player>(1);
            Player.Inventory.ItemList = GetItemListByOwnerId(9999);

            /* LocationId = 9999 ==>> item in player's inventory */
            /* InventoryId = 1 ==>> equpiped item */

            var PlayerWeapon = db.Table<Weapon>().Where(v => v.LocationId == 9999 && v.InventoryId == 1);
            foreach (Weapon weapon in PlayerWeapon)
            {
                Player.Inventory.UsingWeapon = weapon;
                break;
            }
            var PlayerArmor = db.Table<Armor>().Where(v => v.LocationId == 9999 && v.InventoryId == 1);
            foreach (Armor armor in PlayerArmor)
            {
                Player.Inventory.UsingArmor = armor;
                break;
            }
            Player.RefreshMaximumValues();

            return Player;
        }
        public List<AItem> GetItemListByOwnerId(int id)
        {
            List<AItem> itemsList = new List<AItem>();

            List<Weapon> DataList1 = db.Table<Weapon>().Where(v => v.LocationId == id).ToList();
            foreach (Weapon weapon in DataList1)
            {
                itemsList.Add(weapon);
            }
            List<Armor> DataList2 = db.Table<Armor>().Where(v => v.LocationId == id).ToList();
            foreach (Armor armor in DataList2)
            {
                itemsList.Add(armor);
            }
            List<Consumable> DataList3 = db.Table<Consumable>().Where(v => v.LocationId == id).ToList();
            foreach (Consumable consumable in DataList3)
            {
                itemsList.Add(consumable);
            }

            return itemsList;
        }
        public T GetWithChildrenById<T>(int id) where T : ATable, new()
        {
            return db.GetWithChildren<T>(id);
        }
        public void UpdateWithChildren<T>(T table) where T : class, new()
        {
            db.UpdateWithChildren(table);
        }
        public void UpdateOne<T>(T table) where T : class, new()
        {
            db.Update(table);
        }
        public void UpdateItem(AItem table)
        {
            db.Update(table);
        }
        public void DeleteItem(AItem table)
        {
            db.Delete(table);
        }
        public void UpdateList<AItem>(List<AItem> ItemList)
        {
            foreach (AItem table in ItemList)
                db.Update(table);
        }
        public Dictionary<int, string> GetLocationDict()
        {
            Dictionary<int, string> LocationsDict = new Dictionary<int, string>();
            var locations = db.Table<Location>();

            foreach(Location location in locations)
            {
                LocationsDict.Add(location.Id, location.Name);
            }

            return LocationsDict;
        }


        /* START DATA */
        public void FillPlayerToDB()
        {
            Weapon PlayerWeapon = new Weapon("Nůž", 1, 1, WeaponType.Melee, "Malý kapesní nožík", 0, true, 9999, 1);
            Armor PlayerArmor = new Armor("Oblečení", 1, 1, "Klasické hadry", 0, true, 9999, 1);

            Consumable PlayerItem1 = new Consumable("Jablko", 7, true, 5, "popis", 9, true, 9999, 9999);
            Weapon PlayerItem2 = new Weapon("Meč", 1, 3, WeaponType.Melee, "popis", 8, true, 9999, 9999);

            Inventory PlayerInventory = new Inventory() {
                Id = 1,
                ItemList = new List<AItem>() {
                        PlayerWeapon,
                        PlayerArmor,
                        PlayerItem1,
                        PlayerItem2
                    },
                CreatureId = 1,
                UsingArmor = PlayerArmor,
                UsingWeapon = PlayerWeapon,
            };
            this.InsertWithChildren(PlayerInventory);

            PrimaryStats PlayerStats = new PrimaryStats(4, 4, 4, 4, 4)
            {
                CreatureId = 1,
            };
            this.InsertWithChildren(PlayerStats);

            var Player = new Player(4, 4, 4, 4, 4)
            {
                Id = 1,
                InLocationId = 1,
                Inventory = PlayerInventory,
                PrimaryStats = PlayerStats,
            };
            this.InsertWithChildren(Player);
        }
        public void FillLocationsToDB()
        {
            this.InsertWithChildren(new Location
            {
                Id = 1,
                Name = "Severní pláž",
                Image = @"/Dracak;component/Images/Locations/severni-plaz.png",
                Filter = "#4CA49C15",
                Description = "Stojíš na severní pláži. Všude kolem Tebe písek, palmy, kameny a moře. Nehostinné podmínky ostrova na Tebe volají ze všech stran. Co se rozhodneš udělat?",
                FastSearchText = "Rychle jsi prohledal Severní pláž",
                SlowSearchText = "Důkladně prohledáváš Severní pláž",
                AdjacentLocations = new List<LocationBind> { new LocationBind(1, 2), new LocationBind(1, 3) },
                ItemList =  new List<AItem> {
                    new Weapon("Meč", 1, 3, WeaponType.Melee,"popis", 8, false, 1001, 1001),
                    new Weapon("Šavle", 3, 3, WeaponType.Melee,"popis", 20, false, 1001, 1001),
                    new Armor("Kožené brnění",5,5,"popis", 8, false, 1001, 1001),
                    new Consumable("Jablko",7,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",5,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",6,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",8,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",5,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"popis",22, false, 1001, 1001),
                    new Consumable("Jablko",13,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",9,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",11,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"popis",14, false, 1001, 1001),
                    new Consumable("Jablko",4,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",12,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",6,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",14,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"popis",12, false, 1001, 1001),
                    new Consumable("Jablko",5,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",8,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",5,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",9,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"popis",15, false, 1001, 1001),
                    new Consumable("Jablko",1,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",21,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",9,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"popis",13, false, 1001, 1001),
                    new Consumable("Jablko",365,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",7,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",4,true,5,"popis",9, false, 1001, 1001),
                    new Consumable("Jablko",8,true,5,"popis",9, false, 1001, 1001),
                },
            });
            this.InsertWithChildren(new Location
            {
                Id = 2,
                Name = "Severní moře",
                Image = @"/Dracak;component/Images/Locations/severni-more.jpg",
                Filter = "#4CA49C15",
                Description = "Rozhodl ses plavit mořem.",
                FastSearchText = "Rychle jsi prohledal Severní moře",
                SlowSearchText = "Důkladně prohledáváš Severní moře",
                AdjacentLocations = new List<LocationBind> { new LocationBind(2, 1)},
            });
            this.InsertWithChildren(new Location
            {
                Id = 3,
                Name = "Severní les",
                Image = @"/Dracak;component/Images/Locations/severni-les.jpg",
                Filter = "#7FA49C15",
                Description = "Rozhodl jít lesem",
                FastSearchText = "Rychle jsi prohledal Severní les",
                SlowSearchText = "Důkladně prohledáváš Severní les",
                AdjacentLocations = new List<LocationBind> { new LocationBind(3, 1) },
            });
            /*
             * HERE WILL BE ALL LOCATIONS
             */
        }
    }
}
