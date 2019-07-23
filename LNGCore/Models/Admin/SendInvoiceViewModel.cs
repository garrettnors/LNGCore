using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models.Admin
{
    public class SendInvoiceViewModel
    {
        public int CustomerId { get; set; }
        public int InvoiceId { get; set; }
        public string PrimaryEmail { get; set; }
        public bool SendToPrimary { get; set; }
        public string SecondaryEmail { get; set; }
        public bool SendToCompany { get; set; }
        public bool SendToSecondary { get; set; }
        public string Note { get; set; }
        public bool SendAttachments { get; set; }
    }
}
