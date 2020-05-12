using DyadApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public interface IMatchService
	{
		
		bool AddToAwaitingMatch(int UserID);
		bool SearchForMatch(int UserID);
	}
}
