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
        public DateTime? RecurringDate
        {
            get
            {
                var nextEventDate = EventDate;
                if (Recurring)
                {
                    while (nextEventDate < DateTime.Now)
                    {
                        nextEventDate = nextEventDate.AddYears(1);
                    }
                    return nextEventDate;
                }
                return null;
            }
        }

        public virtual Employee Employee { get; set; }
    }
}
