namespace aspnet_core
{
    public class RequestLogginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogginMiddleware> _logger;

        public RequestLogginMiddleware(RequestDelegate next, ILogger<RequestLogginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Requested: {context.Request.Method} {context.Request.Path}");
            await _next(context);
            _logger.LogInformation($"Requested: {context.Response.StatusCode}");

        }
    }
}