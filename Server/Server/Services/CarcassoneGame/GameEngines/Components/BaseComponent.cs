using Server.Services.CarcassoneGame.GameEngines.Puzzle;

namespace Server.Services.CarcassoneGame.GameEngines.Components
{
    public class BaseComponent : IGameComponent
    {
        public void Init(List<UserData> users)
        {
            foreach (var user in users)
            {
                user["pawnsCount"] = 10;
            }
        }

        public void AddDataToDictionary(UserData data, Dictionary<string, object> response)
        {
            response.Add("pawnsCount", data.Attributes["pawnsCount"]);
        }

        PuzzleConnectionEnum ConnectionFromLetter(char letter)
        {
            switch (letter)
            {
                case 'r': return PuzzleConnectionEnum.River;
                case 'c': return PuzzleConnectionEnum.Castle;
                case 'p': return PuzzleConnectionEnum.Path;
                default:
                    return PuzzleConnectionEnum.Grass;
            }
        }

        IPuzzle GetPuzzleFromPhrase(string code)
        {
            return new BasePuzzle(new PuzzleConnectionEnum[] {
                ConnectionFromLetter(code[0]),
                ConnectionFromLetter(code[1]),
                ConnectionFromLetter(code[2]),
                ConnectionFromLetter(code[3])
            }, code[4] == 't', code[5] == 't');
        }

        public List<IPuzzle> GetOrderedPuzzles()
        {
            return new List<IPuzzle>() { GetPuzzleFromPhrase("cpgpnn") };
        }

        public List<IPuzzle> GetPuzzles()
        {
            List<string> puzzles = new List<string>()
            {
                "ggpgtn", "ggpgtn", "ggggtn", "ggggtn", "ggggtn", "ggggtn",
                "ccccnt", "cpgpnn", "cpgpnn", "cpgpnn", "cgggnn", "cgggnn", "cgggnn", "cgggnn", "cgggnn",
                "gcgcnt", "gcgcnt", "gcgcnt", "gcgcnn", "gcgcnn", "gcgcnn", "cggcnn", "cggcnn",
                "cppgnn", "cppgnn", "cppgnn", "cgppnn", "cgppnn", "cgppnn", "cpppnn", "cpppnn", "cpppnn",
                "ccggnt", "ccggnt", "ccggnt", "ccggnt", "ccggnt",
                "cppcnt", "cppcnt", "cppcnt", "cppcnt", "cppcnt",
                "ccgcnt", "ccgcnt", "ccgcnt", "ccgcnt", "ccpcnt", "ccpcnt", "ccpcnt",
                "pgpgnn", "pgpgnn", "pgpgnn", "pgpgnn", "pgpgnn", "pgpgnn", "pgpgnn", "pgpgnn",
                "ggppnn", "ggppnn", "ggppnn", "ggppnn", "ggppnn", "ggppnn", "ggppnn", "ggppnn", "ggppnn",
                "gpppnn", "gpppnn", "gpppnn", "gpppnn", "ppppnn"
            };
            return puzzles.Select(s => GetPuzzleFromPhrase(s)).ToList();
        }
    }
}
