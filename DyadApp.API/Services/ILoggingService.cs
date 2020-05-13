using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public interface ILoggingService
	{
		void SaveLog(string LogDesc, string type);
	}
}
