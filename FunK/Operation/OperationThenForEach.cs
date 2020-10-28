using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunK
{
    using static F;
    public static class OperationThenForEach
    {

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/> to each element in the array.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, IEnumerable<FRR>> ThenForEach<T, FR, FRR>(this Operation<T, IEnumerable<FR>> operation, Func<FR, FRR> func)
            => new Operation<T, IEnumerable<FRR>>(operation.value, x => operation.λ(x).Map(func));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/> to each element in the array.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, IEnumerable<FRR>> ThenForEach<T, FR, FRR>(this Operation<T, IEnumerable<FR>> operation, Func<FR, Task<FRR>> func)
            => new Operation<T, IEnumerable<FRR>>(operation.value, x => operation.λ(x).Map(func));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/> to each element in the array.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, List<FRR>> ThenForEach<T, FR, FRR>(this Operation<T, List<FR>> operation, Func<FR, FRR> func)
            => new Operation<T, List<FRR>>(operation.value, x => operation.λ(x).Map(func));

        /// <summary>
        /// Apply the <paramref name="func"/> to the set of λ from <paramref name="operation"/> to each element in the array.<br/>
        /// Uses Lazy evaluation, hence it will execute once <see cref="OperationFinally.Finally{T, FR}(Operation{T, FR})"/> gets called.
        /// </summary>
        public static Operation<T, List<FRR>> ThenForEach<T, FR, FRR>(this Operation<T, List<FR>> operation, Func<FR, Task<FRR>> func)
            => new Operation<T, List<FRR>>(operation.value, x => operation.λ(x).Map(func));
    }
}
