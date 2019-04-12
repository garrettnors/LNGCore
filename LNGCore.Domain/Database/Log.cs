using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Log
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? InvoiceId { get; set; }
    }
}
