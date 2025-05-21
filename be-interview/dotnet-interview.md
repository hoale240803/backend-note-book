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

Chooses whether to pass the request to the next component in the pipeline.

Can perform work before and after the next component in the pipeline.

Can short-circuit the pipeline to prevent further processin
#### Write a custom middleware to log reqeust paths in a new ASP.NET core WebAPI project
#### Read about RESTful APIi principles ( HTTP verbs, status coes)
##### Create a ProductController to accept a prduct JSON.
##### Add a POST endpoint to ProductController
### Study action filters in asp.net core

## 3.2 Entity Framework and Testing
## 3.3 Design Patterns and Architecture
## 3.4 Devops, Containers, and Cloud
## 3.5 ReactJs fresher
## 3.6 Mock Interviews and soft skills
## 3.7 Final Review and Application Polish
