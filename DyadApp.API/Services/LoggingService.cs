using DyadApp.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Services
{
	public class LoggingService
	{
        private readonly DyadAppContext _context;

        public LoggingService(DyadAppContext context)
        {
            _context = context;
        }

       /* public async Task<string> SaveLog(string LogDesc, string type)
        {
            Log log = new Log();
            log.LogDesc = LogDesc;
            log.Type = type;
            log.TimeStamp = DateTime.Now;

            _context.Logging.Add(log);

            return;
        }*/

    }
}
