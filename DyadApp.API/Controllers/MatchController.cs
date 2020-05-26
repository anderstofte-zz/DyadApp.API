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
        private readonly IChatService _chatService;
        public MatchController(IMatchService matchService, IChatService chatService)
        {
            _matchService = matchService;
            _chatService = chatService;
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

        [HttpGet("FetchMatches")]
        public async Task<List<MatchViewModel>> FetchMatches()
        {
            var userId = User.GetUserId();
            return await _matchService.FetchMatches(userId);
        }

        [HttpGet("{id}")]
        public async Task<MatchConversationModel> FetchChat(int id)
        {
            return await _chatService.FetchChatMessages(id, User.GetUserId());
        }

        [HttpPost("Read/{id}")]
        public async Task MessagesIsRead(int id)
        {
             await _chatService.MarkMessagesAsRead(id, User.GetUserId());
        }
    }
}
