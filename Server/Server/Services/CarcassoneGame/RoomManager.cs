using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Asn1.Ocsp;
using Server.Models;
using Server.Services.CarcassoneGame.GameEngines;
using Server.Services.CarcassoneGame.GameEngines.Components;
using Server.SignalRHubs;

namespace Server.Services.CarcassoneGame
{
    public class RoomManager
    {
        private ICarcassonneGame Game { get; }
        private IClientProxy Group => Game.HubContext.Clients.Group(Name);
        private IClientProxy Client(string conn) => Game.HubContext.Clients.Client(conn);

        private List<string> Colors { get; }
        private GameEngine? Engine { get; set; }

        public string Name { get; }
        public int MaxPlayers { get; private set; }

        public List<UserData> Players { get; }

        public RoomManager(string name, ICarcassonneGame game)
        {
            Name = name;
            MaxPlayers = 4;
            Players = new List<UserData>();
            Game = game;
            Colors = new List<string>() { "#ff0000", "#00ff00", "#0000ff", "#ffff00", "#ff00ff", "#00ffff" };
            Engine = null;
        }

        public void Join(User user)
        {
            Players.Add(new UserData()
            {
                User = user,
                IsAdmin = Players.Count == 0,
                Color = TakeFreeColor()
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
                ?.Invoke(this, new object[] { new Request(connectionId, user, args) });
        }

        public void GameAction(string connectionId, User user, string action, object[] args)
        {
            if (Engine == null) return;
            typeof(GameEngine).GetMethods()
                .FirstOrDefault(m =>
                    (m.GetCustomAttribute<CarcassonneAction>()?.Name == action && action != "none") ||
                    (m.Name == action && m.GetCustomAttribute<CarcassonneAction>()?.Name == "none"))
                ?.Invoke(Engine, new object[] { new Request(connectionId, user, args) });
        }

        private void UpdateUsers() => Group.SendAsync("UpdateUsers",
            Players.Select(p => new ResponseUserData()
            {
                Name = p.User.Nick,
                IsAdmin = p.IsAdmin,
                Color = p.Color
            }), MaxPlayers);

        bool IsAdmin(User user) => Players.Any(p => p.IsAdmin && p.User.IdUser == user.IdUser);
        string TakeFreeColor()
        {
            foreach (string color in Colors)
            {
                if (!Players.Any(p => p.Color == color))
                {
                    return color;
                }
            }
            return "#ffffff";
        }

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
        public void GetUsers(Request r)
        {
            Client(r.Conn).SendAsync("UpdateUsers",
                Players.Select(p => new ResponseUserData()
                {
                    Name = p.User.Nick,
                    IsAdmin = p.IsAdmin,
                    Color = p.Color
                }), MaxPlayers);
        }

        [CarcassonneAction]
        public void StartGame(Request r)
        {
            if (!IsAdmin(r.User)) return;
            if (Engine == null)
            {
                Engine = new GameEngine(Name, Game.HubContext, new List<IGameComponent>()
                {
                    new BaseComponent()
                }, Players.Select(p => new GameEngines.UserData(p.User, p.Color)).ToList());
                
                Group.SendAsync("Navigate", "/game");
            }
        }

        [CarcassonneAction]
        public void GetRoomName(Request r)
        {
            Client(r.Conn).SendAsync("GetRoomName", Name);
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

        public class Request
        {
            public string Conn { get; set; }
            public User User { get; set; }
            public object[] Args { get; set; }

            public Request(string conn, User user, object[] args)
            {
                Conn = conn;
                User = user;
                Args = args;
            }

            public bool ParseArgs(Type[] requied)
            {
                if (requied.Length > Args.Length) return false;
                for (int i = 0; i < Args.Length; i++)
                {
                    if (!Args[i].GetType().Equals(requied[i])) return false;
                }
                return true;
            }
        }
    }
}
