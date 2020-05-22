using Microsoft.Extensions.Configuration;

namespace DyadApp.API.Services
{
    public class SecretKeyService: ISecretKeyService
    {
        private readonly IConfiguration _configuration;

        public SecretKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSecretKey()
        {
            return _configuration.GetSection("TokenKey").Value;
        }
    }
}