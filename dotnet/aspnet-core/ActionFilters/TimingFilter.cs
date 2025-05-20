using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnet_core.ActionFilters
{
    public class TimingFilter : IActionFilter
    {
        private readonly Stopwatch _stopwatch = new();

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Action took {elapsed}ms");
        }
    }
}