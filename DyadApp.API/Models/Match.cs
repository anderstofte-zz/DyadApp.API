using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class Match
	{
		public int MatchId { get; set; }
		public int PrimaryUserID { get; set; }
		public int SecondaryUserID { get; set; }
		public DateTime Modified { get; set; }
		public int ModifiedBy { get; set; }
		public DateTime Created { get; set; }
		public int CreatedBy { get; set; }
	}
}
