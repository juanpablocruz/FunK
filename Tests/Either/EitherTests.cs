using Xunit;
using static System.Math;

namespace FunK.Tests
{

    public class EitherTests
    {
        public static Either<string, double> Calc(double x, double y)
        {
            if (y == 0)
                return "y cannot be 0";

            if (x != 0 && Sign(x) != Sign(y))
                return "x / y cannot be negative";

            return Sqrt(x / y);
        }

        [Theory]
        [InlineData(1d, 0d, false)]
        [InlineData(-90d, 10d, false)]
        [InlineData(90d, 10d, true)]
        public void TestCalc(double x, double y, bool expected) 
             => Assert.Equal(expected, Calc(x, y).Match(_ => false, _ => true));
    }
}
