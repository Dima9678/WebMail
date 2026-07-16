using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain;
using Persistence.Configurations;

namespace Persistence
{

    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Letter> Letters { get; set; }
        public DbSet<LetterState> LetterStates { get; set; }
        public DbSet<Draft> Drafts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LetterStateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
