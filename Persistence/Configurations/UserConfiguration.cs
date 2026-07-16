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
                .WithOne(l => l.Recipient)
                .HasForeignKey(l => l.RecipientId);
            builder
                //У каждого юзера есть много черновиков
                .HasMany(x => x.Drafts)
                //У каждого черновика есть один юзер
                .WithOne(x => x.Author)
                //+ у него есть id
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}