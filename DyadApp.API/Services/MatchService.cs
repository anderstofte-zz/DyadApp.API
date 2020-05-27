using DyadApp.API.Models;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using DyadApp.API.Converters;

namespace DyadApp.API.Services
{
	public class MatchService : IMatchService
	{
        private readonly IUserRepository _userRepository;
        private readonly IMatchRepository _matchRepository;
		public MatchService(IUserRepository userRepository, IMatchRepository matchRepository)
        {
            _userRepository = userRepository;
            _matchRepository = matchRepository;
        }

		public async Task<bool> AddToAwaitingMatch(int userId)
		{
            if (await UserIsAlreadyAwaitingAMatch(userId))
            {
                return true;
            }

			var awaitingMatch = new AwaitingMatch { UserId = userId };
            await _matchRepository.AddAwaitingMatch(awaitingMatch);

			return true;
		}

        private async Task<bool> UserIsAlreadyAwaitingAMatch(int userId)
        {
            var awaitingMatch = await _matchRepository.GetAwaitingMatchByUserId(userId);
            return awaitingMatch != null; 
        }

        public async Task <bool> SearchForMatch(int userId)
		{
			var userToMatch = await _userRepository.GetUserById(userId);

            var awaitingMatch = await GetAwaitingMatchToMatchWithUser(userToMatch);
            if (awaitingMatch == null)
            {
                return false;
            }

            awaitingMatch.IsMatched = true;

            var userMatches = new List<UserMatch>
            {
                new UserMatch
                {
                    UserId = userToMatch.UserId,
                },
                new UserMatch
                {
                    UserId = awaitingMatch.UserId
                }
            };

            var match = new Match
            {
                UserMatches = userMatches
            };

            await _matchRepository.UpdateAwaitingMatch(awaitingMatch);
            await _matchRepository.AddMatch(match);
            return true;
        }

        private async Task<AwaitingMatch> GetAwaitingMatchToMatchWithUser(User userToMatch)
        {
            var awaitingMatches = await _matchRepository.GetAwaitingMatches();

            var filteredAndSortedAwaitingMatches = awaitingMatches?.Where(x =>
                    x.User.DateOfBirth.Year == userToMatch.DateOfBirth.Year && x.UserId != userToMatch.UserId)
                .OrderBy(x => x.Created);

            return filteredAndSortedAwaitingMatches?.FirstOrDefault();
        }

        public async Task<List<MatchViewModel>> FetchMatches(int userId)
        {
            var matches = await _matchRepository.GetMatches(userId);

            return matches.ToMatchViewToModel(userId);
        }
    }
}
