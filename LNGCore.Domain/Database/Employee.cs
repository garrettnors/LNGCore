using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Employee
    {
        public Employee()
        {
            Event = new HashSet<Event>();
            Invoice = new HashSet<Invoice>();
        }

        public int EmployeeId { get; set; }
        public string EmpName { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
