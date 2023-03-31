using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : Controller
    {
        string someString = "";
        public LobbyController()
        {
            someString = new Random().NextDouble().ToString();
        }

        [HttpPost]
        [Route("getroomslist")]
        public string GetRoomsList()
        {
            return someString;
        }
    }
}
