using FluentAssertions;
using UpdateService.Services.VersionChecker;

namespace UpdateService.Tests.Services.VersionCompare
{
    public class VersionCompareTests
    {
        private VersionCompareHandler _sut;

        public VersionCompareTests()
        {
            _sut = new VersionCompareHandler();
        }

        [Theory]
        [InlineData("", "", 0)]
        [InlineData(null, "", 0)]
        [InlineData("", null, 0)]
        [InlineData(" ", "", 0)]
        [InlineData("", " ", 0)]
        [InlineData(" ", " ", 0)]
        [InlineData("1", "2", -1)]
        [InlineData("2", "1", 1)]
        [InlineData("1.1.1.1", "1.1.1.10", -1)]
        [InlineData("1.1.1.1", "1.1.1", 1)]
        [InlineData("1.1.1", "1.1.1.1", -1)]
        [InlineData("1.1.1.2", "1.1.1.1", 1)]
        [InlineData("1.1.1.1", "1.1.1.2", -1)]
        // a < b => -1
        // a = b => 0
        // a > b => 1
        public void CompareVersions(string value1, string value2, int excpect)
        {
            // arrange

            // act
            var result = _sut.CompareVersions(value1, value2);

            // assert

            result.Should().Be(excpect);
        }
    }
}