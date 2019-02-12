using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using NullHab.App.Dto;
using NullHab.AuthCore.Contracts;
using NullHab.AuthCore.ViewModels;
using System;
using System.Threading.Tasks;

namespace NullHab.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenManager _tokenManager;

        public AuthController(IAuthService authService, ITokenManager tokenManager)
        {
            _authService = authService;
            _tokenManager = tokenManager;
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

            var token = await _authService.Login(model.Email, model.Password);
            return new JsonResult(token);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string token)
        {
            var encodedJwt = await _tokenManager.RefreshToken();

            return new JsonResult(encodedJwt);
        }

        [HttpGet]
        public async Task<IActionResult> CancelToken()
        {
            if (await _tokenManager.IsCurrentActiveAsync())
            {
                await _tokenManager.DeactivateCurrentAsync();
            }

            return Ok();
        }
    }
}