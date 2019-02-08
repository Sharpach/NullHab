using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new JsonResult(null);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return Ok();
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