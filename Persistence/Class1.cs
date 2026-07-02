using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Domain;

namespace Persistence
{

    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Letter> Letters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LetterConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class LetterConfigurations : IEntityTypeConfiguration<Letter>
    {
        public void Configure(EntityTypeBuilder<Letter> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Addressee)
                .WithMany(x => x.SentLetters)
                .HasForeignKey(x => x.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Recipient)
                .WithMany(x => x.AcceptLetters)
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
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
