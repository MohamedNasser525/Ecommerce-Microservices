using System.Text.Json.Serialization;

namespace AuthServer.Models
{
    public class AuthModel
    {
        public string userid { get; set; }
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool?EmailConfirmed { get; set; }
        public byte[] ?Profileimg { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
        public byte[]? EncreptedToken { get; set; }

        public DateTime? ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}