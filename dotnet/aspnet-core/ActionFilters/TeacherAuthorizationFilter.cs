using System.Security.Claims;
using aspnet_core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnet_core.ActionFilters
{
    public class TeacherAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || role != "Teacher")
            {
                context.Result = new JsonResult(ApiResponse<string>.Error("Unauthorized: Teacher role required"))
                {
                    StatusCode = 403
                };
                return;
            }

            // Optional: Fetch additional user info (e.g., company) from DB
            // var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            // if (user == null)
            // {
            //     context.Result = new JsonResult(ApiResponse<string>.Error("User not found"))
            //     {
            //         StatusCode = 403
            //     };
            //     return;
            // }

            // Store company in HttpContext for use in controllers
            // context.HttpContext.Items["UserCompany"] = user.Company;
        }
    }
}