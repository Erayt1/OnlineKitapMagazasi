using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineKitapMagazasi.UI.Helpers
{
    public static class TokenHelper
    {
        public static string? GetUserRole(HttpContext context)
        {
            var token = context.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            return roleClaim?.Value;
        }
    }
}
