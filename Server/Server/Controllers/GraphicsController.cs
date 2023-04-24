using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using Server.Services.CarcassoneGame.GameEngines.Puzzle;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphicsController : Controller
    {
        [HttpGet]
        [Route("bitmap")]
        public ActionResult GetBitmap(string name)
        {
            Bitmap b = new(1, 1);
            try
            {
                switch (name[0])
                {
                    case 'B':
                        BasePuzzle bp = new(name);
                        b = bp.GetBitmap();
                        break;
                }
            }
            catch { }
            using var stream = new MemoryStream();
            b.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/jpeg");
        }
    }
}
