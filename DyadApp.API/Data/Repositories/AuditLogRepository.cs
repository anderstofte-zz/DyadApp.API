using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DyadAppContext _context;

        public AuditLogRepository(DyadAppContext context)
        {
            _context = context;
        }

        public async Task Create(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
