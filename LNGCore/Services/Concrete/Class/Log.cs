using System;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
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
