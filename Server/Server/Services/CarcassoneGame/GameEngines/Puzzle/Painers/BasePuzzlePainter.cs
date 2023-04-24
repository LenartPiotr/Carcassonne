using System.Drawing;
using System.Drawing.Imaging;

namespace Server.Services.CarcassoneGame.GameEngines.Puzzle.Painers
{
    public class BasePuzzlePainter
    {
        public static Bitmap Paint(IPuzzle puzzle)
        {
            int size = 64;
            float half = size / 2f;
            Bitmap bitmap = new(size, size);
            Graphics g = Graphics.FromImage(bitmap);

            PuzzleConnectionEnum top = puzzle.GetConnection(Direction.UP);
            PuzzleConnectionEnum right = puzzle.GetConnection(Direction.RIGHT);
            PuzzleConnectionEnum bottom = puzzle.GetConnection(Direction.DOWN);
            PuzzleConnectionEnum left = puzzle.GetConnection(Direction.LEFT);

            // brushes
            Brush bGrass = new SolidBrush(Color.Green);
            Brush bPath = new SolidBrush(Color.Yellow);
            Brush bRiver = new SolidBrush(Color.SkyBlue);
            Brush bCastle = new SolidBrush(Color.Blue);
            Brush bChurch = new SolidBrush(Color.Red);

            // grass
            g.FillRectangle(bGrass, 0, 0, 64, 64);

            // river
            int riverWidth = 10;
            float riverMin = half - riverWidth / 2f;
            float riverMax = half + riverWidth / 2f;
            if (top == PuzzleConnectionEnum.River) g.FillRectangle(bRiver, riverMin, 0, riverWidth, riverMax);
            if (bottom == PuzzleConnectionEnum.Path) g.FillRectangle(bRiver, riverMin, riverMin, riverWidth, riverMax);
            if (left == PuzzleConnectionEnum.Path) g.FillRectangle(bRiver, 0, riverMin, riverMax, riverWidth);
            if (right == PuzzleConnectionEnum.Path) g.FillRectangle(bRiver, riverMin, riverMin, riverMax, riverWidth);

            // paths
            int pathWidth = 16;
            float pathMin = half - pathWidth / 2f;
            float pathMax = half + pathWidth / 2f;
            if (top == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, 0, pathWidth, pathMax);
            if (bottom == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, pathMin, pathWidth, pathMax);
            if (left == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, 0, pathMin, pathMax, pathWidth);
            if (right == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, pathMin, pathMax, pathWidth);

            // castle
            float rCastleMin = half * MathF.Sqrt(2);
            if (top == PuzzleConnectionEnum.Castle) g.FillPie(bCastle, half - rCastleMin, -half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
            if (left == PuzzleConnectionEnum.Castle) g.FillPie(bCastle, -half - rCastleMin, half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
            if (bottom == PuzzleConnectionEnum.Castle) g.FillPie(bCastle, half - rCastleMin, size + half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
            if (right == PuzzleConnectionEnum.Castle) g.FillPie(bCastle, -half - rCastleMin, half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);

            // CASTLES ALAWAYS DISCONNECTED

            // church
            int churchSize = 30;
            float churchStart = half - churchSize / 2f;
            if (puzzle.IsChurch()) g.FillRectangle(bChurch, churchStart, churchStart, churchSize, churchSize);
            
            return bitmap;
        }
    }
}
