using System;
using Unit = System.ValueTuple;

namespace FunK
{
    public partial class F
    {
        public static Result<T> Result<T>(T value) => new Result<T>(value);
        public static Result<T> Result<T>(Exception error) => new Result<T>(error);
    }

    public partial struct Result<T>
    {
        internal Exception _Error { get; }
        internal T _Value { get; }

        public bool IsSuccess => _Error == null;
        public bool IsError => _Error != null;

        internal Result(Exception error)
        {
            _Error = error;
            _Value = default;
        }

        internal Result(T value)
        {
            _Error = default;
            _Value = value;
        }


        public static implicit operator Result<T>(Validation<T> value)
            => value.Match(
                Invalid: err => new Result<T>(new Exception(err.ToString())),
                Valid: v => new Result<T>(v)
            );

        public static implicit operator Result<T>(Exceptional<T> value)
            => value.Match(
                    Exception: ex => new Result<T>(ex),
                    Success: v => new Result<T>(v)
            );

        public static implicit operator Result<T>(Maybe<T> value)
            => value.Match(
                    Nothing: () => new Result<T>(new InvalidProgramException()),
                    Just: v => new Result<T>(v)
            );

        public static implicit operator Result<T>(Exception error) => new Result<T>(error);
        
        public static implicit operator Result<T>(T value) => new Result<T>(value);


        public TR Match<TR>(Func<Exception, TR> Error, Func<T, TR> Success)
            => IsError ? Error(_Error) : Success(_Value);

        public Unit Match<TR>(Action<Exception> Error, Action<T> Success)
            => Match(Error.ToFunc(), Success.ToFunc());

        // Flat
        public T GetOrThrow() => IsError ? throw _Error : _Value;

        public T GetOrThrow<TEx>() where TEx : Exception, new()
            => IsError ? throw new TEx() : _Value;

        // Lift
        public static Result<T> Of(Exception error)
            => new Result<T>(error);

        public static Result<T> Of(T value)
            => new Result<T>(value);
    }

    public static partial class Result
    {
        public static Result<T> Tap<T>(this Result<T> @this, Action action)
        {
            action();
            return @this;
        }

        public static Result<T> Tap<T>(this Result<T> @this, Action<T> func)
        {
            @this.Map(func.ToFunc());
            return @this;
        }
    }
}
