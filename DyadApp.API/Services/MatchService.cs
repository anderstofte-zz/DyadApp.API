using DyadApp.API.Data;
using DyadApp.API.Models;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using DyadApp.API.Converters;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
	public class MatchService : IMatchService
	{
		private readonly DyadAppContext _context;
        private readonly IUserRepository _userRepository;
		public MatchService(DyadAppContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

		public async Task<bool> AddToAwaitingMatch(int userId)
		{
            if (await UserIsAlreadyAwaitingAMatch(userId))
            {
                return true;
            }

			var awaitingMatch = new AwaitingMatch { UserId = userId };
			_context.AwaitingMatches.Add(awaitingMatch);
			await _context.SaveChangesAsync();

			return true;
		}

        private async Task<bool> UserIsAlreadyAwaitingAMatch(int userId)
        {
            return await _context.AwaitingMatches.AnyAsync(x => x.UserId == userId && !x.IsMatched);
        }

        public async Task <bool> SearchForMatch(int userId)
		{
			var userToMatch = await _userRepository.GetUserById(userId);

            var awaitingMatch = await _context.AwaitingMatches.Where(aw =>
                    !aw.IsMatched && aw.User.DateOfBirth.Year == userToMatch.DateOfBirth.Year && aw.UserId != userToMatch.UserId)
                .OrderBy(x => x.Created).FirstOrDefaultAsync();

            if (awaitingMatch == null)
            {
                return false;
            }


            awaitingMatch.IsMatched = true;
            //TODO: lav user matches for de brugeres der er matchet

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

            //TODO: lav et match

            var match = new Match
            {
                UserMatches = userMatches
            };

            _context.AwaitingMatches.Update(awaitingMatch);
            await _context.SaveChangesAsync();
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return true;
        }

		public async Task<List<MatchViewModel>> RetreiveMatchList(int userId)
		{
            var matches = await _context.Matches
                .Include(x => x.UserMatches)
                .ThenInclude(um => um.User)
                .Include(x => x.ChatMessages)
                .Where(x => x.UserMatches.Any(z => z.UserId == userId))
                .ToListAsync();

            return matches.ToMatchViewToModel(userId);
        }

        public async Task<MatchConversationModel> FetchChatMessages(int matchId, int userId)
        {
            var match = await _context.Matches
                .Include(x => x.ChatMessages)
                .Include(x => x.UserMatches)
                .ThenInclude(um => um.User)
                .Where(x => x.MatchId == matchId)
                .SingleOrDefaultAsync();
            
            return match.ToChatMessageModels(userId);
        }

        public async Task MarkMessagesAsRead(int matchId, int userId)
        {
            var match = await _context.Matches.Include(x => x.ChatMessages).Where(x => x.MatchId == matchId)
                .SingleOrDefaultAsync();

            var chatMessages = match.ChatMessages.Where(x => x.ReceiverId == userId).Select(x =>
            {
                x.IsRead = true;
                return x;
            }).ToList();

            _context.ChatMessages.UpdateRange(chatMessages);
            await _context.SaveChangesAsync();
        }
    }
}
