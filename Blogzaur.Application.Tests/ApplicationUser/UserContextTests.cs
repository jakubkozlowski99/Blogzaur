using Xunit;
using Blogzaur.Application.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using FluentAssertions;

namespace Blogzaur.Application.ApplicationUser.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            //arrange - httpContextAccessor mock
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "RegularUser")
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            //arrange - UserManager mock
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);


            var userContext = new UserContext(httpContextAccessorMock.Object, userManagerMock.Object);

            //act

            var currentUser = userContext.GetCurrentUser();

            //arrange
            currentUser.Should().NotBeNull();
            currentUser!.Id.Should().Be("1");
            currentUser.Username.Should().Be("testuser");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().Contain(new List<string> { "Admin", "RegularUser" });
        }

        [Fact]
        public void GetUserById_WhenUserExists_ShouldReturnUserFromUserManager()
        {
            // arrange - create a fake IdentityUser
            var identityUser = new IdentityUser { Id = "1", UserName = "testuser", Email = "test@test.com" };

            // mock IUserStore and UserManager
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(identityUser);
            userManagerMock.Setup(x => x.GetRolesAsync(identityUser)).ReturnsAsync(new List<string> { "Admin", "RegularUser" });

            // httpContextAccessor not used by GetUserById but constructor requires it
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            var userContext = new UserContext(httpContextAccessorMock.Object, userManagerMock.Object);

            // act
            var user = userContext.GetUserById("1");

            // assert
            user.Should().NotBeNull();
            user.Id.Should().Be("1");
            user.Username.Should().Be("testuser");
            user.Email.Should().Be("test@test.com");
            user.Roles.Should().Contain(new List<string> { "Admin", "RegularUser" });
        }

        [Fact]
        public void GetUserById_WhenUserDoesNotExist_ShouldReturnPlaceholderUser()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManagerMock.Setup(x => x.FindByIdAsync("2")).ReturnsAsync((IdentityUser?)null);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            var userContext = new UserContext(httpContextAccessorMock.Object, userManagerMock.Object);

            var user = userContext.GetUserById("2");
            
            user.Should().NotBeNull();
            user.Id.Should().Be("0");
            user.Username.Should().Be("User does not exist");
        }
    }
}