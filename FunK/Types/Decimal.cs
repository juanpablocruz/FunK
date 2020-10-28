using System;
using static FunK.F;

namespace FunK
{
  public static class Decimal
  {
    public static Maybe<decimal> Parse(string s)
    {
      decimal result;
      return decimal.TryParse(s, out result)
        ? Just(result) : Nothing;
    }

    public static bool IsOdd(decimal i) => i % 2 == 1;
    public static bool IsEven(decimal i) => i % 2 == 0;
    public static new Func<decimal, string> ToString = d => d.ToString();
  }
}
