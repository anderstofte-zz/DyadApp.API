using System;

namespace DyadApp.API.Models
{
	public class AuditLog
	{
		public int AuditLogId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }

    public enum AuditActionEnum
    {
        Create,
		Read,
		Update,
		Delete,
        Login,
        Activate,
        Deactivate,
        Signup,
        TokenRefresh,
        Match
    }
}
