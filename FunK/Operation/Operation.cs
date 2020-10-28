using System;
using System.Threading.Tasks;

namespace FunK
{
    public partial class F
    {
        public static Operation<T, T> Begin<T>(T value)
            => Operation<T, T>.Of(value);


        public static Operation<T, T> Begin<T>(Result<T> value)
            => Operation<T, T>.Of(value);


        public static Operation<(T,R), (T,R)> Begin<T, R>((T,R) value)
            => Operation<(T,R), (T,Random)>.Of(value);


        public static Operation<T, TR> Begin<T, TR>(T value, Func<object, TR> func)
            => Operation<T, TR>.Of(value, func);

        public static Operation<T, TR> Begin<T, TR>(Result<T> value, Func<object, TR> func)
            => Operation<T, TR>.Of(value, func);
    }


    public partial class Operation<T, FR>
    {
        internal Result<T> value;
        internal Func<object, Result<FR>> λ;

        internal Operation(T value, Func<object, FR> func)
        {
            this.value = F.Result(value);
            λ = x => func(x);
        }

        internal Operation(Result<T> value, Func<object, FR> func)
        {
            this.value = value;
            λ = x => func(x);
        }

        internal Operation(T value, Func<object, Result<FR>> func)
        {
            this.value = F.Result(value);
            λ = func;
        }

        internal Operation(Result<T> value, Func<object, Result<FR>> func)
        {
            this.value = value;
            λ = func;
        }

        public static Operation<T, T> Of(T value)
            => new Operation<T, T>(value, x => (T)x);

        public static Operation<T, FR> Of(Result<T> value)
            => new Operation<T, FR>(value, x => (FR)x);

        public static Operation<T, TR> Of<TR>(T value, Func<object, TR> func)
            => new Operation<T, TR>(value, func);

        public static Operation<T, TR> Of<TR>(Result<T> value, Func<object, TR> func)
            => new Operation<T, TR>(value, func);
    }


    public static partial class OperationExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Operation{T, FR}"/> with the set of λ from the original but with an starting value of <paramref name="value"/>
        /// </summary>
        public static Operation<T, TR> Using<T, TR>(this Operation<T, TR> operation, T value)
            => new Operation<T, TR>(value, operation.λ);

        /// <summary>
        /// Creates a new <see cref="Operation{T, FR}"/> with the set of λ from the original but with an starting value of <paramref name="value"/>
        /// </summary>
        public static Operation<T, TR> Using<T, TR>(this Operation<T, TR> operation, Result<T> value)
            => new Operation<T, TR>(value, operation.λ);

        /// <summary>
        /// Creates a new async <see cref="Operation{T, FR}"/> with the set of λ from the original but with an starting value of <paramref name="value"/> once it resolves
        /// </summary>
        public static Task<Operation<T, TR>> Using<T, TR>(this Operation<T, TR> operation, Task<Result<T>> task)
            => task.Map(value => new Operation<T, TR>(value, operation.λ));

        /// <summary>
        /// Joins two <see cref="Operation{T, FR}"/> into one, by applying all the λ from <paramref name="operation2"/> to <paramref name="operation"/>
        /// </summary>
        public static Operation<T, FR> Apply<T, R, FR>(this Operation<T, R> operation, Operation<T, FR> operation2)
            => new Operation<T, FR>(operation.value, x => operation.λ(x).Bind(r => operation2.λ(r)));



    }
}
