using Microsoft.AspNetCore.SignalR;
using Server.Models;
using Server.SignalRHubs;

namespace Server.Services.CarcassoneGame
{
    public class RoomManager
    {
        private ICarcassonneGame Game { get; }
        private IClientProxy Group => Game.HubContext.Clients.Group(Name);

        public string Name { get; }
        public int MaxPlayers { get; private set; }

        public List<UserData> Players { get; }

        public RoomManager(string name, ICarcassonneGame game)
        {
            Name = name;
            MaxPlayers = 4;
            Players = new List<UserData>();
            Game = game;
        }

        public void Join(User user)
        {
            Players.Add(new UserData()
            {
                User = user,
                IsAdmin = Players.Count == 0,
                Color = "red"
            });
            UpdateUsers();
        }

        public void Disconnect(User user)
        {
            Players.RemoveAll(data => data.User.IdUser == user.IdUser);
            UpdateUsers();
        }

        public void Action(string connectionId, User user, string action, object[] args)
        {
            //
        }

        private void UpdateUsers() => Group.SendAsync("UpdateUsers",
            Players.Select(p => new ResponseUserData()
            {
                Name = p.User.Nick,
                IsAdmin = p.IsAdmin,
                Color = p.Color
            }));

        public static bool Parse(object[] p, params Type[] what)
        {
            if (p.Length != what.Length) return false;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].GetType() != what[i]) return false;
            }
            return true;
        }

        public class UserData
        {
            public User User { get; set; }
            public bool IsAdmin { get; set; }
            public string Color { get; set; }
        }
        
        class ResponseUserData
        {
            public string Name { get; set; }
            public bool IsAdmin { get; set; }
            public string Color { get; set; }
        }
    }
}
