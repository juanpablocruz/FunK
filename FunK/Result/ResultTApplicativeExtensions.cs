using System;
using System.Threading.Tasks;

namespace FunK
{
    using static F;

    public static class ResultTApplicativeExtensions
    {
        /// <summary>
        /// Lifts the <paramref name="result"/> to an <see cref="Operation{T, FR}"/> and then composes 
        /// with <see cref="OperationExtensions.Then{T, FR, FRR}(Operation{T, FR}, Func{FR, FRR})"/>
        /// </summary>
        public static Task<Operation<T, R>> Apply<T, R>(this Task<Result<T>> result, Func<T, R> func)
            => result.Map(Begin).Map(o => o.Then(func));

        /// <summary>
        /// Lifts the <paramref name="result"/> to an <see cref="Operation{T, FR}"/> and then composes 
        /// with <see cref="OperationExtensions.Then{T, FR, FRR}(Operation{T, FR}, Func{FR, Task{FRR}})"/>
        /// </summary>
        public static Task<Operation<T, R>> Apply<T, R>(this Task<Result<T>> result, Func<T, Task<R>> func)
            => result.Map(Begin).Map(o => o.Then(func));

        /// <summary>
        /// Lifts the <paramref name="result"/> to an <see cref="Operation{T, FR}"/> and then composes 
        /// with <see cref="OperationExtensions.Then{T, FR, FRR}(Operation{T, FR}, Func{FR, FRR})"/>
        /// </summary>
        public static Operation<T, R> Apply<T, R>(this Result<T> result, Func<T, R> func)
            => Begin(result).Then(func);

        /// <summary>
        /// Lifts the <paramref name="result"/> to an <see cref="Operation{T, FR}"/> and then composes 
        /// with <see cref="OperationExtensions.Then{T, FR, FRR}(Operation{T, FR}, Func{FR, Task{FRR}})"/>
        /// </summary>
        public static Operation<T, R> Apply<T, R>(this Result<T> result, Func<T, Task<R>> func)
            => Begin(result).Then(func);





    }
}
