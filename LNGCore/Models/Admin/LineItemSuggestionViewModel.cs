using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class LineItemSuggestionViewModel
    {
        public LineItemSuggestionViewModel()
        {
            OverallLineItems = new List<LineItem>();
            CustomerLineItems = new List<LineItem>();
        }
        public int CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int LineIndex { get; set; }
        public Customer Customer { get; set; }
        public List<LineItem> OverallLineItems { get; set; }
        public List<LineItem> CustomerLineItems { get; set; }
    }
}
