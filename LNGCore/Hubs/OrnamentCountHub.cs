using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LNGCore.UI.Hubs
{
    public class OrnamentCountHub : Hub
    {
        public async Task SendOrnamentCount(string style, int count)
        {
            await Clients.All.SendAsync("OrnamentCount", style, count);
        }
    }
}
