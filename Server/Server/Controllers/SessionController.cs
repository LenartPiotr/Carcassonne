using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

namespace Server.Controllers
{
    public class SessionController : Controller
    {
        protected void CreateSession(User user)
        {
            var session = new SessionRepository();
            int sessionDurationMinutes = 60;
            string sessionUuid = session.CreateUserSession(user, sessionDurationMinutes);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(sessionDurationMinutes)
            };
            Response.Cookies.Append("session_uuid", sessionUuid, cookieOptions);
        }
    }
}
