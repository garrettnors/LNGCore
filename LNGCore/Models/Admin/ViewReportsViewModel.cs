using LNGCore.Domain.Database;
using LNGCore.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class ViewReportsViewModel
    {
        public int SelectedYear { get; set; }
        public Enums.InvoiceReportTypeEnum ReportType { get; set; }
        public List<Enums.InvoiceReportTypeEnum> ReportTypes { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<int> AvailableYears { get; set; }
    }
}
