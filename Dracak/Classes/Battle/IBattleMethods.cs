using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.Battle
{
    public interface IBattleMethods
    {
        double DealDamage();
        void TakeDamage(double DamageDealt, bool UseArmor);
        double GetDefense();
        double GetBattleSpeed();
        string GetStringDamage();
    }
}
