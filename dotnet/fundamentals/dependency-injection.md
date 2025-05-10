# Dependency Injection in .NET

## Table of Contents
1. [Overview](#overview)
2. [Service Lifetimes](#service-lifetimes)
3. [Implementation Examples](#implementation-examples)
4. [Best Practices](#best-practices)
5. [Common Pitfalls](#common-pitfalls)
6. [Additional Resources](#additional-resources)

## Overview

### What is Dependency Injection?
Dependency Injection (DI) is a design pattern that implements Inversion of Control (IoC) between classes and their dependencies. It helps create loosely coupled, maintainable, and testable applications.

### What is a Dependency?
A dependency is an object that another object depends on. For example:

```csharp
public class MyDependency
{
    public void WriteMessage(string message)
    {
        Console.WriteLine($"MyDependency.WriteMessage called. Message: {message}");
    }
}
```

### Problems with Direct Dependencies
1. **Tight Coupling**: To replace `MyDependency` with a different implementation, the dependent class must be modified
2. **Scattered Configuration**: If `MyDependency` has its own dependencies, configuration code becomes scattered
3. **Testing Difficulties**: Direct dependencies make unit testing challenging

### How DI Solves These Problems
1. **Abstraction**: Using interfaces or base classes to abstract dependency implementation
2. **Service Container**: Registration of dependencies in a service container (IServiceProvider)
3. **Constructor Injection**: Framework handles dependency creation and disposal

## Service Lifetimes

### 1. Transient
- Created each time they're requested
- Best for lightweight, stateless services
- Example: `services.AddTransient<IService, Service>();`

### 2. Scoped
- Created once per client request
- Shared within a single request
- Example: `services.AddScoped<IService, Service>();`

### 3. Singleton
- Created once and shared throughout the application lifetime
- Best for services that maintain state
- Example: `services.AddSingleton<IService, Service>();`

## Implementation Examples

### Basic DI Implementation
```csharp
// Interface
public interface IMyDependency
{
    void WriteMessage(string message);
}

// Implementation
public class MyDependency : IMyDependency
{
    public void WriteMessage(string message)
    {
        Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
    }
}

// Registration
builder.Services.AddScoped<IMyDependency, MyDependency>();
```

### Multiple Service Lifetimes Example
```csharp
public interface IOperation
{
    string OperationId { get; }
}

public interface IOperationTransient : IOperation { }
public interface IOperationScoped : IOperation { }
public interface IOperationSingleton : IOperation { }

// Registration
builder.Services.AddTransient<IOperationTransient, Operation>();
builder.Services.AddScoped<IOperationScoped, Operation>();
builder.Services.AddSingleton<IOperationSingleton, Operation>();
```

## Best Practices

1. **Use Constructor Injection**
   - Prefer constructor injection over property injection
   - Makes dependencies explicit and required

2. **Register Services in Program.cs**
   - Keep service registration centralized
   - Makes it easier to manage dependencies

3. **Use Appropriate Lifetime**
   - Choose the shortest lifetime that works
   - Prefer transient for stateless services

4. **Avoid Service Locator Pattern**
   - Don't use `GetService()` when DI is available
   - Makes dependencies explicit

5. **Dispose of Services Properly**
   - The container handles disposal of services
   - Implement IDisposable for cleanup

## Common Pitfalls

1. **Capturing Scoped Services in Singletons**
   - Can lead to incorrect behavior
   - Use scope validation to catch these issues

2. **Calling BuildServiceProvider in ConfigureServices**
   - Creates additional container copies
   - Can lead to torn singletons

3. **Static Access to HttpContext**
   - Use IHttpContextAccessor instead
   - Makes testing easier

4. **Overusing Singleton**
   - Can lead to memory leaks
   - Use appropriate lifetime

## Additional Resources

1. [Official ASP.NET Core DI Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
2. [Dependency Injection Guidelines](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
3. [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

## Open Questions

1. How to handle circular dependencies?
2. Best practices for testing DI containers?
3. When to use custom DI containers?
4. How to handle configuration in DI?
5. Best practices for DI in microservices?

## Recommendations

1. Start with constructor injection
2. Use interfaces for dependencies
3. Keep services focused and small
4. Use appropriate lifetime scopes
5. Write unit tests for your services
6. Monitor service lifetime and disposal
7. Use logging to track service creation and disposal 