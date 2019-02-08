using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NullHab.App.Configuration
{
    internal class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var jsonResult = new JsonResult(new { error = context.Exception.Message })
            {
                StatusCode = (int)System.Net.HttpStatusCode.InternalServerError
            };

            context.Result = jsonResult;
        }
    }
}
