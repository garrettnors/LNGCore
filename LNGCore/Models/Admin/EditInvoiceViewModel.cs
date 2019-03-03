using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;
using LNGCore.UI.Enums;
using Microsoft.AspNetCore.Http;

namespace LNGCore.UI.Models.Admin
{
    public class EditInvoiceViewModel
    {
        public EditInvoiceViewModel()
        {
            LineItems = new List<LineItemRow>();
            Customers = new List<ICustomer>();
            Employees = new List<IEmployee>();
            UploadedFiles = new List<IFormFile>();
            UploadedProofs = new List<IFormFile>();
        }
        public InvoiceItem Invoice { get; set; }
        [Required]
        public InvoiceTypeEnum InvoiceType { get; set; }
        [Required]
        public string ShippingMethod { get; set; }
        public List<ICustomer> Customers { get; set; }
        public List<IEmployee> Employees { get; set; }
        public List<LineItemRow> LineItems { get; set; }
        public List<IFormFile> UploadedFiles { get; set; }
        public List<IFormFile> UploadedProofs { get; set; }
    }

    public class LineItemRow : ILineItem
    {
        public IInvoice Invoice { get; set; }
        public int InvoiceId { get; set; }
        public IItem Item { get; set; }
        public string ItemDesc { get; set; }
        public int ItemId { get; set; }
        public decimal? ItemPrice { get; set; }
        public int LineItemId { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }

    public class InvoiceItem : IInvoice
    {
        public string CompletedBy { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public int Id { get; set; }
        public string InvoiceProofUrl { get; set; }
        public bool? IsDonated { get; set; }
        public bool? IsPaid { get; set; }
        public bool IsQuote { get; set; }
        public string Notes { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Pofield { get; set; }
        public decimal? ShipCost { get; set; }
        [Required]
        public string ShippingMethod { get; set; }
        public decimal? TaxPercent { get; set; }
        public bool Voided { get; set; }
        public ICustomer Customer { get; set; }
        public IEnumerable<ILineItem> LineItem { get; set; }
        public decimal? InvoiceTotal { get; }
        public IEnumerable<IInvoiceAttachment> InvoiceAttachments { get; set; }
    }
}
