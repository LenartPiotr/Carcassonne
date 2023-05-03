namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class GamePlacePuzzleResponse : DefaultResponse
    {
        public string BitmapData { get; set; } = "";
        public int X { get; set; }
        public int Y { get; set; }
    }
}
