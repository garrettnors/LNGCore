using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class ViewInvoiceViewModel
    {
        public Invoice Invoice { get; set; }      
        public string InvoiceData { get; set; }
        public List<Log> Emails { get; set; }
    }
}
