using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Class
{
    public interface ILog
    {
        int Id { get; set; }
        string LogType { get; set; }
        string Summary { get; set; }
        DateTime Date { get; set; }
        int? CustomerId { get; set; }
        int? InvoiceId { get; set; }
    }
}
