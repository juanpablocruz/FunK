using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static FunK.F;

namespace FunK.Tests
{
  public class EnumerableExtensionsTest
  {
    [Property]
    public void Find_AnyInNonEmptyStrings_First(NonEmptyArray<NonNull<string>> list)
        => Assert.Equal(Just(list.Get[0].Get), list.Get.Map(x => x.Get).Find(_ => true));

    [Property]
    public void Find_AnyInNonEmptyDecimals_First(NonEmptyArray<decimal> list)
      => Assert.Equal(Just(list.Get[0]), list.Get.Find(_ => true));

    [Fact]
    public void Find_EmptyStrings_Nothing()
      => Assert.Equal(Nothing, Enumerable.Empty<string>().Find(_ => true));

    [Fact]
    public void Find_EmptyDecimals_Nothing()
      => Assert.Equal(Nothing, Enumerable.Empty<decimal>().Find(_ => true));
  }
}
