using LNGCore.Services.Abstract.Class;
using LNGCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models
{
    public class IndexViewModel
    {
        public EtsyListings EtsyListing { get; set; }
        public IEnumerable<ICustomer> Customers { get; set; }
    }
}
