using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public interface IChatService
	{
		void AddMessage(int SenderId, int ReceiverId, string Message);
	}
}
