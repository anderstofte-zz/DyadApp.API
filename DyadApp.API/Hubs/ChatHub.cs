using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DyadApp.API.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {
        private readonly DyadAppContext _context;

        public ChatHub(DyadAppContext context)
        {
            _context = context;
        }


        Dictionary<string, int> connections = new Dictionary<string, int>();


        public async Task SendMessage(string message)
        {
            var UserId = Context.ConnectionId;
            AddMessage(1, 2, message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }


        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var contextUserId = 1;//Context.User.GetUserId();
            AddConnection(connectionId, contextUserId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            RemoveConnecection(connectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public void AddConnection(string connectionId, int userId)
        {
            connections.Add(connectionId, userId);
        }

        public void RemoveConnecection(string connectionId)
        {
            connections.Remove(connectionId);
        }

        public void AddMessage(int SenderId, int ReceiverId, string Message)
        {
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.Message = Message;
            chatMessage.SenderId = SenderId;
            chatMessage.ReceiverId = ReceiverId;
            chatMessage.Timestamp = DateTime.Now;
            _context.Messenges.Add(chatMessage);
            _context.SaveChanges();

        }

    }
}