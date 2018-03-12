using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public abstract class ATable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
