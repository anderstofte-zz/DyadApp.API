using System.Threading.Tasks;
using DyadApp.Emails;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Services
{
    public interface IEmailService
    {
        Task<ActionResult> SendEmail<T>(T model, EmailTypeEnum emailType);
    }
}
