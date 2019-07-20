using LNGCore.Domain.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{       
    public class ViewInvoicePdfViewModel
    {
        public ViewInvoicePdfViewModel()
        {
            AttachmentPaths = new List<string>();
        }
        public string DocTitle { get; set; }
        public Invoice Invoice { get; set; }
        public int TotalLineItems { get; set; }
        public int RowsPerPage { get; set; }
        public List<string> AttachmentPaths { get; set; }
    }
}
