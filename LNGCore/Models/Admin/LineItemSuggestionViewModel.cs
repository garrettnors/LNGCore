using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.UI.Models.Admin
{
    public class LineItemSuggestionViewModel
    {
        public LineItemSuggestionViewModel()
        {
            OverallLineItems = new List<ILineItem>();
            CustomerLineItems = new List<ILineItem>();
        }
        public int CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int LineIndex { get; set; }
        public ICustomer Customer { get; set; }
        public List<ILineItem> OverallLineItems { get; set; }
        public List<ILineItem> CustomerLineItems { get; set; }
    }
}
