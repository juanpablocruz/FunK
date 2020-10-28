namespace FunK
{
  using static F;

  public static class Long
  {
    public static Maybe<long> Parse(string s)
    {
      long result;
      return long.TryParse(s, out result)
         ? Just(result) : Nothing;
    }
  }
}