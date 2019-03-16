namespace LNGCore.Services.Abstract.Class
{
    public interface ILineItem
    {
        IInvoice Invoice { get; set; }
        int InvoiceId { get; set; }
        IItem Item { get; set; }
        string ItemDesc { get; set; }
        int ItemId { get; set; }
        decimal? ItemPrice { get; set; }
        int LineItemId { get; set; }
        decimal? Price { get; set; }
        int Quantity { get; set; }
    }
}