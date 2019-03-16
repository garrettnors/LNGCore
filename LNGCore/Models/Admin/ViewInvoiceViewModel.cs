using LNGCore.Services.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class ViewInvoiceViewModel
    {
        public IInvoice Invoice { get; set; }      
        public string InvoiceData { get; set; }
        public List<ILog> Emails { get; set; }
    }
}
