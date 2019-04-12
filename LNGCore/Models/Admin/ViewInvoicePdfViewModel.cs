using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{       
    public class ViewInvoicePdfViewModel
    {
        public string DocTitle { get; set; }
        public Invoice Invoice { get; set; }
        public int TotalLineItems { get; set; }
        public int RowsPerPage { get; set; }
    }
}
