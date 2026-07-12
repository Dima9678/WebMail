using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class Extensions
    {
        public static void AddDatabase(this IServiceCollection serviceColection)
        {
            serviceColection.AddDbContext<DatabaseContext>(o =>
            {
                o.UseNpgsql("Host=localhost;Username=Mail;Password=11111111;Database=WebMail");
            });
        }
    }
}
