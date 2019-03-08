using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{       
    public class ViewInvoicePdfViewModel
    {
        public string DocTitle { get; set; }
        public IInvoice Invoice { get; set; }
        public int TotalLineItems { get; set; }
        public int RowsPerPage { get; set; }
    }
}
