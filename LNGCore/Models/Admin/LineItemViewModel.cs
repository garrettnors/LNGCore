using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.UI.Models.Admin
{
    public class LineItemViewModel
    {
        public LineItemViewModel()
        {
            LineItems = new List<ILineItem>();
            ItemTypes = new List<IItem>();
        }

        public List<ILineItem> LineItems { get; set; }
        public List<IItem> ItemTypes { get; set; }
        public int InvoiceId { get; set; }
        public int LineIndex { get; set; }
    }
}
