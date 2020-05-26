using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;
        public AccountController(IUserRepository userRepository, ILoggingService loggingService)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        [HttpPost("Status")]
        public async Task<IActionResult> SetAccountStatus([FromBody] bool status)
        {
            var message = status
                ? $"Activated account with user id {User.GetUserId()}."
                : $"Deactivated account with user id {User.GetUserId()}.";
            var action = status
                ? AuditActionEnum.Activate
                : AuditActionEnum.Deactivate;

            await _loggingService.SaveAuditLog(message, action);

            var user = await _userRepository.GetUserById(User.GetUserId());
            user.Active = status;
            await _userRepository.SaveChangesAsync();
            return Ok();
        }
    }
}