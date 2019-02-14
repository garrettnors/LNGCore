using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.UI.Enums;

namespace LNGCore.UI.Models.Admin
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int Take { get; set; }
        public int NumberOfPages { get; set; }
        public InvoiceTypeEnum InvoiceType { get; set; }
    }
}
