using IGaming.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IGaming.Infrastructure.Configurations;

public class BetConfiguration : IEntityTypeConfiguration<Bet>
{
    public void Configure(EntityTypeBuilder<Bet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Details).IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.HasOne(b => b.User)
               .WithMany(u => u.Bets)
               .HasForeignKey(b => b.UserId);
    }
}