using System.Security.Claims;

namespace DyadApp.API.Extensions
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            return claims == null ? 0 : int.Parse(claims.Identity.Name);
        }
    }
}