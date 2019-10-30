using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Item
    {
        public Item()
        {
            LineItem = new HashSet<LineItem>();
            PriceList = new HashSet<PriceList>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }

        public virtual ICollection<LineItem> LineItem { get; set; }
        public virtual ICollection<PriceList> PriceList { get; set; }
    }
}
