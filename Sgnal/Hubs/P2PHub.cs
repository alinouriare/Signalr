using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgnal.Hubs
{
    public class P2PHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        public async Task SendMessage(string name1, string name, string msg)
        {
            var cId = OnlineUsers.FirstOrDefault(x => x.Value == name1).Key;
            await Clients.Client(cId).SendAsync("RecieveMessage", name, msg);
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            //http://localhost:12994/p2phub?token=ali
            var token = httpContext.Request.Query["Token"];

            OnlineUsers.TryAdd(this.Context.ConnectionId, token);

            //برگردوندن کاربرا با سیگنال ار به خود کاربر جاری
            Clients.All.SendAsync("GetUsersList", OnlineUsers);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string ip;//can be username;
            OnlineUsers.TryRemove(this.Context.ConnectionId, out ip);

            //برگردوندن کاربرا با سیگنال ار به خود کاربر جاری
            Clients.All.SendAsync("GetUsersList", OnlineUsers);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
