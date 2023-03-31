using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Models;
using Server.Repositories;

namespace Server.Controllers
{
    public class SessionController : Controller
    {
        protected void CreateSession(User user)
        {
            HttpContext.Session.SetString("nick", user.Nick);
            HttpContext.Session.SetInt32("authorized", 1);
        }
        protected bool GetUser(out User user, UserRepository? repo = null)
        {
            if (HttpContext.Session.GetInt32("authorized") == null)
            {
                user = new User();
                return false;
            }
            if (repo == null) repo = new UserRepository();
            try
            {
                user = repo.GetUserByNickName(HttpContext.Session.GetString("nick") ?? "x");
            }catch (NoMatchingRecordsException)
            {
                user = new User();
                return false;
            }
            return true;
        }
    }
}
