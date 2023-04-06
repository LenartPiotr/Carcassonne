using Microsoft.AspNetCore.Mvc;
using Server.DatabaseContext;
using Server.Exceptions;
using Server.Models;
using System.Text.Json;

namespace Server.Controllers
{
    public class SessionController : Controller
    {
        protected void CreateSession(User user)
        {
            HttpContext.Session.SetString("nick", user.Nick);
            HttpContext.Session.SetInt32("authorized", 1);
        }
        protected bool GetUser(out User user, AppDatabaseContext context)
        {
            if (HttpContext.Session.GetInt32("authorized") == null)
            {
                user = new User();
                return false;
            }
            User[] users = context.Users.Where(u => u.Nick == HttpContext.Session.GetString("nick")).ToArray();
            if (users.Length == 0)
            {
                user = new User();
                return false;
            }
            user = users[0];
            return true;
        }

        protected string NavigateToLogin(string message = "")
        {
            return JsonSerializer.Serialize(new StatusResponse() { Success = false, Message = message, Navigate = "/" });
        }

        class StatusResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public string Navigate { get; set; } = "";
        }
    }
}
