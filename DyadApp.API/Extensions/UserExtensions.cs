using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DyadApp.API.Extensions
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            return claims == null ? 0 : int.Parse(claims.Identity.Name);
        }

        public static int GetUserId(this HttpContext context)
        {
            return context.User.Identity.Name == null ? 0 : int.Parse(context.User.Identity.Name);
        }
    }
}
