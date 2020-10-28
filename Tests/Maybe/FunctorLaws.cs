﻿using System;
using FsCheck.Xunit;
using Xunit;

namespace FunK.Tests
{
  using static F;
  public static class Maybe_FunctorLaws
  {
    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
    static void FirstFunctorLawHolds(Maybe<object> a)
      => Assert.Equal(a, a.Map(x => x));

    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe)})]
    static void SecondFunctorLawHolds(Maybe<int> val)
    {
      Func<int, int> f = i => i - 2;
      Func<int, int> g = i => i * 50;
      // h = f `andthen` g
      Func<int, int> h = i => g(f(i));

      Assert.Equal(
        val.Map(f).Map(g),
        val.Map(h));
    }

    public static Maybe<R> MapInTermsOfApply<T, R>
      (this Maybe<T> @this, Func<T, R> func)
      => Just(func).Apply(@this);

    [Property]
    static void SecondFunctorLawHolds_WhenMapIsDefinedInTermsOfApply(int input)
    {
      Func<int, int> f = i => i - 2;
      Func<int, int> g = i => i * 50;
      Func<int, int> h = i => g(f(i));

      Assert.Equal(
        Just(input).MapInTermsOfApply(f).MapInTermsOfApply(g),
        Just(input).MapInTermsOfApply(h));
    }
  }
}
