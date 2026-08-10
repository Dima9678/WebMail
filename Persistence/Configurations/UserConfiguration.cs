using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;
/*
dotnet ef migrations add Test --project Persistence --startup-project Server
dotnet ef database update --project Persistence --startup-project Server
 */

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //У каждого юзера есть ID
            builder.HasKey(x => x.Id);

            builder
                .HasMany(u => u.SentLetters)         
                .WithOne(l => l.Addressee)
                .HasForeignKey(l => l.AddresseeId);

            builder
                .HasMany(u => u.AcceptLetters)
                .WithMany(l => l.Recipients);

            builder
                .HasMany(x => x.Drafts)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}