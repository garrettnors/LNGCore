using System;
using System.Linq;
using LNGCore.Domain.Abstract.Class;
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
        public ILog GetLog(int logId)
        {
            return db.Log.FirstOrDefault(f => f.Id == logId);
        }
        public void SaveLog(ILog log)
        {
            var newLog = AutoMapper.Mapper.Map<Log>(log);
            db.Log.Add(newLog);
            db.Commit();
        }
    }
}
