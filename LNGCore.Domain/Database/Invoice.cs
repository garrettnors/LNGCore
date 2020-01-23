using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Invoice
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
        public decimal TaxPercent { get; set; }
        public int EmployeeId { get; set; }
        public string Pofield { get; set; }
        public bool Voided { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsDonated { get; set; }
        public decimal? ShipCost { get; set; }
        public string InvoiceProofUrl { get; set; }
        public decimal? JobCost { get; set; }
        public string JobCostDesc { get; set; }
        public bool IsPaidToEmployees { get; set; }
        public string StripeChargeId { get; set; }
        public string Identifier { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<LineItem> LineItem { get; set; }
    }
}
