using Biblioteca.Tests.Configuration;
using Xunit.Abstractions;

namespace Biblioteca.Tests
{
    public class TestServiceBase : IClassFixture<TestServiceFixture>
    {
        protected readonly TestServiceFixture _fixture;

        private readonly ITestOutputHelper _output;

        public TestServiceBase(TestServiceFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
    }
}