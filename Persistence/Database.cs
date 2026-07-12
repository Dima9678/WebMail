using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain;
using Persistence.Configurations;

namespace Persistence
{

    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Letter> AcceptLetters { get; set; }
        public DbSet<Letter> SentLetters { get; set; }
        public DbSet<Letter> Drafts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
