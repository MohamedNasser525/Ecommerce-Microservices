using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace AuthServer.Helper
{
    public class IDCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IDCheck(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserIdValid(string id)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return false;

            // Get the JWT from the Authorization Header
            var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(jwtToken))
            {
                // Token is missing
                return false;
            }

            // Decode the JWT
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                // Token could not be read
                return false;
            }

            // Extract the userId claim
            var userIdFromToken = jsonToken.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            // Match the extracted userId with the provided id
            return userIdFromToken == id;
        }
    }
}
