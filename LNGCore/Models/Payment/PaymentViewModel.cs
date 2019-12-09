using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Payment
{
    public class PaymentViewModel
    {
        public Invoice Invoice { get; set; }
        public string StripePubKey { get; set; }
    }
}
