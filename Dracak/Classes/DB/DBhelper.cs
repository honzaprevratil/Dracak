using Dracak.Classes.Creatures;
using Dracak.Classes.AItems;
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
            FillAllDefaultData();
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
                db.CreateTable<Enemy>();
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
            GC.Collect();
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

            var PlayerWeapon = Player.Inventory.ItemList.Where(v => v.InventoryId == 1);
            foreach (AItem weapon in PlayerWeapon)
            {
                if (weapon is Weapon)
                {
                    Player.Inventory.UsingWeapon = (Weapon)weapon;
                    break;
                }
            }
            var PlayerArmor = Player.Inventory.ItemList.Where(v => v.InventoryId == 1);
            foreach (AItem armor in PlayerArmor)
            {
                if (armor is Armor)
                {
                    Player.Inventory.UsingArmor = (Armor)armor;
                    break;
                }
            }

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
        public void DeleteOne<T>(T table) where T : class, new()
        {
            db.Delete(table);
        }
        public void DeleteAllFromDB()
        {
            /*LOACTIONS TABLES*/
            db.DeleteAll<Location>();
            db.DeleteAll<LocationBind>();
            /*ITEMS TABLES*/
            db.DeleteAll<Armor>();
            db.DeleteAll<Consumable>();
            db.DeleteAll<Weapon>();
            db.DeleteAll<AItem>();
            /* CREATURES TABLES */
            db.DeleteAll<Creature>();
            db.DeleteAll<Enemy>();
            db.DeleteAll<PrimaryStats>();
            db.DeleteAll<Inventory>();
            db.DeleteAll<Player>();
            /* DEFAULT DATA */
            FillAllDefaultData();
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
        public void FillAllDefaultData()
        {
            var AllLocations = this.GetAllWithChildren<Location>();

            if (AllLocations.Count == 0)
            {
                this.FillPlayerToDB();
                this.FillLocationsToDB();
            }
        }
        public void FillPlayerToDB()
        {
            Weapon PlayerWeapon = new Weapon("Nůž", 1, 1, WeaponType.Melee, "Malý kapesní nožík", 0, true, 9999, 1);
            Armor PlayerArmor = new Armor("Oblečení", 1, 1, "Klasické hadry", 0, true, 9999, 1);

            Consumable PlayerItem1 = new Consumable("Jablko", 7, true, 5, "popis", 9, true, 9999, 9999);

            Inventory PlayerInventory = new Inventory() {
                Id = 1,
                ItemList = new List<AItem>() {
                        PlayerWeapon,
                        PlayerArmor,
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

            var Player = new Player()
            {
                Id = 1,
                InLocationId = 1,
                InFight = false,
                Inventory = PlayerInventory,
                PrimaryStats = PlayerStats,
            };
            Player.DefaultData(10);
            this.InsertWithChildren(Player);
        }
        public void FillLocationsToDB()
        {
            Random randInt = new Random();


            this.InsertWithChildren(new Location
            {
                Id = 1,
                Name = "Severní pláž",
                Image = @"/Dracak;component/Images/Locations/severni-plaz.png",
                Filter = "#4CA49C15",
                Description = "Stojíš na severní pláži. Všude kolem Tebe písek, palmy, kameny a moře. Nehostinné podmínky ostrova na Tebe volají ze všech stran. Co se rozhodneš udělat?",
                FastSearchText = "V levo písek, v pravo písek, v botě? Noha, co jinýho bys tam čekal. V hromadě písku jsi krom písku našel: ",
                SlowSearchText = "Začal jsi kopat v písku jednu díru za druhou. Přece tu něco někde musí být.. Co třeba pirátský poklad?... Po 3 hodinách hledání jsi našel: ",
                AdjacentLocations = new List<LocationBind> { new LocationBind(1, 2), new LocationBind(1, 3) },
                ItemList = new List<AItem> {
                    new Weapon("Meč", 1, 3, WeaponType.Melee,"Klasický meč, nijak zvlášť ostrý.", randInt.Next(4,8), false, 1001, 1001),
                    new Weapon("Šavle", 2, 3, WeaponType.Melee,"Nabroušený zahnutý meč, ideální pro sekání hlav.", randInt.Next(8,15), false, 1001, 1001),
                    new Weapon("Ubodávač", 4, 5, WeaponType.Melee, "Jak s ním hodáš zabít svého nepřítele? Správně.. (Ubodat)", randInt.Next(15,21), false, 1001, 1001),
                    new Weapon("Drakobijec", 6, 4, WeaponType.Melee, "Tak který drak dneska dostane nakládačku?", randInt.Next(21,26), false, 1001, 1001),
                    new Armor("Kožené brnění", 2, 1, "Brnění vyrobeno z pevné kůže", randInt.Next(4,8), false, 1001, 1001),
                    new Consumable("Jablko",7,true,5,"Šťavnaté jablíčko", randInt.Next(4,8), false, 1001, 1001),
                    new Consumable("Jablko",5,true,5,"Šťavnaté jablíčko", randInt.Next(4,8), false, 1001, 1001),
                    new Consumable("Zlaté Jablko",5,true,5,"Zlaté šťavnaté jablíčko, plné magické energie. Co se asi stane, když ho sníš?",randInt.Next(8,15), false, 1001, 1001),
                },
            });
            this.InsertWithChildren(new Enemy("Vlk", 30, 6, 10, 6, 5,
                "Zrovna sis začínal říkat, že je ideální čas na to, tohle místo opustit.. Když v tom uslyšíš zašustění křoví a do Tvých zad na Tebe skáče zuřivý vlk!.. Utečeš nebo budeš bojovat?",
                "Tělo vlka leží bezvládně na zemi. Bohužel si dovolil na tabulkově silnějšího nepřítele a to se mu moc nevyplatilo. Třeba Tě sundá příští hru, až nebudeš připraven."
            ) { LocationId = 1 });

            this.InsertWithChildren(new Location
            {
                Id = 2,
                Name = "Severní moře",
                Image = @"/Dracak;component/Images/Locations/severni-more.jpg",
                Filter = "#4CA49C15",
                Description = "Rozhodl ses plavit mořem. Ale jak daleko asi můžeš doplavat bez lodi? No moc daleko to nebude.. Takže vlastně jen stojíš na severní pláži s nohama ve vodě a čumíš směrem do moře.",
                FastSearchText = "Rozhlédnul ses kolem nějakou chvíli jsi chodil ve vodě, když v tom jsi na dně v kromadě toho písku zahlédl... Kámen. Krom toho jsi našel:",
                SlowSearchText = "Co chceš v moři důkladně prohledávat?.. Tady stejně nic nenajdeš.. No dobře, tak jsi našel:.",
                AdjacentLocations = new List<LocationBind> { new LocationBind(2, 1)},
            });
            this.InsertWithChildren(new Location
            {
                Id = 3,
                Name = "Severní les",
                Image = @"/Dracak;component/Images/Locations/severni-les.jpg",
                Filter = "#7FA49C15",
                Description = "Rozhodl ses jít lesem. Vysoké listnaté stromy a hustý travní porost nyní křižují Tvou cestu. ",
                FastSearchText = "Rychle jsi prohledal Severní les.",
                SlowSearchText = "Důkladně prohledáváš Severní les.",
                AdjacentLocations = new List<LocationBind> { new LocationBind(3, 1), new LocationBind(3, 4), new LocationBind(3, 5), new LocationBind(3, 6)},
            });

            this.InsertWithChildren(new Location
            {
                Id = 4,
                Name = "Západní les",
                Image = @"/Dracak;component/Images/Locations/zapadni-les.jpg",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(4, 3), new LocationBind(4, 6), new LocationBind(4, 7), new LocationBind(4, 10) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 5,
                Name = "Východní les",
                Image = @"/Dracak;component/Images/Locations/vychodni-les.jpg",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(5, 3), new LocationBind(5, 6), new LocationBind(5, 7) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 6,
                Name = "Hory",
                Image = @"/Dracak;component/Images/Locations/hory.png",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(6, 5), new LocationBind(6, 4), new LocationBind(6, 3), new LocationBind(6, 7) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 7,
                Name = "Jižní les",
                Image = @"/Dracak;component/Images/Locations/jizni-les.jpg",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(7, 6), new LocationBind(7, 5), new LocationBind(7, 4), new LocationBind(7, 8) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 8,
                Name = "Jižní pláž",
                Image = @"/Dracak;component/Images/Locations/jizni-plaz.jpg",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(8, 7), new LocationBind(8, 9) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 9,
                Name = "Jižní moře",
                Image = @"/Dracak;component/Images/Locations/jizni-more.png",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(9, 8) },
            });

            this.InsertWithChildren(new Location
            {
                Id = 10,
                Name = "Ruiny vesnice",
                Image = @"/Dracak;component/Images/Locations/ruiny-vesnice.jpg",
                Filter = "#4CA49C15",
                Description = ".",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(10, 4) },
            });
            //this.InsertWithChildren(new Location
            //{
            //    Id = 3,
            //    Name = "",
            //    Image = @"/Dracak;component/Images/Locations/",
            //    Filter = "#4CA49C15",
            //    Description = ".",
            //    FastSearchText = ".",
            //    SlowSearchText = ".",
            //    AdjacentLocations = new List<LocationBind> { new LocationBind(3, 1) },
            //});
        }
    }
}
