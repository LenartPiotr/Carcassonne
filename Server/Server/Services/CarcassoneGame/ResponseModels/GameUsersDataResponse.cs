namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class GameUsersDataResponse : DefaultResponse
    {
        public class UserData
        {
            public string Name { get; set; } = "";
            public string Color { get; set; } = "";
            public int Score { get; set; } = 0;
            public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        }
        public List<UserData> Users { get; set; } = new List<UserData>();
    }
}
