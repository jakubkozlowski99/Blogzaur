using Xunit;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Application.ApplicationUser;
using FluentAssertions;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry.Tests
{
    public class EditBlogEntryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_EditsBlogEntry_WhenUserIsRegularUser()
        {
            //arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var userContextMock = new Mock<IUserContext>();

            var request = new EditBlogEntryCommand
            {
                Title = "Updated Title",
                Content = "Updated content that is long enough",
                CategoryIds = new List<int> { 1, 3 }
            };

            var existingBlogEntry = new Domain.Entities.BlogEntry
            {
                Id = 42,
                Title = "Original Title",
                Content = "Original content",
                AuthorId = "1"
            };
            blogEntryRepoMock.Setup(r => r.GetById(request.Id)).ReturnsAsync(existingBlogEntry);

            var currentUser = new User("1", "testuser", "test@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            //category 1 exists, category 3 exists
            categoryRepoMock.Setup(c => c.GetById(1)).ReturnsAsync(new Domain.Entities.Category { Id = 1, Name = "Cat1" });
            categoryRepoMock.Setup(c => c.GetById(3)).ReturnsAsync(new Domain.Entities.Category { Id = 3, Name = "Cat3" });

            var existingCategories = new List<Domain.Entities.BlogEntryCategory>
            {
                new Domain.Entities.BlogEntryCategory { BlogEntryId = 42, CategoryId = 2 }
            };
            categoryRepoMock.Setup(c => c.GetBlogEntryCategories(request.Id)).ReturnsAsync(existingCategories);

            var handler = new EditBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, userContextMock.Object);

            //act
            await handler.Handle(request, CancellationToken.None);

            //assert
            blogEntryRepoMock.Verify(r => r.GetById(request.Id), Times.Once);
            blogEntryRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.BlogEntry>()), Times.Once);

            categoryRepoMock.Verify(c => c.AddBlogEntryCategory(It.Is<Domain.Entities.BlogEntryCategory>(bec => bec.CategoryId == 1)), Times.Once);
            categoryRepoMock.Verify(c => c.AddBlogEntryCategory(It.Is<Domain.Entities.BlogEntryCategory>(bec => bec.CategoryId == 3)), Times.Once);
            categoryRepoMock.Verify(c => c.RemoveBlogEntryCategory(It.Is<Domain.Entities.BlogEntryCategory>(bec => bec.CategoryId == 2)), Times.Once);

            existingBlogEntry.Title.Should().Be(request.Title);
            existingBlogEntry.Content.Should().Be(request.Content);
        }

        [Fact]
        public async Task Handle_DoesNotEditBlogEntry_WhenUserIsNotAuthorOrModerator()
        {
            //arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var userContextMock = new Mock<IUserContext>();

            var request = new EditBlogEntryCommand
            {
                Title = "Updated Title",
                Content = "Updated content that is long enough",
                CategoryIds = new List<int> { 1, 3 }
            };

            var existingBlogEntry = new Domain.Entities.BlogEntry
            {
                Id = 42,
                Title = "Original Title",
                Content = "Original content",
                AuthorId = "2" // Different author
            };

            blogEntryRepoMock.Setup(r => r.GetById(request.Id)).ReturnsAsync(existingBlogEntry);

            var currentUser = new User("1", "testuser", "test@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            var handler = new EditBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, userContextMock.Object);

            //act
            await handler.Handle(request, CancellationToken.None);

            //assert
            blogEntryRepoMock.Verify(r => r.GetById(request.Id), Times.Once);
            blogEntryRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.BlogEntry>()), Times.Never);
        }

        [Fact]
        public async Task Handle_EditsBlogEntry_WhenUserIsModerator()
        {
            //arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var userContextMock = new Mock<IUserContext>();
            var request = new EditBlogEntryCommand
            {
                Title = "Updated Title",
                Content = "Updated content that is long enough",
                CategoryIds = new List<int> { 1, 3 }
            };

            var existingBlogEntry = new Domain.Entities.BlogEntry
            {
                Id = 42,
                Title = "Original Title",
                Content = "Original content",
                AuthorId = "2" // Different author
            };
            blogEntryRepoMock.Setup(r => r.GetById(request.Id)).ReturnsAsync(existingBlogEntry);

            var currentUser = new User("1", "testuser", "test@test.com", new[] { "RegularUser", "Moderator" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            //category 1 exists, category 3 exists
            categoryRepoMock.Setup(c => c.GetById(1)).ReturnsAsync(new Domain.Entities.Category { Id = 1, Name = "Cat1" });
            categoryRepoMock.Setup(c => c.GetById(3)).ReturnsAsync(new Domain.Entities.Category { Id = 3, Name = "Cat3" });

            var existingCategories = new List<Domain.Entities.BlogEntryCategory>
            {
                new Domain.Entities.BlogEntryCategory { BlogEntryId = 42, CategoryId = 2 }
            };
            categoryRepoMock.Setup(c => c.GetBlogEntryCategories(request.Id)).ReturnsAsync(existingCategories);

            var handler = new EditBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, userContextMock.Object);

            //act
            await handler.Handle(request, CancellationToken.None);

            //assert
            blogEntryRepoMock.Verify(r => r.GetById(request.Id), Times.Once);
            blogEntryRepoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.BlogEntry>()), Times.Once);
        }
    }
}