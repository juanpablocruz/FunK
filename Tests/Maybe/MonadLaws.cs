using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using Xunit;

namespace FunK.Tests
{
  using static F;

  public class Maybe_MonadLaws
  {
    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
    void RightIdentityHolds(Maybe<object> m) => Assert.Equal(
      m,
      m.Bind(Just));

    [Property]
    public void LeftIdentityHolds(int x)
    {
      // given a world crossing function f...
      Func<int, Maybe<int>> f = i => i % 2 == 0 ? Just(i) : Nothing;

      // then applying f to a value x
      // is the same as lifting x and binding f to it
      Assert.Equal(Just(x).Bind(f), f(x));
    }


    [Property] // only works for non-null
    public void LeftIdentityHoldsRefValues(NonNull<string> x)
    {
      // given a world crossing function f
      Func<string, Maybe<string>> f = s => Just($"{s} World");
      // then applying f to a value x
      // is the same as lifting x and binding f to it
      Assert.Equal(Just(x.Get).Bind(f), f(x.Get));
    }

    // 3. Associativity
    // (m `bind` f) `bind` g
    // m `bind` (f `bind` g) 
    // m `bind` (x => f(x) `bind` g)
    Func<double, Maybe<double>> safeSqrt = d
      => d < 0 ? Nothing : Just(Math.Sqrt(d));

    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
    void AssociativityHolds(Maybe<string> m) => Assert.Equal(
      m.Bind(Double.Parse).Bind(safeSqrt),
      m.Bind(x => Double.Parse(x).Bind(safeSqrt)));

  }
}
