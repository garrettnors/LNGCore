using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class InvoiceAttachment
    {
        public InvoiceAttachment()
        {
            InverseInvoice = new HashSet<InvoiceAttachment>();
        }

        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string AttachmentLocation { get; set; }
        public bool ShowToCustomer { get; set; }

        public virtual InvoiceAttachment Invoice { get; set; }
        public virtual ICollection<InvoiceAttachment> InverseInvoice { get; set; }
    }
}
