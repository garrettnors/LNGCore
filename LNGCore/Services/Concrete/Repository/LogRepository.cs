using System;
using System.Linq;
using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Context;
using LNGCore.Services.Abstract.Repository;
using LNGCore.Services.Concrete.Class;
using LNGCore.Services.Concrete.Context;

namespace LNGCore.Services.Concrete.Repository
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
