using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
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

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Status")]
        public async Task<IActionResult> SetAccountStatus([FromBody] bool status)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            user.Active = status;
            await _userRepository.SaveChangesAsync();
            return Ok();
        }
    }
}