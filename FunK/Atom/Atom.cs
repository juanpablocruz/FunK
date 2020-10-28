using System;
using System.Threading;

namespace FunK
{
  public sealed class Atom<T>
    where T : class
  {
    private volatile T value;
    public T Value => value;

    public Atom(T value)
    {
      this.value = value;
    }

    public T Swap(Func<T, T> update)
    {
      T original, updated;
      original = value;
      updated = update(original);

      if (original != Interlocked.CompareExchange(ref value, updated, original))
      {
        var spinner = new SpinWait();
        do
        {
          spinner.SpinOnce();
          original = value;
          updated = update(original);
        }
        while (original != Interlocked.CompareExchange(ref value, updated, original));
      }
      return updated;
    }

    T Swap_SimplerButLessEfficient(Func<T, T> update)
    {
      T original, updated;
      do
      {
        original = value;
        updated = update(original);
      } while (original != Interlocked.CompareExchange(ref value, updated, original));
      return updated;
    }

  }
}
