using Xunit;
using Blogzaur.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Blogzaur.Application.BlogEntry;
using FluentAssertions;

namespace Blogzaur.Application.Mappings.Tests
{
    public class BlogEntryMappingProfileTests
    {
        [Fact]
        public void MappingProfile_ShouldMapBlogEntryToBlogEntryDto()
        {
            // arrange
            var userContextMock = new Mock<ApplicationUser.IUserContext>();
            var currentUser = new ApplicationUser.User("1", "testuser", "testuser@test.com", new[] { "RegularUser", "Moderator" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlogEntryMappingProfile(userContextMock.Object));
            });
            var mapper = config.CreateMapper();
            var blogEntry = new Domain.Entities.BlogEntry
            {
                AuthorId = "1",
                Title = "Test Title",
                Content = "Test Content",
                Description = "Test Description"
            };

            // act
            var result = mapper.Map<BlogEntryDto>(blogEntry);

            // assert
            result.Should().NotBeNull();

            result.isEditable.Should().BeTrue();

            result.Title.Should().Be(blogEntry.Title);
            result.Content.Should().Be(blogEntry.Content);
            result.Description.Should().Be(blogEntry.Description);
        }
    }
}