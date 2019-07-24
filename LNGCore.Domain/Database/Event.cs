using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LNGCore.Domain.Database
{
    public partial class Event
    {
        public int Id { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public string EventDescription { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public bool Completed { get; set; }
        public int EmployeeId { get; set; }
        public int? InvoiceId { get; set; }
        public bool Recurring { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
