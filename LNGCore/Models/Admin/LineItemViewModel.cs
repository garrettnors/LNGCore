using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class LineItemViewModel
    {
        public LineItemViewModel()
        {
            LineItems = new List<LineItem>();
            ItemTypes = new List<Item>();
        }

        public List<LineItem> LineItems { get; set; }
        public List<Item> ItemTypes { get; set; }
        public int InvoiceId { get; set; }
        public int LineIndex { get; set; }
    }
}
