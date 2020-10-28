using System;
using System.Collections.Generic;
using System.Text;

namespace FunK
{
  using static F;
  public static class Int
  {
    public static Maybe<int> Parse(string s)
    {
      int result;
      return int.TryParse(s, out result)
        ? Just(result) : Nothing;
    }

    public static bool IsOdd(int i) => i % 2 == 1;
    public static bool IsEven(int i) => i % 2 == 0;
  }
}
