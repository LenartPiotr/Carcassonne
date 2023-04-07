using Microsoft.AspNetCore.SignalR;

namespace Server.SignalRHubs
{
    public class GlobalHub : Hub
    {
        public async Task SendToAll(string user, string message)
        {
            await Clients.All.SendAsync("Send", user, message);
        }
    }
}
