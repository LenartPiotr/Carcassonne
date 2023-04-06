namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class DefaultResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public string Navigate { get; set; } = "";
    }
}
