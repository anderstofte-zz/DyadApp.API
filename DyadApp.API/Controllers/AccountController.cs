using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.Emails.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;
        private readonly IEmailService _emailService;
        private readonly IMatchRepository _matchRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IUserRepository userRepository, ILoggingService loggingService, IEmailService emailService, IMatchRepository matchRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
            _emailService = emailService;
            _matchRepository = matchRepository;
            _configuration = configuration;
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
            var user = await _userRepository.GetUserById(userId);
            var matches = await _matchRepository.GetMatches(userId);

            var encryptionKey = _configuration.GetEncryptionKey();
            var model = user.ToUserDataModel(matches, encryptionKey);
            var jsonString = JsonConvert.SerializeObject(model);
            
            var userData = new EmailData
            {
                Email = user.Email,
                UserData = jsonString,
                Type = EmailTypeEnum.DataInsight
            };

            await _emailService.SendEmail(userData);
            return Ok();
        }
    }
}
