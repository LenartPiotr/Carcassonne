namespace Server.Services.CarcassoneGame.GameEngines
{
    public class Board<T>
    {
        private readonly Dictionary<int, Dictionary<int, T>> tab;

        public bool Empty => tab.Count == 0;

        public Board()
        {
            tab = new();
        }

        public T? this[int x, int y]
        {
            set
            {
                if (value == null) return;
                if (!tab.ContainsKey(x))
                {
                    tab.Add(x, new Dictionary<int, T>());
                    tab[x].Add(y, value);
                    return;
                }
                if (!tab[x].ContainsKey(y))
                {
                    tab[x].Add(y, value);
                    return;
                }
                tab[x][y] = value;
            }
            get
            {
                if (!tab.ContainsKey(x)) return default;
                if (!tab[x].ContainsKey(y)) return default;
                return tab[x][y];
            }
        }
    }
}
