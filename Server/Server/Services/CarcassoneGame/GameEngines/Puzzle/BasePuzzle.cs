using System.Drawing;
using Server.Services.CarcassoneGame.GameEngines.Puzzle.Painers;

namespace Server.Services.CarcassoneGame.GameEngines.Puzzle
{
    public class BasePuzzle : IPuzzle
    {
        PuzzleConnectionEnum[] connections;
        bool isChurch;
        bool isPathEnd;
        bool isCastlseOne;

        public BasePuzzle(string data)
        {
            isChurch = data[1] == 't';
            isPathEnd = data[2] == 't';
            isCastlseOne = data[3] == 't';
            connections = new PuzzleConnectionEnum[]
            {
                (PuzzleConnectionEnum)(data[4] - '0'),
                (PuzzleConnectionEnum)(data[5] - '0'),
                (PuzzleConnectionEnum)(data[6] - '0'),
                (PuzzleConnectionEnum)(data[7] - '0')
            };
        }

        public string GetBitmapData()
        {
            return "B" + (isChurch ? 't' : 'f') + (isPathEnd ? 't' : 'f') + (isCastlseOne ? 't' : 'f') +
                ((int)connections[0]).ToString() + ((int)connections[1]).ToString() + ((int)connections[2]).ToString() + ((int)connections[3]).ToString();
        }

        public BasePuzzle(PuzzleConnectionEnum[] connections, bool haveChurch, bool oneCastle)
        {
            this.connections = connections;
            isChurch = haveChurch;
            isCastlseOne = oneCastle;
            isPathEnd = true;
            int pathConnections = 0;
            foreach (var c in connections) if (c == PuzzleConnectionEnum.Path) pathConnections++;
            if (pathConnections == 2 && !isChurch)
            {
                if (!isCastlseOne) isPathEnd = false;
                else isPathEnd = (connections[0] == PuzzleConnectionEnum.Castle && connections[2] == PuzzleConnectionEnum.Castle)
                        || (connections[1] == PuzzleConnectionEnum.Castle && connections[3] == PuzzleConnectionEnum.Castle);
            }
        }

        public List<Direction> AreaConnectedWith(Direction direction)
        {
            var conn = new List<Direction>();
            var type = connections[(int)direction];
            switch (type)
            {
                case PuzzleConnectionEnum.Path:
                case PuzzleConnectionEnum.Castle:
                case PuzzleConnectionEnum.River:
                    if (type == PuzzleConnectionEnum.Path && isPathEnd) break;
                    if (type == PuzzleConnectionEnum.Castle && !isCastlseOne) break;
                    foreach (Direction d in Enum.GetValues<PuzzleConnectionEnum>())
                    {
                        if (d != direction && connections[(int)d] == type) conn.Add(d);
                    }
                    break;
            }
            return conn;
        }

        public Bitmap GetBitmap()
        {
            return BasePuzzlePainter.Paint(this);
        }

        public PuzzleConnectionEnum GetConnection(Direction direction)
        {
            return connections[(int)direction];
        }

        public bool IsChurch()
        {
            return isChurch;
        }
    }
}
