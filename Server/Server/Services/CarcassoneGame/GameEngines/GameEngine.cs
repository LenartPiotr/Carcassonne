using System.Drawing.Imaging;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Server.Extensions;
using Server.Models;
using Server.Services.CarcassoneGame.GameEngines.Components;
using Server.Services.CarcassoneGame.GameEngines.Puzzle;
using Server.Services.CarcassoneGame.ResponseModels;
using Server.SignalRHubs;
using static Server.Services.CarcassoneGame.RoomManager;

namespace Server.Services.CarcassoneGame.GameEngines
{
    public class GameEngine
    {
        private string Name { get; }
        private IHubContext<GlobalHub> Hub { get; }

        private IClientProxy Group => Hub.Clients.Group(Name);
        private IClientProxy Client(string conn) => Hub.Clients.Client(conn);

        List<IGameComponent> Components { get; }
        List<UserData> Users { get; }
        List<IPuzzle> Puzzles { get; }

        Board<IPuzzle> board;

        RoomManager roomManager { get; }

        int turnIndex;

        public GameEngine(string name, IHubContext<GlobalHub> hub, List<IGameComponent> components, List<UserData> users, RoomManager roomManager)
        {
            Name = name;
            Hub = hub;

            Components = components;
            Users = users;
            board = new();
            components.ForEach(c => c.Init(users));

            Puzzles = new List<IPuzzle>();
            components.ForEach(c => Puzzles.AddRange(c.GetPuzzles()));
            Puzzles.Shuffle();

            components.ForEach(c => Puzzles.AddRange(c.GetOrderedPuzzles()));
            Puzzles.Reverse();

            turnIndex = 0;
            this.roomManager = roomManager;
        }

        private bool GetPuzzleFromData(string data, int rot, out IPuzzle puzzle)
        {
            try
            {
                switch (data[0])
                {
                    case 'B': puzzle = new BasePuzzle(data); puzzle.Rotate(rot); return true;
                }
            }
            catch { }
            puzzle = new BasePuzzle();
            return false;
        }

        private void End()
        {
            var usersData = new EndGameResponse.UserData[Users.Count];
            for (int i = 0; i < Users.Count; i++)
            {
                usersData[i] = new EndGameResponse.UserData()
                {
                    Nick = Users[i].User.Nick,
                    Score = Users[i].Score
                };
            }
            Array.Sort(usersData, (a, b) => -a.Score.CompareTo(b.Score));

            roomManager.EndGame();

            Group.SendAsync("EndGame", new EndGameResponse()
            {
                Users = usersData
            });
        }

        private void NextPlayer()
        {
            turnIndex = (turnIndex + 1) % Users.Count;
            Puzzles.RemoveAt(0);

            if (Puzzles.Count == 0)
            {
                End();
                return;
            }

            Group.SendAsync("PlacePiece", new GamePlacePieceResponse()
            {
                PlayerTurnNick = Users[turnIndex].User.Nick,
                Bitmap = Puzzles[0].GetBitmapData()
            });
        }

        [CarcassonneAction]
        public void GetPlayersData(Request r)
        {
            Client(r.Conn).SendAsync("GetPlayersData", new GameUsersDataResponse()
            {
                Users = Users.Select(u =>
                {
                    Dictionary<string, object> responseData = new();
                    Components.ForEach(c => c.AddDataToDictionary(u, responseData));
                    return new GameUsersDataResponse.UserData()
                    {
                        Name = u.User.Nick,
                        Color = u.Color,
                        Score = u.Score,
                        Data = responseData
                    };
                }).ToList()
            });
        }

        [CarcassonneAction]
        public void GetGameStage(Request r)
        {
            Client(r.Conn).SendAsync("PlacePiece", new GamePlacePieceResponse()
            {
                PlayerTurnNick = Users[turnIndex].User.Nick,
                Bitmap = Puzzles[0].GetBitmapData()
            });
        }

        [CarcassonneAction]
        public void PlacePuzzle(Request r)
        {
            if (r.User.IdUser != Users[turnIndex].User.IdUser) return;
            string pieceData;
            int x, y, rot;
            try
            {
                pieceData = ((JsonElement)r.Args[0]).GetString() ?? throw new Exception();
                x = ((JsonElement)r.Args[1]).GetInt32();
                y = ((JsonElement)r.Args[2]).GetInt32();
                rot = ((JsonElement)r.Args[3]).GetInt32();
            }
            catch { return; }
            
            if (pieceData != Puzzles[0].GetBitmapData()) return;

            // Check if board has empty place
            if (board[x, y] != null) return;

            // Check if puzzle data if corrent
            if (!GetPuzzleFromData(pieceData, rot, out IPuzzle puzzle)) return;

            // Check if can connect with others
            int connectionsCount = 0;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                IPuzzle? puzzleInDirection = board[x + direction.GetX(), y + direction.GetY()];
                if (puzzleInDirection == null) continue;
                connectionsCount++;
                foreach (IGameComponent component in Components)
                    if (!component.CanPlace(puzzleInDirection, direction.Opposite(), puzzle, direction)) return;
            }

            // Check if any connection or first puzzle
            if (connectionsCount == 0 && !board.Empty) return;

            board[x, y] = puzzle;

            Group.SendAsync("PlacePuzzle", new GamePlacePuzzleResponse()
            {
                BitmapData = puzzle.GetBitmapData(),
                X = x,
                Y = y
            });

            // Change turn
            NextPlayer();
        }

        [CarcassonneAction]
        public void GetAllBoardData(Request r)
        {
            List<GameBoardDataResponse.PuzzleData> puzzles = new();
            board.ForEach((int x, int y, IPuzzle puzzle) =>
            {
                puzzles.Add(new GameBoardDataResponse.PuzzleData()
                {
                    X = x,
                    Y = y,
                    BitmapData = puzzle.GetBitmapData()
                });
            });
            Client(r.Conn).SendAsync("GetAllBoardData", new GameBoardDataResponse()
            {
                Puzzles = puzzles.ToArray()
            });
        }
    }
}
