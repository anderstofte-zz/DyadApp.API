using DyadApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
	public interface IMatchService
	{
		
		Task<bool> AddToAwaitingMatch(int userId);
        Task<bool> SearchForMatch(int userId);
		Task<List<MatchViewModel>> RetreiveMatchList(int userId);
        Task<MatchConversationModel> FetchChatMessages(int matchId, int userId);
        Task MarkMessagesAsRead(int matchId, int userId);
    }
}
