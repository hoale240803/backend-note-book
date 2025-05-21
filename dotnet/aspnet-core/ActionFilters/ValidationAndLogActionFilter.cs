using System.Security.Claims;
using aspnet_core.Contexts;
using aspnet_core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnet_core.ActionFilters
{
    public class ValidateAndLogActionFilter : IActionFilter
    {
        public readonly CourseContext _context;

        public ValidateAndLogActionFilter(CourseContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(ApiResponse<string>.Error("Invalid input"));
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var action = context.ActionDescriptor.DisplayName ?? "Unknown";
            var resource = context.HttpContext.Request.Path;
            var input = context.ActionArguments.FirstOrDefault();
            var details = System.Text.Json.JsonSerializer.Serialize(input);

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                Resource = resource,
                Details = $"Input: {details}",
                Timestamp = DateTime.UtcNow
            };
            _context.AuditLogs.Add(auditLog);
            _context.SaveChanges(); // Synchronous for simplicity; use async queue in production
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result && result.Value is ApiResponse<object> apiResponse && apiResponse.Data is Course course)
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = context.ActionDescriptor.DisplayName ?? "Unknown",
                    Resource = context.HttpContext.Request.Path,
                    Details = $"Result: Course ID {course.Id} processed",
                    Timestamp = DateTime.UtcNow
                };
                _context.AuditLogs.Add(auditLog);
                _context.SaveChanges(); // Synchronous for simplicity
            }
        }
    }
}