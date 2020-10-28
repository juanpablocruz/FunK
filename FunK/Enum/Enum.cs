namespace FunK
{
  using static F;
  public static class Enum
  {
    public static Maybe<T> Parse<T>(this string s) where T : struct
      => System.Enum.TryParse(s, out T t) ? Just(t) : Nothing;
  }
}
