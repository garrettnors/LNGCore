using System.Collections.Generic;

namespace LNGCore.Services.Abstract.Class
{
    public interface IItem
    {
        int ItemId { get; set; }
        string ItemName { get; set; }
        IEnumerable<ILineItem> LineItem { get; set; }
    }
}