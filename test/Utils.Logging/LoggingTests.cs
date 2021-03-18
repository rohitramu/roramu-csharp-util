namespace RoRamu.Utils.Logging
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public partial class LoggingTests
    {
        public static IEnumerable<object[]> vals = new List<object[]>
        {
            new object[]
            {
                new object[][] { new object[] { 1, 2, 3 }, new object[] { 1, 2 } },
                new object[][] { new object[] { 1, 1 }, new object[] { 2, 1 }, new object[] { 3, 1 }, new object[] { 1, 2 }, new object[] { 2, 2 }, new object[] { 3, 2 } }
            },
        };
        [Theory]
        [MemberData(nameof(vals))]
        public void TestCartesianProduct(object[][] input, object[][] expectedOutput)
        {
            var output = EnumerableUtils.CartesianProduct(input);
            output.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
