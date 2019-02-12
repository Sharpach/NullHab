using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NullHab.AuthCore.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NullHab.AuthCore.Contracts;

namespace NullHab.AuthCore.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthOptions _jwtOptions;

        public TokenManager(IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            AuthOptions jwtOptions
        )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveAsync() => await IsActiveAsync(GetCurrent());

        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrent());

        public async Task<bool> IsActiveAsync(string token) => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
        {
            await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtOptions.Lifetime)
                });
        }

        public Task<string> GenerateEncodedJwt(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(_jwtOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(encodedJwt);
        }

        public async Task<string> RefreshToken()
        {
            var encodedClaims = await GetClaimsFromJwt();

            var encodedJwt = await GenerateEncodedJwt(encodedClaims);

            if (await IsCurrentActiveAsync())
            {
                await DeactivateCurrentAsync();
            }

            return encodedJwt;
        }

        private Task<List<Claim>> GetClaimsFromJwt(string token = null)
        {
            var jwt = token ?? GetCurrent();

            return Task.FromResult(new JwtSecurityTokenHandler()
                .ReadJwtToken(jwt).Claims.ToList());
        }

        private string GetCurrent()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
