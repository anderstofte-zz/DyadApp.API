using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
	public interface IChatService
	{
		Task AddMessage(NewChatMessageModel model);
        Task<MatchConversationModel> FetchChatMessages(int matchId, int userId);
        Task MarkMessagesAsRead(int matchId, int userId);
	}
}
