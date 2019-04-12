
using LNGCore.Domain.Database;
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
        public IEnumerable<Customer> Customers { get; set; }
    }
}
