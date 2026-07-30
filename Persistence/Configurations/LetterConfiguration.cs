using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models;
/*
dotnet ef migrations add Test --project Persistence --startup-project Server
dotnet ef database update --project Persistence --startup-project Server
 */

namespace Persistence.Configurations
{
    public class LetterConfiguration : IEntityTypeConfiguration<Letter>
    {
        public void Configure(EntityTypeBuilder<Letter> builder)
        {
            builder
                .HasMany(x => x.ChildrenLetters)
                .WithOne(x => x.ParentLetter)
                .HasForeignKey(x => x.ParentLetterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}