using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DyadApp.Emails
{
    public static class EmailConfiguration
    {
        public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
            services.AddTransient<IEmailHandler, EmailHandler>();
            services.AddTransient<SmtpClient>();
        }
    }
}
