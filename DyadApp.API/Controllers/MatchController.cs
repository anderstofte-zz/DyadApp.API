using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("AwaitingMatch")]
        public IActionResult AwaitingMatch([FromBody]int UserID)
        {
            var completion = _matchService.AddToAwaitingMatch(UserID);
            if (completion == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Match([FromBody]int UserID)
        {
            var completion =_matchService.SearchForMatch(UserID);
            if(completion == true)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}