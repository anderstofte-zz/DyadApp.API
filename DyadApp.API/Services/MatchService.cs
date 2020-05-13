using DyadApp.API.Data;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Http;
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

		public bool AddToAwaitingMatch(int UserID)
		{
			AwaitingMatch aw = new AwaitingMatch();
			aw.Date = DateTime.Now;
			aw.UserID = UserID;
			_context.AwaitingMatches.Add(aw);
			_context.SaveChanges();
			return true;
		}


		public bool SearchForMatch(int UserID)
		{
			User primeUser = new User();
			User matchUser = new User();
			List<AwaitingMatch> UserIDList = new List<AwaitingMatch>();
			primeUser = _context.Users.Where(u => u.UserId == UserID).FirstOrDefault();

			UserIDList = _context.AwaitingMatches.Where(a => a.IsMatched == false && a.UserID != UserID).ToList();

			foreach (var item in UserIDList)
			{
				User user = new User();
				user = _context.Users.Where(u => u.UserId == item.UserID).FirstOrDefault();
				if(primeUser.DateOfBirth.Year == user.DateOfBirth.Year)
				{
					matchUser = user;
					AwaitingMatch primeAW = new AwaitingMatch();
					AwaitingMatch secondaryAW = new AwaitingMatch();
					Match match = new Match();

					primeAW = _context.AwaitingMatches.Where(aw => aw.UserID == primeUser.UserId && aw.IsMatched == false ).FirstOrDefault();
					secondaryAW = _context.AwaitingMatches.Where(aws => aws.UserID == matchUser.UserId && aws.IsMatched == false).FirstOrDefault();
					primeAW.IsMatched = true;
					secondaryAW.IsMatched = true;
					_context.AwaitingMatches.Update(primeAW);
					_context.AwaitingMatches.Update(secondaryAW);
					_context.SaveChanges();


					match.MatchedDate = DateTime.Now;
					match.PrimaryUserID = primeAW.UserID;
					match.SecondaryUserID = secondaryAW.UserID;

					_context.Matches.Add(match);
					_context.SaveChanges();

					return true;
				}
			}

			return false;
		}


	}
}
