using AutoFixture;
using AutoFixture.AutoMoq;

namespace FundDataApi.Tests;

public class TestWithFixture
{
    private IFixture _fixture;

    protected TestWithFixture()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization());
        _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }

    protected IFixture Fixture => _fixture;
}