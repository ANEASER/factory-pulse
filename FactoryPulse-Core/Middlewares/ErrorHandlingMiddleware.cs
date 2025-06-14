using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FactoryPulse_Core.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred for {Method} {Path}", 
                    context.Request.Method, context.Request.Path);
                
                if (context.Response.HasStarted)
                {
                    throw; 
                }
                
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new { error = "Internal server error.", details = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
            finally
            {
                Console.WriteLine($"=== ERROR MIDDLEWARE END - Status: {context.Response.StatusCode} ===");
            }
        }
    }
}