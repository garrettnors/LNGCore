using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly LngDbContext _db;
        public LogService(LngDbContext context)
        {
            _db = context;
        }
        public Log GetLog(int logId)
        {
            return _db.Log.FirstOrDefault(f => f.Id == logId);
        }
        public void SaveLog(Log log)
        {
            _db.Log.Add(log);
            _db.SaveChanges();
        }
    }
}
