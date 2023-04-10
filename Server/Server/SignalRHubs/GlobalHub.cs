using Microsoft.AspNetCore.SignalR;
using Server.Models;
using Server.Services.CarcassoneGame;
using Server.Services.HubSessionBridge;
using System.Diagnostics;

namespace Server.SignalRHubs
{
    public class GlobalHub : Hub
    {
        private readonly IBridge _bridge;
        private readonly ICarcassonneGame game;

        public GlobalHub(IBridge bridge, ICarcassonneGame carcassonneGame)
        {
            _bridge = bridge;
            game = carcassonneGame;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_bridge.GetUser(Context.ConnectionId, out User user))
            {
                game.Disconnect(user);
            }
            _bridge.Leave(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Join(string key)
        {
            bool success = _bridge.Join(key, Context.ConnectionId);
            await Clients.Caller.SendAsync("Connection", success);
        }

        public async Task Ping() => _bridge.Ping(Context.ConnectionId);

        #region Private functions
        private async Task Navigate(string path)
        {
            await Clients.Caller.SendAsync("Navigate", path);
        }

        private async Task SendMessage(bool status, string message)
        {
            await Clients.Caller.SendAsync("Message", status, message);
        }

        /// <summary> Returns True if fail </summary>
        private bool GetUserOrNavigate(out User user)
        {
            if (!_bridge.GetUser(Context.ConnectionId, out user))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Room actions
        public async Task CreateRoom(string name)
        {
            if (GetUserOrNavigate(out User user)) { await Navigate("/"); return; }
            if (game.CreateRoom(this, user, name)) await Navigate("/room");
            else await SendMessage(false, "Cannot create room");
        }

        public async Task JoinRoom(string name)
        {
            if (GetUserOrNavigate(out User user)) { await Navigate("/"); return; }
            if (game.JoinRoom(this, user, name)) await Navigate("/room");
            else await SendMessage(false, "Cannot join to this room");
        }

        public async Task LeaveRoom()
        {
            if (GetUserOrNavigate(out User user)) { await Navigate("/"); return; }
            if (game.LeaveRoom(this, user)) await Navigate("/lobby");
            else await SendMessage(false, "Cannot leave room");
        }
        #endregion
    }
}
