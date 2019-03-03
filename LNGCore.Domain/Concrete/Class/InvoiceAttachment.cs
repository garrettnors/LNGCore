using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LNGCore.Domain.Concrete.Class
{
    public class InvoiceAttachment : IInvoiceAttachment
    {
        public int Id { get;set; }
        [StringLength(200)]
        public int InvoiceId { get; set; }
        public string AttachmentLocation { get; set; }
        public bool ShowToCustomer { get; set; }
    }
}
