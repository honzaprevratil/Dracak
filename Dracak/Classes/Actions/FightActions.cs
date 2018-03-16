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

            /* ESCAPED */
            if (Attempt >= 3)
            {
                enemy.Aggressivity -= 2;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars(); // player's DB-update
                return true;
            }

            Random randInt = new Random();

            /* ESCAPED */
            if (player.GetBattleSpeed() + randInt.Next(1, 7) > enemy.GetBattleSpeed() + randInt.Next(1, 7))
            {
                enemy.Aggressivity -= 2;
                App.LocationViewModel.DBhelper.UpdateOne(enemy); // enemy's DB-update
                player.InFight = false;
                App.PlayerViewModel.ReRenderBars();// player's DB-update
                return true;
            }

            /* DID'T ESCAPE */
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
            if (enemy.CurrentHealth <= 0)
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
    }
}
