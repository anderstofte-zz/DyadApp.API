using Microsoft.Extensions.Configuration;

namespace DyadApp.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetEncryptionKey(this IConfiguration configuration)
        {
            return configuration.GetSection("EncryptionKey").Value;
        }

        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection("TokenKey").Value;
        }
    }
}