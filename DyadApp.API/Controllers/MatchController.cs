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
    public class MatchController : Controller
    {

        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

       /* [HttpPost("AwaitingMatch")]
        public async Task<IActionResult> AwaitingMatch()
        {
            var userId = User.GetUserId();
            var isAddedToQueueOfAwaitingMatches = await _matchService.AddToAwaitingMatch(userId);
            
            if (!isAddedToQueueOfAwaitingMatches)
            {
                return BadRequest("Der gik noget galt.");
            }

            return Ok("Der er ikke nogle tilgængelige matches til dig lige nu. Vi arbejder på højtryk for at finde et!");
        }*/

        [HttpPost]
        public IActionResult Match()
        {
            var userId = User.GetUserId();
            _matchService.AddToAwaitingMatch(userId);
            var isMatchFound =_matchService.SearchForMatch(userId);

            if(!isMatchFound)
            {
                return BadRequest();
            }
            return Ok("Vi fandt et nyt match!");
        }

        [HttpGet("RetreiveMatchList")]
        public List<Match> RetreiveMatchList()
        {
            var userId = User.GetUserId();
            return _matchService.RetreiveMatchList(userId);

        }
    }
}