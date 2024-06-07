using AutoFixture;
using FluentAssertions;
using FundDataApi.Services.HistoricalData;

namespace FundDataApi.Tests;

public class Sp500ConstituentsQueryHandlerTests : TestWithFixture
{
    private readonly Sp500ConstituentsQueryHandler _sut;

    public Sp500ConstituentsQueryHandlerTests()
    {
        _sut = Fixture.Create<Sp500ConstituentsQueryHandler>();
    }

    [Fact]
    public async Task Handle_Returns_Sp500_Constituents_Symbols()
    {
        //Act
        var results = await _sut.Handle(new Sp500ConstituentsQuery(), CancellationToken.None);

        //Assert
        results.Should().HaveCount(503); // TODO: will need to update this test everytime S&P 500 changes
    }
}
