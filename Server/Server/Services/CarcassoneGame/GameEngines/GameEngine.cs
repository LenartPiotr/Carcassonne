using System.Drawing.Imaging;
using Microsoft.AspNetCore.SignalR;
using Server.Extensions;
using Server.Models;
using Server.Services.CarcassoneGame.GameEngines.Components;
using Server.Services.CarcassoneGame.GameEngines.Puzzle;
using Server.Services.CarcassoneGame.ResponseModels;
using Server.SignalRHubs;

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

        int turnIndex;

        public GameEngine(string name, IHubContext<GlobalHub> hub, List<IGameComponent> components, List<UserData> users)
        {
            Name = name;
            Hub = hub;

            Components = components;
            Users = users;
            components.ForEach(c => c.Init(users));

            Puzzles = new List<IPuzzle>();
            components.ForEach(c => Puzzles.AddRange(c.GetPuzzles()));
            Puzzles.Shuffle();

            components.ForEach(c => Puzzles.AddRange(c.GetOrderedPuzzles()));
            Puzzles.Reverse();

            turnIndex = 0;
        }

        [CarcassonneAction]
        public void GetPlayersData(string conn, User user, object[] args)
        {
            Client(conn).SendAsync("GetPlayersData", new GameUsersDataResponse()
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
        public void GetGameStage(string conn, User user, object[] args)
        {
            Client(conn).SendAsync("PlacePiece", new GamePlacePieceResponse()
            {
                PlayerTurnNick = Users[turnIndex].User.Nick,
                Bitmap = Puzzles[0].GetBitmapData()
            });
        }
    }
}
