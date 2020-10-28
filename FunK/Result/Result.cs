using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace FunK
{
    public partial class F
    {
        public static Result<T, E> Result<T, E>(T value) => new Result<T, E>(value);
        public static Result<T, E> Result<T, E>(E error) => new Result<T, E>(error);
    }

    public partial struct Result<T,E>
    {
        internal E _Error { get; }
        internal T _Value { get; }

        public bool IsSuccess => EqualityComparer<E>.Default.Equals(_Error, default);
        public bool IsError => !EqualityComparer<E>.Default.Equals(_Error, default);

        internal Result(E error)
        {
            _Error = error;
            _Value = default;
        }

        internal Result(T value)
        {
            _Error = default;
            _Value = value;
        }

        public static implicit operator Result<T,E>(E error) => new Result<T, E>(error);
        public static implicit operator Result<T, E>(T value) => new Result<T, E>(value);


        public TR Match<TR>(Func<E, TR> Error, Func<T, TR> Success)
            => IsError ? Error(_Error) : Success(_Value);
        public Unit Match<TR>(Action<E> Error, Action<T> Success)
            => Match(Error.ToFunc(), Success.ToFunc());



        public Exception handleError(E Error)
            => Error switch
            {
                Exception e => e,
                Error e => new Exception(e.Message),
                object e => new Exception(e.ToString())
            };

        // Lift 
        public static Result<T, E> Of(E error)
            => new Result<T, E>(error);

        public static Result<T, E> Of(T value)
            => new Result<T, E>(value);

    }

    public static partial class Result
    {

        #region Functor

        public static Result<R, E> Map<T, R, E>(this Result<T, E> @this, Func<T, R> func)
            => @this.IsSuccess ? func(@this._Value) : new Result<R, E>(@this._Error);

        public static Result<R, E> MapSync<T, R, E>(this Result<T, E> @this, Func<T, Task<R>> func)
            => @this.IsSuccess ? func(@this._Value).GetAwaiter().GetResult() : new Result<R, E>(@this._Error);

        public static Result<Unit, E> ForEach<T, E>(this Result<T, E> @this, Action<T> act)
            => @this.Map(act.ToFunc());
        #endregion

        #region Monad
        public static Result<R, E> Bind<T, R, E>(this Result<T, E> @this, Func<T, Result<R, E>> func)
            => @this.IsSuccess ? func(@this._Value) : new Result<R, E>(@this._Error);




        // Flat
        public static T GetOrThrow<T,E>(this Result<T,E> result) 
            => result.IsError ? throw result.handleError(result._Error) : result._Value;

        public static T GetOrThrow<T, E, TEx>(this Result<T,E> result ) where TEx : Exception, new()
            => result.IsError ? throw new TEx() : result._Value;

        #endregion

        public static Result<T,E> Tap<T,E>(this Result<T,E> @this, Action action)
        {
            action();
            return @this;
        }
        public static Result<T, E> Tap<T, E>(this Result<T, E> @this, Action<T> func)
        {
            @this.Map(func.ToFunc());
            return @this;
        }



    }
}