using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunK
{
    public static class ResultTFunctorExtensions
    {
        /// <summary>
        /// Apply <paramref name="func"/> to <paramref name="this"/> if <see cref="Result{T}.IsSuccess"/>.
        /// </summary>
        public static Result<R> Map<T, R>(this Result<T> @this, Func<T, R> func)
            => @this.IsSuccess ? func(@this._Value) : new Result<R>(@this._Error);

        /// <summary>
        /// Apply synchronously <paramref name="func"/> to <paramref name="this"/> if <see cref="Result{T}.IsSuccess"/>.
        /// </summary>
        public static Result<R> MapSync<T, R, E>(this Result<T> @this, Func<T, Task<R>> func)
            => @this.IsSuccess ? func(@this._Value).GetAwaiter().GetResult() : new Result<R>(@this._Error);



        public static Result<IEnumerable<R>> Map<T, R>(this Result<IEnumerable<T>> result, Func<T, R> func)
            => result.Map(list => list.MapToList(func));

        public static Task<Result<IEnumerable<R>>> Map<T, R>(this Task<Result<IEnumerable<T>>> task, Func<T, R> func)
            => task.Map(result => result.Map(list => list.MapToList(func)));

        public static Result<List<R>> Map<T, R>(this Result<List<T>> result, Func<T, R> func)
            => result.Map(list => list.MapToList(func));

        public static Task<Result<List<R>>> Map<T, R>(this Task<Result<List<T>>> task, Func<T, R> func)
            => task.Map(result => result.Map(list => list.MapToList(func)));


        public static Result<IEnumerable<R>> Map<T, R>(this Result<IEnumerable<T>> result, Func<T, Task<R>> func)
            => result.Map(list => list.MapToList(func));

        public static Task<Result<IEnumerable<R>>> Map<T, R>(this Task<Result<IEnumerable<T>>> task, Func<T, Task<R>> func)
            => task.Map(result => result.Map(list => list.MapToList(func)));

        public static Result<List<R>> Map<T, R>(this Result<List<T>> result, Func<T, Task<R>> func)
            => result.Map(list => list.MapToList(func));

        public static Task<Result<List<R>>> Map<T, R>(this Task<Result<List<T>>> task, Func<T, Task<R>> func)
            => task.Map(result => result.Map(list => list.MapToList(func)));









        #region MapAsync
        public static Result<IEnumerable<R>> MapAsync<T, R>(this Result<IEnumerable<T>> result, Func<T, R> func)
            => result.Map(list => list.AsParallel().Select(func).AsEnumerable());

        public static Task<Result<IEnumerable<R>>> MapAsync<T, R>(this Task<Result<IEnumerable<T>>> task, Func<T, R> func)
            => task.Map(result => result.Map(list => list.AsParallel().Select(func).AsEnumerable()));

        public static Result<List<R>> MapAsync<T, R>(this Result<List<T>> result, Func<T, R> func)
            => result.Map(list => list.AsParallel().Select(func).AsEnumerable().ToList());

        public static Task<Result<List<R>>> MapAsync<T, R>(this Task<Result<List<T>>> task, Func<T, R> func)
            => task.Map(result => result.Map(list => list.AsParallel().Select(func).AsEnumerable().ToList()));


        public static Result<IEnumerable<R>> MapAsync<T, R>(this Result<IEnumerable<T>> result, Func<T, Task<R>> func)
            => result.Map(list => list.AsParallel().Select(e => func(e).GetAwaiter().GetResult()).AsEnumerable());

        public static Task<Result<IEnumerable<R>>> MapAsync<T, R>(this Task<Result<IEnumerable<T>>> task, Func<T, Task<R>> func)
            => task.Map(result => result.Map(list => list.AsParallel().Select(e => func(e).GetAwaiter().GetResult()).AsEnumerable()));

        public static Result<List<R>> MapAsync<T, R>(this Result<List<T>> result, Func<T, Task<R>> func)
            => result.Map(list => list.AsParallel().Select(e => func(e).GetAwaiter().GetResult()).AsEnumerable().ToList());

        public static Task<Result<List<R>>> MapAsync<T, R>(this Task<Result<List<T>>> task, Func<T, Task<R>> func)
            => task.Map(result => result.Map(list => list.AsParallel().Select(e => func(e).GetAwaiter().GetResult()).AsEnumerable().ToList()));
        #endregion






        public static T WaitTask<T>(Task<T> task) => task.WaitResult();



        public static List<R> MapToList<T, R>(this List<T> list, Func<T, R> func)
            => list.Map(func).ToList();
        
        public static IEnumerable<R> MapToList<T, R>(this IEnumerable<T> list, Func<T, R> func)
            => list.Map(func);


        public static List<R> MapToList<T, R>(this List<T> list, Func<T, Task<R>> func)
            => list.Map(func).Map(WaitTask).ToList();

        public static IEnumerable<R> MapToList<T, R>(this IEnumerable<T> list, Func<T, Task<R>> func)
            => list.Map(func).Map(WaitTask);
    }
}
