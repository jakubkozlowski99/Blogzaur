using Xunit;
using Blogzaur.Application.Like.Commands.RemoveCommentLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Blogzaur.Application.ApplicationUser;

namespace Blogzaur.Application.Like.Commands.RemoveCommentLike.Tests
{
    public class RemoveCommentLikeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_RemovesCommentLike_IfUserExists()
        {
            // arrange
            var likeRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ILikeRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();
            var request = new RemoveCommentLikeCommand
            {
                CommentId = 1
            };
            var currentUser = new User("1", "testuser", "testuser@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            likeRepoMock
                .Setup(r => r.RemoveCommentLike(Moq.It.IsAny<Domain.Entities.UserCommentLike>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var handler = new RemoveCommentLikeCommandHandler(likeRepoMock.Object, userContextMock.Object);

            // act
            var result = await handler.Handle(request, CancellationToken.None);

            // assert
            result.Should().Be(Unit.Value);
            likeRepoMock.Verify(r => r.RemoveCommentLike(Moq.It.Is<Domain.Entities.UserCommentLike>(l =>
                l.CommentId == request.CommentId &&
                l.UserId == currentUser.Id
            )), Moq.Times.Once);
        }

        [Fact]
        public async Task Handle_DoesNotRemoveCommentLike_IfUserDoesNotExist()
        {
            // arrange
            var likeRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ILikeRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();
            var request = new RemoveCommentLikeCommand
            {
                CommentId = 1
            };
            userContextMock.Setup(x => x.GetCurrentUser()).Returns((User?)null);
            var handler = new RemoveCommentLikeCommandHandler(likeRepoMock.Object, userContextMock.Object);

            // act
            var result = await handler.Handle(request, CancellationToken.None);

            // assert
            result.Should().Be(Unit.Value);
            likeRepoMock.Verify(r => r.RemoveCommentLike(Moq.It.IsAny<Domain.Entities.UserCommentLike>()), Moq.Times.Never);
        }
    }
}