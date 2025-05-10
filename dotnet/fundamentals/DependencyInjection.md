Transient:
Created: Every time it's requested (in this case, every time IndexModel is instantiated)
Disposed: At the end of the request when the IndexModel is disposed
In your code, it's created when the IndexModel constructor is called
Scoped:
Created: Once per HTTP request
Disposed: At the end of the HTTP request
In your code, it's created when the first service is requested within a request scope
Singleton:
Created: Once when the application starts
Disposed: When the application shuts down
In your code, it's created when the application starts up




Debug 
https://learn.microsoft.com/en-us/dotnet/core/diagnostics