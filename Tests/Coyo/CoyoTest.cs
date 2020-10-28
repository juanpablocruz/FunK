using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace FunK.Tests
{
  using static F;
  public class CoyoTest
  {
    [Fact]
    public void Test_ThousandMillionData_Transforms()
    {
      var data = Range(1, 1000000000);
      Func<int, int> multiply = (a) => a * 2;
      Func<int, int> add2 = (a) => a + 2;
      Func<int, string> toString = (a) => a.ToString();

      var coyoVal = Coyo.Of<IEnumerable<int>, int>(data)
        .Map(multiply)
        .Map(add2)
        .Map(toString)
        .Map(String.ToUpper);

      Stopwatch sw1 = new Stopwatch();
      Stopwatch sw2 = new Stopwatch();

      sw1.Start();
      var chained = data.Map(multiply).Map(add2).Map(toString).Map(String.ToUpper);
      sw1.Stop();
      var elapsed1 = sw1.Elapsed;

      sw2.Start();
      var composed = coyoVal.Value.Map(x =>coyoVal.Func(x));
      sw2.Stop();
      var elapsed2 = sw2.Elapsed;

      // Better test first for the equality check takes way too long
      var first1 = chained.FirstOr("");
      var first2 = composed.FirstOr("");

      Assert.True(elapsed1 > elapsed2);
      Assert.Equal(first1, first2);
    }

    

    [Fact]
    public void Test_Run_Coyo_After_Composing()
    {
      Func<string, char[]> splitByChars = s => s.ToCharArray();
      Func<char[], int> countLength = s => s.Count();

      var coyo = Coyo.Of<string, string>("hola")
        .Map(String.ToUpper)
        .Map(splitByChars)
        .Map(countLength)
        .Run();

      Assert.Equal(4, coyo);
      
    }
  }
}
