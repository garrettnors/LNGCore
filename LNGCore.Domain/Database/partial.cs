using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Database
{
    public partial class Invoice
    {
        public decimal? InvoiceTotal
        {
            get { return LineItem.Sum(s => (s.ItemPrice ?? 0) * s.Quantity); }
        }
    }

    public partial class Customer
    {
        public string DisplayName => BusinessName == null ? Name : $"{BusinessName} ➤ {Name}";
    }
}
