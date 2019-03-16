using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LNGCore.Services.Abstract.Class
{
    public interface IEvent
    {
        int Id { get; set; }
        [Required]
        [MaxLength(50)]
        string EventName { get; set; }
        [Required]
        string EventDescription { get; set; }
        [Required]
        DateTime EventDate { get; set; }
        [Required]
        bool Completed { get; set; }
        int EmployeeId { get; set; }
        IEmployee Employee { get; set; }
        int? InvoiceId { get; set; }
        IInvoice Invoice { get; set; }

    }
}
