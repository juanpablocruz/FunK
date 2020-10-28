using System;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace FunK
{
    using static F;
    public static class ResultTMonadExtensions
    {
        public static Result<R> Bind<T, R>(this Result<T> @this, Func<T, Result<R>> func)
            => @this.IsSuccess ? func(@this._Value) : new Result<R>(@this._Error);

        // Flatten type

        /// <summary>
        /// Flattens the <see cref="Result{T}"/> extracting its inner value or, in case of error, throwing the exception it holds
        /// </summary>
        public static Task<T> GetOrThrow<T>(this Task<Result<T>> @this)
            => @this.Map(v => v.GetOrThrow());


        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Unit Match<T>(this Result<T> @this, Action<Exception> OnError, Action<T> OnSuccess)
            => @this.IsError ? OnError.ToFunc()(@this._Error) : OnSuccess.ToFunc()(@this._Value);

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<Unit> Match<T>(this Task<Result<T>> @this, Action<Exception> OnError, Action<T> OnSuccess)
            => @this.Map(result => result.IsError ? OnError.ToFunc()(result._Error) : OnSuccess.ToFunc()(result._Value));

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static T Match<T>(this Result<T> @this, Func<Exception, T> OnError, Func<T, T> OnSuccess)
            => @this.IsError ? OnError(@this._Error) : OnSuccess(@this._Value);

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Task<Result<T>> @this, Func<Exception, T> OnError, Func<T, T> OnSuccess)
            => @this.Map(result => result.IsError ? OnError(result._Error) : OnSuccess(result._Value));

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Result<T> @this, Func<Exception, Task<T>> OnError, Func<T, Task<T>> OnSuccess)
            => @this.IsError
                ? OnError(@this._Error)
                : OnSuccess(@this._Value);

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Result<T> @this, Func<Exception, Task<T>> OnError, Func<T, T> OnSuccess)
            => @this.IsError
                ? OnError(@this._Error)
                : Async(OnSuccess(@this._Value));

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Result<T> @this, Func<Exception, T> OnError, Func<T, Task<T>> OnSuccess)
            => @this.IsError
                ? Async(OnError(@this._Error))
                : OnSuccess(@this._Value);

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Task<Result<T>> @this, Func<Exception, Task<T>> OnError, Func<T, Task<T>> OnSuccess)
            => @this.Bind(result => result.IsError ? OnError(result._Error) : OnSuccess(result._Value));

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Task<Result<T>> @this, Func<Exception, T> OnError, Func<T, Task<T>> OnSuccess)
            => @this.Bind(result => result.IsError ? Async(OnError(result._Error)) : OnSuccess(result._Value));

        /// <summary>
        /// Apply an <paramref name="OnError"/> in case of error, or <paramref name="OnSuccess"/> if <see cref="Result{T}"/> holds a value
        /// </summary>
        public static Task<T> Match<T>(this Task<Result<T>> @this, Func<Exception, Task<T>> OnError, Func<T, T> OnSuccess)
            => @this.Bind(result => result.IsError ? OnError(result._Error) : Async(OnSuccess(result._Value)));

    }
}
