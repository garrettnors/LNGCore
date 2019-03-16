﻿using System;
using System.Collections.Generic;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
{
    public partial class OrnamentOrders : IOrnamentOrders
    {
        public int Id { get; set; }
        public string OrnamentStyle { get; set; }
        public string OrnamentDesign { get; set; }
        public string SpecialInstructions { get; set; }
        public int Amount { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
    }
}
