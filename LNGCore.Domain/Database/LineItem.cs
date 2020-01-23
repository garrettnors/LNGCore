using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class LineItem
    {
        public int LineItemId { get; set; }
        public int InvoiceId { get; set; }
        public string ItemDesc { get; set; }
        public int Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public int ItemId { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}
