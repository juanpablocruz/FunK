using Xunit;

namespace FunK.Tests
{
    internal static class Assertions
    {
        public static void Succeed() {; }
        public static void Fail() => Assert.True(false);

        public static void ResultEquals<T>(T expected, Result<T> result) 
            => Assert.Equal(expected, result.GetOrThrow());
    }


}
