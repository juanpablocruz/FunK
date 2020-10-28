using System;
using System.Threading.Tasks;
using Xunit;

namespace FunK.Tests
{
  public class TaskTest
  {
    async Task<string> Fail(string message = null) => await Task.FromException<string>(new Exception(message));
    async Task<T> Succeed<T>(T t) => await Task.FromResult(t);

    [Fact]
    public async void WhenTSucceeds_ThenMapSucceeds()
    {
      var actual = await Succeed("value").Map(String.ToUpper);
      Assert.Equal("VALUE", actual);
    }

    [Fact]
    public async void TaskFailsWithGivenException() =>
      await Assert.ThrowsAsync<Exception>(() => Fail().Map(String.ToUpper));

    [Fact]
    public async void WhenTFails_ThenMapFails() =>
      await Assert.ThrowsAsync<Exception>(() => Fail().Map(String.ToUpper));

    [Fact]
    public void MapThrowsNoException() =>
      Fail().Map(String.ToUpper).Map(String.Trim);

    [Fact]
    public void BindThrowsNoException() =>
      Fail().Bind(_ => Fail());

    [Fact]
    public async void BindSuccess()
    {
      var result = await Succeed("value").Bind(s => Succeed("next value"));
      Assert.Equal("next value", result);
    }

    [Fact]
    public async void WhenTFails_ThenBindFails()
    {
      var task = Fail().Bind(s => Succeed("next value"));
      await Assert.ThrowsAsync<Exception>(async () => await task);
    }

    [Fact]
    public async void WhenFFails_ThenBindFails()
    {
      var task = Succeed("value").Bind(s => Fail());
      await Assert.ThrowsAsync<Exception>(async () => await task);
    }

  }


  public class Task_Apply_test
  {
    async Task<string> Fail(string message = null) => await Task.FromException<string>(new Exception(message));
    async Task<T> Succeed<T>(T t) => await Task.FromResult(t);

    Func<int, int, int> add = (a, b) => a + b;
    Func<int, int, int> multiply = (i, j) => i * j;

    private readonly Func<int, int, int, int> add3Integers =
      (a, b, c) => a + b + c;

    [Fact]
    public async void MapAndApplyArg_ReturnsExpectedContent()
    {
      var result = await Succeed(3)
        .Map(add)
        .Apply(Succeed(4));

      Assert.Equal(7, result);
    }


    [Fact]
    public async void MapAndApplyArg_to_function_requiring_3_arguments()
    {
      var result = await Succeed(1)
        .Map(add3Integers)
        .Apply(Succeed(2))
        .Apply(Succeed(3));

      Assert.Equal(6, result);
    }
  }

}
