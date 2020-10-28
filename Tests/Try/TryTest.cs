using Xunit;
using System;
using static FunK.F;
using Newtonsoft.Json.Linq;

namespace FunK.Tests
{
    using static TestUtils;
    public class TryTest
    {
        Try<Uri> CreateUri(string uri) => () => new Uri(uri);
        Try<JObject> Parse(string s) => () => JObject.Parse(s);

        Try<Uri> ExtractUri(string json) =>
           from jObj in Parse(json)
           let uriStr = (string)jObj["Uri"]
           from uri in Try(() => new Uri(uriStr))
           select uri;


        [Fact]
        public void SuccessfulTry()
        {
            var uriTry = CreateUri("http://github.com");

            uriTry.Run().Match(
              Success: uri => Assert.NotNull(uri),
              Exception: ex => Fail());
        }

        [Fact]
        public void FailingTry()
        {
            var uriTry = CreateUri("rubbish");

            uriTry.Run().Match(
              Success: uri => Fail(),
              Exception: ex => Assert.NotNull(ex));
        }

        [Fact]
        public void ItIsLazy()
        {
            bool tried = false;

            Func<string, Try<Uri>> createUri = (uri) => Try(() =>
            {
                tried = true;
                return new Uri(uri);
            });

            var uriTry = createUri("http://github.com");
            Assert.False(tried, "creating a Try should not run it");

            var schemeTry = uriTry.Map(uri => uri.Scheme);
            Assert.False(tried, "mapping onto a try should not run it");

            uriTry.Run().Match(
              Success: uri => Assert.NotNull(uri),
              Exception: ex => Assert.True(false, "should have suceeded")
            );

            Assert.True(tried, "matching should run the Try");
        }

        [Theory]
        [InlineData(@"{'Uri': 'http://github.com'}", "Ok")]
        [InlineData("{'Uri': 'rubbish'}", "Invalid URI")]
        [InlineData("{}", "Value cannot be null")]
        [InlineData("blah!", "Unexpected character encountered")]
        public void SuccessExtractUri(string json, string expected)
         => Assert.StartsWith(expected, ExtractUri(json)
               .Run()
               .Match(
                  ex => ex.Message,
                  _ => "Ok"));
    }
}
