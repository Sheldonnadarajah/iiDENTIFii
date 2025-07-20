using System.Linq;
using System.Security.Claims;

namespace AspireApp1.ApiService.Application.Auth
{
    public static class AuthUtils
    {
        public static int? GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault(c => int.TryParse(c.Value, out _));
            if (userIdClaim == null) return null;
            if (!int.TryParse(userIdClaim.Value, out var userId)) return null;
            return userId;
        }
    }
}
