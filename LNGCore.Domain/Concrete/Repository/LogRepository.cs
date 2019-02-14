using System;
using LNGCore.Domain.Abstract.Context;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Class;
using LNGCore.Domain.Concrete.Context;

namespace LNGCore.Domain.Concrete.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IInvoiceDbContext db;
        public LogRepository(IInvoiceDbContext dbContext)
        {
            db = dbContext;
        }
        public void SaveLog(string logText)
        {
            db.Logs.Add(new Logs { Log = logText, Date = DateTime.Now });
            db.Commit();
        }
    }
}
