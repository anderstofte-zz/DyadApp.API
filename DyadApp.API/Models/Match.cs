using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class Match
	{
		public int ID { get; set; }
		public int PrimaryUserID { get; set; }
		public int SecondaryUserID { get; set; }
		public DateTime MatchedDate { get; set; }
	}
}
