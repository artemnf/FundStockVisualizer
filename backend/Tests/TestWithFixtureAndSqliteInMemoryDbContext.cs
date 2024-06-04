using TestSupport.EfHelpers;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FundDataApi.Data;

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
    }

    protected Func<FundDataDbContext> DbContextFactory => _dbContextFactory;
}
