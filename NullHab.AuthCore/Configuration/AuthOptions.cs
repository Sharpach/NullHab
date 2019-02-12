using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NullHab.AuthCore.Configuration
{
    public class AuthOptions
    {
        private readonly IConfiguration _configuration;

        private string Key => _configuration.GetSection("JsonWebToken")["JwtKey"]; // ключ для шифрации

        public string Issuer => _configuration.GetSection("JsonWebToken")["JwtIssuer"]; // издатель токена

        public string Audience => _configuration.GetSection("JsonWebToken")["JwtAudience"]; // потребитель токена

        public int Lifetime => int.Parse(_configuration.GetSection("JsonWebToken")["JwtLifetime"]); // время жизни токена

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
