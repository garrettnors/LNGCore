using System.Collections.Generic;

namespace LNGCore.Domain.Abstract.Class
{
    public interface IItem
    {
        int ItemId { get; set; }
        string ItemName { get; set; }
        IEnumerable<ILineItem> LineItem { get; set; }
    }
}