﻿using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IEventService : IBaseService<Event>
    {
        IEnumerable<Event> GetUpcomingEvents();
        IEnumerable<Event> GetCompletedEvents();
    }
}
