using System;
using System.Collections.Generic;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
{
    public partial class Item : IItem
    {
        public Item()
        {
            LineItem = new HashSet<LineItem>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public ICollection<LineItem> LineItem { get; set; }
        IEnumerable<ILineItem> IItem.LineItem { get { return LineItem; } set => throw new NotImplementedException(); }
    }
}
