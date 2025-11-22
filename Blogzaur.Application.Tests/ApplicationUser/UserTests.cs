using Xunit;
using Blogzaur.Application.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Blogzaur.Application.ApplicationUser.Tests
{
    public class UserTests
    {
        [Fact()]
        public void IsInRole_WithMatchingRole_ShouldReturnTrue()
        {
            //arrange
            var user = new User("1", "testuser", "test@test@com", new List<string> { "Admin", "RegularUser" });

            //act
            var isInRole = user.IsInRole("Admin");

            //assert
            isInRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRole_WithNonMatchingRole_ShouldReturnFalse()
        {
            //arrange
            var user = new User("1", "testuser", "test@test@com", new List<string> { "Admin", "RegularUser" });

            //act
            var isInRole = user.IsInRole("Moderator");

            //assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRole_WithNonMatchingCaseRole_ShouldReturnFalse()
        {
            //arrange
            var user = new User("1", "testuser", "test@test@com", new List<string> { "Admin", "RegularUser" });

            //act
            var isInRole = user.IsInRole("admin");

            //assert
            isInRole.Should().BeFalse();
        }
    }
}