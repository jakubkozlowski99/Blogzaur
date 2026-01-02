using Xunit;
using Blogzaur.Application.Like.Commands.AddBlogEntryLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blogzaur.Application.ApplicationUser;
using FluentAssertions;
using MediatR;

namespace Blogzaur.Application.Like.Commands.AddBlogEntryLike.Tests
{
    public class AddBlogEntryLikeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_AddsBlogEntryLike_WhenUserExists()
        {
            // arrange
            var likeRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ILikeRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();
            var request = new AddBlogEntryLikeCommand
            {
                BlogEntryId = 1
            };
            var currentUser = new User("1", "testuser", "testuser@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            likeRepoMock
                .Setup(r => r.AddBlogEntryLike(Moq.It.IsAny<Domain.Entities.UserBlogEntryLike>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var handler = new AddBlogEntryLikeCommandHandler(likeRepoMock.Object, userContextMock.Object);

            // act
            var result = await handler.Handle(request, CancellationToken.None);

            // assert
            result.Should().Be(Unit.Value);
            likeRepoMock.Verify(r => r.AddBlogEntryLike(Moq.It.Is<Domain.Entities.UserBlogEntryLike>(l =>
                l.BlogEntryId == request.BlogEntryId &&
                l.UserId == currentUser.Id
            )), Moq.Times.Once);
        }

        [Fact]
        public async Task Handle_DoesNotAddBlogEntryLike_WhenUserDoesNotExist()
        {
            // arrange
            var likeRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ILikeRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();
            var request = new AddBlogEntryLikeCommand
            {
                BlogEntryId = 1
            };
            userContextMock.Setup(x => x.GetCurrentUser()).Returns((User?)null);
            var handler = new AddBlogEntryLikeCommandHandler(likeRepoMock.Object, userContextMock.Object);

            // act
            var result = await handler.Handle(request, CancellationToken.None);

            // assert
            result.Should().Be(Unit.Value);
            likeRepoMock.Verify(r => r.AddBlogEntryLike(Moq.It.IsAny<Domain.Entities.UserBlogEntryLike>()), Moq.Times.Never);
        }
    }
}