using System.Threading.Tasks;
using MimeKit;

namespace DyadApp.Emails
{
    public interface IEmailHandler
    {
        Task Send(MimeMessage email);
    }
}
