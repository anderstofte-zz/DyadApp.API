using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ILoggingService _loggingService;
        public MatchController(IMatchService matchService, ILoggingService loggingService)
        {
            _matchService = matchService;
            _loggingService = loggingService;
        }

        [HttpPost("AwaitingMatch")]
        public async Task<IActionResult> AwaitingMatch()
        {
            var userId = User.GetUserId();

            await _loggingService.SaveAuditLog($"Creating awating match for user with user id {userId}",
                AuditActionEnum.Create);

            var isAddedToQueueOfAwaitingMatches = await _matchService.AddToAwaitingMatch(userId);
            if (!isAddedToQueueOfAwaitingMatches)
            {
                return BadRequest("Der gik noget galt.");
            }

            return Ok("Der er ikke nogle tilgængelige matches til dig lige nu. Vi arbejder på højtryk for at finde et!");
        }

        [HttpPost]
        public async Task<IActionResult> Match()
        {
            var userId = User.GetUserId();

            await _loggingService.SaveAuditLog($"Match process initiated for user with user id {userId}",
                AuditActionEnum.Match);

            var isMatchFound = await _matchService.SearchForMatch(userId);
            if(!isMatchFound)
            {
                return BadRequest();
            }
            return Ok("Vi fandt et nyt match!");
        }

        [HttpGet("FetchMatches")]
        public async Task<List<MatchViewModel>> FetchMatches()
        {
            var userId = User.GetUserId();
            await _loggingService.SaveAuditLog($"Retrieving matches for user with user id {userId}",
                AuditActionEnum.Read);

            return await _matchService.FetchMatches(userId);
        }
    }
}
