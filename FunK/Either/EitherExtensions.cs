using System;
using Unit = System.ValueTuple;

namespace FunK
{
    using static F;
    public static class EitherExtensions
    {
        public static Either<L, RR> Map<L, R, RR>
             (this Either<L, R> @this, Func<R, RR> f)
             => @this.Match<Either<L, RR>>(
                l => Left(l),
                r => Right(f(r)));

        public static Either<LL, RR> Map<L, LL, R, RR>
           (this Either<L, R> @this, Func<L, LL> left, Func<R, RR> right)
           => @this.Match<Either<LL, RR>>(
              l => Left(left(l)),
              r => Right(right(r)));

        public static Either<L, Unit> ForEach<L, R>
           (this Either<L, R> @this, Action<R> act)
           => Map(@this, act.ToFunc());

        public static Either<L, RR> Bind<L, R, RR>
           (this Either<L, R> @this, Func<R, Either<L, RR>> f)
           => @this.Match(
              l => Left(l),
              r => f(r));

        public static Either<L,RR> Tap<L,RR>(this Either<L,RR> @this, Action action)
        {
            action();
            return @this;
        }

        public static Either<L, RR> Tap<L, RR, R>(this Either<L, RR> @this, Func<L, R> funcLeft, Func<RR,R> funcRight)
        {
            @this.Map(funcLeft, funcRight);
            return @this;
        }

        public static Either<L, RR> Tap<L, RR, R>(this Either<L, RR> @this, Func<Either<L,RR>, R> func)
        {
            func(@this);
            return @this;
        }

        // LINQ

        public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
       , Func<T, R> map) => @this.Map(map);


        public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this
           , Func<T, Either<L, R>> bind, Func<T, R, RR> project)
           => @this.Match(
              Left: l => Left(l),
              Right: t =>
                 bind(@this.Right).Match<Either<L, RR>>(
                    Left: l => Left(l),
                    Right: r => project(t, r)));
    }
}

