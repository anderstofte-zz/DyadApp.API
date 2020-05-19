using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public interface IMatchService
	{
		
		Task<bool> AddToAwaitingMatch(int userId);
		bool SearchForMatch(int userId);
	}
}
