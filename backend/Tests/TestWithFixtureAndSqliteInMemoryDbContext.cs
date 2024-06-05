using AutoFixture;
using FundDataApi.Data;
using TestSupport.EfHelpers;

namespace FundDataApi.Tests;

public class TestWithFixtureAndSqliteInMemoryDbContext : TestWithFixture
{
    private Func<FundDataDbContext> _dbContextFactory;

    protected TestWithFixtureAndSqliteInMemoryDbContext()
    {
        var dbContextOptions = SqliteInMemory.CreateOptions<FundDataDbContext>();
        _dbContextFactory = () => new FundDataDbContext(dbContextOptions);

        var dbContext = DbContextFactory();
        dbContext.Database.EnsureCreated();

        Fixture.Inject(dbContext);
    }

    protected Func<FundDataDbContext> DbContextFactory => _dbContextFactory;
}
