using Microsoft.AspNetCore.SignalR;

namespace DyadApp.API.Providers
{
    public class NameUserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}