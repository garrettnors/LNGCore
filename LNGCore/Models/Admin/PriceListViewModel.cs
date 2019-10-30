using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class PriceListViewModel
    {
        public List<PriceList> Prices { get; set; }
        public string SearchTerm { get; set; }
        public PaginationViewModel PaginationParameters { get; set; }
    }
}
