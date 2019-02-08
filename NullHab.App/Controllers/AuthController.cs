using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using NullHab.App.Dto;
using NullHab.AuthCore.Services;
using NullHab.AuthCore.ViewModels;
using System;
using System.Threading.Tasks;
using NullHab.App.Configuration;

namespace NullHab.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var user = await _authService.Register(model.Email, model.UserName, model.Password);
            var result = TinyMapper.Map<UserDto>(user);
            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new JsonResult(null);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            return new JsonResult(null);
        }

        [HttpGet]
        public async Task<IActionResult> CancelToken()
        {
            return Ok();
        }
    }
}