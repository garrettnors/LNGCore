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
        public Event Get(int id)
        {
            return _db.Event.FirstOrDefault(f => f.Id == id) ?? new Event();
        }
        public int Add(Event eventItem)
        {
            _db.Event.Add(eventItem);
            _db.SaveChanges();
            return eventItem.Id;
        }

        public void Edit(Event eventItem)
        {
            var item = _db.Event.Find(eventItem.Id);
            if (item == null)
                return;

            _db.Entry(item).CurrentValues.SetValues(eventItem);
            _db.SaveChanges();
        }

        public void Delete(int eventItemId)
        {
            var item = _db.Event.Find(eventItemId);

            if (item == null)
                return;

            _db.Remove(item);
            _db.SaveChanges();
        }
      
        public IEnumerable<Event> GetAllEvents()
        {
            return _db.Event.Include(e => e.Employee);
        }
        public IEnumerable<Event> GetCompletedEvents()
        {
            return _db.Event.Where(w => w.Completed).Include(e => e.Employee);
        }
    }
}
