using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models;

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

            builder
                .HasOne(l => l.OriginalAuthor)
                .WithMany()
                .HasForeignKey(l => l.OriginalAuthorId)
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .Property(x => x.Forwarded)
                .HasDefaultValue(false);
        }
    }
}