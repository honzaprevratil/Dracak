﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes
{
    public class ItemInfo
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public int SellPrice { get; set; }
        public bool SellAble { get; set; }
        public ItemInfo(string name, string imageName, string description, bool sellAble, int sellPrice = 0)
        {
            this.Name = name;
            this.ImageName = imageName;
            this.Description = description;
            this.SellAble = sellAble;
            this.SellPrice = sellPrice;
        }
    }
}