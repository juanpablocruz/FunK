using System;
using System.Threading.Tasks;

namespace FunK
{
    public static class OperationIfThen
    {
        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true
        /// </summary>
        public static Operation<T, FR> IfThen<T, FR>(this Operation<T, FR> operation, Predicate<FR> predicate, Operation<T, FR> operation2)
            => new Operation<T, FR>(operation.value, x =>
            {
                var newVal = operation.λ(x);
                var res = newVal.GetOrThrow();
                if (predicate(res))
                {
                    return new Operation<T,FR>(operation.value, x => res)
                            .Apply(operation2)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
                }

                return newVal;
            });

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Operation<T, R> IfThenElse<T, R, FR>(this Operation<T, FR> operation, Predicate<FR> predicate, Operation<T, R> operation2, Operation<T, R> operation3)
            => new Operation<T, R>(operation.value, x =>
            {
                var newVal = operation.λ(x);
                var res = newVal.GetOrThrow();
                if (predicate(res))
                {
                    return new Operation<T, FR>(operation.value, x => res)
                            .Apply(operation2)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
                }

                return new Operation<T, FR>(operation.value, x => res)
                            .Apply(operation3)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
            });

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true
        /// </summary>
        public static Task<Operation<T, FR>> IfThen<T, FR>(this Task<Operation<T, FR>> operation, Predicate<FR> predicate, Operation<T, FR> operation2)
            => operation.Map(o => o.IfThen(predicate, operation2));

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Task<Operation<T, R>> IfThenElse<T, R, FR>(this Task<Operation<T, FR>> operation, Predicate<FR> predicate, Operation<T, R> operation2, Operation<T, R> operation3)
            => operation.Map(o => o.IfThenElse(predicate, operation2, operation3));




        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true
        /// </summary>
        public static Operation<T, FR> IfThen<T, FR>(this Operation<T, FR> operation, Predicate<FR> predicate, Func<FR, FR> operation2)
            => new Operation<T, FR>(operation.value, x =>
            {
                var newVal = operation.λ(x);
                var res = newVal.GetOrThrow();
                if (predicate(res))
                {
                    return new Operation<T, FR>(operation.value, x => res)
                            .Then(operation2)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
                }

                return newVal;
            });

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Operation<T, R> IfThenElse<T, R, FR>(this Operation<T, FR> operation, Predicate<FR> predicate, Func<FR, R> operation2, Func<FR, R> operation3)
            => new Operation<T, R>(operation.value, x =>
            {
                var newVal = operation.λ(x);
                var res = newVal.GetOrThrow();
                if (predicate(res))
                {
                    return new Operation<T, FR>(operation.value, x => res)
                            .Then(operation2)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
                }

                return new Operation<T, FR>(operation.value, x => res)
                            .Then(operation3)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
            });

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Operation<T, R> IfThenElse<T, R, FR>(this Operation<T, FR> operation, Predicate<FR> predicate, Func<FR, Task<Result<R>>> operation2, Func<FR, Task<Result<R>>> operation3)
            => new Operation<T, R>(operation.value, x =>
            {
                var newVal = operation.λ(x);
                var res = newVal.GetOrThrow();
                if (predicate(res))
                {
                    return new Operation<T, FR>(operation.value, x => res)
                            .Then(operation2)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
                }

                return new Operation<T, FR>(operation.value, x => res)
                            .Then(operation3)
                            .Finally()
                            .GetAwaiter()
                            .GetResult();
            });

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true
        /// </summary>
        public static Task<Operation<T, FR>> IfThen<T, FR>(this Task<Operation<T, FR>> operation, Predicate<FR> predicate, Func<FR, FR> operation2)
            => operation.Map(o => o.IfThen(predicate, operation2));

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Task<Operation<T, R>> IfThenElse<T, R, FR>(this Task<Operation<T, FR>> operation, Predicate<FR> predicate, Func<FR, R> operation2, Func<FR, R> operation3)
            => operation.Map(o => o.IfThenElse(predicate, operation2, operation3));

        /// <summary>
        /// Apply <paramref name="operation2"/> to <paramref name="operation"/> only if <paramref name="predicate"/> is true, else it will apply <paramref name="operation3"/>
        /// </summary>
        public static Task<Operation<T, R>> IfThenElse<T, R, FR>(this Task<Operation<T, FR>> operation, Predicate<FR> predicate, Func<FR, Task<Result<R>>> operation2, Func<FR, Task<Result<R>>> operation3)
            => operation.Map(o => o.IfThenElse(predicate, operation2, operation3));


    }
}
