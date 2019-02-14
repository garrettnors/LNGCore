using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Context;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Class;
using Microsoft.EntityFrameworkCore;

namespace LNGCore.Domain.Concrete.Repository
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
