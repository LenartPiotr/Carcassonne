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
        public RoomListResponse GetRoomLists();
        public RoomManager? GetRoom(User user);
        public bool CreateRoom(Hub hub, User user, string name);
        public bool JoinRoom(Hub hub, User user, string name);
        public bool LeaveRoom(Hub hub, User user);
        public void Disconnect(User user);
    }

    public class CarcassonneGame : ICarcassonneGame
    {
        readonly List<RoomManager> rooms;

        public CarcassonneGame()
        {
            rooms = new();
        }

        public RoomManager? GetRoom(User user)
        {
            var list = rooms.Where(r => r.Players.Contains(user)).ToList();
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

        public bool CreateRoom(Hub hub, User user, string name)
        {
            if (!new Regex(@"^[a-zA-Z]*$").IsMatch(name)) return false;
            if (rooms.Where(r => r.Players.Contains(user) || r.Name == name).Any()) return false;
            
            RoomManager room = new(name);
            room.Join(user);
            rooms.Add(room);

            hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, name);

            return true;
        }

        public bool JoinRoom(Hub hub, User user, string name)
        {
            if (rooms.Where(r => r.Players.Contains(user)).Any()) return false;
            var list = rooms.Where(r => r.Name == name).ToList();
            if (list.Count != 1) return false;
            var room = list[0];
            
            room.Join(user);
            hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, name);

            return true;
        }

        public bool LeaveRoom(Hub hub, User user)
        {
            var list = rooms.Where(r => r.Players.Contains(user)).ToList();
            if (list.Count == 0) return false;
            list.ForEach(r =>
            {
                r.Disconnect(user);
                hub.Groups.RemoveFromGroupAsync(hub.Context.ConnectionId, r.Name);
            });

            return true;
        }

        public void Disconnect(User user)
        {
            rooms.Where(r => r.Players.Contains(user)).ToList().ForEach(r => r.Disconnect(user));
        }
    }
}
