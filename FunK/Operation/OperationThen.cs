using System;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace FunK
{
    public static class OperationThen
    {
        /// <summary>
        /// Apply the <paramref name="action"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, Unit> Then<T, FR>(this Operation<T, FR> operation, Action<FR> action)
            => new Operation<T, Unit>(operation.value, x => operation.λ(x).Map(y => action.ToFunc()(y)));

        /// <summary>
        /// Apply the <paramref name="action"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, FR> Then<T, FR>(this Operation<T, FR> operation, Func<FR, Task> action)
            => new Operation<T, FR>(operation.value, x => operation.λ(x).Map(y => action(y).ToFuncTask()).Map(t => t.Map(_ => (FR)x).GetAwaiter().GetResult()));

        /// <summary>
        /// Apply the <paramref name="action"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, FR>> Then<T, FR>(this Task<Operation<T, FR>> operation, Func<FR, Task> action)
            => operation.Map( o => new Operation<T, FR>(o.value, x => o.λ(x).Map(y => action(y).ToFuncTask().Map(_ => (FR)x).GetAwaiter().GetResult())));


        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, FRR> Then<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, FRR> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(func));

        /// <summary>
        /// Apply synchronously <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, FRR> Then<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Task<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(y => func(y).GetAwaiter().GetResult()));

        /// <summary>
        /// Apply the <paramref name="action"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, Unit>> Then<T, FR>(this Task<Operation<T, FR>> operation, Action<FR> action)
            => operation.Map(o => Then(o, action));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, FRR>> Then<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, FRR> func)
            => operation.Map(o => Then(o, func));

        /// <summary>
        /// Apply synchronously <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, FRR>> Then<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Task<FRR>> func)
            => operation.Map(o => Then(o, func));


        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, FRR> Then<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Result<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(func));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, FRR>> Then<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Result<FRR>> func)
            => operation.Map( o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(func)));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, FRR> Then<T, FR, FRR>(this Operation<T, FR> operation, Func<FR, Task<Result<FRR>>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(y => func(y).GetAwaiter().GetResult()));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/>.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Task<Operation<T, FRR>> Then<T, FR, FRR>(this Task<Operation<T, FR>> operation, Func<FR, Task<Result<FRR>>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(y => func(y).GetAwaiter().GetResult())));


        
    }
}
