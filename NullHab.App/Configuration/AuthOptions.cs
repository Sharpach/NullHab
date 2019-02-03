using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NullHab.App.Configuration
{
    public class AuthOptions
    {
        private readonly IConfiguration _configuration;

        private string Key => _configuration["JwtKey"]; // ключ для шифрации

        public string Issuer => _configuration["JwtIssuer"]; // издатель токена

        public string Audience => _configuration["JwtAudience"]; // потребитель токена

        public int Lifetime => int.Parse(_configuration["JwtLifetime"]); // время жизни токена

        public AuthOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
