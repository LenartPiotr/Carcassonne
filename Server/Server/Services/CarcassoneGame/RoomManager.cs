using System.Reflection;
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
            if (Players.Count == 0) Game.RemoveRoom(this);
            UpdateUsers();
        }

        public void Action(string connectionId, User user, string action, object[] args)
        {
            GetType().GetMethods()
                .FirstOrDefault(m =>
                    (m.GetCustomAttribute<CarcassonneAction>()?.Name == action && action != "none") ||
                    (m.Name == action && m.GetCustomAttribute<CarcassonneAction>()?.Name == "none"))
                ?.Invoke(this, new object[] { connectionId, user, args });
        }

        private void UpdateUsers() => Group.SendAsync("UpdateUsers",
            Players.Select(p => new ResponseUserData()
            {
                Name = p.User.Nick,
                IsAdmin = p.IsAdmin,
                Color = p.Color
            }), MaxPlayers);

        public static bool Parse(object[] p, params Type[] what)
        {
            if (p.Length != what.Length) return false;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].GetType() != what[i]) return false;
            }
            return true;
        }

        #region Actions

        [CarcassonneAction]
        public void GetUsers(string conn, User user, object[] args)
        {
            Game.HubContext.Clients.Client(conn).SendAsync("UpdateUsers",
                Players.Select(p => new ResponseUserData()
                {
                    Name = p.User.Nick,
                    IsAdmin = p.IsAdmin,
                    Color = p.Color
                }), MaxPlayers);
        }

        #endregion

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
