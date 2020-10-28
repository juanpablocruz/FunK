using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunK
{
    using static F;
    public static class OperationFinallyForEach
    {
        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<IEnumerable<FRR>>> FinallyForEach<T, FR, FRR>(this Operation<T, IEnumerable<FR>> operation, Func<FR, FRR> func)
            => Async(
                Try(() => Identity(operation.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<IEnumerable<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<IEnumerable<FRR>>> FinallyForEach<T, FR, FRR>(this Operation<T, IEnumerable<FR>> operation, Func<FR, Task<FRR>> func)
            => Async(
                Try(() => Identity(operation.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<IEnumerable<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<List<FRR>>> FinallyForEach<T, FR, FRR>(this Operation<T, List<FR>> operation, Func<FR, FRR> func)
            => Async(
                Try(() => Identity(operation.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<List<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<List<FRR>>> FinallyForEach<T, FR, FRR>(this Operation<T, List<FR>> operation, Func<FR, Task<FRR>> func)
            => Async(
                Try(() => Identity(operation.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<List<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<IEnumerable<FRR>>> FinallyForEach<T, FR, FRR>(this Task<Operation<T, IEnumerable<FR>>> operation, Func<FR, FRR> func)
            => operation.Map(o =>
                Try(() => Identity(o.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<IEnumerable<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<IEnumerable<FRR>>> FinallyForEach<T, FR, FRR>(this Task<Operation<T, IEnumerable<FR>>> operation, Func<FR, Task<FRR>> func)
            => operation.Map(o =>
                Try(() => Identity(o.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<IEnumerable<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<List<FRR>>> FinallyForEach<T, FR, FRR>(this Task<Operation<T, List<FR>>> operation, Func<FR, FRR> func)
            => operation.Map(o =>
                Try(() => Identity(o.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<List<FRR>>(ex),
                    Success: v => v
                ));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> for each element in the array and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<List<FRR>>> FinallyForEach<T, FR, FRR>(this Task<Operation<T, List<FR>>> operation, Func<FR, Task<FRR>> func)
            => operation.Map(o =>
                Try(() => Identity(o.ThenForEach(func)).Map(op => op.value.Bind(x => op.λ(x)))())
                .Run()
                .Match(
                    Exception: ex => new Result<List<FRR>>(ex),
                    Success: v => v
                ));
    }
}
