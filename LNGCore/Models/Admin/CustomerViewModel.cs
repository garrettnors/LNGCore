using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.UI.Models.Admin
{
    public class CustomerViewModel
    {
        public List<ICustomer> Customers { get; set; }
        public string SearchTerm { get; set; }
        public PaginationViewModel PaginationParameters { get; set; }
    }
}
