using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IMatchRepository
    {
        Task AddMatch(Match match);
        Task AddAwaitingMatch(AwaitingMatch awaitingMatch);
        Task UpdateAwaitingMatch(AwaitingMatch awaitingMatch);
        Task<bool> UserIsAlreadyAwaitingAMatch(int userId);
        Task<AwaitingMatch> GetAwaitingMatch(User userToMatch);
        Task<List<Match>> GetMatchList(int userId);
    }
}