using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.UI.Models.Admin
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Events = new List<IEvent>();
        }
        public List<IEvent> Events { get; set; }
        public decimal YtdSales { get; set; }
        public decimal OpenInvoiceAmount { get; set; }
        public decimal PastDueAmount { get; set; }
    }
}
