using Server.Services.CarcassoneGame.ResponseModels;

namespace Server.Services.CarcassoneGame
{
    public interface ICarcassonneGame
    {
        public RoomListResponse getRoomLists();
    }

    public class CarcassonneGame : ICarcassonneGame
    {
        public CarcassonneGame()
        {
            //
        }

        public RoomListResponse getRoomLists()
        {
            return new RoomListResponse()
            {
                Rooms = new List<RoomListResponse.RoomElement>()
                {
                    new RoomListResponse.RoomElement(){Name = "Lobby 1", Min = 1, Max = 5},
                    new RoomListResponse.RoomElement(){Name = "Lobby 2", Min = 2, Max = 5},
                    new RoomListResponse.RoomElement(){Name = "Lobby 3", Min = 3, Max = 5},
                    new RoomListResponse.RoomElement(){Name = "Lobby 4", Min = 4, Max = 5},
                }
            };
        }
    }
}
