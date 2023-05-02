using System.Drawing;

namespace Server.Services.CarcassoneGame.GameEngines.Puzzle
{
    public interface IPuzzle
    {
        PuzzleConnectionEnum GetConnection(Direction direction);
        List<Direction> AreaConnectedWith(Direction direction);
        Bitmap GetBitmap();
        string GetBitmapData();
        bool IsChurch();
        void Rotate(int quarters);
    }
}
