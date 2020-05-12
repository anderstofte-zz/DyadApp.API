using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class AwaitingMatch
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public DateTime Date { get; set; }
		public bool IsMatched { get; set; }
	}
}
