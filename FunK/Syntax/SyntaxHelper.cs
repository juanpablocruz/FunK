using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Unit = System.ValueType;

namespace FunK
{
    using static F;

    public class Syntax<V, T>
    {
        public V Value { get; }
        public Func<object, T> Func { get; }

        public Syntax(V value, Func<object, T> func)
        {
            Value = value;
            Func = func;
        }
    }

    public static partial class F
    {
        public static Syntax<V, V> With<V>(V value)
           => new Syntax<V, V>(value, x => (V)x);

    }

    public static class Syntax
    {
        
        public static Syntax<V, R> Do<V, T, R>(this Syntax<V, T> @this
           , Func<T, R> func)
           => new Syntax<V, R>(@this.Value, x => func(@this.Func(x)));

        public static Syntax<V, R> Do<V, T, R>(this Syntax<V, T> @this
           , Func<T, Task<R>> func)
           => new Syntax<V, R>(@this.Value, x => func(@this.Func(x)).GetAwaiter().GetResult());

        public static Task<T> Run<V, T>(this Syntax<V, T> @this)
          => Async(@this.Func(@this.Value));

        public static Unit InCase<L, R>(this Task<Either<L, R>> task, Action<L> False, Action<R> True)
          => task.GetAwaiter().GetResult().Match(False, True);



        #region IFBlock



        public static Syntax<V, Either<T,R>> If<V, T, R>(this Syntax<V, T> @this, bool cond, Func<T, R> func)
            => cond 
                ? new Syntax<V, Either<T,R>>(@this.Value, v => Right(func(@this.Func(v)))) 
                : new Syntax<V, Either<T, R>>(@this.Value, v => Left(@this.Func(v)));

        public static Syntax<V, Either<T, R>> If<V, T, R>(this Syntax<V, T> @this, bool cond, Func<T, Task<R>> func)
            => cond
                ? new Syntax<V, Either<T, R>>(@this.Value, v => Right(func(@this.Func(v)).GetAwaiter().GetResult()))
                : new Syntax<V, Either<T, R>>(@this.Value, v => Left(@this.Func(v)));


        public static Syntax<V, Either<T, R>> If<V, T, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, R> func)
            => new Syntax<V, Either<T, R>>(@this.Value,
                val =>
                {
                    var value = @this.Func(val);
                    if (cond(value))
                        return Right(func(value));
                    else
                        return Left(value);
                });

        public static Syntax<V, Either<T, R>> If<V, T, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, Task<R>> func)
            => new Syntax<V, Either<T, R>>(@this.Value,
                val =>
                {
                    var value = @this.Func(val);
                    if (cond(value))
                        return Right(func(value).GetAwaiter().GetResult());
                    else
                        return Left(value);
                });

        #endregion

        #region IFElse
        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, bool cond, Func<T, R> trueFunc, Func<T, L> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    if (cond)
                        return Right(trueFunc(@this.Func(val)));
                    else
                        return Left(falseFunc(@this.Func(val)));
                });


        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, bool cond, Func<T, Task<R>> trueFunc, Func<T, L> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    if (cond)
                        return Right(trueFunc(@this.Func(val)).GetAwaiter().GetResult());
                    else
                        return Left(falseFunc(@this.Func(val)));
                });

        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, bool cond, Func<T, R> trueFunc, Func<T, Task<L>> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    if (cond)
                        return Right(trueFunc(@this.Func(val)));
                    else
                        return Left(falseFunc(@this.Func(val)).GetAwaiter().GetResult());
                });

        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, bool cond, Func<T, Task<R>> trueFunc, Func<T, Task<L>> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    if (cond)
                        return Right(trueFunc(@this.Func(val)).GetAwaiter().GetResult());
                    else
                        return Left(falseFunc(@this.Func(val)).GetAwaiter().GetResult());
                });



        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, R> trueFunc, Func<T, L> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value, 
                val => 
                {
                    var value = @this.Func(val);
                    if (cond(value))
                        return Right(trueFunc(value));
                    else
                        return Left(falseFunc(value));
                });

        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, Task<R>> trueFunc, Func<T, L> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    var value = @this.Func(val);
                    if (cond((T)val))
                        return Right(trueFunc(value).GetAwaiter().GetResult());
                    else
                        return Left(falseFunc(value));
                });

        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, R> trueFunc, Func<T, Task<L>> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    var value = @this.Func(val);
                    if (cond((T)val))
                        return Right(trueFunc(value));
                    else
                        return Left(falseFunc(value).GetAwaiter().GetResult());
                });

        public static Syntax<V, Either<L, R>> IfElse<V, T, L, R>(this Syntax<V, T> @this, Func<T, bool> cond, Func<T, Task<R>> trueFunc, Func<T, Task<L>> falseFunc)
            => new Syntax<V, Either<L, R>>(@this.Value,
                val =>
                {
                    var value = @this.Func(val);
                    if (cond(value))
                        return Right(trueFunc(value).GetAwaiter().GetResult());
                    else
                        return Left(falseFunc(value).GetAwaiter().GetResult());
                });

        #endregion



    }
}
