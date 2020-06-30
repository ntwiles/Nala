using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace NalaWeb.Hubs
{
    public class NalaHub : Hub<INalaHub>
    {
        /*
        public async Task SendOutputChange()
        {
            await Clients.All.SendAsync("OutputChange", output);
        }
        */
    }
}
