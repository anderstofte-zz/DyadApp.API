using DyadApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public interface IMatchService
	{
		
		Task<bool> AddToAwaitingMatch(int userId);
		bool SearchForMatch(int userId);
		List<Match> RetreiveMatchList(int userId);
	}
}
