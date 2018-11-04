using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Infrastructure;

namespace LNGCore.UI.Models
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public EtsyListings EtsyListing { get; set; }
    }
}
