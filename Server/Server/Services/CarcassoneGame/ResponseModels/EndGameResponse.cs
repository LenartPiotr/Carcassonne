namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class EndGameResponse : DefaultResponse
    {
        public UserData[] Users { get; set; } = Array.Empty<UserData>();

        public class UserData
        {
            public string Nick { get; set; } = "";
            public int Score { get; set; }
        }
    }
}
