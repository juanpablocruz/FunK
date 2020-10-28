using System;
using System.Threading.Tasks;

namespace FunK
{
  using static F;

  public static class TaskMaybeMonad
  {
    public static Task<Maybe<T>> OrElse<T>
      (this Task<Maybe<T>> task, Func<Task<Maybe<T>>> fallback)
      => task.ContinueWith(t =>
        t.Status == TaskStatus.Faulted
        ? fallback()
        : t.Result.Match(
          Nothing: fallback,
          Just: val => Async(t.Result)))
      .Unwrap();

    public static Task<Maybe<U>> Select<T, U>
         (this Task<Maybe<T>> self
         , Func<T, U> mapper)
         => self.Map(x => x.Map(mapper));

    public static Task<Maybe<R>> SelectMany<T, R>
       (this Task<Maybe<T>> task       // Task<Maybe<T>> 
       , Func<T, Task<Maybe<R>>> bind) // -> (T -> Task<Maybe<R>>)
       => task.Bind(vt => vt.TraverseBind(bind));
    //=> task.Bind(vt => vt.Traverse(bind).Map(vvr => vvr.Bind(vr => vr)));

    public static Task<Maybe<RR>> SelectMany<T, R, RR>
       (this Task<Maybe<T>> task       // Task<Maybe<T>> 
       , Func<T, Task<Maybe<R>>> bind  // -> (T -> Task<Maybe<R>>)
       , Func<T, R, RR> project)
       => task
          .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
          .Unwrap();
  }
}
