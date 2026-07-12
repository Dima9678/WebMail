using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;
using Domain.Models;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //У каждого юзера есть ID
            builder.HasKey(x => x.Id);

            builder
                //У каждого юзера есть принятые письма
                .HasMany(x => x.AcceptLetters)
                //В свою очередь у которых есть адресат
                .WithOne(x => x.Addressee)
                //+ у него есть id
                .HasForeignKey(x => x.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                //У каждого юзера есть отправленные письма
                .HasMany(x => x.SentLetters)
                //В свою очередь у которых есть получатель
                .WithOne(x => x.Recipient)
                //+ у него есть id
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

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
