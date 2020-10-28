using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunK.Tests
{
    using static F;
    public class ResultTest
    {
        Func<int, int> doubleTheIn = x => x * 2;
        Func<int, Func<int, Result<int>>> checkGreaterThan = x => val => val > x ? Result(val) : Result<int>(new InvalidProgramException($"Should be greater than {x}"));
        Func<int, string> randomString = len => len.ToString();
        Func<string, Result<string>> toRes = x => Result(x);
        Func<string, int> length = x => x.Length;
        Func<int, Result<int>> getInt = x => Result(x);
        
        
        public Result<int> Test()
            => getInt(1)
                .Map(doubleTheIn)
                .Map(randomString)
                .Bind(toRes)
                .Map(length);

        [Fact]
        public void Test2()
        {
            var checkGreaterThan5 = checkGreaterThan(5);
            var checkGreaterThan1 = checkGreaterThan(1);

            var res = getInt(6)
                .Bind(checkGreaterThan1)
                .Map(doubleTheIn)
                .Bind(checkGreaterThan5)
                .Map(randomString)
                .Map(length);

            Assert.True(true);
        }

        
    }
}
