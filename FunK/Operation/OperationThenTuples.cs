using System;
using System.Threading.Tasks;

namespace FunK
{
    public static class OperationThenTuples
    {
        #region Two values tuple
       
        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<FT, FR, FRR> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(y => y.Match(func)));

        public static Operation<T, FRR> Then<T,FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<FT, FR, Task<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(y => y.Match(func).GetAwaiter().GetResult()));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<FT, FR, FRR> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Map(y => y.Match(func))));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<FT, FR, Task<FRR>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Map(y => y.Match(func).GetAwaiter().GetResult())));



        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<(FT, FR), FRR> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(func));

        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<(FT, FR), Task<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Map(y => func(y).GetAwaiter().GetResult()));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<(FT, FR), FRR> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Map(func)));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<(FT, FR), Task<FRR>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Map(y => func(y).GetAwaiter().GetResult())));



        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<FT, FR, Result<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(y => y.Match(func)));

        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<FT, FR, Task<Result<FRR>>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(y => y.Match(func).GetAwaiter().GetResult()));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<FT, FR, Result<FRR>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(y => y.Match(func))));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<FT, FR, Task<Result<FRR>>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(y => y.Match(func).GetAwaiter().GetResult())));



        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<(FT, FR), Result<FRR>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(func));

        public static Operation<T, FRR> Then<T, FT, FR, FRR>(this Operation<T, (FT, FR)> operation, Func<(FT, FR), Task<Result<FRR>>> func)
            => new Operation<T, FRR>(operation.value, x => operation.λ(x).Bind(y => func(y).GetAwaiter().GetResult()));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<(FT, FR), Result<FRR>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(func)));

        public static Task<Operation<T, FRR>> Then<T, FT, FR, FRR>(this Task<Operation<T, (FT, FR)>> operation, Func<(FT, FR), Task<Result<FRR>>> func)
            => operation.Map(o => new Operation<T, FRR>(o.value, x => o.λ(x).Bind(y => func(y).GetAwaiter().GetResult())));



        #endregion

        #region Three values tuples
        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<T, FT, FR, FRR> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Map(y => y.Match(func)));

        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<T, FT, FR, Task<FRR>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Map(y => y.Match(func).GetAwaiter().GetResult()));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<T, FT, FR, FRR> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Map(y => y.Match(func))));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<T, FT, FR, Task<FRR>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Map(y => y.Match(func).GetAwaiter().GetResult())));



        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<(T, FT, FR), FRR> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Map(func));

        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<(T, FT, FR), Task<FRR>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Map(y => func(y).GetAwaiter().GetResult()));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<(T, FT, FR), FRR> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Map(func)));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<(T, FT, FR), Task<FRR>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Map(y => func(y).GetAwaiter().GetResult())));



        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<T, FT, FR, Result<FRR>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Bind(y => y.Match(func)));

        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<T, FT, FR, Task<Result<FRR>>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Bind(y => y.Match(func).GetAwaiter().GetResult()));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<T, FT, FR, Result<FRR>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Bind(y => y.Match(func))));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<T, FT, FR, Task<Result<FRR>>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Bind(y => y.Match(func).GetAwaiter().GetResult())));



        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<(T, FT, FR), Result<FRR>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Bind(func));

        public static Operation<D, FRR> Then<D, T, FT, FR, FRR>(this Operation<D, (T, FT, FR)> operation, Func<(T, FT, FR), Task<Result<FRR>>> func)
            => new Operation<D, FRR>(operation.value, x => operation.λ(x).Bind(y => func(y).GetAwaiter().GetResult()));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<(T, FT, FR), Result<FRR>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Bind(func)));

        public static Task<Operation<D, FRR>> Then<D, T, FT, FR, FRR>(this Task<Operation<D, (T, FT, FR)>> operation, Func<(T, FT, FR), Task<Result<FRR>>> func)
            => operation.Map(o => new Operation<D, FRR>(o.value, x => o.λ(x).Bind(y => func(y).GetAwaiter().GetResult())));
        #endregion

    }
}
