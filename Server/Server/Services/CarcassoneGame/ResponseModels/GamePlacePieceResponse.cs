namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class GamePlacePieceResponse : DefaultResponse
    {
        public string Bitmap { get; set; } = "";
        public string PlayerTurnNick { get; set; } = "";
    }
}
