using System.Text.Json.Serialization;

namespace Server.Models
{
    public class User
    {
        public int IdUser { get; set; }

        [JsonPropertyName("nick")]
        public string Nick { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}
