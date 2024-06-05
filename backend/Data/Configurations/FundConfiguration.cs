using FundDataApi.Entities.Domain;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Data.Configurations;

internal class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("Fund");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.Symbol)
               .HasMaxLength(4)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(255)
               .IsRequired();

        builder.HasMany(x => x.Stocks)
               .WithOne(x => x.Fund)
               .HasForeignKey("FundId")
               .IsRequired(); // We will not track stocks without funds
    }
}