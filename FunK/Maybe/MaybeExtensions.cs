using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace FunK
{
    using System.Threading.Tasks;
    using static F;
    public static class MaybeExtensions
    {
        public static Maybe<R> Apply<T, R>
          (this Maybe<Func<T, R>> @this, Maybe<T> arg)
          => @this.Match(
            () => Nothing,
            (func) => arg.Match(
              () => Nothing,
              (val) => Just(func(val))));

        public static Maybe<Func<T2, R>> Apply<T1, T2, R>
             (this Maybe<Func<T1, T2, R>> @this, Maybe<T1> arg)
             => Apply(@this.Map(F.Curry), arg);

        public static Maybe<Func<T2, T3, R>> Apply<T1, T2, T3, R>
           (this Maybe<Func<T1, T2, T3, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
           (this Maybe<Func<T1, T2, T3, T4, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
           (this Maybe<Func<T1, T2, T3, T4, T5, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
           (this Maybe<Func<T1, T2, T3, T4, T5, T6, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
           (this Maybe<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
           (this Maybe<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
           (this Maybe<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Maybe<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Maybe<R> Bind<T, R>
           (this Maybe<T> optT, Func<T, Maybe<R>> f)
            => optT.Match(
               () => Nothing,
               (t) => f(t));

        public static IEnumerable<R> Bind<T, R>
           (this Maybe<T> @this, Func<T, IEnumerable<R>> func)
            => @this.AsEnumerable().Bind(func);

        public static Maybe<Unit> ForEach<T>(this Maybe<T> @this, Action<T> action)
           => Map(@this, action.ToFunc());

        public static Maybe<R> Map<T, R>
           (this Maybe.Nothing _, Func<T, R> f)
           => Nothing;

        public static Maybe<R> Map<T, R>
           (this Maybe.Just<T> some, Func<T, R> f)
           => Just(f(some.Value));

        public static Maybe<R> Map<T, R>
           (this Maybe<T> optT, Func<T, R> f)
           => optT.Match(
              () => Nothing,
              (t) => Just(f(t)));

        public static Maybe<Func<T2, R>> Map<T1, T2, R>
           (this Maybe<T1> @this, Func<T1, T2, R> func)
            => @this.Map(func.Curry());

        public static Maybe<Func<T2, T3, R>> Map<T1, T2, T3, R>
           (this Maybe<T1> @this, Func<T1, T2, T3, R> func)
            => @this.Map(func.CurryFirst());

        public static IEnumerable<Maybe<R>> Traverse<T, R>(this Maybe<T> @this
           , Func<T, IEnumerable<R>> func)
           => @this.Match(
              () => List((Maybe<R>)Nothing),
              (t) => func(t).Map(r => Just(r)));

        // utilities

        public static Unit Match<T>(this Maybe<T> @this, Action Nothing, Action<T> Just)
            => @this.Match(Nothing.ToFunc(), Just.ToFunc());

        internal static bool IsJust<T>(this Maybe<T> @this)
           => @this.Match(
              () => false,
              (_) => true);

        internal static T ValueUnsafe<T>(this Maybe<T> @this)
           => @this.Match(
              () => { throw new InvalidOperationException(); },
              (t) => t);

        public static T GetOrElse<T>(this Maybe<T> opt, T defaultValue)
           => opt.Match(
              () => defaultValue,
              (t) => t);

        public static T GetOrElse<T>(this Maybe<T> opt, Func<T> fallback)
           => opt.Match(
              () => fallback(),
              (t) => t);

        public static Task<T> GetOrElse<T>(this Maybe<T> opt, Func<Task<T>> fallback)
           => opt.Match(
              () => fallback(),
              (t) => Async(t));

        public static Maybe<T> OrElse<T>(this Maybe<T> left, Maybe<T> right)
           => left.Match(
              () => right,
              (_) => left);

        public static Maybe<T> OrElse<T>(this Maybe<T> left, Func<Maybe<T>> right)
           => left.Match(
              () => right(),
              (_) => left);


        public static Validation<T> ToValidation<T>(this Maybe<T> opt, Func<Error> error)
           => opt.Match(
              () => Invalid(error()),
              (t) => Valid(t));


        public static Maybe<T> Tap<T>(this Maybe<T> @this, Action action)
        {
            action();
            return @this;
        }

        public static Maybe<T> Tap<T, R>(this Maybe<T> @this, Func<T, R> func)
        {
            @this.Map(func);
            return @this;
        }
        public static Maybe<T> Tap<T, R>(this Maybe<T> @this, Func<Maybe<T>, R> func)
        {
            func(@this);
            return @this;
        }

        // LINQ

        public static Maybe<R> Select<T, R>(this Maybe<T> @this, Func<T, R> func)
           => @this.Map(func);

        public static Maybe<T> Where<T>
           (this Maybe<T> optT, Func<T, bool> predicate)
           => optT.Match(
              () => Nothing,
              (t) => predicate(t) ? optT : Nothing);

        public static Maybe<RR> SelectMany<T, R, RR>
           (this Maybe<T> opt, Func<T, Maybe<R>> bind, Func<T, R, RR> project)
           => opt.Match(
              () => Nothing,
              (t) => bind(t).Match(
                 () => Nothing,
                 (r) => Just(project(t, r))));

    }
}
