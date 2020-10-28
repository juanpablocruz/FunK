using System;
using System.Threading.Tasks;

namespace FunK
{
    using static F;
    public static class OperationFinally
    {

        private static Result<FRR> ProcessOperation<FRR>(Func<Result<FRR>> action)
            => Try(() => action()).Run().Match(ex => new Result<FRR>(ex), v => v);

        private static Task<Result<FRR>> AsyncProcessOperation<FRR>(Func<Result<FRR>> action)
            => Async(ProcessOperation(action));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, FRR> func)
            => AsyncProcessOperation(() => Identity(operation.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))());


        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Task<FRR>> func)
            => AsyncProcessOperation(() => Identity(operation.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))());

        /// <summary>
        /// Asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FR>> Finally<T, FR>(this Operation<T, FR> operation)
            => operation.Finally(x => x);

        /// <summary>
        /// Asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FR>> Finally<T, FR>(this Task<Operation<T, FR>> operation)
            => operation.Finally(x => x);


        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, FRR> func)
            => operation.Map(o => ProcessOperation(() => Identity(o.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))()));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Task<FRR>> func)
            => operation.Map(o => ProcessOperation(() => Identity(o.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))()));




        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Result<FRR>> func)
            => AsyncProcessOperation<FRR>(() => Identity(operation.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))());

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Task<Result<FRR>>> func)
            => AsyncProcessOperation<FRR>(() => Identity(operation.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))());


        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Result<FRR>> func)
            => operation.Map(o => ProcessOperation<FRR>(() => Identity(o.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))()));

        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="operation"/> and asynchronously evaluate the <see cref="Operation{T, FR}"/>
        /// </summary>
        public static Task<Result<FRR>> Finally<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Task<Result<FRR>>> func)
            => operation.Map(o => ProcessOperation<FRR>(() => Identity(o.Then(func)).Map(op => op.value.Bind(x => op.λ(x)))()));

    }
}
