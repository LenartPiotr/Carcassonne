using System.Drawing;
using System.Drawing.Imaging;

namespace Server.Services.CarcassoneGame.GameEngines.Puzzle.Painers
{
    public class BasePuzzlePainter
    {
        public static Bitmap Paint(IPuzzle puzzle)
        {
            int size = 256;
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
            g.FillRectangle(bGrass, 0, 0, size, size);

            // river
            float riverWidth = size * 0.5f;
            float riverMin = half - riverWidth / 2f;
            float riverMax = half + riverWidth / 2f;
            if (top == PuzzleConnectionEnum.River) g.FillRectangle(bRiver, riverMin, 0, riverWidth, riverMax);
            if (bottom == PuzzleConnectionEnum.River) g.FillRectangle(bRiver, riverMin, riverMin, riverWidth, riverMax);
            if (left == PuzzleConnectionEnum.River) g.FillRectangle(bRiver, 0, riverMin, riverMax, riverWidth);
            if (right == PuzzleConnectionEnum.River) g.FillRectangle(bRiver, riverMin, riverMin, riverMax, riverWidth);

            // paths
            float pathWidth = size * 0.3f;
            float pathMin = half - pathWidth / 2f;
            float pathMax = half + pathWidth / 2f;
            if (top == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, 0, pathWidth, pathMax);
            if (bottom == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, pathMin, pathWidth, pathMax);
            if (left == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, 0, pathMin, pathMax, pathWidth);
            if (right == PuzzleConnectionEnum.Path) g.FillRectangle(bPath, pathMin, pathMin, pathMax, pathWidth);

            // castle
            float rCastleMin = half * MathF.Sqrt(2);
            float rCastleMax = size * 2;
            float dCastle = size * 0.8f;
            float dCastleMin = (size - dCastle) / 2f;
            if (top == PuzzleConnectionEnum.Castle)
            {
                g.FillPie(bCastle, half - rCastleMin, -half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
                foreach (var side in puzzle.AreaConnectedWith(Direction.UP))
                    switch (side)
                    {
                        case Direction.LEFT: g.FillPie(bCastle, -size, -size, rCastleMax, rCastleMax, 0, 360); break;
                        case Direction.RIGHT: g.FillPie(bCastle, 0, -size, rCastleMax, rCastleMax, 0, 360); break;
                        case Direction.DOWN: g.FillRectangle(bCastle, dCastleMin, 0, dCastle, size); break;
                    }
            }
            if (left == PuzzleConnectionEnum.Castle)
            {
                g.FillPie(bCastle, -half - rCastleMin, half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
                foreach (var side in puzzle.AreaConnectedWith(Direction.LEFT))
                    switch (side)
                    {
                        case Direction.RIGHT: g.FillRectangle(bCastle, 0, dCastleMin, size, dCastle); break;
                        case Direction.DOWN: g.FillPie(bCastle, -size, 0, rCastleMax, rCastleMax, 0, 360); break;
                    }
            }
            if (bottom == PuzzleConnectionEnum.Castle)
            {
                g.FillPie(bCastle, half - rCastleMin, size + half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);
                foreach (var side in puzzle.AreaConnectedWith(Direction.DOWN))
                    switch (side)
                    {
                        case Direction.RIGHT: g.FillPie(bCastle, 0, 0, rCastleMax, rCastleMax, 0, 360); break;
                    }
            }
            if (right == PuzzleConnectionEnum.Castle) g.FillPie(bCastle, size + half - rCastleMin, half - rCastleMin, 2 * rCastleMin, 2 * rCastleMin, 0, 360);

            // church
            float churchSize = size * 0.6f;
            float churchStart = half - churchSize / 2f;
            if (puzzle.IsChurch()) g.FillRectangle(bChurch, churchStart, churchStart, churchSize, churchSize);
            
            return bitmap;
        }
    }
}
