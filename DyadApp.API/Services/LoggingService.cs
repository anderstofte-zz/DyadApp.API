using System;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Http;

namespace DyadApp.API.Services
{
	public class LoggingService : ILoggingService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuditLogRepository _repository;
        public LoggingService(IHttpContextAccessor contextAccessor, IAuditLogRepository repository)
        {
            _contextAccessor = contextAccessor;
            _repository = repository;
        }

        public async Task SaveAuditLog(string description, AuditActionEnum action)
        {
            var userId = _contextAccessor.HttpContext.GetUserId();
            var auditLog = new AuditLog
            {
                Action = action,
                Description = description,
                IpAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Created = DateTime.Now,
                CreatedBy = userId
            };

            await _repository.Create(auditLog);
        }
    }
}
