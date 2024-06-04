using AutoFixture;

namespace FundDataApi.Tests;

public class TestWithFixture
{
    private IFixture _fixture;

    protected TestWithFixture() => _fixture = new Fixture();

    protected IFixture Fixture => _fixture;
}