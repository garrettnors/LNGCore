using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Item
    {
        public Item()
        {
            LineItem = new HashSet<LineItem>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }

        public virtual ICollection<LineItem> LineItem { get; set; }
    }
}
