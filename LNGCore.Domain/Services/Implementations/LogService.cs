using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using LNGCore.Services.Logical;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly LngDbContext _db;
        private readonly IConfiguration _config;
        public LogService(LngDbContext context, IConfiguration config)
        {
            _db = context;
            _config = config;
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

            if (!log.Summary.Contains("Model State Invalid") && log.LogType != "SendInvoiceToCustomer") //don't really give a shit about these
            {
                var email = new Email(_config)
                {
                    MailSubject = $"An error occured at LNG",
                    Message = log.Summary,
                    SenderEmail = _config.GetSection("SiteConfiguration")["SiteEmail"],
                    SenderDisplayName = _config.GetSection("SiteConfiguration")["SiteName"],
                    RecipientEmail = _config.GetSection("SiteConfiguration")["GEmail"],
                    RecipientDisplayName = _config.GetSection("SiteConfiguration")["GEmail"]
                };

                email.SendEmail();
            }
            
            return log.Id;
        }

        public IEnumerable<Log> GetLogsByInvoiceId(int invoiceId)
        {
            return _db.Log.Where(w => w.InvoiceId == invoiceId && w.LogType == "SendInvoiceToCustomer");
        }

        public IEnumerable<Log> GetLogsByInvoiceId(List<int> invoiceIds)
        {
            return _db.Log.Where(w => invoiceIds.Contains(w.InvoiceId ?? 0));
        }
    }
}
