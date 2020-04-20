using System;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data;
using DyadApp.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DyadAppContext _context;

        public UsersController(DyadAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentDateTime = DateTime.Now;

            var user = model.ToUser();
            user.Modified = currentDateTime;
            user.ModifiedBy = 0;
            user.Created = currentDateTime;
            user.CreatedBy = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}