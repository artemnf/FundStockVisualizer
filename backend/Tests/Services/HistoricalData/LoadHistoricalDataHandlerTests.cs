using AutoFixture;
using FluentAssertions;
using FundDataApi.Entities.Domain;
using FundDataApi.Services.ExternalFinancialApi;
using FundDataApi.Services.HistoricalData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FundDataApi.Tests.Services.HistoricalData;

public class LoadHistoricalDataHandlerTests : TestWithFixtureAndSqliteInMemoryDbContext
{
    private readonly LoadHistoricalDataHandler _sut;
    private readonly Mock<IExternalFinancialApi> _externalFinancialApiMock;
    private readonly LoadHistoricalDataCommand _command;

    public LoadHistoricalDataHandlerTests()
    {
        _externalFinancialApiMock = Fixture.Freeze<Mock<IExternalFinancialApi>>();
        Fixture.Inject<IExternalFinancialApi>(Fixture.Create<YahooFinanceApi>());
        _sut = Fixture.Create<LoadHistoricalDataHandler>();
        _command = Fixture.Create<LoadHistoricalDataCommand>();
    }

    [Fact]
    public async Task Handle_Loads_Data_To_Db()
    {
        //Arrange
        var historicalDataPoints = Fixture.CreateMany<HistoricalDataPoint>();
        _externalFinancialApiMock.Setup(x => x.GetHistoricalData(It.IsAny<string>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(historicalDataPoints);
        //Act
        await _sut.Handle(_command, CancellationToken.None);

        //Assert
        var assertDbContext = DbContextFactory();
        var dataPoints = await assertDbContext.HistoricalDataPoints.ToListAsync();
        dataPoints.Should().HaveSameCount(historicalDataPoints);
    }


}