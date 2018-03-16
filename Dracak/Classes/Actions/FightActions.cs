using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public class FightActions
    {
        /* 
         * ACTIONS FOR FIGHTING
         */
        public ActionResult Ambush(double hours)
        {
            if (App.LocationViewModel.CurrentLocation.Enemy is null)
                return ActionResult.SuccessfullyDone;
            if (!App.LocationViewModel.CurrentLocation.Enemy.IsAlive || App.LocationViewModel.CurrentLocation.Enemy.CurrentHealth <= 0)
                return ActionResult.SuccessfullyDone;

            Random randInt = new Random();
            int rage = App.LocationViewModel.CurrentLocation.Enemy.Aggressivity;
            int dex = App.PlayerViewModel.Player.PrimaryStats.Dextirity;

            if ((rage + randInt.Next(1, 6) + hours) > (dex + randInt.Next(1, 6)))
            {
                App.PlayerViewModel.Player.InFight = true;
                App.PlayerViewModel.ReRenderBars(); // player's render + DB-update
                return ActionResult.PlayerAmbushed;
            }
            else
                return ActionResult.SuccessfullyDone;
        }
        public ActionResult TryEscape(int Attempt)
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var player = App.PlayerViewModel.Player;

            /* ESCAPED */
            if (Attempt >= 3)
            {
                enemy.Aggressivity -= 2;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars(); // player's DB-update
                return ActionResult.SuccessfullyDone;
            }

            Random randInt = new Random();

            /* ESCAPED */
            if (player.GetBattleSpeed() + randInt.Next(1, 7) > enemy.GetBattleSpeed() + randInt.Next(1, 7))
            {
                enemy.Aggressivity -= 2;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars();// player's DB-update
                return ActionResult.SuccessfullyDone;
            }

            /* DID'T ESCAPE */
            App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update

            if (EnemyAtack() == ActionResult.PlayerDied) //Survived?
                return ActionResult.PlayerDied;
            else
                return ActionResult.PlayerAmbushed;
        }

        public ActionResult FightOneRound(bool playerStarts)
        {
            var playerVM = App.PlayerViewModel;
            var locationVM = App.LocationViewModel;
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;

            ActionResult result = ActionResult.SuccessfullyDone;

            if (playerStarts)
            {
                if (PlayerAtack() == ActionResult.EnemyDied)
                    result = ActionResult.EnemyDied;
                else
                {
                    if (EnemyAtack() == ActionResult.PlayerDied)
                        return ActionResult.PlayerDied;
                }
            }
            else
            {
                if (EnemyAtack() == ActionResult.PlayerDied)
                    return ActionResult.PlayerDied;
                else
                {
                    if (PlayerAtack() == ActionResult.EnemyDied)
                        result = ActionResult.EnemyDied;
                }
            }

            /* ENEMY KILLED */
            if (result == ActionResult.EnemyDied)
            {
                App.SlowWriter.StoryFull = enemy.DeathStory; // story write
                playerVM.Player.InFight = false;
                playerVM.Player.StatsPoints += 1; // skill point for kill
                playerVM.ReRenderBars(); // player's render + DB-update
                locationVM.DBhelper.DeleteOne(enemy); // enemy's DB-update
            }

            return result;
        }

        public ActionResult EnemyAtack()
        {
            var enemy = App.LocationViewModel.CurrentLocation.Enemy;
            var playerVM = App.PlayerViewModel;

            double SpeedDifference = enemy.GetBattleSpeed() - playerVM.Player.GetBattleSpeed();

            /* BONUS DAMAGE FOR SPEED */
            if (SpeedDifference >= 5)
                playerVM.Player.TakeDamage(enemy.DealDamage() + 3, true);
            else
                playerVM.Player.TakeDamage(enemy.DealDamage(), true);

            /* SECOND STRIKE */
            if (SpeedDifference >= 10)
                playerVM.Player.TakeDamage(enemy.DealDamage(), true);

            App.PlayerViewModel.ReRenderBars(); // player's render + DB-update

            if (playerVM.Player.IsAlive)
                return ActionResult.SuccessfullyDone;
            else
                return ActionResult.PlayerDied;
        }
        public ActionResult PlayerAtack()
        {
            var loactionVM = App.LocationViewModel;
            var enemy = loactionVM.CurrentLocation.Enemy;
            var playerVM = App.PlayerViewModel;

            double SpeedDifference = enemy.GetBattleSpeed() - playerVM.Player.GetBattleSpeed();

            /* BONUS DAMAGE FOR SPEED */
            if (SpeedDifference <= -5)
                enemy.TakeDamage(playerVM.Player.DealDamage() + 3, true);
            else
                enemy.TakeDamage(playerVM.Player.DealDamage(), true);

            /* SECOND STRIKE */
            if (SpeedDifference <= -10)
                enemy.TakeDamage(playerVM.Player.DealDamage(), true);

            loactionVM.DBhelper.UpdateOne(enemy); // enemy's DB-update

            if (enemy.CurrentHealth > 0) // Is Enemy Alive?
                return ActionResult.SuccessfullyDone;
            else
                return ActionResult.EnemyDied;
        }
    }
}
