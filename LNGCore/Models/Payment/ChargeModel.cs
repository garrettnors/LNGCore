using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Payment
{
    public class ChargeModel
    {
        public string StripeToken { get; set; }
        public string InvoiceIdentifier { get; set; }
    }
}
