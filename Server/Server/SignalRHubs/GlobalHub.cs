using Microsoft.AspNetCore.SignalR;
using Server.Models;
using Server.Services.HubSessionBridge;

namespace Server.SignalRHubs
{
    public class GlobalHub : Hub
    {
        private readonly IBridge _bridge;

        public GlobalHub(IBridge bridge)
        {
            _bridge = bridge;
        }

        public async Task SendToAll(string user, string message)
        {
            // To REMOVE
            await Clients.All.SendAsync("Send", Context.ConnectionId + " " + Context.UserIdentifier);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _bridge.Leave(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Join(string key)
        {
            bool success = _bridge.Join(key, Context.ConnectionId);
            await Clients.Caller.SendAsync("Connection", success);
        }

        public async Task Username()
        {
            bool success = _bridge.GetUser(Context.ConnectionId, out User user);
            string name = success ? user.Nick : ":(";
            await Clients.Caller.SendAsync("Username", name, Context.ConnectionId);
        }
    }
}
