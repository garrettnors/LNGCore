using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{
    public partial class Invoice : IInvoice
    {
        public decimal? InvoiceTotal
        {
            get { return LineItem.Sum(s => (s.ItemPrice ?? 0) * s.Quantity); }
        }
    }
}
