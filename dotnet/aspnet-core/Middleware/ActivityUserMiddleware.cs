namespace aspnet_core.Middleware
{
    public class ActivityUserMiddleware
    {
        public RequestDelegate _next { get; set; }
        private readonly ILogger<ActivityUserMiddleware> _logger;
        public ActivityUserMiddleware(RequestDelegate next, ILogger<ActivityUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // _logger.LogInformation()
        }
    }
}