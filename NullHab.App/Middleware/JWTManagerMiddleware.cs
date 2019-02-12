using Microsoft.AspNetCore.Http;
using NullHab.AuthCore.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace NullHab.App.Middleware
{
    public class JWTManagerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        public JWTManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveAsync())
            {
                await next(context);

                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
