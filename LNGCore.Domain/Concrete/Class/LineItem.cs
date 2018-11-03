using System;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{
    public partial class LineItem : ILineItem
    {
        public int LineItemId { get; set; }
        public int InvoiceId { get; set; }
        public string ItemDesc { get; set; }
        public int Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? Price { get; set; }
        public int ItemId { get; set; }

        public Invoice Invoice { get; set; }
        public Item Item { get; set; }
        IInvoice ILineItem.Invoice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IItem ILineItem.Item { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
