using System;
using System.Collections.Generic;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{
    public partial class Invoice : IInvoice
    {
        public Invoice()
        {
            LineItem = new HashSet<LineItem>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime Deadline { get; set; }
        public string CompletedBy { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Notes { get; set; }
        public string ShippingMethod { get; set; }
        public bool IsQuote { get; set; }
        public decimal? TaxPercent { get; set; }
        public int EmployeeId { get; set; }
        public string Pofield { get; set; }
        public bool Voided { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsDonated { get; set; }
        public decimal? ShipCost { get; set; }
        public string InvoiceProofUrl { get; set; }
        public Customer Customer { get; set; }
        public ICollection<LineItem> LineItem { get; set; }
        public ICollection<InvoiceAttachment> InvoiceAttachments { get; set; }
        ICustomer IInvoice.Customer
        {
            get => Customer;
            set => throw new NotImplementedException();
        }

        IEnumerable<ILineItem> IInvoice.LineItem
        {
            get => LineItem;
            set => throw new NotImplementedException();
        }

        IEnumerable<IInvoiceAttachment> IInvoice.InvoiceAttachments
        {
            get => InvoiceAttachments;
            set => throw new NotImplementedException();
        }
    }
}
