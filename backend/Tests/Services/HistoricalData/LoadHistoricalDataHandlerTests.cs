using FundDataApi.Entities.Domain;
using System.Threading.Tasks;
using Xunit;

namespace FundDataApi.Tests.Services.HistoricalData;

public class LoadHistoricalDataHandlerTests : TestWithFixtureAndSqliteInMemoryDbContext
{
    [Fact]
    public async Task Handle_Loads_Data_To_Db()
    {
        var dbContext = DbContextFactory();
        dbContext.Add(new Fund());
        await dbContext.SaveChangesAsync();
    }
}