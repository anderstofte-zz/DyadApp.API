using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class MatchViewModel
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string ProfileImage { get; set; }
		public string LastReceivedMessage { get; set; }
		public DateTime MessageReceivedTime { get; set; }
		
	}
}
