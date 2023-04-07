using Server.Services.CarcassoneGame.ResponseModels;
using System.Linq;

namespace Server.Services.CarcassoneGame
{
    public interface ICarcassonneGame
    {
        public RoomListResponse getRoomLists();
    }

    public class CarcassonneGame : ICarcassonneGame
    {
        readonly List<RoomManager> rooms;

        public CarcassonneGame()
        {
            rooms = new();
        }

        public RoomListResponse getRoomLists()
        {
            return new RoomListResponse()
            {
                Rooms = rooms.Select(r => new RoomListResponse.RoomElement()
                {
                    Name = r.Name,
                    Min = r.PlayersCount,
                    Max = r.MaxPlayers
                }).ToList()
            };
        }
    }
}
