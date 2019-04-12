using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public bool Completed { get; set; }
        public int EmployeeId { get; set; }
        public int? InvoiceId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
