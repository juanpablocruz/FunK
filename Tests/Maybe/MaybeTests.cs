using Xunit;
using System;
using FsCheck.Xunit;

namespace FunK.Tests
{
  using static F;
  public class MaybeTests
  {
    
    [Fact]
    public void MatchCallsAppropriateFunc()
    {
      Assert.Equal("hello, John", Greet(Just("John")));
      Assert.Equal("sorry, who?", Greet(Nothing));
    }

    private string Greet(Maybe<string> name)
         => name.Match(
               Just: n => $"hello, {n}",
               Nothing: () => "sorry, who?");

    [Property(Arbitrary = new[] {typeof(ArbitraryMaybe)})]
    public void SingleClauseLINQExpr(Maybe<string> maybe)
      => Assert.Equal(
        from x in maybe select String.ToUpper(x),
        maybe.Map(String.ToUpper));


    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
    public void TwoClauseLINQExpr(Maybe<string> maybeA, Maybe<string> maybeB)
      => Assert.Equal(
        from a in maybeA
        from b in maybeB
        select a + b,
        maybeA.Bind(a => maybeB.Map(b => a + b)));

    [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
    public void ThreeClauseLINQExpr(Maybe<string> maybeA, Maybe<string> maybeB, Maybe<string> maybeC)
      => Assert.Equal(
        from a in maybeA
        from b in maybeB
        from c in maybeC
        select a + b + c,
        maybeA.Bind(a => maybeB.Bind(b => maybeC.Map(c => a + b + c))));
  }

  public class Maybe_Map_Test
  {
    class Apple { }
    class ApplePie { public ApplePie(Apple apple) { } }
    Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

    [Fact]
    public void GivenSomeApple_WhenMakePie_ThenJustPie()
    {
      var maybeApple = Just(new Apple());
      var maybePie = maybeApple.Map(makePie);
      Assert.True(maybePie.IsJust());
    }

    [Fact]
    public void GivenNoApple_WhenMakePie_ThenNoPie()
    {
      Maybe<Apple> maybeApple = Nothing;
      var maybePie = maybeApple.Map(makePie);
      Assert.False(maybePie.IsJust());
    }
  }

  public class Maybe_Apply_Test
  {
    Func<int, int, int> add = (a, b) => a + b;
    Func<int, int, int> multiply = (i, j) => i * j;

    private readonly Func<int, int, int, int> add3Integers =
      (a, b, c) => a + b + c;

    [Fact]
    public void MapAndApplyJustArg_ReturnsJust()
    {
      var maybe = Just(3)
        .Map(multiply)
        .Apply(Just(4));
      Assert.Equal(Just(12), maybe);
    }

    [Property]
    public void MapAndApplyNothingArg_ReturnsNothing(int i)
    {
      var maybe = Just(i)
        .Map(multiply)
        .Apply(Nothing);
      var maybe2 = ((Maybe<int>)Nothing)
        .Map(multiply)
        .Apply(i);

      Assert.Equal(Nothing, maybe);
      Assert.Equal(Nothing, maybe2);
    }

    [Fact]
    public void ApplyJustArgs()
    {
      var maybe = Just(add)
        .Apply(Just(3))
        .Apply(Just(4));
      Assert.Equal(Just(7), maybe);
    }

    [Fact]
    public void ApplyJustArgs_to_function_requiring_3_args()
    {
      var maybe = Just(add3Integers)
        .Apply(Just(1))
        .Apply(Just(2))
        .Apply(Just(3));
      Assert.Equal(Just(6), maybe);
    }

    [Property]
    public void ApplyNothingArgs(int i)
    {
      var maybe = Just(add)
        .Apply(Nothing)
        .Apply(Just(i));

      Assert.Equal(Nothing, maybe);
    }
  }
}
