using Microsoft.AspNetCore.Identity;
using NullHab.AuthCore.Configuration;
using NullHab.AuthCore.Contracts;
using NullHab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NullHab.AuthCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthOptions _jwtOptions;
        private readonly ITokenManager _tokenManager;

        public AuthService(UserManager<User> userManager, AuthOptions jwtOptions, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _tokenManager = tokenManager;
        }

        public async Task<User> Register(string email, string userName, string password)
        {
            var user = new User
            {
                Email = email,
                UserName = userName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault()?.Description);
            }

            return user;
        }

        public async Task<string> Login(string email, string password)
        {
            var identity = await GetIdentity(email, password);

            var encodedJwt = await _tokenManager.GenerateEncodedJwt(identity.Claims);

            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("Password is incorrect");
            }

            var claims = new List<Claim>
            {
                new Claim("issuer", _jwtOptions.Issuer),
                new Claim("user", user.Email)
            };

            var claimsIdentity = new ClaimsIdentity
            (
                claims,
                "Token",
                ClaimsIdentity.DefaultIssuer,
                ClaimsIdentity.DefaultNameClaimType
            );

            return claimsIdentity;
        }
    }
}