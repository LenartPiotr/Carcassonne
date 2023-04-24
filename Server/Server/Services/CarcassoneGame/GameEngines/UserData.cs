using System.Runtime.CompilerServices;
using Server.Models;

namespace Server.Services.CarcassoneGame.GameEngines
{
    public class UserData
    {
        public User User { get; set; }
        public string Color { get; set; }
        public int Score { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
        public UserData(User user, string color) {
            User = user;
            Attributes = new Dictionary<string, object>();
            Color = color;
            Score = 0;
        }

        public object this[string name]
        {
            get { return Attributes[name]; }
            set { Attributes[name] = value; }
        }
    }
}
