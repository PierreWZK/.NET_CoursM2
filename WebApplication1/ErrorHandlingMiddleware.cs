using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace WebApplication1.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IStringLocalizer<ErrorHandlingMiddleware> _localizer;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IStringLocalizer<ErrorHandlingMiddleware> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                string detail;
                string title = _localizer["InternalServerError"];

                if (ex is AppException appEx)
                {
                    detail = _localizer[appEx.Code] ?? appEx.Message;
                    title = _localizer[appEx.Code] ?? title;
                }
                else
                {
                    detail = _localizer["InternalServerError"];
                }

                var problem = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = title,
                    status = context.Response.StatusCode, 
                    detail = detail,
                    instance = context.Request.Path
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
