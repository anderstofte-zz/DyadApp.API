using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class Logs
	{
		public int ID { get; set; }
		public string LogDesc { get; set; }
		public string Type { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}
