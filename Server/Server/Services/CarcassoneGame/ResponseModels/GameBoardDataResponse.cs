namespace Server.Services.CarcassoneGame.ResponseModels
{
    public class GameBoardDataResponse: DefaultResponse
    {
        public PuzzleData[] Puzzles { get; set; } = Array.Empty<PuzzleData>();

        public class PuzzleData
        {
            public string BitmapData { get; set; } = "";
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
