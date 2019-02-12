using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NullHab.AuthCore.Contracts
{
    public interface ITokenManager
    {
        Task<string> GenerateEncodedJwt(IEnumerable<Claim> claims);
        Task<string> RefreshToken();
        Task DeactivateAsync(string token);
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task<bool> IsCurrentActiveAsync();
    }
}