using Xunit;
using Shouldly;

namespace BddShowdown.Tests.XUnit
{
    public class ResponseCodesTests
    {
        public class CanCascade : ResponseCodesTests
        {
            [Theory]
            [InlineData(ResponseCodes.DoNotHonour)]
            public void ShouldCascade(string responseCode)
            {
                ResponseCodes.CanCascade(responseCode).ShouldBeTrue();
            }

            [Theory]
            [InlineData("10000")]
            public void ShouldNotCascade(string responseCode)
            {
                ResponseCodes.CanCascade(responseCode).ShouldBeFalse();
            }
        }
    }
}