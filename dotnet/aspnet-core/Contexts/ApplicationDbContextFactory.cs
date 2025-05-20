using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=MyApiDatabase;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}