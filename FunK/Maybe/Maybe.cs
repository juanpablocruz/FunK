using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace FunK
{
    using static F;

    public static partial class F
    {
        /// <summary>
        /// Maybe.Just shortcut, is used for representing a value in the Maybe monad.
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Value</param>
        /// <returns>Maybe.Just<T> with value</returns>
        public static Maybe<T> Just<T>(T value) => new Maybe.Just<T>(value);

        /// <summary>
        /// Maybe.Nothing shortcut, is used for representing the absence of value in the Maybe monad.
        /// </summary>
        public static Maybe.Nothing Nothing => Maybe.Nothing.Default;
    }

    /// <summary>
    /// Maybe monad used for error handling.
    /// The Maybe monad can represent the duality when an operation may return a value or not,
    /// providing the functor and monad interface Map and Bind to allow composition.
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public struct Maybe<T> : IEquatable<Maybe.Nothing>, IEquatable<Maybe<T>>
    {
        private readonly T value;
        private readonly bool isJust;


        private Maybe(T value)
        {
            if (value == null)
                throw new ArgumentNullException();

            this.isJust = true;
            this.value = value;
        }

        public static implicit operator Maybe<T>(Maybe.Nothing _) => new Maybe<T>();
        public static implicit operator Maybe<T>(Maybe.Just<T> just) => new Maybe<T>(just.Value);

        public static implicit operator Maybe<T>(T value)
          => value == null ? Nothing : Just(value);

        public R Match<R>(Func<R> Nothing, Func<T, R> Just)
          => isJust ? Just(value) : Nothing();

        public IEnumerable<T> AsEnumerable()
        {
            if (isJust) yield return value;
        }

        public bool Equals(Maybe<T> other)
          => this.isJust == other.isJust
          && (this.IsNothing || this.value.Equals(other.value));

        public bool Equals(Maybe.Nothing _) => IsNothing;

        public static bool operator ==(Maybe<T> @this, Maybe<T> other) => @this.Equals(other);
        public static bool operator !=(Maybe<T> @this, Maybe<T> other) => !(@this == other);

        public override string ToString() => isJust ? $"Just({value})" : "Nothing";

        public bool IsJust() => isJust;
        public bool IsNothing => !isJust;

        public static Maybe<T> From(T obj)
        {
            return new Maybe<T>(obj);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Maybe<T>))
            {
                if (obj is T)
                {
                    obj = new Maybe<T>((T)obj);
                }
                if (!(obj is Maybe<T>))
                    return false;
            }
            var other = (Maybe<T>)obj;
            return Equals(other);
        }
        public override int GetHashCode()
        {
            if (IsNothing)
                return 0;
            return value.GetHashCode();
        }
    }


    namespace Maybe
    {
        public struct Nothing
        {
            internal static readonly Nothing Default = new Nothing();
        }

        public struct Just<T>
        {
            internal T Value { get; }
            internal Just(T value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Cannot wrap a null value in a 'Just'; use 'Nothing' instead");
                }
                Value = value;
            }
        }
    }

}
