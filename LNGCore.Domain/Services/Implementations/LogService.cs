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

        public void Delete(int itemId)
        {
            var log = _db.Log.Find(itemId);

            if (log == null)
                return;

            _db.Remove(log);
            _db.SaveChanges();
        }

        public void Edit(Log item)
        {
            var log = _db.Log.Find(item.Id);

            if (log == null)
                return;

            _db.Entry(log).CurrentValues.SetValues(item);
            _db.SaveChanges();
        }

        public Log Get(int logId)
        {
            return _db.Log.FirstOrDefault(f => f.Id == logId) ?? new Log();
        }
        
        public int Add(Log log)
        {
            _db.Log.Add(log);
            _db.SaveChanges();

            return log.Id;
        }
    }
}
