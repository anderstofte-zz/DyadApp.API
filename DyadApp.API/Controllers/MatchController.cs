using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller
    {

        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost("AwaitingMatch")]
        public async Task<IActionResult> AwaitingMatch()
        {
            var userId = User.GetUserId();
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
            var isMatchFound = await _matchService.SearchForMatch(userId);

            if(!isMatchFound)
            {
                return BadRequest();
            }
            return Ok("Vi fandt et nyt match!");
        }

        [HttpGet("RetreiveMatchList")]
        public async Task<List<MatchViewModel>> RetreiveMatchList()
        {
            var userId = User.GetUserId();
            return await _matchService.RetreiveMatchList(userId);
        }

        [HttpGet("{id}")]
        public async Task<MatchConversationModel> FetchChat(int id)
        {
            return await _matchService.FetchChatMessages(id, User.GetUserId());
        }

        [HttpPost("Read/{id}")]
        public async Task MessagesIsRead(int id)
        {
             await _matchService.MarkMessagesAsRead(id, User.GetUserId());
        }
    }
}
