using System;

namespace FunK
{
    public static partial class F
    {
        public static Func<T> Func<T>(Func<T> f) => f;

        /// <summary>
        /// Lifts a value to a monad wrapper of that value, allowing it to compose.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="value">Value</param>
        /// <returns>Identity<T> with the value</returns>
        public static Identity<T> Identity<T>(T value) => () => value;
    }

    // () -> T (aka. Identity)
    public static class FuncTExt
    {
        public static Func<R> Map<T, R>
          (this Func<T> f, Func<T, R> g)
          => () => g(f());
        public static Func<R> Bind<T, R>
          (this Func<T> f, Func<T, Func<R>> g)
          => () => g(f())();

        // LINQ

        public static Func<R> Select<T, R>(this Func<T> @this
           , Func<T, R> func) => @this.Map(func);

        public static Func<P> SelectMany<T, R, P>(this Func<T> @this
           , Func<T, Func<R>> bind, Func<T, R, P> project)
           => () =>
           {
               T t = @this();
               R r = bind(t)();
               return project(t, r);
           };
    }


    public delegate T Identity<T>();

    /// <summary>
    /// Monadic type for composing with values.
    /// </summary>
    public static class Identity
    {
        public static Identity<R> Map<T, R>(this Identity<T> @this,
          Func<T, R> func) => () => func(@this());

        public static Identity<R> Bind<T, R>(this Identity<T> @this,
          Func<T, Identity<R>> func) => () => func(@this())();

        public static Identity<T> Tap<T>(this Identity<T> @this, Action action)
        {
            action();
            return @this;
        }

        public static Identity<T> Tap<T, R>(this Identity<T> @this, Func<T, R> func)
        {
            func(@this());
            return @this;
        }

        public static Identity<T> Tap<T, R>(this Identity<T> @this, Func<Identity<T>, R> func)
        {
            func(@this);
            return @this;
        }
    }
}
