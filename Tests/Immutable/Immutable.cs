using System.Collections.Generic;
using Xunit;
namespace FunK.Tests
{
    using static Assert;

    class B
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    class A
    {
        public int B { get; }
        public string C { get; }
        public B Obj { get; set; }
        public List<string> BasicList { get; set; }
        public List<B> ComplexList { get; set; }
        public A(int b, string c) { B = b; C = c; BasicList = new List<string>(); ComplexList = new List<B>(); Obj = new B() { Id = 7, Name = "Obj" }; }
    }

    public class Immutable_With_PropertyName
    {
        A original = new A(123, "hello");

        [Fact]
        public void TestCopy()
        {
            var a = new A(2, "a")
            {
                BasicList = new List<string> { "test1", "testc" },
                ComplexList = new List<B>{ new B() { Id = 1, Name = "B1" }, new B() { Id = 2, Name = "B2" } }
            };

            var copied = a.With("B", 4);
            var copiedComples = a.With("ComplexList", new List<B>(){ new B() { Id=4, Name="T" } });
            var modifiyObj = copiedComples.With("Obj", new B() { Id = 19, Name = "modified" });
            
            Assert.NotNull(copied);
        }


        [Fact]
        public void ItChangesTheDesiredProperty()
        {
            var @new = original.With("B", 777);

            Assert.Equal(777, @new.B);
            Assert.Equal("hello", @new.C);
        }

        [Fact]
        public void ItDoesNotChangeTheOriginal()
        {
            var @new = original.With("B", 777);

            Assert.Equal(123, original.B);
            Assert.Equal("hello", original.C);
        }
    }

    public class Immutable_With_PropertyExpression
    {
        A original = new A(123, "hello");

        [Fact]
        public void ItChangesTheDesiredProperty()
        {
            var @new = original.With(a => a.C, "hi");

            Assert.Equal(123, original.B);
            Assert.Equal("hello", original.C);

            Assert.Equal(123, @new.B);
            Assert.Equal("hi", @new.C);
        }

        [Fact]
        public void YouCanChainWith()
        {
            var @new = original
              .With(a => a.B, 345)
              .With(a => a.C, "howdy");

            Assert.Equal(345, @new.B);
            Assert.Equal("howdy", @new.C);
        }
    }
}
