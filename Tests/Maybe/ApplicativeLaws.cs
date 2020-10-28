using FsCheck;
using FsCheck.Xunit;
using System;
using static FunK.F;
using Xunit;

namespace FunK.Tests
{

    // teach FsCheck to create an arbitrary Option
    static class ArbitraryMaybe
    {
        public static Arbitrary<Maybe<T>> Maybe<T>()
        {
            var gen = from isJust in Arb.Generate<bool>()
                      from val in Arb.Generate<T>()
                      select isJust && val != null ? Just(val) : Nothing;
            return gen.ToArbitrary();
        }
    }

    public static class Maybe_ApplicativeLaws
    {
        // the identity function is still the identity function, in the applicative world
        [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
        public static void IdentityHolds(Maybe<int> v)
        {
            Func<int, int> id = x => x;
            Assert.Equal(Just(id).Apply(v), v);
        }

        static readonly Func<int, int> minus10 = x => x - 10;
        static readonly Func<int, int> times2 = x => x * 2;

        [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
        public static void CompositionHolds(Maybe<int> w, bool uIsJust, bool vIsJust)
        {
            Func<Func<int, int>, Func<int, int>, Func<int, int>> compose
              = (f, g) => x => f(g(x));
            var u = uIsJust ? Just(minus10) : Nothing;
            var v = vIsJust ? Just(times2) : Nothing;

            Assert.Equal(
              Just(compose)
                .Apply(u)
                .Apply(v)
                .Apply(w),
              u.Apply(v.Apply(w)));
        }

        [Property]
        public static void HomomorphismHolds(int x)
          => Assert.Equal(
            Just(minus10).Apply(Just(x)),
            Just(minus10(x))
            );

        [Property]
        public static void InterchangeHolds(bool uIsJust, int y)
        {
            var u = uIsJust ? Just(minus10) : Nothing;
            Func<Func<int, int>, int> applyY = f => f(y);

            Assert.Equal(
              u.Apply(Just(y)),
              Just(applyY).Apply(u));
        }

        static readonly Func<int, int, int> multiply = (i, j) => i * j;

        [Property(Arbitrary = new[] { typeof(ArbitraryMaybe) })]
        public static void _ApplicativeLawHolds(Maybe<int> a, Maybe<int> b)
            => Assert.Equal(
                Just(multiply).Apply(a).Apply(b),
                a.Map(multiply).Apply(b)
            );

        [Property(Verbose = true)]
        public static void ApplicativeLawHolds(int a, int b)
        {
            var first = Just(multiply)
              .Apply(Just(a))
              .Apply(Just(b));

            var second = Just(a)
              .Map(multiply)
              .Apply(Just(b));

            Assert.Equal(first, second);
        }

        public static Maybe<R> ApplyInTermsOfBind<T, R>(this Maybe<Func<T, R>> func, Maybe<T> arg)
            => arg.Bind(t => func.Bind<Func<T, R>, R>(f => f(t)));

        [Property]
        public static void ApplicativeLawHolds_WhenApplyIsDefinedInTermsOfBind(int a, int b)
        {
            Func<int, int, int> multiply = (i, j) => i * j;

            var first = Just(multiply)
              .Apply(Just(a))
              .ApplyInTermsOfBind(Just(b));

            var second = Just(a)
              .Map(multiply)
              .ApplyInTermsOfBind(Just(b));

            Assert.Equal(first, second);
        }
    }

}
