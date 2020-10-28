using System;
using Xunit;

namespace FunK.Tests
{
  public static class TestUtils
  {
    public static void Fail() => Assert.Equal(1, 2);

    public static T Tap<T>(T data)
    {
      Console.WriteLine(data);
      return data;
    }
  }
}
