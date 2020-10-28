using System;
using System.Threading.Tasks;
using Xunit;
using static FunK.F;

namespace FunK.Tests
{
    public class SyntaxTests
    {
                
        Func<int, int> Add3 = i => i + 3;
        Func<int, int> Add4 = i => i + 4;
        Func<int, Task<int>> Add5 = i => Async(i + 5);

        Func<int, string> IfGreater = i => $"{i} is Greater";
        Func<int, string> IfSmaller = i => $"{i} is Smaller";

        Func<int, Task<int>> WaitAndReturn = i => Task.FromResult(i);
        Func<int, bool> GreaterThan3 = i => i > 3;


        [Fact]
        public void Test()
        {
            With(2)
                .Do(Add3)
                .Do(WaitAndReturn)
                .Do(WaitAndReturn)
                .IfElse(GreaterThan3, IfGreater, IfSmaller)
                .Run()
                .InCase(
                    False: l => Assert.Equal("2 is Smaller", l),
                    True: r => Assert.Equal("5 is Greater", r)
                );
        }
    }
}
