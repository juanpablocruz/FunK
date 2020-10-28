using System;
using Xunit;

namespace FunK.Tests
{
    public class TuplesTests
    {
        Func<string, int, string> fun = (name, value) => $"{name}: {value}";

        [Fact]
        public void ValueTuples_Can_Apply_Deconstructed_Values_To_Func()
        {
            var a = (name: "name", value: 2);

            var applied = a.Match(fun);

            Assert.Equal("name: 2", applied);
        }

        [Fact]
        public void Tuples_Can_Apply_Deconstructed_Values_To_Func()
        {
            Tuple<string,int> a = Tuple.Create("name", 2);
            
            var applied = a.Match(fun);

            Assert.Equal("name: 2", applied);
        }
    }
}
