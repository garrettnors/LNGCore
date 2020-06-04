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
            UpcomingEvents = new List<Event>();
            AllNonUpcomingEvents = new List<Event>();
        }
        public List<Event> UpcomingEvents { get; set; }
        public List<Event> AllNonUpcomingEvents { get; set; }
        public decimal YtdSales { get; set; }
        public decimal OpenInvoiceAmount { get; set; }
        public decimal PastDueAmount { get; set; }
    }
}
