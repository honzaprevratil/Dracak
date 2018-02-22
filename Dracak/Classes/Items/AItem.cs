using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public abstract class AItem
    {
        public int Id { get; set; }
        public double Xleft { get; set; }
        public double Ytop { get; set; }
        public ItemInfo ItemInfo { get; set; }
    }
}
