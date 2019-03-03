using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Abstract.Class
{
    public interface IInvoice
    {
        string CompletedBy { get; set; }
        int CustomerId { get; set; }
        DateTime Deadline { get; set; }
        //string Employee { get; set; }
        int EmployeeId { get; set; }
        int Id { get; set; }
        string InvoiceProofUrl { get; set; }
        bool? IsDonated { get; set; }
        bool? IsPaid { get; set; }
        bool IsQuote { get; set; }
        string Notes { get; set; }
        DateTime OrderDate { get; set; }
        DateTime? PaidDate { get; set; }
        string Pofield { get; set; }
        decimal? ShipCost { get; set; }
        string ShippingMethod { get; set; }
        decimal? TaxPercent { get; set; }
        bool Voided { get; set; }
        ICustomer Customer { get; set; }
        IEnumerable<ILineItem> LineItem { get; set; }
        IEnumerable<IInvoiceAttachment> InvoiceAttachments { get; set; }
        decimal? InvoiceTotal { get; }
    }
}