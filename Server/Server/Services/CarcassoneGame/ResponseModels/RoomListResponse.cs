namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class RoomListResponse : DefaultResponse
    {
        public class RoomElement
        {
            public string Name { get; set; } = "";
            public int Min { get; set; } = 0;
            public int Max { get; set; } = 0;
        }

        public List<RoomElement> Rooms { get; set; } = new List<RoomElement>();
    }
}
