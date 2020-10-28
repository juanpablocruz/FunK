using System;
using System.Linq;
using Xunit;
using static FunK.F;

namespace FunK.Tests
{
    public class ValidTest
    {
        Validation<int> Invalid(string m = "Some error") => Error(m);

        Func<int, int, int> add = (a, b) => a + b;
        Func<int, int, int> multiply = (i, j) => i * j;

        private readonly Func<int, int, int, int> add3Integers =
          (a, b, c) => a + b + c;

        Func<string, Validation<int>> parseInt =>
          s => Int.Parse(s).Match(
            Nothing: () => Error($"{s} is not an int"),
            Just: (i) => Valid(i));

        // test that errors are accumulated
        [Fact]
        public void ItTracksErrors() => Assert.Equal(
          actual: Valid(add)
          .Apply(parseInt("4"))
          .Apply(parseInt("x")),
          expected: Invalid("x is not an int"));

        [Fact]
        public void ItAccumulatesErrors() => Assert.Equal(
          actual: Valid(add)
          .Apply(parseInt("y"))
          .Apply(parseInt("x"))
          .Errors.Count(),
          expected: 2);

        [Fact]
        public void TraversableA_HappyPath() => Assert.Equal(
          actual: Range(1, 4).Map(i => i.ToString())
          .Traverse(parseInt)
          .Map(list => list.Sum()),
          expected: Valid(10));

        [Fact]
        public void TraversableA_UnhappyPath() => Assert.Equal(
          actual: List("1", "2", "rubbish", "4", "more rubbish")
          .Traverse(parseInt)
          .Map(list => list.Sum())
          .Errors.Count(),
          expected: 2);

        // standard applicative tests

        [Fact]
        public void MapAndApplySomeArg_ReturnsJust() => Assert.Equal(
          actual: Valid(3)
          .Map(multiply)
          .Apply(Valid(4)),
          expected: Valid(12));

        [Fact]
        public void MapAndApplyNothingArg_ReturnsNothing()
        {
            var maybe = Valid(3)
              .Map(multiply)
              .Apply(Invalid());
            var maybe2 = (Invalid())
              .Map(multiply)
              .Apply(Valid(4));

            Assert.Equal(Invalid(), maybe);
            Assert.Equal(Invalid(), maybe2);
        }

        [Fact]
        public void ApplyJustArgs()
        {
            var maybe = Valid(add)
              .Apply(Valid(3))
              .Apply(Valid(4));

            Assert.Equal(Valid(7), maybe);
        }

        [Fact]
        public void ApplyJustARgs_to_function_requiring_3_args()
        {
            var maybe = Valid(add3Integers)
              .Apply(Valid(1))
              .Apply(Valid(2))
              .Apply(Valid(3));
            Assert.Equal(Valid(6), maybe);
        }

        [Fact]
        public void ApplyNothingArgs()
        {
            var maybe = Valid(add)
              .Apply(Invalid())
              .Apply(Valid(4));

            Assert.Equal(Invalid(), maybe);
        }

        [Fact]
        public void ApplicativeLawHolds()
        {
            var first = Valid(add)
              .Apply(Valid(3))
              .Apply(Valid(4));

            var second = Valid(3)
              .Map(add)
              .Apply(Valid(4));

            Assert.Equal(first, second);
        }

        [Fact]
        public void Aggregate_Valid_And_Invalid_Yields_Invalid()
        {
            var first = Valid(1);
            var second = Invalid("error");
            var aggregate = first.Aggregate(second);
            Assert.Single(aggregate.Errors);
        }
        [Fact]
        public void Aggregate_Valid_And_Valid_Yields_Valid()
        {
            var first = Valid(1);
            var second = Valid("ok");
            var aggregate = first.Aggregate(second);
            Assert.Empty(aggregate.Errors);
            Assert.Equal("ok", aggregate.GetOrElse("error"));
        }
        [Fact]
        public void Aggregate_Invalid_And_Invalid_Yields_Invalid_With_Two_Errors()
        {
            var first = Invalid("error1");
            var second = Invalid("error2");
            var aggregate = first.Aggregate(second);
            Assert.Equal(2,aggregate.Errors.Count());
        }
    }
}
