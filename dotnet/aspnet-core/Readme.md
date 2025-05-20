Source: https://codewithmukesh.com/blog/aspnet-core-api-with-jwt-authentication/

# Setting up the project

Setup Central Package Management (cpm)
Directory.Packages.props

```
<Project>
  <ItemGroup>
    <PackageVersion Include="Newtonsoft.Json" Version="13.0.1" />
    <PackageVersion Include="Serilog" Version="2.12.0" />
  </ItemGroup>
</Project>

```

```
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design
Install-Package System.IdentityModel.Tokens.Jwt
Install-Package Swashbuckle.AspNetCore
```

or on vscode
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add Swashbuckle.AspNetCore
```

# Setup Sqlserver through docker

```
docker pull mcr.microsoft.com/mssql/server:2019-latest
```

```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -e "MSSQL_PID=Express" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

5. Apply Migrations
Ensure the dotnet-ef tool is installed:
```
dotnet tool install --global dotnet-ef
```
Add the migration
```
dotnet ef migrations add InitialCreate
```

Apply the migration to create the database schema
```
dotnet ef database update
```


Nutget problems please clear cache
A corrupted NuGet cache can cause incorrect package versions to be restored. Clear the cache to ensure a clean restore:
```
dotnet nuget locals all --clear
```