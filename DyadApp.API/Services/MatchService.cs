using DyadApp.API.Models;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using DyadApp.API.Converters;
using DyadApp.API.Extensions;
using Microsoft.Extensions.Configuration;

namespace DyadApp.API.Services
{
	public class MatchService : IMatchService
	{
        private readonly IUserRepository _userRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IConfiguration _configuration;

		public MatchService(IUserRepository userRepository, IMatchRepository matchRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _matchRepository = matchRepository;
            _configuration = configuration;
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
            var awaitingMatches = await _matchRepository.RetrieveAwaitingMatches();
            return awaitingMatches.Any(x => x.UserId == userId && x.IsMatched == false);
        }

        public async Task <bool> SearchForMatch(int userId)
		{
			var userToMatch = await _userRepository.GetUserById(userId);
            var awaitingMatch = await GetAwaitingMatchToMatchWithUser(userToMatch);
            if (awaitingMatch == null)
            {
                return false;
            }

            var isUserToMatchAndAwaitingMatchAlreadyMatched = await IsMatchUnique(userId, awaitingMatch.UserId);
            if (isUserToMatchAndAwaitingMatchAlreadyMatched)
            {
                return false;
            }

            awaitingMatch.IsMatched = true;

            var userMatches = CreateUserMatches(userToMatch, awaitingMatch);

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
            var awaitingMatches = await _matchRepository.RetrieveAwaitingMatches();
            if (awaitingMatches == null)
            {
                return null;
            }

            var filteredAndSortedAwaitingMatches = awaitingMatches.Where(x =>
                    x.User.DateOfBirth.Year == userToMatch.DateOfBirth.Year && x.UserId != userToMatch.UserId)
                .OrderBy(x => x.Created);

            return filteredAndSortedAwaitingMatches?.FirstOrDefault();
        }

        private async Task<bool> IsMatchUnique(int userId, int awaitingMatchUserId)
        {
            var userMatches = await _matchRepository.RetrieveUserMatches();
            if (userMatches == null)
            {
                return false;
            }

            var filteredUserMatches = userMatches.Where(x => x.UserId == userId || x.UserId == awaitingMatchUserId).ToList();
            var usersAreAlreadyMatched = filteredUserMatches.GroupBy(x => x.MatchId).Any(x => x.Count() == 2);
            return usersAreAlreadyMatched;
        }

        private static List<UserMatch> CreateUserMatches(User userToMatch, AwaitingMatch awaitingMatch)
        {
            return new List<UserMatch>
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
        }

        public async Task<List<MatchViewModel>> RetrieveMatches(int userId)
        {
            var matches = await _matchRepository.RetrieveMatches(userId);
            var encryptionKey = _configuration.GetEncryptionKey();
            return matches.ToMatchViewToModel(userId, encryptionKey);
        }
    }
}
