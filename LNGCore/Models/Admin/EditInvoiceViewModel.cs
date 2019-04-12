using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using LNGCore.UI.Enums;
using Microsoft.AspNetCore.Http;

namespace LNGCore.UI.Models.Admin
{
    public class EditInvoiceViewModel
    {
        public EditInvoiceViewModel()
        {
            LineItems = new List<LineItem>();
            Customers = new List<Customer>();
            Employees = new List<Employee>();
            UploadedFiles = new List<IFormFile>();
            UploadedProofs = new List<IFormFile>();
        }
        public Invoice Invoice { get; set; }
        [Required]
        public InvoiceTypeEnum InvoiceType { get; set; }
        [Required]
        public string ShippingMethod { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Employee> Employees { get; set; }
        public List<LineItem> LineItems { get; set; }
        public List<IFormFile> UploadedFiles { get; set; }
        public List<IFormFile> UploadedProofs { get; set; }
    }

    //public class LineItemRow
    //{
    //    public Invoice Invoice { get; set; }
    //    public int InvoiceId { get; set; }
    //    public Item Item { get; set; }
    //    public string ItemDesc { get; set; }
    //    public int ItemId { get; set; }
    //    public decimal? ItemPrice { get; set; }
    //    public int LineItemId { get; set; }
    //    public decimal? Price { get; set; }
    //    public int Quantity { get; set; }
    //}    
}
