using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Events = new List<Event>();
        }
        public List<Event> Events { get; set; }
        public decimal YtdSales { get; set; }
        public decimal OpenInvoiceAmount { get; set; }
        public decimal PastDueAmount { get; set; }
    }
}
