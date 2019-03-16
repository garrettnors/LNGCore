using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
{
    public partial class Invoice : IInvoice
    {
        public decimal? InvoiceTotal
        {
            get { return LineItem.Sum(s => (s.ItemPrice ?? 0) * s.Quantity); }
        }
    }
}
