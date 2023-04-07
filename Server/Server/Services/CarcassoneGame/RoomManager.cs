namespace Server.Services.CarcassoneGame
{
    public class RoomManager
    {
        public string Name { get; }
        public int MaxPlayers { get; private set; }
        public int PlayersCount { get {
                // TODO
                return 1;
            } }


        public RoomManager(string name)
        {
            Name = name;
            MaxPlayers = 4;
        }
    }
}
