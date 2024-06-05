using AutoFixture;
using FluentAssertions;
using FundDataApi.Services.HistoricalData;

namespace FundDataApi.Tests;

public class GetSp500ConstituentsHandlerTests : TestWithFixture
{
    private readonly GetSp500ConstituentsHandler _sut;

    public GetSp500ConstituentsHandlerTests()
    {
        _sut = Fixture.Create<GetSp500ConstituentsHandler>();
    }

    [Fact]
    public async Task Handle_Returns_Sp500_Constituents_Symbols()
    {
        //Act
        var results = await _sut.Handle(new GetSp500ConstituentsQuery(), CancellationToken.None);

        //Assert
        results.Should().HaveCount(503); // TODO: will need to update this test everytime S&P 500 changes
    }
}
