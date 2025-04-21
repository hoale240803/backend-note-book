# ASP.NET Core Dependency Injection Best Practices and Anti-Patterns

## 1. Async/await and Task-based Service Resolution

### Anti-Pattern
```csharp
// DON'T DO THIS
public class BadAsyncService
{
    public async Task<BadAsyncService> CreateAsync()
    {
        await Task.Delay(1000); // Simulating async work
        return new BadAsyncService();
    }
}

// Registration
services.AddSingleton<BadAsyncService>(async sp => 
    await BadAsyncService.CreateAsync()); // Won't work!
```

**Problems:**
- C# doesn't support asynchronous constructors
- Service resolution must be synchronous
- Can cause deadlocks or unexpected behavior

### Best Practice
```csharp
// DO THIS INSTEAD
public class GoodAsyncService
{
    public GoodAsyncService() { } // Synchronous constructor

    public async Task InitializeAsync()
    {
        await Task.Delay(1000); // Async initialization
    }
}

// Usage
public class Consumer
{
    private readonly GoodAsyncService _service;
    
    public Consumer(GoodAsyncService service)
    {
        _service = service;
    }

    public async Task UseServiceAsync()
    {
        await _service.InitializeAsync();
        // Use the service
    }
}
```

## 2. Storing Data in Service Container

### Anti-Pattern
```csharp
// DON'T DO THIS
public class BadDataStorage
{
    private static Dictionary<string, UserData> _userData = new();
}

// Registration
services.AddSingleton(_userData); // Storing mutable data
```

**Problems:**
- Thread safety issues
- Memory leaks
- Hard to test
- Violates DI principles

### Best Practice
```csharp
// DO THIS INSTEAD
public class UserDataService
{
    private readonly ConcurrentDictionary<string, UserData> _userData = new();
    
    public UserData GetUserData(string userId)
    {
        return _userData.GetOrAdd(userId, _ => new UserData());
    }
}

// Configuration using Options Pattern
public class AppSettings
{
    public string ApiKey { get; set; }
}

// Registration
services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
```

## 3. Static Service Access

### Anti-Pattern
```csharp
// DON'T DO THIS
public static class BadStaticService
{
    public static IServiceProvider Services { get; set; }
    
    public static void DoSomething()
    {
        var service = Services.GetService<IMyService>();
        // Use service
    }
}
```

**Problems:**
- Hard to test
- Violates DI principles
- Can cause memory leaks
- Makes code harder to maintain

### Best Practice
```csharp
// DO THIS INSTEAD
public class GoodServiceConsumer
{
    private readonly IMyService _service;
    
    public GoodServiceConsumer(IMyService service)
    {
        _service = service;
    }
    
    public void DoSomething()
    {
        // Use _service
    }
}
```

## 4. Service Locator Pattern

### Anti-Pattern
```csharp
// DON'T DO THIS
public class BadServiceLocator
{
    private readonly IServiceProvider _services;
    
    public BadServiceLocator(IServiceProvider services)
    {
        _services = services;
    }
    
    public void DoSomething()
    {
        var service = _services.GetService<IMyService>();
        // Use service
    }
}
```

**Problems:**
- Hides dependencies
- Hard to test
- Violates DI principles
- Makes code harder to understand

### Best Practice
```csharp
// DO THIS INSTEAD
public class GoodDependencyInjection
{
    private readonly IMyService _service;
    
    public GoodDependencyInjection(IMyService service)
    {
        _service = service;
    }
    
    public void DoSomething()
    {
        // Use _service
    }
}
```

## 5. BuildServiceProvider in Configuration

### Anti-Pattern
```csharp
// DON'T DO THIS
services.AddSingleton<IService>(sp => {
    var provider = services.BuildServiceProvider();
    return new Service(provider.GetService<IOtherService>());
});
```

**Problems:**
- Creates multiple service containers
- Can cause memory leaks
- Violates DI principles
- Makes testing harder

### Best Practice
```csharp
// DO THIS INSTEAD
services.AddSingleton<IService>(sp => 
    new Service(sp.GetRequiredService<IOtherService>()));
```

## 6. Disposable Transient Services

### Anti-Pattern
```csharp
// DON'T DO THIS
services.AddTransient<IDisposableService, DisposableService>();
```

**Problems:**
- Memory leaks
- Resources not properly disposed
- Can cause system instability

### Best Practice
```csharp
// DO THIS INSTEAD
services.AddScoped<IDisposableService, DisposableService>();
```

## 7. Scope Validation

### Anti-Pattern
```csharp
// DON'T DO THIS
services.AddScoped<IScopedService, ScopedService>();
services.AddSingleton<ISingletonService>(sp => 
    new SingletonService(sp.GetService<IScopedService>())); // Capturing scoped in singleton
```

**Problems:**
- Memory leaks
- Inconsistent service lifetime
- Can cause unexpected behavior

### Best Practice
```csharp
// Enable scope validation
builder.Host.UseDefaultServiceProvider(options => 
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

// Proper service registration
services.AddScoped<IScopedService, ScopedService>();
services.AddSingleton<ISingletonService, SingletonService>();
```

## General Recommendations

1. **Always use constructor injection** when possible
2. **Keep services focused** and single-responsibility
3. **Use appropriate lifetimes** for services
4. **Enable scope validation** in development
5. **Avoid circular dependencies**
6. **Use interfaces** for better testability
7. **Consider using options pattern** for configuration
8. **Keep factories simple** and synchronous
9. **Properly dispose** of resources
10. **Use async/await correctly** in service methods

## Common Pitfalls to Avoid

1. **Capturing scoped services in singletons**
2. **Using service locator pattern**
3. **Storing mutable state in services**
4. **Creating circular dependencies**
5. **Not properly disposing of resources**
6. **Using static service access**
7. **Building service provider during configuration**
8. **Using async constructors**
9. **Not validating scopes in development**
10. **Not using appropriate lifetimes for services** 