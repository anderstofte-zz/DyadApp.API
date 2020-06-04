using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.Extensions;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DyadApp.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        public readonly Dictionary<int, string> Connections = new Dictionary<int, string>();

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(NewChatMessageModel model)
        {
            var userId = Context.User.GetUserId();
            await _chatService.AddMessage(model);

            var newChatMessage = new ChatMessageModel
            {
                Message = model.Message,
                UserId = userId,
                Sent = DateTime.Now
            };
            await Clients.All.SendAsync("ReceiveMessage", newChatMessage);
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var contextUserId = Context.User.GetUserId();
            AddConnection(contextUserId, connectionId);
            await base.OnConnectedAsync();
        }

        public void AddConnection(int userId, string connectionId)
        {
            Connections.Add(userId, connectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.GetUserId();
            RemoveConnection(userId);
            await base.OnDisconnectedAsync(exception);
        }

        public void RemoveConnection(int userId)
        {
            Connections.Remove(userId);
        }
    }
}
