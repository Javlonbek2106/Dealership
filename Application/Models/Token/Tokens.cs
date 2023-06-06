using System.Text.Json.Serialization;

namespace Domain.Entities.IdentityEntities.Token
{
    public class Tokens
    {
        [JsonPropertyName("access_Token")]
        public string Access_Token { get; set; }
        [JsonPropertyName("refresh_Token")]
        public string Refresh_Token { get; set; }
    }

}
