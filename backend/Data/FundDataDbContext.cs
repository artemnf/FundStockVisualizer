using FundDataApi.Data.Configurations;
using FundDataApi.Entities.Domain;
using Microsoft.EntityFrameworkCore;

namespace FundDataApi.Data;

public class FundDataDbContext(DbContextOptions<FundDataDbContext> options) : DbContext(options)
{
    public DbSet<Fund> Funds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FundConfiguration());
    }
}