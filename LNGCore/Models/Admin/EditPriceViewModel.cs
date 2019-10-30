using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class EditPriceViewModel
    {
        public EditPriceViewModel()
        {
            Items = new List<Item>();
        }
        public PriceList Price { get; set; }
        public List<Item> Items { get; set; }
    }
}
