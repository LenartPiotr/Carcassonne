using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class User
    {
        [Column("id_user")]
        public int IdUser { get; set; }

        [JsonPropertyName("nick")]
        [Column("nick")]
        public string Nick { get; set; } = "";

        [JsonPropertyName("email")]
        [Column("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("password")]
        [Column("password")]
        public string Password { get; set; } = "";

        public override bool Equals(object? obj)
        {
            if (obj is User)
            {
                User u2 = (User)obj;
                return u2.IdUser == IdUser;
            }
            return base.Equals(obj);
        }
    }
}
