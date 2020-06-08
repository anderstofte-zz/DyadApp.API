using System.Threading.Tasks;
using EmailData = DyadApp.Emails.Models.EmailData;

namespace DyadApp.API.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailData model);
    }
}
