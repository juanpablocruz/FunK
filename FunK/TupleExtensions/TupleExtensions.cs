using System;

namespace FunK
{
    public static class TupleExtensions
    {
        public static R Match<T1, T2, R>(this Tuple<T1, T2> @this
            , Func<T1, T2, R> func) => func(@this.Item1, @this.Item2);

        public static R Match<T1, T2, T3, R>(this Tuple<T1, T2, T3> @this
            , Func<T1, T2, T3, R> func) => func(@this.Item1, @this.Item2, @this.Item3);

        public static R Match<T1, T2, T3, T4, R>(this Tuple<T1, T2, T3, T4> @this
            , Func<T1, T2, T3, T4, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4);

        public static R Match<T1, T2, T3, T4, T5, R>(this Tuple<T1, T2, T3, T4, T5> @this
            , Func<T1, T2, T3, T4, T5, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5);

        public static R Match<T1, T2, T3, T4, T5, T6, R>(this Tuple<T1, T2, T3, T4, T5, T6> @this
            , Func<T1, T2, T3, T4, T5, T6, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5, @this.Item6);


        public static R Match<T1, T2, R>(this ValueTuple<T1, T2> @this
            , Func<T1, T2, R> func) => func(@this.Item1, @this.Item2);

        public static R Match<T1, T2, T3, R>(this ValueTuple<T1, T2, T3> @this
            , Func<T1, T2, T3, R> func) => func(@this.Item1, @this.Item2, @this.Item3);

        public static R Match<T1, T2, T3, T4, R>(this ValueTuple<T1, T2, T3, T4> @this
            , Func<T1, T2, T3, T4, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4);
        
        public static R Match<T1, T2, T3, T4, T5, R>(this ValueTuple<T1, T2, T3, T4, T5> @this
            , Func<T1, T2, T3, T4, T5, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5);

        public static R Match<T1, T2, T3, T4, T5, T6, R>(this ValueTuple<T1, T2, T3, T4, T5, T6> @this
            , Func<T1, T2, T3, T4, T5, T6, R> func) => func(@this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5, @this.Item6);
    }
}
