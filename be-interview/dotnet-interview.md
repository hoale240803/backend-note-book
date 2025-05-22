# 1. Job Description
Play in a dynamic, collaborative, transparent, non-hierarchical, and ego-free culture where your talent is valued over a role title.
Work in collaborative teams and build quality code.
Help the team champion software quality, engender technical vision, and ensure clients are satisfied.
Be problem solvers, think through complex problems and work with amazing people to make the solutions reality.
Practice agile software development and be a great team player.
Learn something new every day, and work on your great innovative idea with a team to apply to the project.

**Requirements:**

Must have:

More than 05 years of experience in .NET development with expertise in developing large-scale enterprise applications and solutions.

Experienced the best practices of frontend technologies using HTML5, CSS3, 

JavaScript and Angular or TypeScript

OOP C# .NET best practices, and common design principles (SOLID, DRY)

ASP.NET Core MVC, WebAPI, RESTFul Web API, HTTP Codes and verbs.

Entity Framework Core, Unit Test and Integration Test

Domain Driven Design and Clean Architecture, SoA and Micro Services.

Container-based using Docker and Kubernetes, RabbitMQ/ AWS SQS/SNS.

Understand and follow DevOps CI/CD.

Proficiency in GIT using GIT flow.

Quality-focused professional with experience in Unit Testing and Integration 

Testing

Experience working with AWS, Azure,...

**Soft-skills:**

Effective verbal English skills in video conferences and face-to-face communication.

Be a problem solver with great problem-solving skills.

Proactive and effective self-learning skills and research.

# 2. Bloom approach

Remembering > Understanding > Applying > Analytizing > Evaluating > Creating

# 3. Apply to .NET knowledge

## 3.1 .NET core concepts, ASP.NET core MVC, WebAPI, and RESTful services.
### Review Dependency injection in .NET core

1. What is Dependency Injection (DI)?
A design pattern that enables loose coupling.
Supports Inversion of Control (IoC) via external dependency provisioning.
Built-in in ASP.NET Core via the Service Container.

2. Why Use It?
Easier testing (mocking)
Better modularity and maintenance
Promotes SOLID principles (especially D & I)

3. Service Lifetimes

| Lifetime  | Description                         | Use Case                   |
| --------- | ----------------------------------- | -------------------------- |
| Transient | New instance every time             | Stateless services         |
| Scoped    | Same instance per HTTP request      | DB context, business logic |
| Singleton | One instance for the app's lifetime | Logging, caching           |

4. How to Register a Service

```
services.AddTransient<IMyService, MyService>();
services.AddScoped<IMyService, MyService>();
services.AddSingleton<IMyService, MyService>();
```

5. Injecting Dependencies
```
public class HomeController : Controller
{
    private readonly IMyService _service;
    public HomeController(IMyService service)
    {
        _service = service;
    }
}
```
Middleware Injection 
```
public class MyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMyService _service;
    public MyMiddleware(RequestDelegate next, IMyService service)
    {
        _next = next;
        _service = service;
    }
}
```
[More DI](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-9.0)

### Study middleware pipeline in ASP.NET Core
1. What is Middleware?
Definition: Middleware is software assembled into an app pipeline to handle requests and responses. Each component:

- Chooses whether to pass the request to the next component in the pipeline.
- Can perform work before and after the next component in the pipeline.
- Can short-circuit the pipeline to prevent further processin

2. Why is middlware important?
Purpose: Middleware components are used to:

Handle cross-cutting concerns like authentication, logging, error handling, etc.

Build a request-processing pipeline where each component can inspect, modify, or terminate requests and responses.

3. Implement middleware in ASP.NET core
Using built-in Middleware
```
public void Configure(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```
**The order of middleware is crucial; for instance, UseRouting must come before UseEndpoints**

Creating custom middleware
```
public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Pre-processing logic here

        await _next(context);

        // Post-processing logic here
    }
}

// Extension method to add the middleware
public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomMiddleware>();
    }
}
```

4. Analyzing: middleware execution flow

**Request Flow**: When an HTTP request is received, it passes through each middleware component in the order they are added. Each middleware can:

Process the request and pass it to the next middleware.

Short-circuit the pipeline and generate a response immediately.

**Response Flow**: After the response is generated, it travels back through the middleware pipeline in reverse order, allowing each middleware to process the response.

#### Read about RESTful API principles ( HTTP verbs, status coes)
🔹 Best Practices for RESTful API Design
Use Nouns for Endpoints: Endpoints should represent resources (e.g., /users, /orders).

