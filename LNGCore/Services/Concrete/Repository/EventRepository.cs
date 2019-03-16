using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Context;
using LNGCore.Services.Abstract.Repository;
using LNGCore.Services.Concrete.Class;
using Microsoft.EntityFrameworkCore;

namespace LNGCore.Services.Concrete.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly IInvoiceDbContext _db;
        public EventRepository(IInvoiceDbContext dbContext)
        {
            _db = dbContext;
        }
        public IEvent GetEvent(int id)
        {
            return _db.Event.FirstOrDefault(f => f.Id == id) ?? new Event();
        }

        public IEnumerable<IEvent> GetUpcomingEvents()
        {
            return _db.Event.Where(w => !w.Completed).Include(e => e.Employee).Include(e => e.Invoice);
        }

        public IEnumerable<IEvent> GetCompletedEvents()
        {
            return _db.Event.Where(w => w.Completed).Include(e => e.Employee).Include(e => e.Invoice);
        }
    }
}
