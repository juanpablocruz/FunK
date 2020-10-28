using System;
using System.Threading.Tasks;

namespace FunK
{
    public static class ResultTaskExtensions
    {
        public static Task<Result<R>> MapT<T, R>(this Task<Result<T>> result, Func<T, R> f)
            => result.Map(r => r.Map(f));

        public static Task<Result<R>> BindT<T, R>(this Task<Result<T>> result, Func<T, Task<Result<R>>> f)
            => result.Map(r => r.Bind(o => f(o).GetAwaiter().GetResult()));

        public static Task<Result<R>> BindT<T, R>(this Task<Result<T>> result, Func<T, Result<R>> f)
            => result.Map(r => r.Bind(f));
    }
}
