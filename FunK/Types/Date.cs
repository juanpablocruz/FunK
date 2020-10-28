using System;
using System.Collections.Generic;
using System.Text;

namespace FunK
{
  using static F;
  public static class Date
  {
    public static Maybe<DateTime> Parse(string s)
    {
      DateTime d;
      return DateTime.TryParse(s, out d) ? Just(d) : Nothing;
    }
  }
}
