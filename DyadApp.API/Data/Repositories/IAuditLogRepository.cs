using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IAuditLogRepository
    {
        Task Create(AuditLog auditLog);
    }
}