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
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILoggingService _loggingService;
        public ChatController(IChatService chatService, ILoggingService loggingService)
        {
            _chatService = chatService;
            _loggingService = loggingService;
        }

        [HttpGet("{id}")]
        public async Task<MatchConversationModel> FetchChat(int id)
        {
            var userId = User.GetUserId();
            await _loggingService.SaveAuditLog($"Retrieving match with id {id} for user with user id {userId}",
                AuditActionEnum.Read);
            return await _chatService.FetchChatMessages(id, userId);
        }

        [HttpPost("Read/{id}")]
        public async Task MessagesIsRead(int id)
        {
            var userId = User.GetUserId();
            await _loggingService.SaveAuditLog($"Updating chat messages for match with id {id} and user id {userId}",
                AuditActionEnum.Update);
            await _chatService.MarkMessagesAsRead(id, userId);
        }
    }
}
