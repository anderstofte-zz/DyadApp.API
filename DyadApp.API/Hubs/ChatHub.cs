using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Extensions;
using DyadApp.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DyadApp.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ChatHub : Hub
    {
        private readonly DyadAppContext _context;
        public readonly Dictionary<int, string> Connections = new Dictionary<int, string>();

        public ChatHub(DyadAppContext context)
        {
            _context = context;
        }

        public async Task SendMessage(NewChatMessageModel model)
        {
            var userId = Context.User.GetUserId();

            var chatMethods = new ChatMethods(_context);
            await chatMethods.AddMessage(model);

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
