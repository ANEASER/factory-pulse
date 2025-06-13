public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await _next(context);  
        }
        catch (Exception ex)
        {
            Console.WriteLine("erro middleware");
            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var response = new { error = "Internal server error." };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
