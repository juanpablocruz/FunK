using Xunit;
using Unit = System.ValueTuple;

namespace FunK.Tests
{
  public class UnitTest
  {
    [Fact]
    public void ThereCanOnlyBeOneUnit()
    {
      Assert.Equal(new Unit(), F.Unit());
    }
  }
}
