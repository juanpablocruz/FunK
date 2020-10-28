using System;
using System.Threading.Tasks;

namespace FunK
{
  using static F;
  public static class MaybeTraversable
  {
    // Exceptional
    public static Exceptional<Maybe<R>> Traverse<T, R>
       (this Maybe<T> tr, Func<T, Exceptional<R>> f)
       => tr.Match(
             Nothing: () => Exceptional((Maybe<R>)Nothing),
             Just: t => f(t).Map(Just)
          );

    // Task
    public static Task<Maybe<R>> Traverse<T, R>
       (this Maybe<T> @this, Func<T, Task<R>> func)
       => @this.Match(
             Nothing: () => Async((Maybe<R>)Nothing),
             Just: t => func(t).Map(Just)
          );

    public static Task<Maybe<R>> TraverseBind<T, R>(this Maybe<T> @this
       , Func<T, Task<Maybe<R>>> func)
       => @this.Match(
             Nothing: () => Async((Maybe<R>)Nothing),
             Just: t => func(t)
          );
  }
}
