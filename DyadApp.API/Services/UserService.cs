using Microsoft.AspNetCore.Http;

namespace DyadApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int GetUserId()
        {
            var userId = _httpContext.HttpContext.User.Identity.Name;
            return userId != null ? int.Parse(userId) : 0;
        }
    }
}
