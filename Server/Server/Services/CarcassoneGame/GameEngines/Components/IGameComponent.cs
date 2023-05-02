using Server.Services.CarcassoneGame.GameEngines.Puzzle;

namespace Server.Services.CarcassoneGame.GameEngines.Components
{
    public interface IGameComponent
    {
        public void Init(List<UserData> users);
        public void AddDataToDictionary(UserData data, Dictionary<string, object> response);
        public List<IPuzzle> GetOrderedPuzzles();
        public List<IPuzzle> GetPuzzles();
        public bool CanPlace(IPuzzle firstPuzzle, Direction firstPuzzleDirection, IPuzzle secondPuzzle, Direction secondPuzzleDirection);
    }
}
