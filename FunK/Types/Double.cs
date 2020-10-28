namespace FunK
{
  using static F;
  public static class Double
  {
    public static Maybe<double> Parse(string s)
    {
      double result;
      return double.TryParse(s, out result)
        ? Just(result) : Nothing;
    }
  }
}
