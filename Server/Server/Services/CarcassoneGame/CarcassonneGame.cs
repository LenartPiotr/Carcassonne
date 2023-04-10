using Microsoft.AspNetCore.SignalR;
using Server.Models;
using Server.Services.CarcassoneGame.ResponseModels;
using Server.Services.HubSessionBridge;
using Server.SignalRHubs;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server.Services.CarcassoneGame
{
    public interface ICarcassonneGame
    {
        public IHubContext<GlobalHub> HubContext { get; }
        public RoomListResponse GetRoomLists();
        public RoomManager? GetRoom(User user);
        public Task<bool> CreateRoom(string connectionId, User user, string name);
        public Task<bool> JoinRoom(string connectionId, User user, string name);
        public bool LeaveRoom(string connectionId, User user);
        public void Disconnect(User user);
    }

    public class CarcassonneGame : ICarcassonneGame
    {
        readonly List<RoomManager> rooms;
        public IHubContext<GlobalHub> HubContext { get; }

        public CarcassonneGame(IHubContext<GlobalHub> hubContext)
        {
            rooms = new();
            HubContext = hubContext;
        }

        public RoomManager? GetRoom(User user)
        {
            var list = rooms.Where(r => r.Players.Where(d => d.User.IdUser == user.IdUser).Any()).ToList();
            if (list.Count == 1) return list[0];
            return null;
        }

        public RoomListResponse GetRoomLists()
        {
            return new RoomListResponse()
            {
                Rooms = rooms.Select(r => new RoomListResponse.RoomElement()
                {
                    Name = r.Name,
                    Min = r.Players.Count,
                    Max = r.MaxPlayers
                }).ToList()
            };
        }

        public async Task<bool> CreateRoom(string connectionId, User user, string name)
        {
            if (!new Regex(@"^[a-zA-Z]*$").IsMatch(name)) return false;
            if (rooms.Where(r => r.Players.Where(d => d.User.IdUser == user.IdUser).Any() || r.Name == name).Any()) return false;
            
            RoomManager room = new(name, this);
            await HubContext.Groups.AddToGroupAsync(connectionId, name);
            room.Join(user);
            rooms.Add(room);

            return true;
        }

        public async Task<bool> JoinRoom(string connectionId, User user, string name)
        {
            if (rooms.Where(r => r.Players.Where(d => d.User.IdUser == user.IdUser).Any()).Any()) return false;
            var list = rooms.Where(r => r.Name == name).ToList();
            if (list.Count != 1) return false;
            var room = list[0];

            await HubContext.Groups.AddToGroupAsync(connectionId, name);
            room.Join(user);

            return true;
        }

        public bool LeaveRoom(string connectionId, User user)
        {
            var list = rooms.Where(r => r.Players.Where(d => d.User.IdUser == user.IdUser).Any()).ToList();
            if (list.Count == 0) return false;
            list.ForEach(r =>
            {
                HubContext.Groups.RemoveFromGroupAsync(connectionId, r.Name);
                r.Disconnect(user);
            });
            rooms.RemoveAll(r => r.Players.Count == 0);

            return true;
        }

        public void Disconnect(User user)
        {
            rooms.Where(r => r.Players.Where(d => d.User.IdUser == user.IdUser).Any())
                .ToList().ForEach(r => r.Disconnect(user));
        }
    }
}
