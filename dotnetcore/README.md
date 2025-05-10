# Todo Application

A simple ASP.NET Core MVC Todo application for managing your tasks.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or Visual Studio Code
- SQL Server (LocalDB or full version)

## Build Instructions

1. Clone the repository:
```bash
git clone <repository-url>
cd dotnetcore
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

## Run Instructions

1. Update the database (first time only):
```bash
dotnet ef database update
```

2. Run the application:
```bash
dotnet run
```

The application will start and be available at:
- https://localhost:7001
- http://localhost:5001

## Development Workflow

- To make changes to the database model:
  1. Update the model classes in the Models folder
  2. Create a new migration:
     ```bash
     dotnet ef migrations add <MigrationName>
     ```
  3. Apply the migration:
     ```bash
     dotnet ef database update
     ```

- To build in release mode:
  ```bash
  dotnet build -c Release
  ```

- To run in production mode:
  ```bash
  dotnet run -c Release
  ```

## Project Structure

- `Controllers/`: MVC Controllers
- `Models/`: Data models and ViewModels
- `Views/`: Razor views
- `Data/`: DbContext and Migrations
- `wwwroot/`: Static files (CSS, JS, images)

## Common Issues

1. If the database connection fails:
   - Check if SQL Server is running
   - Verify connection string in appsettings.json
   - Ensure database is created: `dotnet ef database update`

2. If build fails:
   - Clean solution: `dotnet clean`
   - Delete bin and obj folders
   - Restore packages: `dotnet restore`
   - Try building again 