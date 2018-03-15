using Dracak.Classes.AItems;
using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Creatures
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        /* DB HELPER */
        public DBhelper DBhelper;

        public PlayerViewModel()
        {
            DBhelper = new DBhelper();

            this.Player = DBhelper.GetPlayer();
        }

        /* CURRENT PLAYER DATA */
        private Player player;
        public Player Player
        {
            get { return player; }
            set
            {
                player = value;
                ReRenderBars();
            }
        }
        /* RERENDER BARS */
        public void ReRenderBars()
        {
            healthBarValue = (Player.CurrentHealth / Player.MaxHealth) * 100;
            OnPropertyChanged("HealthBarValue");

            hungerBarValue = (Player.CurrentHunger / Player.MaxHunger) * 100;
            OnPropertyChanged("HungerBarValue");

            thirstBarValue = (Player.CurrentThirst / Player.MaxThirst) * 100;
            OnPropertyChanged("ThirstBarValue");

            energyBarValue = (Player.CurrentEnergy / Player.MaxEnergy) * 100;
            OnPropertyChanged("EnergyBarValue");

            UpdatePlayer();
        }

        public void UpdatePlayer()
        {
            DBhelper.UpdateWithChildren(Player);
            DBhelper.UpdateOne(Player.Inventory);
            DBhelper.UpdateList(Player.Inventory.ItemList);
            DBhelper.UpdateOne(Player.PrimaryStats);
        }
        public void UpdateItem(AItem item)
        {
            DBhelper.UpdateItem(item);
        }

        /* PLAYER CURRENT HEALTH BAR VALUE*/
        private double healthBarValue;
        public double HealthBarValue
        {
            get { return healthBarValue; }
            set
            {
                healthBarValue = (Player.CurrentHealth / Player.MaxHealth) * 100;
                OnPropertyChanged("HealthBarValue");
            }
        }

        /* PLAYER CURRENT HUNGER BAR VALUE*/
        private double hungerBarValue;
        public double HungerBarValue
        {
            get { return hungerBarValue; }
            set
            {
                hungerBarValue = (Player.CurrentHunger / Player.MaxHunger) * 100;
                OnPropertyChanged("HungerBarValue");
            }
        }

        /* PLAYER CURRENT THIRST BAR VALUE*/
        private double thirstBarValue;
        public double ThirstBarValue
        {
            get { return thirstBarValue; }
            set
            {
                thirstBarValue = (Player.CurrentThirst / Player.MaxThirst) * 100;
                OnPropertyChanged("ThirstBarValue");
            }
        }

        /* PLAYER CURRENT ENERGY BAR VALUE*/
        private double energyBarValue;
        public double EnergyBarValue
        {
            get { return energyBarValue; }
            set
            {
                energyBarValue = (Player.CurrentEnergy / Player.MaxEnergy) * 100;
                OnPropertyChanged("EnergyBarValue");
            }
        }

        public string GetTextLivingStatus()
        {
            /* TO DO - víc variací oznámení */
            string status = " ";

            /* HEALTH */
            if (Player.CurrentHealth == 0)
                status += "Jsi mrtvý. ";
            else if (Player.CurrentHealth <= Player.MaxHealth / 10)
                status += "Jsi těžce raněný. ";

            /* ENERGY */
            if (Player.CurrentEnergy == 0)
                status += "Jsi totálně vyčerpaný. ";
            else if (Player.CurrentEnergy <= Player.MaxEnergy/10)
                status += "Jsi velice unavený. ";

            /* HUNGER */
            if (Player.CurrentHunger == 0)
                status += "Hladovíš. ";
            else if (Player.CurrentHunger <= Player.MaxHunger / 10)
                status += "Máš velký hlad. ";

            /* THIRST */
            if (Player.CurrentThirst == 0)
                status += "Žízníš. ";
            else if (Player.CurrentThirst <= Player.MaxThirst / 10)
                status += "Máš obrovskou žízeň. ";

            return status;
        }
        /* EVENT AND FUNCTION FOR EVENT */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
