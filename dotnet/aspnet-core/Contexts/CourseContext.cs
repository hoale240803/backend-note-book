using Microsoft.EntityFrameworkCore;

namespace aspnet_core.Contexts
{
    public class CourseContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }

    public class AuditLog
    {
        public object UserId { get; set; }
        public object Action { get; set; }
        public object Resource { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}