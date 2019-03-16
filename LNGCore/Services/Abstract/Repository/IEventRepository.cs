using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Abstract.Repository
{
    public interface IEventRepository
    {
        IEvent GetEvent(int id);
        IEnumerable<IEvent> GetUpcomingEvents();
        IEnumerable<IEvent> GetCompletedEvents();
    }
}
