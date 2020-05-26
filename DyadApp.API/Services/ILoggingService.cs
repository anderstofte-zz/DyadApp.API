using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Services
{
	public interface ILoggingService
	{
		Task SaveAuditLog(string description, AuditActionEnum action);
	}
}
