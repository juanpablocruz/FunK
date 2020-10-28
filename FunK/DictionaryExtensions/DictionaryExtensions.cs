using System;
using System.Collections.Generic;
using System.Text;

namespace FunK
{
  using static F;
  public static class DictionaryExtensions
  {
    public static Maybe<T> Lookup<K, T>(this IDictionary<K,T> dict, K key)
    {
      T value;
      return dict.TryGetValue(key, out value) ? Just(value) : Nothing;
    }
  }
}
