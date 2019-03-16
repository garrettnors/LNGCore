using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Services.Abstract.Class;
using LNGCore.UI.Enums;

namespace LNGCore.UI.Models.Admin
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            PaginationParameters = new PaginationViewModel();
            Invoices = new List<IInvoice>();
        }
        public InvoiceTypeEnum InvoiceType { get; set; }
        public List<IInvoice> Invoices { get; set; }
        public PaginationViewModel PaginationParameters { get; set; }
        public string SearchTerm { get; set; }
        public string ViewTitle { get; set; }
    }
}
