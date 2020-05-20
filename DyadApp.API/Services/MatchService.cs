using DyadApp.API.Data;
using DyadApp.API.Models;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

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

            var awaitingMatch = new AwaitingMatch {UserId = userId};
            _context.AwaitingMatches.Add(awaitingMatch);
			await _context.SaveChangesAsync();

			return true;
		}

        private async Task<bool> UserIsAlreadyAwaitingAMatch(int userId)
        {
            return await _context.AwaitingMatches.AnyAsync(x => x.UserId == userId);
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
            var match = new Match
            {
				PrimaryUserId = userToMatch.UserId,
				SecondaryUserId = awaitingMatch.UserId,
            };

            _context.AwaitingMatches.Update(awaitingMatch);
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return true;


            //foreach (var item in awaitingMatches)
			//{
			//	User user = new User();
			//	user = _context.Users.Where(u => u.UserId == item.UserId).FirstOrDefault();
			//	if(primeUser.DateOfBirth.Year == user.DateOfBirth.Year)
			//	{
			//		matchUser = user;
			//		AwaitingMatch primeAW = new AwaitingMatch();
			//		AwaitingMatch secondaryAW = new AwaitingMatch();
			//		Match match = new Match();

			//		primeAW = _context.AwaitingMatches.Where(aw => aw.UserId == primeUser.UserId && aw.IsMatched == false ).FirstOrDefault();
			//		secondaryAW = _context.AwaitingMatches.Where(aws => aws.UserId == matchUser.UserId && aws.IsMatched == false).FirstOrDefault();
			//		primeAW.IsMatched = true;
			//		secondaryAW.IsMatched = true;
			//		_context.AwaitingMatches.Update(primeAW);
			//		_context.AwaitingMatches.Update(secondaryAW);
			//		_context.SaveChanges();


			//		match.MatchedDate = DateTime.Now;
			//		match.PrimaryUserID = primeAW.UserId;
			//		match.SecondaryUserID = secondaryAW.UserId;

			//		_context.Matches.Add(match);
			//		_context.SaveChanges();

			//		return true;
			//	}
			//}

			//return false;
		}


	}
}
