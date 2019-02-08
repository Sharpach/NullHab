using Microsoft.AspNetCore.Identity;
using NullHab.DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NullHab.AuthCore.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}