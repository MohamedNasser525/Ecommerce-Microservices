using Newtonsoft.Json;
using System.Net;

namespace AuthServer.Helper
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleCustomExceptionAsync(context, ex);
            }
        }

        private Task HandleCustomExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.UnavailableForLegalReasons; // 451

            var result = JsonConvert.SerializeObject(new { error = "Something went wrong", details = exception.Message });
            return context.Response.WriteAsync(result);
        }
    }

}
