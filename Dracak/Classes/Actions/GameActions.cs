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
        /* ACTIONS FOR FIGHTS */
        public FightActions FightActions = new FightActions();

        /* ACTIONS WITH ITEMS */
        public ItemActions ItemActions = new ItemActions();

        public bool Move(MoveOptions Speed, int AdjacentLocationId)
        {
            Player player = App.PlayerViewModel.Player;

            /*SPEND TIME -- BASED ON MOVE OPTION*/
            switch (Speed)
            {
                case MoveOptions.Walk:
                    player.DoAction(3, 1);
                    break;

                case MoveOptions.March:
                    player.DoAction(2, 3);
                    break;

                case MoveOptions.Run:
                    player.DoAction(1, 6);
                    break;
            }

            int locationId = App.LocationViewModel.CurrentLocation.AdjacentLocations[AdjacentLocationId].BindedId;
            App.LocationViewModel.ChangeLocation(locationId);
            player.InLocationId = locationId;

            if (FightActions.Ambush(player.GetTrueActionCost((int)Speed)))
            {
                return false;
            }

            App.PlayerViewModel.ReRenderBars();
            return true;
        }
        public bool Sleep(int hours)
        {
            var playerVM = App.PlayerViewModel;
            Random randInt = new Random();

            if (FightActions.Ambush(playerVM.Player.GetTrueActionCost(hours / 2)))
            {
                playerVM.Player.Sleep(randInt.Next(0, hours));
                playerVM.ReRenderBars();
                return false;
            }
            playerVM.Player.Sleep(hours);

            playerVM.ReRenderBars();
            App.SlowWriter.StoryFull = "Hodil sis šlofíka na " + hours + (hours < 5 ? " hodiny." : " hodin.") + App.PlayerViewModel.GetTextLivingStatus();

            return true;
        }
    }
}
