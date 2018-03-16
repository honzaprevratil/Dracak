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

        public ActionResult Move(MoveOptions Speed, int AdjacentLocationIndex)
        {
            var playerVM = App.PlayerViewModel;
            var locationVM = App.LocationViewModel;

            /*SPEND TIME -- BASED ON MOVE OPTION*/
            switch (Speed)
            {
                case MoveOptions.Walk:
                    playerVM.Player.DoAction(3, 1);
                    break;

                case MoveOptions.March:
                    playerVM.Player.DoAction(2, 3);
                    break;

                case MoveOptions.Run:
                    playerVM.Player.DoAction(1, 6);
                    break;
            }

            int AdjacentLocationId = locationVM.CurrentLocation.AdjacentLocations[AdjacentLocationIndex].BindedId;
            locationVM.ChangeLocation(AdjacentLocationId);
            playerVM.Player.InLocationId = AdjacentLocationId;

            if (FightActions.Ambush(playerVM.Player.GetTrueActionCost((int)Speed)) == ActionResult.PlayerAmbushed)
            {
                return ActionResult.PlayerAmbushed;
            }

            playerVM.ReRenderBars();

            if (playerVM.Player.IsAlive)
                return ActionResult.SuccessfullyDone;
            else
                return ActionResult.PlayerDied;
        }
        public ActionResult Sleep(int hours)
        {
            var playerVM = App.PlayerViewModel;
            Random randInt = new Random();

            if (FightActions.Ambush(playerVM.Player.GetTrueActionCost(hours / 2)) == ActionResult.PlayerAmbushed)
            {
                playerVM.Player.Sleep(randInt.Next(0, hours));
                playerVM.ReRenderBars();
                return ActionResult.PlayerAmbushed;
            }
            playerVM.Player.Sleep(hours);

            playerVM.ReRenderBars();
            App.SlowWriter.StoryFull = "Hodil sis šlofíka na " + hours + (hours < 5 ? " hodiny." : " hodin.") + App.PlayerViewModel.GetTextLivingStatus();

            if (playerVM.Player.IsAlive)
                return ActionResult.SuccessfullyDone;
            else 
                return ActionResult.PlayerDied;
        }
    }
}
