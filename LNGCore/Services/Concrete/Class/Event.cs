using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
{
    public class Event : IEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public bool Completed { get; set; }
        public int EmployeeId { get; set; }
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        IInvoice IEvent.Invoice
        {
            get => Invoice;
            set => throw new NotImplementedException();
        }
        public Employee Employee { get; set; }

        IEmployee IEvent.Employee
        {
            get => Employee;
            set => throw new NotImplementedException();
        }
    }
}
