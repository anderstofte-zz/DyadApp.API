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
        Task<AwaitingMatch> GetAwaitingMatchByUserId(int userId);
        Task<List<AwaitingMatch>> GetAwaitingMatches();
        Task<List<Match>> GetMatches(int userId);
    }
}
