using System.Text.Json.Serialization;

namespace Application.Models.UserCredentials
{
    public class UserCredentials
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
