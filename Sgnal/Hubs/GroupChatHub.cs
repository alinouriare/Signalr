using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgnal.Hubs
{
    public class GroupChatHub:Hub
    {
        public static readonly ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            OnlineUsers.TryAdd(this.Context.ConnectionId, " ");
            await UpdateUsersOnlineCount();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string ip;//can be username;
            OnlineUsers.TryRemove(this.Context.ConnectionId, out ip);
            await UpdateUsersOnlineCount();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateUsersOnlineCount()
        {
            var conectId = OnlineUsers.Select(x => x.Key).Distinct().Count();
            await Clients.All.SendAsync("GetOnlineUsers", conectId);
        }
        //is typing part
        public async Task IsTypingUser(string group, string message)
        {
            await Clients.Group(group).SendAsync("SayWhoIsTyping", message);
        }
        //end is typing
        public async Task SendMessage(string group, string message)
        {

            await Clients.Group(group).SendAsync("ReceiveMessage", message);
        }

        public async Task Join(string groupName)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
