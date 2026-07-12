using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();

            options.UseNpgsql(
                "Host=localhost;Username=Mail;Password=11111111;Database=WebMail");

            return new DatabaseContext(options.Options);
        }
    }
}
