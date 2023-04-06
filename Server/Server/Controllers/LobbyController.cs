using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server.DatabaseContext;
using Server.Models;
using Server.Services.CarcassoneGame;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : SessionController
    {
        private ICarcassonneGame game;
        private AppDatabaseContext _context;

        public LobbyController(ICarcassonneGame carcassonneGame, AppDatabaseContext databaseContext)
        {
            game = carcassonneGame;
            _context = databaseContext;
        }

        [HttpPost]
        [Route("getroomslist")]
        public string GetRoomsList()
        {
            if (!GetUser(out User user, _context)) return NavigateToLogin();
            return JsonSerializer.Serialize(game.getRoomLists());
        }
    }
}
