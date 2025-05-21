namespace aspnet_core.Middleware
{
    public class MaintenanceMiddleware
    {
        private RequestDelegate _next { get; set; }

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = 503;
            await context.Response.WriteAsync("Service is under maintenance");
        }
    }

    public static class MaintenanceMiddlewareExtensions
    {
        public static IApplicationBuilder UseMaintenanceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}