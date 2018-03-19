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
            GC.WaitForPendingFinalizers();
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
            /*IF ARE DATA NOT OK*/
            if (!this.CheckData())
            {
                this.FillPlayerToDB();
                this.FillLocationsToDB();
            }
        }
        public bool CheckData()
        {
            var AllLocations = this.GetAllWithChildren<Location>();
            var AllPlayers = this.GetAllWithChildren<Player>();

            if (AllLocations.Count == 0 || AllPlayers.Count == 0)
                return false;

            return true;
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

            PrimaryStats PlayerStats = new PrimaryStats(5, 5, 5, 5, 5)
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
            Player.DefaultData(5);
            this.InsertWithChildren(Player);
        }
        public void FillLocationsToDB()
        {
            Random randInt = new Random();

            /*SEVERNÍ PLÁŽ*/
            this.InsertWithChildren(new Location
            {
                Id = 1,
                Name = "Severní pláž",
                Image = @"/Dracak;component/Images/Locations/severni-plaz.png",
                Filter = "#4CA49C15",
                Description = "Stojíš na severní pláži. Všude kolem Tebe písek, palmy, kameny a moře. Nehostinné podmínky ostrova na Tebe volají ze všech stran. Co se rozhodneš udělat?",
                FastSearchText = "V levo písek, v pravo písek, v botě? Noha, co jinýho bys tam čekal. Jinak jsi v hromadě písku krom písku našel:",
                SlowSearchText = "Začal jsi kopat v písku jednu díru za druhou. Přece tu něco někde musí být.. Co třeba pirátský poklad? A nebo nějaká stopa po Johnovi... Po 3 hodinách hledání jsi našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(1, 2), new LocationBind(1, 3) },
                ItemList = new List<AItem> {
                    new Weapon("Starý meč", 1, 3, WeaponType.Melee,"Klasický meč, nijak zvlášť ostrý. Avšak co postrádá v ostrosti dohání ve... Stáří.", randInt.Next(4,8), false, 1001, 1001),
                    new Weapon("Šavle", 2, 3, WeaponType.Melee,"Nabroušený zahnutý meč, ideální pro sekání hlav. Že by ho tu nechal nějaký pirát?", randInt.Next(8,15), false, 1001, 1001),
                    new Armor("Banánokroj", 0, 9, "Možná Tě moc neochrání, ale svojí známou kluzkostí banámů zvyšuje tento kroj Tvojí rychlost o krutých 9 bodů.", randInt.Next(21, 26), false, 1001, 1001),
                    new Consumable("Kokosy", randInt.Next(2, 6), true, 5, "Sladký kokosový ořech plný výtečného kokosového mléka. No dobře, i voda je lepší než tohle, ale máš na výběr?", randInt.Next(4, 8), false, 1001, 1001),
                    new Consumable("Kokosy", randInt.Next(1, 7), true, 5, "Sladký kokosový ořech plný výtečného kokosového mléka. No dobře, i voda je lepší než tohle, ale máš na výběr?", randInt.Next(4, 8), false, 1001, 1001),
                    new Consumable("Banány", randInt.Next(1, 4), true, 5, "žluté a zahnuté", randInt.Next(8, 15), false, 1001, 1001),
                },
            });

            /*SEVERNÍ MOŘE*/
            this.InsertWithChildren(new Location
            {
                Id = 2,
                Name = "Severní moře",
                Image = @"/Dracak;component/Images/Locations/severni-more.jpg",
                Filter = "#4CA49C15",
                Description = "Rozhodl ses plavit mořem. Ale jak daleko asi můžeš doplavat bez lodi? No moc daleko to nebude.. Takže vlastně jen stojíš na severní pláži s nohama ve vodě a čumíš směrem do moře.",
                FastSearchText = "Rozhlédnul ses kolem a nějakou chvíli jsi chodil ve vodě, když v tom jsi na dně zahlédl něco krásného a blyštivého... Tvoje zvědavost zmizela, když jsi zjistil, že se jedná o kámen. Krom toho jsi našel:",
                SlowSearchText = "Co chceš v moři důkladně prohledávat?.. Tady stejně nic nenajdeš.. No dobře, tak jsi našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(2, 1)},
                ItemList = new List<AItem> {
                    new Weapon("Kus prkna", 0, 2, WeaponType.Melee, "Promáčený kus prkna.. Počkat, je na něm kus nápisu: \"Titan-\"", randInt.Next(8, 15), false, 1002, 1002),
                    new Weapon("Mečoun", 6, 3, WeaponType.Melee, "Existuje stylovější způsob vraždy než napíchnutí na mrskajícího se mečouna?", randInt.Next(21, 26), false, 1002, 1002),
                    new Consumable("Chaluhy", randInt.Next(1, 6), true, 5, "Ty chceš žrát zelenou trávu z moře? Jako fak?...", randInt.Next(4, 8), false, 1002, 1002),
                    new Consumable("Chaluhy", randInt.Next(1, 6), true, 5, "Ty chceš žrát zelenou trávu z moře? Jako fak?...", randInt.Next(4, 8), false, 1002, 1002),
                    new Consumable("Mrtvá ryba", 1, true, 5, "Datum spotřeby této ryby muselo skončit tak před rokem, protože pěkně smrdí.", randInt.Next(15, 21), false, 1002, 1002),
                },
            });
            this.InsertWithChildren(new Enemy("Medůza", 5, 10, 3, 1, 3, 30,
                "Stojíš v tom zpropadeném moři a vyhlížíš nějakou loď, která by Tě mohla vysvobodit. Najednou Ti projde bolest skrz nohu. Cukneš sebou a když se podíváš dolů, spatříš medůzu, která Tě právě požahala svým otráveným chapadlem.",
                "Jedním bodnutím jsi zničil tohoto chapadlovitého bezbranného protivníka. Avšak jeho žahnutí nebylo vůbec příjemné. Otázka zní, stálo mu to za jeho ubohou smrt?..."
            )
            { LocationId = 2 });

            /*SEVERNÍ LES*/
            this.InsertWithChildren(new Location
            {
                Id = 3,
                Name = "Severní les",
                Image = @"/Dracak;component/Images/Locations/severni-les.jpg",
                Filter = "#579E9823",
                Description = "Vstupuješ do lesa. Vysoké listnaté stromy a hustý travní porost nyní křižují Tvou cestu. A jak tak jdeš tou lesní cestou, vidíš v dáli.. \"Au\".. Nečum do dáli a radši koukej pod nohy na kořeny!..",
                FastSearchText = "Rychle ses porozhlédnul po okolí. Krom mechu na kůře, listů na stromech a kořenů v zemi jsi našel také:",
                SlowSearchText = "Les. Ideální místo na to, něco schovat... Koukáš do dutin stromů, do korun stromů a pod kořeny stromů. A co to vidíš teď? Oválovitou prohlubeň v bahně, kterou zde zanechal někdo, kdo měl boty.. Krom stopy jsi našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(3, 1), new LocationBind(3, 4), new LocationBind(3, 5), new LocationBind(3, 6)},
                ItemList = new List<AItem> {
                    new Weapon("Ubodávač", 4, 5, WeaponType.Melee, "Dlouhé dřevěné kopí zdobené vyřezanými ornamenty... Zbývá jen otázka: \"Jak s ním hodáš zabít svého nepřítele?\" Správně.. Ubodat!", randInt.Next(15,21), false, 1003, 1003),
                    new Consumable("Brusinky", randInt.Next(1, 8), true, 5, "\"Prej sou dobrý na močák.\"", randInt.Next(4, 8), false, 1003, 1003),
                    new Consumable("Maliny", randInt.Next(1, 8), true, 5, "Sladké lesní ovoce", randInt.Next(8, 15), false, 1003, 1003),
                    new Consumable("Ostružiny", randInt.Next(4, 10), true, 5, "Jako maliny, akorát trochu kyselejší a modřejší", randInt.Next(15, 21), false, 1003, 1003),
                    new Consumable("Zelená houba", 1, true, 5, "Co kdybych Ti řekl, že Tě s 50% šancí vyléčí doplna a s 50% šancí zabije?...", randInt.Next(21, 26), false, 1003, 1003),
                },
            });
            this.InsertWithChildren(new Enemy("Vlk", randInt.Next(25, 35), 6, 10, 6, 5, 3,
                "Zrovna sis začínal říkat, že je ideální čas na to, tohle místo opustit.. Když v tom uslyšíš zašustění křoví a do Tvých zad na Tebe skáče zuřivý vlk!.. Utečeš nebo budeš bojovat?",
                "Tělo vlka leží bezvládně na zemi. Bohužel si dovolil na tabulkově silnějšího nepřítele a to se mu moc nevyplatilo. Třeba Tě sundá příští hru, až nebudeš připraven."
            )
            { LocationId = 3 });

            /*ZÁPADNÍ LES*/
            this.InsertWithChildren(new Location
            {
                Id = 4,
                Name = "Západní les",
                Image = @"/Dracak;component/Images/Locations/zapadni-les.jpg",
                Filter = "#44A29A21",
                Description = "Pokračuješ směrem na západ, hlouběji do lesa.",
                FastSearchText = "Hromada větví sem, hromada větví tam.. Vzteky jsi jednu z nich rozkopnul, že se její větve zastavily až o strom vzdálený přibližně 10 metrů. Pod rozkopnutou hromádkou zůstalo:",
                SlowSearchText = "Všimnul sis, že jeden strom, je podrápaný jako od nějaké šelmy. Má docela nízko větve a tak jsi na něj začal lézt. Jak se chytáš rukou v pořadí už asi čtvté větve, všímáš si něčeho, co Tě zaráží - kusu látky. John tady byl a nejspíš utíkal před něčím strašlivým... Dále jsi našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(4, 3), new LocationBind(4, 6), new LocationBind(4, 7) },
                ItemList = new List<AItem> {
                    new Armor("Kožené brnění", 2, 3, "Brnění vyrobeno z pevné kančí kůže", randInt.Next(8, 15), false, 1004, 1004),
                    new Consumable("Brusinky", randInt.Next(1, 8), true, 5, "\"Prej sou dobrý na močák.\"", randInt.Next(4, 8), false, 1004, 1004),
                    new Consumable("Maliny", randInt.Next(1, 8), true, 5, "Sladké lesní ovoce.", randInt.Next(8, 15), false, 1004, 1004),
                    new Consumable("Ostružiny", randInt.Next(4, 10), true, 5, "Jako maliny, akorát trochu kyselejší a modřejší.", randInt.Next(15, 21), false, 1004, 1004),
                    new Consumable("Jahody", randInt.Next(3, 6), true, 5, "Červené královny lesa.", randInt.Next(21, 26), false, 1004, 1004),
                },
            });
            this.InsertWithChildren(new Enemy("Divočák", randInt.Next(35, 45), randInt.Next(5, 8), 8, 3, 8, -5,
                "Tak si tak v klidu sbíráš lesní ovoce, když v tom uslyšíš neúprosné chrochtání několik desítek metrů před sebou. Zvedneš hlavu a spatříš ohromné chlupaté prase, se čpičatými tesákami špinavých od.. Bukvic...",
                "Prase neúprosně kvičí a mrská se v poslední křeči, jakoby ho brali na porážku. Inu vlastně to je to, co ho teď čeká... Smrt."
            )
            { LocationId = 4 });

            /*VÝCHODNÍ LES*/
            this.InsertWithChildren(new Location
            {
                Id = 5,
                Name = "Východní les",
                Image = @"/Dracak;component/Images/Locations/vychodni-les.png",
                Filter = "#59615D22",
                Description = "Vcházíš do východní části lesa. Celou dobu máš takový zvláštní pocit, jako kdyby Tě někdo sledoval. Často se otáčíš ale nikdo tu není. Jenom nehybné stromy. Nebo se snad jeden z nich právě teď pohnul?!",
                FastSearchText = "Jelikož se Ti to tu moc nezamlouvá, nechce se Ti to tu prohledávat dlouho, tak jsi to vzal na rychlovku. Našel jsi těchto pár věcí:",
                SlowSearchText = "Sice se Ti to tu nelíbí, ale když se musí něco prohledat, tak jedině pořádně. .",
                AdjacentLocations = new List<LocationBind> { new LocationBind(5, 3), new LocationBind(5, 6), new LocationBind(5, 7), new LocationBind(5, 10) },
                ItemList = new List<AItem> {
                    new Armor("Hadí kůže", 3, 3, "Lehké brnění vyrobeno z kůže \"nejmenovaných plazivých ještěrů\".", randInt.Next(8, 15), false, 1005, 1005),
                    new Weapon("Flusačka", 5, 1, WeaponType.Ranged, "Dlouhá dutá tyč, zdobená kouzelnými znaky. Součástí sady je také neomezná munice, takže se nemusíš bát, že by došly šipky.", randInt.Next(15,21), false, 1005, 1005),
                    new Consumable("Mochomůrky", randInt.Next(5, 10), true, 5, "\"Třeba budou v tejhle hře jedle ne?..\"", randInt.Next(8, 15), false, 1005, 1005),
                    new Consumable("Hřiby", randInt.Next(1, 8), true, 5, "Usmažil sis je pohledem, takže je nežereš syrové, neboj.", randInt.Next(8, 15), false, 1005, 1005),
                    new Consumable("Hřiby", randInt.Next(3, 5), true, 5, "Usmažil sis je pohledem, takže je nežereš syrové, neboj.", randInt.Next(8, 15), false, 1005, 1005),
                    new Consumable("Borůvky", randInt.Next(4, 10), true, 5, "Malé modré princezny lesa.", randInt.Next(15, 21), false, 1005, 1005),
                    new Consumable("Jahody", randInt.Next(3, 6), true, 5, "Červené královny lesa.", randInt.Next(21, 26), false, 1005, 1005),
                },
            });
            this.InsertWithChildren(new Enemy("Domorodec", randInt.Next(40, 50), randInt.Next(6, 8), 12, 7, 11, 30,
                "Procházíš lesem, všude kolem Tebe ticho. Když v tom Ti kolem hlavy prosviští otrávená šipka. Otočíš se a v tu chvíli na Tebe skočí domorodec se sekyrou v ruce, který Tě před chvílí minul šipkou z flusačky. Boj může začít.",
                "Domorodec schytal poslední ránu a padl k zemi. Při boji s Tebou mu někde vypadnula jeho vzácná flusačka, ale kde přesně to bylo? Hledej..."
            )
            { LocationId = 5 });

            /* HORY */
            this.InsertWithChildren(new Location
            {
                Id = 6,
                Name = "Hory",
                Image = @"/Dracak;component/Images/Locations/hory.png",
                Filter = "#33FFF787",
                Description = "Dorazil jsi ke skalním útesům. Zhruba v půlce hory vidíš jeskyni. Téhle oblasti by bylo lepší se vyhnout a právě proto ses sem vydal. Bude to dlouhá a strmá cesta...",
                FastSearchText = "Rychle ses tu poohlédnul. Do jeskyně se jsi jen tak nahlédnul, protože prohledat jí by bylo nad dlouho. Mezi dvěma kameny jsi našel:",
                SlowSearchText = "Rozhodl ses to tu prohledat. A jako první se vydáváš do jeskyně, jelikož to je místo, jehož tajemství Tě nejvíce zajímají. Hned při vstupu do jeskyně jsi spatřil jeskynní malby. Kdo je tu zanechal? Byla tu někdy civilizace?... Jinak jis našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(6, 5), new LocationBind(6, 4), new LocationBind(6, 3), new LocationBind(6, 7) },
                ItemList = new List<AItem> {
                    new Consumable("Kosti", randInt.Next(5, 10), false, 5, "John si zaslouží důstojný pohřeb.. A nebo alespoň to, co z něj zbylo si ho zaslouží.", randInt.Next(8, 15), false, 1006, 1006),
                    new Armor("Šavlozubka", 4, 6, "Aneb hezká teplákovka z šavlozubiny. Je to retro 10.000 let.", randInt.Next(21, 26), true, 1006, 1006),
                },
            });
            this.InsertWithChildren(new Enemy("Šavlozub", randInt.Next(90, 110), randInt.Next(7, 9), 15, randInt.Next(7, 9), 15, 2,
                "Vysápal ses do půlky skály, kde uviděl jeskyni. Jeskyni před kterou vidíš hromádku lidských kostí.. Přiblížíš se k nim a v hromádce rozeznáváš Johnův deník... Néé!... A aby to nebylo vše, z jeskyně vylézá šavlozub, kterému se dvakrát nezamlouváš. Pomsta může začít...",
                "V zuřivém vzteku jsi této nemilosrdné zubaté kočičce udeřil poslední ránu a poslal jí tak bezvladně k zemi. Šavlozub je mrtev. Msta je dokonána a Johnova duše nyní může v klidu odejít... Což je Tvoje jedinná útěch nad bratrovou smrtí"
            )
            { LocationId = 6 });

            /* JIŽNÍ LES */
            this.InsertWithChildren(new Location
            {
                Id = 7,
                Name = "Jižní les",
                Image = @"/Dracak;component/Images/Locations/jizni-les.jpg",
                Filter = "#3DBDB31D",
                Description = "Další a další stromy.. Je na tomhle zpropadeném ostrově i něco jiného? Jo jasně, krom stromů, písku a moře, je tu taky různá havěť, netoužící po ničem jiném, než po Tvé smrti.",
                FastSearchText = "Rychle jsi prohledal jižní les. Našel jsi:",
                SlowSearchText = "Pomalu prohledáváš jižní les. Našel jsi:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(7, 6), new LocationBind(7, 5), new LocationBind(7, 4), new LocationBind(7, 8) },
                ItemList = new List<AItem> {
                    new Armor("Mechochvoj", 4, 3, "Mechová obrana pro opravdového pána lesů", randInt.Next(15, 21), false, 1007, 1007),
                    new Consumable("Jahody", randInt.Next(1, 3), true, 5, "Červené královny lesa.", randInt.Next(21, 26), false, 1007, 1007),
                    new Consumable("Mochomůrky", randInt.Next(2, 6), true, 5, "\"Třeba budou v tejhle hře jedle ne?..\"", randInt.Next(8, 15), false, 1007, 1007),
                },
            });
            this.InsertWithChildren(new Enemy("Alfa Vlk", randInt.Next(75, 100), randInt.Next(8, 11), 12, randInt.Next(7, 9), randInt.Next(10,20), 10,
                "Krev prvního vlka ještě ani póřádně nezachla v místech, kde jsi ho zabil a Ty už čelíš dalšímu vlkovi. Ovšem ten první mu nejspíše jenom sloužil, protože tenhle je o dost větší. Připrav se!",
                "Jack : vlci, 2:0, wohoo!..."
            )
            { LocationId = 7 });

            /* JIŽNÍ PLÁŽ */
            this.InsertWithChildren(new Location
            {
                Id = 8,
                Name = "Jižní pláž",
                Image = @"/Dracak;component/Images/Locations/jizni-plaz.jpg",
                Filter = "#43635F32",
                Description = "Dorazil jsi na konec tohodle ostrova. Další pláž plná písku ze které by se mohl vztyčit plachty a odplout. No jo, ale kde vezmeš tu loď? Počkat.. Vždyť támhle na břehu jedna stojí!.",
                FastSearchText = "Tahle pláž je mnohem víc bohatá na palmy, kokos sem, banán tam, všude kam se podívám.. Ha há... Krom hromady jídla sis všimnul i lodi, stojící na břehu. Jsi zachráněn?... Z kokosovo-banánové úrody jsi sklidil:",
                SlowSearchText = "Tahle pláž je mnohem víc bohatá na palmy, kokos sem, banán tam, všude kam se podívám.. Ha há... Krom hromady jídla sis všimnul i lodi, stojící na břehu. Jsi zachráněn?... Z kokosovo-banánové úrody jsi sklidil:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(8, 7), new LocationBind(8, 9) },
                ItemList = new List<AItem> {
                    new Weapon("Kokosák", 9, 1, WeaponType.Melee, "Co si hravě strčí mečouna do kapsy? Mega ultimátní kanón střílející celé kokosy. A ano, funguje i na bowlingové koule, výrobce zaručuje 98% úspěšnost na strike.", randInt.Next(21, 26), false, 1008, 1008),
                    new Weapon("Klepeto", 6, 2, WeaponType.Melee, "Obří krabí klepeto. Jen škoda, že už si ho nejspíš moc neužiješ.", randInt.Next(15, 21), false, 1008, 1008),
                    new Consumable("Kokosy", randInt.Next(2, 6), true, 5, "Sladký kokosový ořech plný výtečného kokosového mléka. No dobře, i voda je lepší než tohle, ale máš na výběr?", randInt.Next(4, 8), false, 1008, 1008),
                    new Consumable("Kokosy", randInt.Next(1, 7), true, 5, "Sladký kokosový ořech plný výtečného kokosového mléka. No dobře, i voda je lepší než tohle, ale máš na výběr?", randInt.Next(4, 8), false, 1008, 1008),
                    new Consumable("Kokosy", randInt.Next(3, 8), true, 5, "Sladký kokosový ořech plný výtečného kokosového mléka. No dobře, i voda je lepší než tohle, ale máš na výběr?", randInt.Next(6, 8), false, 1008, 1008),
                    new Consumable("Banány", randInt.Next(1, 4), true, 5, "žluté a zahnuté", randInt.Next(8, 15), false, 1008, 1008),
                    new Consumable("Banány", randInt.Next(1, 4), true, 5, "žluté a zahnuté", randInt.Next(8, 15), false, 1008, 1008),
                    new Consumable("Banány", randInt.Next(3, 8), true, 5, "žluté a zahnuté", randInt.Next(12, 15), false, 1008, 1008),
                },
            });
            this.InsertWithChildren(new Enemy("Obří krab", randInt.Next(100, 120), randInt.Next(5, 7), 5, randInt.Next(9, 11), 20, 2,
                "Jak tak stojíš na jižní pláži a vyhlížíš loď, najednou uslyšíš dupání v písku. Je stále hlasitější a hlasitější.. Když v tom spatříš obrovského běžícího.. Kraba? Co je tohle za hříčku přírody? Kde se tu krucinál vzal dvoumetrový krab???... Připrav se na závěrečný boj.",
                "Mocným úderem do již nakřupnuté krabovi ulity jsi jí roztříštil a v následujících sekundách jsi několikrát brutálně udeřil do tohot oslabeného místa. Krab s odporným pískotem padl k zemi.... Je libo krabí salát či krabí polévka?"
            )
            { LocationId = 8 });

            /* JIŽNÍ MOŘE */
            this.InsertWithChildren(new Location
            {
                Id = 9,
                Name = "Jižní moře",
                Image = @"/Dracak;component/Images/Locations/jizni-more.png",
                Filter = "#56635D0D",
                Description = "Došel jsi až na kraj k jižnímu břehu. Koukáš na pirátskou loď, ale nikde nevidíš posádku. Kde jsou všichni? Zemřeli?.. Na druhou stranu komu to vadí, vždyť loď je teď Tvoje.",
                FastSearchText = "Záleží na tom vůbec? Naskoč na loď a jeď!.. Našel jsi tu:",
                SlowSearchText = "Záleží na tom vůbec? Naskoč na loď a jeď!.. Našel jsi tu:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(9, 8), new LocationBind(9, 11) },
            });

            /*RUINY VESNICE*/
            this.InsertWithChildren(new Location
            {
                Id = 10,
                Name = "Ruiny vesnice",
                Image = @"/Dracak;component/Images/Locations/ruiny-vesnice.jpg",
                Filter = "#3AA49B13",
                Description = "Vcházíš do opuštěné Aztécké stavby. Kdysi to bývala tepna ostrova, s několika stovkami až tisíci obyvatel, avšak dnes jsou pravděpodobně všichni mrtví.. Co se tu stalo? .",
                FastSearchText = "Rychle jsi to tu prohlédnul. Vlezl jsi do několika domů a vše cenné, co tam zbylo, jsi posbíral. Našel jsi:.",
                SlowSearchText = "Procházíš každý dům a snažíš se najít stopy toho, co se tu stalo. Teprve až když se dostáváš do hlavního nejvýznamějšího úbytku, všechno Ti dochází. Na stěně vysí zlatá šavle, na stole je několik lahví rumu. Piráti je zpustošili... Při hledání jsi našel:",
                AdjacentLocations = new List<LocationBind> { new LocationBind(10, 5) },
                ItemList = new List<AItem> {
                    new Armor("Rudý plášť", 1, 5, "Co postrádá na obraně, dohání na stylu.", randInt.Next(8, 15), false, 1010, 1010),
                    new Weapon("Kovaný meč", 3, 3, WeaponType.Ranged, "S příjemnou rukojetí!", randInt.Next(8, 15), false, 1010, 1010),
                    new Armor("Aztécká zbroj", 6, 0, "Těžké aztécké brnění, v celku nemobilní, za to neprůstřelné 9mm... OK to bylo přehnaný.. No i když kdybys stál dostatečně daleko.", randInt.Next(15, 21), false, 1010, 1010),
                    new Weapon("Palcát", 5, 3, WeaponType.Ranged, "Ideální na drcení hlav, lámání kostí a louskání ořechů.", randInt.Next(15, 21), false, 1010, 1010),
                    new Consumable("Chléb", randInt.Next(1, 3), true, 5, "Chléb z pšeničné mouky dle prastarého Aztéckého receptu.", randInt.Next(15, 21), false, 1010, 1010),
                    new Consumable("Chléb", randInt.Next(1, 3), true, 5, "Chléb z pšeničné mouky dle prastarého Aztéckého receptu.", randInt.Next(15, 21), false, 1010, 1010),
                    new Weapon("Drakobijec", 6, 4, WeaponType.Melee, "Tak který drak dneska dostane nakládačku?", randInt.Next(21, 26), false, 1010, 1010),
                },
            });
            this.InsertWithChildren(new Enemy("Pirát", randInt.Next(40, 50), randInt.Next(9, 14), 12, 3, 6, 5,
                "Procházíš domky Aztéků, dávno opuštěné a zpušoné.. Kým? Na to je těžké odpověděť.. Najednou z jednoho domku slyšíš rámus.. Že by odpověď přišla sama?. Avšak k Tvému překvapení z domku vybíhá pirát.",
                "Pirát padá k zemi jako když se potápí loď. Pomalu a bolestivě, jelikož jsi ho ke konci souboje shodil ze sřechy, kde se taky odehrával váš souboj."
            )
            { LocationId = 10 });

            /* LOĎ */
            this.InsertWithChildren(new Location
            {
                Id = 11,
                Name = "Loď",
                Image = @"/Dracak;component/Images/shipstorm.jpg",
                Filter = "#3AA49B13",
                Description = "Nastoupil jsi na loď a odplul jsi z ostrova.",
                FastSearchText = ".",
                SlowSearchText = ".",
                AdjacentLocations = new List<LocationBind> { new LocationBind(11, 9) },
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

            //new Consumable("Jablko", 7, true, 5, "Šťavnaté jablíčko", randInt.Next(4, 8), false, 1001, 1001),
            //new Consumable("Jablko", 5, true, 5, "Šťavnaté jablíčko", randInt.Next(4, 8), false, 1001, 1001),
            //new Consumable("Zlaté Jablko",5,true,5,"Zlaté šťavnaté jablíčko, plné magické energie. Co se asi stane, když ho sníš?",randInt.Next(8,15), false, 1001, 1001),
        }
    }
}
