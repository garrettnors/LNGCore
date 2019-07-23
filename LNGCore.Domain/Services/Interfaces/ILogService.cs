using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;
using static LNGCore.Domain.Infrastructure.Enums;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface ILogService : IBaseService<Log>
    {
        IEnumerable<Log> GetLogsByInvoiceId(int invoiceId);
        IEnumerable<Log> GetLogsByInvoiceId(List<int> invoiceIds);

    }

}
