namespace Server.Services.CarcassoneGame.GameEngines
{
    public enum Direction
    {
        UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3
    }

    public static class DirectionExcension
    {
        public static Direction Opposite(this Direction direction)
        {
            int n = (int)direction;
            return (Direction)((n + 2) % 4);
        }
        public static Direction Right(this Direction direction)
        {
            int n = (int)direction;
            return (Direction)((n + 1) % 4);
        }
        public static Direction Left(this Direction direction)
        {
            int n = (int)direction;
            return (Direction)((n + 3) % 4);
        }
        public static int GetX(this Direction direction)
        {
            int n = (int)direction;
            if (n % 2 != 0) return -n + 2;
            return 0;
        }
        public static int GetY(this Direction direction)
        {
            int n = (int)direction;
            if (n % 2 == 0) return n - 1;
            return 0;
        }
    }
}
