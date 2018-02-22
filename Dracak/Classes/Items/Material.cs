using Dracak.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak
{
    public class Material : AItem
    {
        public bool Mined { get; set; }
        public int[] IdNeedToMine { get; set; }
        public int[] CanBeCombinedWith { get; set; }
    }
}
