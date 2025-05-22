
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Parameterless constructor for design-time (EF Core CLI)
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ISet<AuditLog> AuditLogs { get; set; }
    public DbSet<Product> Products { get; set; }

}

public class AuditLog
{
    public object UserId { get; set; }
    public object Action { get; set; }
    public object Resource { get; set; }
    public string Details { get; set; }
    public DateTime Timestamp { get; set; }
}