using FundDataApi.Entities.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundDataApi.Data.Configurations;

public class HistoricalDataPointConfiguration : IEntityTypeConfiguration<HistoricalDataPoint>
{
    public void Configure(EntityTypeBuilder<HistoricalDataPoint> builder)
    {
        builder.ToTable("HistoricalDataPoint");

        builder.HasKey("Date", "StockId");

        builder.Property(x => x.Open)
               .IsRequired();

        builder.Property(x => x.High)
               .IsRequired();

        builder.Property(x => x.Low)
               .IsRequired();

        builder.Property(x => x.Close)
               .IsRequired();

        builder.Property(x => x.AdjClose)
               .IsRequired();

        builder.Property(x => x.Volume)
               .IsRequired();

        builder.Property(x => x.Date)
               .HasColumnType("date")
               .IsRequired();
    }
}
