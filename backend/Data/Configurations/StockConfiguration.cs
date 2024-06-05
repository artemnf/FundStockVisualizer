using FundDataApi.Entities.Domain;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Data.Configurations;

internal class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.Ticker)
               .HasMaxLength(10)
               .IsRequired();

        builder.HasMany(x => x.HistoricalDataPoints)
               .WithOne(x => x.Stock)
               .HasForeignKey("StockId")
               .IsRequired();
    }
}
