using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly LngDbContext _db;
        public EventService(LngDbContext context)
        {
            _db = context;
        }
        public Event GetEvent(int id)
        {
            return _db.Event.FirstOrDefault(f => f.Id == id) ?? new Event();
        }

        public IEnumerable<Event> GetUpcomingEvents()
        {
            return _db.Event.Where(w => !w.Completed).Include(e => e.Employee);
        }

        public IEnumerable<Event> GetCompletedEvents()
        {
            return _db.Event.Where(w => w.Completed).Include(e => e.Employee);
        }
    }
}
