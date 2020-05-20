using DyadApp.API.Data;
using DyadApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public class MatchService : IMatchService
	{
		private readonly DyadAppContext _context;

		public MatchService(DyadAppContext context)
		{
			_context = context;
		}

		public async Task<bool> AddToAwaitingMatch(int userId)
		{
			var user = _context.AwaitingMatches.Where(aw => aw.UserId == userId || aw.IsMatched == false).FirstOrDefault();
			if (user == null)
			{
				var awaitingMatch = new AwaitingMatch { UserId = userId };
				_context.AwaitingMatches.Add(awaitingMatch);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

        public bool SearchForMatch(int userId)
		{
			User primeUser = new User();
			User matchUser = new User();
			List<AwaitingMatch> UserIDList = new List<AwaitingMatch>();
			primeUser = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

			UserIDList = _context.AwaitingMatches.Where(a => a.IsMatched == false && a.UserId != userId).ToList();

			foreach (var item in UserIDList)
			{
				User user = new User();
				user = _context.Users.Where(u => u.UserId == item.UserId).FirstOrDefault();
				if(primeUser.DateOfBirth.Year == user.DateOfBirth.Year)
				{
					matchUser = user;
					AwaitingMatch primeAW = new AwaitingMatch();
					AwaitingMatch secondaryAW = new AwaitingMatch();
					Match match = new Match();

					primeAW = _context.AwaitingMatches.Where(aw => aw.UserId == primeUser.UserId && aw.IsMatched == false ).FirstOrDefault();
					secondaryAW = _context.AwaitingMatches.Where(aws => aws.UserId == matchUser.UserId && aws.IsMatched == false).FirstOrDefault();
					primeAW.IsMatched = true;
					secondaryAW.IsMatched = true;
					_context.AwaitingMatches.Update(primeAW);
					_context.AwaitingMatches.Update(secondaryAW);
					_context.SaveChanges();


					match.Created = DateTime.Now;
					match.Modified = DateTime.Now;
					match.PrimaryUserID = primeAW.UserId;
					match.SecondaryUserID = secondaryAW.UserId;

					_context.Matches.Add(match);
					_context.SaveChanges();

					return true;
				}
			}

			return false;
		}

		public List<Match> RetreiveMatchList(int userId)
		{
			return _context.Matches.Where(m => m.PrimaryUserID == userId || m.SecondaryUserID == userId).ToList();
		}

	}
}
