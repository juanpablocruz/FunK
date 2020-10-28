using Xunit;

namespace FunK.Tests
{
    using static F;
    public class FTest
    {
        [Fact]
        public void Should_Return_First_Element_OF_Array()
        {
            Assert.Equal(1, Head(List(1, 2)).GetOrElse(0));
        }
        [Fact]
        public void Should_Return_Nothing_When_Enumerable_Is_Empty()
        {
            Assert.Equal(0, Head(List<int>()).GetOrElse(0));
        }


        // Get tests

        class Rol
        {
            public string Name { get; set; }
        }
        class User
        {
            public string Name { get; set; }
            public Rol rol { get; set; }
        }
        class Group
        {
            public User user { get; set; }
        }
        Group group = new Group { user = new User { Name = "user", rol = new Rol { Name = "rol" } } };

        [Fact]
        public void Should_Get_Property_By_Path()
          => Assert.Equal("rol", Get("user.rol.Name", group).Match((ex) => "err", (t) => t));
        [Fact]
        public void Should_Fail_Get_Property_By_Path()
          => Assert.Equal("", Get("user.roles", group).Match((ex) => "", (t) => t));


        [Fact]
        public void Should_Return_Value_When_Property_Not_Exists()
          => Assert.Equal("rol", GetOr("default", "user.rol.Name", group));
        [Fact]
        public void Should_Return_Default_When_Property_Not_Exists()
          => Assert.Equal("default", GetOr("default", "user.roles", group));

        
    }
}
