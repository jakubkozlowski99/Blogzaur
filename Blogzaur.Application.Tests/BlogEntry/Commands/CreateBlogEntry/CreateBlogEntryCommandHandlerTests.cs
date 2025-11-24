using Xunit;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Domain.Entities;
using AutoMapper;
using FluentAssertions;

namespace Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry.Tests
{
    public class CreateBlogEntryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_CreatesBlogEntry_WhenUserIsRegularUser()
        {
            // arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();
            var userContextMock = new Mock<IUserContext>();

            var request = new CreateBlogEntryCommand
            {
                Title = "Test Title",
                Content = "Test content that is long enough",
                Description = "Test description",
                CategoryIds = new List<int> { 1, 2 }
            };

            var currentUser = new User("1", "testuser", "test@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            var mappedEntity = new Domain.Entities.BlogEntry
            {
                Title = request.Title,
                Content = request.Content,
                Description = request.Description
            };
            mapperMock.Setup(m => m.Map<Domain.Entities.BlogEntry>(It.IsAny<CreateBlogEntryCommand>())).Returns(mappedEntity);

            Domain.Entities.BlogEntry? capturedBlogEntry = null;
            blogEntryRepoMock
                .Setup(r => r.Create(It.IsAny<Domain.Entities.BlogEntry>()))
                .Returns(Task.CompletedTask)
                .Callback<Domain.Entities.BlogEntry>(be =>
                {
                    capturedBlogEntry = be;
                    // simulate DB assigning Id
                    capturedBlogEntry.Id = 42;
                });

            // category 1 exists, category 2 does not
            categoryRepoMock.Setup(c => c.GetById(1)).ReturnsAsync(new Domain.Entities.Category { Id = 1, Name = "Cat1" });
            categoryRepoMock.Setup(c => c.GetById(2)).ReturnsAsync((Domain.Entities.Category?)null);

            var addedBlogEntryCategories = new List<Domain.Entities.BlogEntryCategory>();
            categoryRepoMock
                .Setup(c => c.AddBlogEntryCategory(It.IsAny<Domain.Entities.BlogEntryCategory>()))
                .Returns(Task.CompletedTask)
                .Callback<Domain.Entities.BlogEntryCategory>(bec => addedBlogEntryCategories.Add(bec));

            var handler = new CreateBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, mapperMock.Object, userContextMock.Object);

            // act
            await handler.Handle(request, CancellationToken.None);

            // assert
            blogEntryRepoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.BlogEntry>()), Times.Once);

            capturedBlogEntry.Should().NotBeNull();
            capturedBlogEntry!.AuthorId.Should().Be(currentUser.Id);

            categoryRepoMock.Verify(c => c.GetById(1), Times.Once);
            categoryRepoMock.Verify(c => c.GetById(2), Times.Once);

            addedBlogEntryCategories.Should().HaveCount(1);
            var blogEntryCategory = addedBlogEntryCategories.Single();
            blogEntryCategory.BlogEntryId.Should().Be(capturedBlogEntry.Id);
            blogEntryCategory.CategoryId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_DoesNotCreateBlogEntry_WhenUserIsNotAuthenticated()
        {
            // arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();
            var userContextMock = new Mock<IUserContext>();
            var request = new CreateBlogEntryCommand
            {
                Title = "Test Title",
                Content = "Test content that is long enough",
                Description = "Test description",
                CategoryIds = new List<int> { 1, 2 }
            };

            // User is null (not authenticated)
            userContextMock.Setup(x => x.GetCurrentUser()).Returns((User?)null);
            var handler = new CreateBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, mapperMock.Object, userContextMock.Object);

            // act
            await handler.Handle(request, CancellationToken.None);

            // assert
            blogEntryRepoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.BlogEntry>()), Times.Never);
            categoryRepoMock.Verify(c => c.GetById(It.IsAny<int>()), Times.Never);
            categoryRepoMock.Verify(c => c.AddBlogEntryCategory(It.IsAny<Domain.Entities.BlogEntryCategory>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DoesNotCreateBlogEntry_WhenUserIsNotInRegularUserRole()
        {
            // arrange
            var blogEntryRepoMock = new Mock<IBlogEntryRepository>();
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var mapperMock = new Mock<IMapper>();
            var userContextMock = new Mock<IUserContext>();
            var request = new CreateBlogEntryCommand
            {
                Title = "Test Title",
                Content = "Test content that is long enough",
                Description = "Test description",
                CategoryIds = new List<int> { 1, 2 }
            };
            var currentUser = new User("1", "testuser", "test@test.com", new[] {"Guest"});
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            var handler = new CreateBlogEntryCommandHandler(blogEntryRepoMock.Object, categoryRepoMock.Object, mapperMock.Object, userContextMock.Object);

            // act
            await handler.Handle(request, CancellationToken.None);

            // assert
            blogEntryRepoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.BlogEntry>()), Times.Never);
            categoryRepoMock.Verify(c => c.GetById(It.IsAny<int>()), Times.Never);
            categoryRepoMock.Verify(c => c.AddBlogEntryCategory(It.IsAny<Domain.Entities.BlogEntryCategory>()), Times.Never);
        }
    }
}