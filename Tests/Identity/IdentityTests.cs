using Moq;
using System;
using Xunit;
using Unit = System.ValueTuple;

namespace FunK.Tests
{
    using static F;

    public class IdentityTests
    {
        [Fact]
        public void Identity_Should_Compose()
        {
            bool hasPrinted = false;
            string value = "Anton";
            Func<string, string> toUpper = s => s.ToUpper();
            Func<string, Unit> print = s => { hasPrinted=true; return Unit(); };

            Identity(value)
               .Map(toUpper)
               .Map(print)();

            Assert.True(hasPrinted);
        }

        [Fact]
        public void Tap_Should_Execute_Action()
        {
            var action = new Mock<Action>();
            Identity(2).Tap(action.Object);
            action.Verify(_ => _.Invoke(), Times.Once);
        }

        [Fact]
        public void Tap_Should_Execute_Func()
        {
            var fn = new Mock<Func<int, string>>();
            fn.Setup(_ => _(It.IsAny<int>())).Returns((int i) => i.ToString());
            Identity(2).Tap(fn.Object);
            fn.Verify(_ => _.Invoke(2), Times.Once);
        }

        [Fact]
        public void Tap_Should_Execute_Func_Of_Identity()
        {
            var fn = new Mock<Func<Identity<int>, string>>();
            fn.Setup(_ => _(It.IsAny<Identity<int>>())).Returns((Identity<int> i) => i().ToString());
            var obj = Identity(2);
            obj.Tap(fn.Object);
            fn.Verify(_ => _.Invoke(obj), Times.Once);
        }

        [Fact]
        public void Tap_Should_Return_Same_Identity()
        {
            var fn = new Mock<Func<int, string>>();
            fn.Setup(_ => _(It.IsAny<int>())).Returns((int i) => i.ToString());
            var obj1 = Identity(2);
            var obj2 = obj1.Tap(fn.Object);
            fn.Verify(_ => _.Invoke(2), Times.Once);

            Assert.IsType<Identity<int>>(obj2);
            Assert.Equal(obj1(), obj2());
        }

    }
}
