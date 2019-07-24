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
        public IEnumerable<Event> GetUpcomingEvents()
        {
            var daysBeforeEvent = 90;
            //var events = _db.Event.Where(w => !w.Completed
            //                                  && ((w.EventDate - DateTime.Now).Days <= daysBeforeEvent
            //                                  || (w.Recurring && (new DateTime(DateTime.Now.Year, w.EventDate.Month, w.EventDate.Day) - DateTime.Now).Days <= daysBeforeEvent)));

            var events = from e in _db.Event
                          let recurringDate = new DateTime(DateTime.Now.Year, e.EventDate.Month, e.EventDate.Day)
                          where !e.Completed &&
                              (((e.EventDate - DateTime.Now).Days <= daysBeforeEvent && e.EventDate.Date >= DateTime.Now.Date)
                              || (e.Recurring && (recurringDate - DateTime.Now).Days <= daysBeforeEvent && recurringDate.Date >= DateTime.Now.Date))
                          select e;


            return events.Include(e => e.Employee);
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
