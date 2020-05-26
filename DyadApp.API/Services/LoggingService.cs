using DyadApp.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Services
{
	public class LoggingService : ILoggingService
	{
        private readonly DyadAppContext _context;

        public LoggingService(DyadAppContext context)
        {
            _context = context;
        }

        public void SaveLog(string LogDesc, string type)
        {
            Logs log = new Logs();
            log.LogDesc = LogDesc;
            log.Type = type;
            log.TimeStamp = DateTime.Now;

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

    }
}
