using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;

namespace FunK
{
    using static F;

    public static partial class F
    {
        public static Validation<T> Valid<T>(T value) => new Validation<T>(value);

        // create a Validation in the Invalid state
        public static Validation.Invalid Invalid(params Error[] errors) => new Validation.Invalid(errors);
        public static Validation<R> Invalid<R>(params Error[] errors) => new Validation.Invalid(errors);
        public static Validation.Invalid Invalid(IEnumerable<Error> errors) => new Validation.Invalid(errors);
        public static Validation<R> Invalid<R>(IEnumerable<Error> errors) => new Validation.Invalid(errors);
    }

    public struct Validation<T>
    {
        public IEnumerable<Error> Errors { get; }
        internal T Value { get; }

        public bool IsValid { get; }

        // the Return function for Validation
        public static Func<T, Validation<T>> Return = t => Valid(t);

        public static Validation<T> Fail(IEnumerable<Error> errors)
           => new Validation<T>(errors);

        public static Validation<T> Fail(params Error[] errors)
           => new Validation<T>(errors.AsEnumerable());

        private Validation(IEnumerable<Error> errors)
        {
            IsValid = false;
            Errors = errors;
            Value = default;
        }

        internal Validation(T right)
        {
            IsValid = true;
            Value = right;
            Errors = Enumerable.Empty<Error>();
        }

        public static implicit operator Validation<T>(Error error)
           => new Validation<T>(new[] { error });

        public static implicit operator Validation<T>(Validation.Invalid left)
           => new Validation<T>(left.errors);

        public static implicit operator Validation<T>(T right) => Valid(right);

        public TR Match<TR>(Func<IEnumerable<Error>, TR> Invalid, Func<T, TR> Valid)
           => IsValid ? Valid(this.Value) : Invalid(this.Errors);

        public Unit Match(Action<IEnumerable<Error>> Invalid, Action<T> Valid)
           => Match(Invalid.ToFunc(), Valid.ToFunc());

        public IEnumerator<T> AsEnumerable()
        {
            if (IsValid) yield return Value;
        }

        public override string ToString()
           => IsValid
              ? $"Valid({Value})"
              : $"Invalid([{string.Join(", ", Errors)}])";

        public override bool Equals(object obj) => this.ToString() == obj.ToString(); // hack

        public override int GetHashCode()
        {
            var hashCode = -1141250687;
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Error>>.Default.GetHashCode(Errors);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + IsValid.GetHashCode();
            return hashCode;
        }
    }


    public static class Validation
    {
        public struct Invalid
        {
            internal IEnumerable<Error> errors;
            public Invalid(IEnumerable<Error> errors) { this.errors = errors; }
        }

        public static T GetOrElse<T>(this Validation<T> opt, T defaultValue)
           => opt.Match(
              (errs) => defaultValue,
              (t) => t);

        public static T GetOrElse<T>(this Validation<T> opt, Func<T> fallback)
           => opt.Match(
              (errs) => fallback(),
              (t) => t);

        public static Validation<R> Aggregate<T, R>(this Validation<T> @this, Validation<R> next)
            => @this.Match(
                Invalid: errors => next.Match(
                    Invalid: errors2 => Invalid(errors.Concat(errors2)),
                    Valid: _ => Invalid(errors)
                ),
                Valid: v => next.Match(
                    Invalid: errors2 => Invalid(errors2),
                    Valid: v2 => Valid(v2)
                    )
                );

        public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> valF, Validation<T> valT)
           => valF.Match(
              Valid: (f) => valT.Match(
                 Valid: (t) => Valid(f(t)),
                 Invalid: (err) => Invalid(err)),
              Invalid: (errF) => valT.Match(
                 Valid: (_) => Invalid(errF),
                 Invalid: (errT) => Invalid(errF.Concat(errT))));


        public static Validation<Func<T2, R>> Apply<T1, T2, R>
           (this Validation<Func<T1, T2, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.Curry), arg);

        public static Validation<Func<T2, T3, R>> Apply<T1, T2, T3, R>
           (this Validation<Func<T1, T2, T3, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
           (this Validation<Func<T1, T2, T3, T4, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
           (this Validation<Func<T1, T2, T3, T4, T5, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
           (this Validation<Func<T1, T2, T3, T4, T5, T6, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
           (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
           (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
           (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Validation<RR> Map<R, RR>
           (this Validation<R> @this, Func<R, RR> f)
           => @this.IsValid
              ? Valid(f(@this.Value))
              : Invalid(@this.Errors);

        public static Validation<Func<T2, R>> Map<T1, T2, R>(this Validation<T1> @this
           , Func<T1, T2, R> func)
            => @this.Map(func.Curry());

        public static Validation<Unit> ForEach<R>
           (this Validation<R> @this, Action<R> act)
           => Map(@this, act.ToFunc());

        public static Validation<T> Do<T>
           (this Validation<T> @this, Action<T> action)
        {
            @this.ForEach(action);
            return @this;
        }

        public static Validation<R> Bind<T, R>
           (this Validation<T> val, Func<T, Validation<R>> f)
            => val.Match(
               Invalid: (err) => Invalid(err),
               Valid: (r) => f(r));

        public static Validation<T> Tap<T>(this Validation<T> @this, Action action)
        {
            action();
            return @this;
        }

        public static Validation<T> Tap<T, R>(this Validation<T> @this, Func<T, R> func)
        {
            @this.Map(func);
            return @this;
        }

        public static Validation<T> Tap<T, R>(this Validation<T> @this, Func<Validation<T>, R> func)
        {
            func(@this);
            return @this;
        }

        // LINQ

        public static Validation<R> Select<T, R>(this Validation<T> @this
           , Func<T, R> map) => @this.Map(map);

        public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> @this
           , Func<T, Validation<R>> bind, Func<T, R, RR> project)
           => @this.Match(
              Invalid: (err) => Invalid(err),
              Valid: (t) => bind(t).Match(
                 Invalid: (err) => Invalid(err),
                 Valid: (r) => Valid(project(t, r))));
    }
}
