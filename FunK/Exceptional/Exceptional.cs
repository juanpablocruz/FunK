using System;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace FunK
{
    using static F;
    public static partial class F
    {
        /// <summary>
        /// Creates a new Exceptional object from the value
        /// </summary>
        public static Exceptional<T> Exceptional<T>(T value) => new Exceptional<T>(value);
    }

    public struct Exceptional<T>
    {
        internal Exception Ex { get; }
        internal T Value { get; }

        public bool IsSuccess => Ex == null;
        public bool IsException => Ex != null;

        internal Exceptional(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));
            Ex = ex;
            Value = default(T);
        }

        internal Exceptional(T right)
        {
            Value = right;
            Ex = null;
        }
        public static implicit operator Exceptional<T>(Exception left) => new Exceptional<T>(left);
        public static implicit operator Exceptional<T>(T right) => new Exceptional<T>(right);

        public TR Match<TR>(Func<Exception, TR> Exception, Func<T, TR> Success)
          => IsException ? Exception(Ex) : Success(Value);

        public Unit Match(Action<Exception> Exception, Action<T> Success)
          => Match(Exception.ToFunc(), Success.ToFunc());

        public T GetOrThrow()
            => IsException ? throw Ex : Value;

        public T GetOrThrow<TEx>() where TEx : Exception, new()
            => IsException ? throw new TEx() : Value;

        public override string ToString()
          => Match(
            ex => $"Exception({ex.Message})",
            t => $"Success({t})");
    }

    public static class Exceptional
    {
        public static Func<T, Exceptional<T>> Return<T>()
          => t => t;

        public static Exceptional<R> Of<R>(Exception left)
          => new Exceptional<R>(left);

        public static Exceptional<R> Of<R>(R right)
          => new Exceptional<R>(right);

        // applicative

        public static Exceptional<R> Apply<T, R>
          (this Exceptional<Func<T, R>> @this, Exceptional<T> arg)
          => @this.Match(
            Exception: ex => ex,
            Success: func => arg.Match(
              Exception: ex => ex,
              Success: t => new Exceptional<R>(func(t))));

        // functor

        public static Exceptional<RR> Map<R, RR>(this Exceptional<R> @this,
          Func<R, RR> func) => @this.IsSuccess ? func(@this.Value) : new Exceptional<RR>(@this.Ex);

        public static Exceptional<Unit> ForEach<R>(this Exceptional<R> @this, Action<R> act)
          => Map(@this, act.ToFunc());

        public static Exceptional<RR> Bind<R, RR>(this Exceptional<R> @this
             , Func<R, Exceptional<RR>> func)
            => @this.IsSuccess ? func(@this.Value) : new Exceptional<RR>(@this.Ex);

        /// <summary>
        /// Converts an <see cref="Exceptional"/> to a <see cref="Validation"/>
        /// </summary>
        public static Validation<T> ToValidation<T>(this Exceptional<T> exceptional)
            => exceptional.Match(Exception: ex => Invalid(ex.Message), Success: t => Valid(t));

        /// <summary>
        /// Maps an <see cref="Exceptional"/> inner value with an async func and awaits for it to finish
        /// </summary>
        public static Exceptional<RR> MapTask<R, RR>(this Exceptional<R> @this, Func<R, Task<RR>> func)
            => @this.Map(e => func(e).GetAwaiter().GetResult());

        /// <summary>
        /// Binds an <see cref="Exceptional"/> inner value with an async func and awaits for it to finish
        /// </summary>
        public static Exceptional<RR> BindTask<R, RR>(this Exceptional<R> @this, Func<R, Task<Exceptional<RR>>> func)
            => @this.Bind(e => func(e).GetAwaiter().GetResult());

        /// <summary>
        /// Binds an <see cref="Exceptional"/> with a <see cref="Try{RR}"/> returning function and runs it to extract the Exceptional
        /// </summary>
        public static Exceptional<RR> Bind<R, RR>(this Exceptional<R> exceptional, Func<R, Try<RR>> func)
            => exceptional.Match(
                Exception: ex => ex,
                Success: t => func(t).Run()
               );

        public static Exceptional<T> Tap<T>(this Exceptional<T> @this, Action action)
        {
            action();
            return @this;
        }

        public static Exceptional<T> Tap<T, R>(this Exceptional<T> @this, Func<T, R> func)
        {
            @this.Map(func);
            return @this;
        }

        // LINQ

        public static Exceptional<R> Select<T, R>(this Exceptional<T> @this
       , Func<T, R> map) => @this.Map(map);

        public static Exceptional<RR> SelectMany<T, R, RR>(this Exceptional<T> @this
           , Func<T, Exceptional<R>> bind, Func<T, R, RR> project)
        {
            if (@this.IsException) return new Exceptional<RR>(@this.Ex);
            var bound = bind(@this.Value);
            return bound.IsException
               ? new Exceptional<RR>(bound.Ex)
               : project(@this.Value, bound.Value);
        }
    }
}
