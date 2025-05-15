public class ValidateModelFilter : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        Console.WriteLine($"{context.HttpContext.Features}");

        next(context);

        return ValueTask.FromResult<int>(1);
    }
}