using System;
using Xunit;

namespace FunK.Tests
{
    using static Console;
    public class ReaderTests
    {
        public static Reader<string, string> FirstThing()
            => from name in Reader.Ask<string>()
               select $"First, {name} looked up";

        public static Reader<string, string> SecondThing()
            => from name in Reader.Ask<string>()
               select $"Then, {name} turned to me...";

        [Fact]
        public void test()
        {
            var reader = from first in FirstThing()
                         from second in SecondThing()
                         select first + "\n" + second;

            var story = reader("Tamerlano");

            var expected = "First, Tamerlano looked up\nThen, Tamerlano turned to me...";
            Assert.Equal(expected, story);
        }
    }
}

