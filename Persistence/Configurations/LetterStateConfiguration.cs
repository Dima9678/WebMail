using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Models;

namespace Persistence.Configurations
{
    public class LetterStateConfiguration : IEntityTypeConfiguration<LetterState>
    {
        public void Configure(EntityTypeBuilder<LetterState> builder)
        {
            //У каждого юзера есть ID
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Letter)
                .WithMany(x => x.LetterStates)
                .HasForeignKey(x => x.LetterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.LetterStates)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
