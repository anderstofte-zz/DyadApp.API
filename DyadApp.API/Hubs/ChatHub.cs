using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DyadApp.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string something)
        {
            var somethingElse = Context.ConnectionId;
            await Clients.All.SendAsync("ReceiveMessage", something);
        }

        public override async Task OnConnectedAsync()
        {
            var something = Context.User.FindFirst(c =>
                c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            Debug.WriteLine(something);

            var connectionId = Context.ConnectionId;
            var contextUserId = Context.User;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}