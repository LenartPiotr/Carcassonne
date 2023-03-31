using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Models;
using Server.Repositories;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : SessionController
    {
        [HttpPost]
        [Route("register")]
        public string Register([FromBody] JsonElement element)
        {
            User user = JsonSerializer.Deserialize<User>(element) ?? new User();

            if (!ValidPassword(user.Password, out string message) || !ValidUserEmail(user.Email, out message) || !ValidUserNick(user.Nick, out message))
            {
                StatusResponse res = new() { Success = false, Message = message };
                return JsonSerializer.Serialize(res);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt());

            UserRepository repository = new();
            int affectedRows = repository.AddUser(user);
            if (affectedRows == 1)
            {
                CreateSession(user);
                return JsonSerializer.Serialize(new StatusResponse() { Success = true, Message = "ok" });
            }
            return JsonSerializer.Serialize(new StatusResponse() { Success = false, Message = "cannot register user", Navigate = "Lobby" });
        }

        [HttpPost]
        [Route("login")]
        public string Login([FromBody] JsonElement element)
        {
            User clientUser = JsonSerializer.Deserialize<User>(element) ?? new User();
            User databaseUser;
            UserRepository repository = new();

            StatusResponse failedResponse = new() { Success = false, Message = "Login failed" };

            try {
                databaseUser = repository.GetUserByNickName(clientUser.Nick);
            } catch (NoMatchingRecordsException) {
                return JsonSerializer.Serialize(failedResponse);
            }

            if (!BCrypt.Net.BCrypt.Verify(clientUser.Password, databaseUser.Password))
            {
                return JsonSerializer.Serialize(failedResponse);
            }

            CreateSession(databaseUser);
            return JsonSerializer.Serialize(new StatusResponse() { Success = true, Message = "ok", Navigate = "Lobby" });
        }

        private bool ValidPassword(string password, out string message)
        {
            if (password.Length < 8)
            {
                message = "Password is too short";
                return false;
            }
            if (new Regex(@"^[a-zA-Z]*$").IsMatch(password))
            {
                message = "Password is too weak";
                return false;
            }
            // other conditions
            message = "";
            return true;
        }

        private bool ValidUserNick(string nick, out string message)
        {
            if (nick.Length < 3)
            {
                message = "Nick must have at least 3 characters";
                return false;
            }
            if (!new Regex(@"^[a-zA-Z 0-9]*$").IsMatch(nick))
            {
                message = "Unsupported character";
                return false;
            }
            // other conditions
            message = "";
            return true;
        }

        private bool ValidUserEmail(string email, out string message)
        {
            if (!new Regex(
                @"^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])$"
                ).IsMatch(email))
            {
                message = "Wrong email";
                return false;
            }
            // other conditions
            message = "";
            return true;
        }

        class StatusResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public string Navigate { get; set; } = "";
        }
    }
}
