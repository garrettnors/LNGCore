using System;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{

    public partial class Log : ILog
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? InvoiceId { get; set; }
    }
}
