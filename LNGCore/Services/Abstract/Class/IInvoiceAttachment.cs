using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LNGCore.Domain.Abstract.Class
{
    public interface IInvoiceAttachment
    {
        int Id { get; set; }
        int InvoiceId { get; set; }
        [StringLength(200)]
        string AttachmentLocation { get; set; }
        bool ShowToCustomer { get; set; }
        IInvoice Invoice { get; set; }
    }
}
