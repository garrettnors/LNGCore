using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;
using static LNGCore.Domain.Infrastructure.Enums;

namespace LNGCore.UI.Models.Admin
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            PaginationParameters = new PaginationViewModel();
            Invoices = new List<Invoice>();
        }
        public InvoiceTypeEnum InvoiceType { get; set; }
        public List<Invoice> Invoices { get; set; }
        public PaginationViewModel PaginationParameters { get; set; }
        public string SearchTerm { get; set; }
        public string ViewTitle { get; set; }
    }
}
