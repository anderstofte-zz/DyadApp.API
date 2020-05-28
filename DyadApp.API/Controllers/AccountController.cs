using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;
        private readonly IMatchRepository _matchRepository;
        public AccountController(IUserRepository userRepository, ILoggingService loggingService, IMatchRepository matchRepository)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
            _matchRepository = matchRepository;
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

        [HttpGet]
        public async Task<IActionResult> DataGeneratedByUser()
        {
            var userId = User.GetUserId();
            var user = await  _userRepository.GetUserById(userId);
            var matches = await _matchRepository.GetMatches(userId);

            var model = new Something
            {
                User = user,
                Matches = matches
            };

            var jsonString = JsonSerializer.Serialize(model);
            var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
            return File(bytes, MediaTypeNames.Application.Json, "file.json");
        }
    }

    public class Something
    {
        public User User { get; set; }
        public List<Match> Matches { get; set; }
    }
}