Plural Naming: Use plural nouns for consistency (e.g., /products instead of /product).

HTTP Methods for Actions: Utilize HTTP verbs to define actions (e.g., GET /users, POST /users).

Statelessness: Each request should contain all the information needed to process it.

Versioning: Implement API versioning to manage changes (e.g., /api/v1/users).

Error Handling: Provide meaningful error messages and use appropriate status codes.

Pagination: For large datasets, implement pagination to manage responses.

Filtering and Sorting: Allow clients to filter and sort data through query parameters.

##### Create a ProductController to accept a prduct JSON.
##### Add a POST endpoint to ProductController
1. What is Action Filter?
In ASP.NET Core, Action Filters are components that run before and after the execution of an action method. They are part of the broader filter pipeline, which includes various filter types that execute at different stages of the request processing pipeline.

2. Types of Filters in ASP.NET Core

Auhtorization Filters: Run fist to determin if the user is authorized for the request.

Resource Filters: Run after authorization and before model biding; useful for caching and resource initialization.

Action Filters: Run before and after the action methods executes; ideal for logging, validation, and modifying action parameters or results.

Exception Filters: Handle exceptions thrown by action methods or other filters.

Result Filters: Run before and after the execution of action results; useful for modifying the response.

3. Implementing a Custom Action Filter

To create a custom action filter, you can inherit from ActionFilterAttribute class and override it's methods

```
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

public class LogActionFilter : ActionFilterAttribute
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Executing action: {ActionName}", context.ActionDescriptor.DisplayName);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Executed action: {ActionName}", context.ActionDescriptor.DisplayName);
    }
}
```
4. Applying the Action Filter

At the Action Level:
```
[LogActionFilter]
public IActionResult MyAction()
{
    // Action logic here
}
```

At the Controller Level
```
[LogActionFilter]
public class MyController : Controller
{
    // All actions in this controller will use the filter
}
```

Globally in Startup.cs
```
services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogActionFilter>();
});

```
Applying filters globally ensures that all controllers and actions use the filter, promoting consistency across the application

5. Best Practices
Use Filters for Cross-Cutting Concerns: Ideal for loggin, error handling, and authorization.

Avoid Business Logic in Filters: keep business logic winthin action methods or services

Leverage Dependency Injection: Filters can receive services via constructor injection, promoting testability and modularity.

Be mindfull of Execution Order: Filters execute in a specific order; understanding this helps in designing predictable behavior.


[Compare Filters vs Middleware](https://codewithmukesh.com/blog/filters-in-aspnet-core/#filters-vs-middleware-when-to-choose-what)



### Study action filters in asp.net core

## 3.2 Entity Framework and Testing
What is this?
is an object-relational mapper that enables . NET developers to work with relational data using domain-specific objects.

2. Why use repository and Unit of work pattern?
2.1 Repository pattern acts as an abstraction layer between the business logic (domain) and the data access layer (e.g EF). It encapsulates data access logic, providing a collection-like interface for querying and manipulating entities
Why use it?

- Separation of concerns: isolates data access logic from business logic, aligning with Clean Architecutre and DDD. 
- Testablility: Enables unit testing by allowing you to mock the repository instead of the DbContext. This simplifies testing business logic without hitting the database.

- Maintanability: Centrolizes data access logic, making it easier to modify queries or switch database (e.g. from in-memory to SQL Server)

2.2 Unit of work pattern
The unit of work pattern coordinates multiple repository operations, ensuring the've commited as a single transaction. It typeically wraps DbContext to manage changes and database commits.

Why use it?
Transactional consistency: Ensurees all database operations (e.g creating a course and logging an audit entry) succeed or fail together. This prevents partial updates if an error occurs during entities creation.

Reduced Database calls: manages a single DbContext instance across repositories minimizing resource usage. For example, one SaveChanges call commits all changes (courses, audit logs) in a request.

## 3.3 Unit Test vs Integration Test
1. Unit Test: 
- Testing individual componets(controller, services) in isolation.
- Hight test coverage of business logic (e.g edcases, error handling)
2. Integration test:
- Verifying component interactions (e.g controller to EF core)
- Ensuring end-to-end functionality (e.g., API endpoints work as expected)
- Validating database queries, as in your course app's data access

## 3.3 Design Patterns and Architecture
## 3.4 Devops, Containers, and Cloud
## 3.5 ReactJs fresher
## 3.6 Mock Interviews and soft skills
## 3.7 Final Review and Application Polish
