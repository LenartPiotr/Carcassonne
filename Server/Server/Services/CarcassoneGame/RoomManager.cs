using Server.Models;

namespace Server.Services.CarcassoneGame
{
    public class RoomManager
    {
        public string Name { get; }
        public int MaxPlayers { get; private set; }

        public List<User> Players { get; }

        public RoomManager(string name)
        {
            Name = name;
            MaxPlayers = 4;
            Players = new List<User>();
        }

        public void Join(User user)
        {
            Players.Add(user);
            //
        }

        public void Disconnect(User user)
        {
            Players.Remove(user);
            // Remove when empty
            //
        }
    }
}
