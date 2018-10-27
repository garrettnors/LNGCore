using System;
using System.Collections.Generic;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete
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
