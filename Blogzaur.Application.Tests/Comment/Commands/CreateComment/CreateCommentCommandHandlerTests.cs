using Xunit;
using Blogzaur.Application.Comment.Commands.CreateComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blogzaur.Application.ApplicationUser;
using FluentAssertions;
using MediatR;

namespace Blogzaur.Application.Comment.Commands.CreateComment.Tests
{
    public class CreateCommentCommandHandlerTests
    {
        [Fact]
        public void Handle_CreatesComment_WhenUserIsAuthorized()
        {
            //arrange
            var commentRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ICommentRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();

            var request = new CreateCommentCommand
            {
                BlogEntryId = 1,
                Content = "This is a test comment."
            };

            var currentUser = new User("1", "testuser", "test@test.com", new[] { "RegularUser" });
            userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

            commentRepoMock
                .Setup(r => r.Create(Moq.It.IsAny<Domain.Entities.Comment>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var handler = new CreateCommentCommandHandler(commentRepoMock.Object, userContextMock.Object);

            //act
            var result = handler.Handle(request, CancellationToken.None).Result;

            //assert
            result.Should().Be(Unit.Value);
            commentRepoMock.Verify(r => r.Create(Moq.It.Is<Domain.Entities.Comment>(c =>
                c.Content == request.Content &&
                c.BlogEntryId == request.BlogEntryId &&
                c.AuthorId == currentUser.Id
            )), Moq.Times.Once);
        }

        [Fact]
        public void Handle_DoesNotCreateComment_WhenUserIsNotAuthorized()
        {
            //arrange
            var commentRepoMock = new Moq.Mock<Blogzaur.Domain.Interfaces.ICommentRepository>();
            var userContextMock = new Moq.Mock<Blogzaur.Application.ApplicationUser.IUserContext>();
            var request = new CreateCommentCommand
            {
                BlogEntryId = 1,
                Content = "This is a test comment."
            };
            userContextMock.Setup(x => x.GetCurrentUser()).Returns((User?)null);
            commentRepoMock
                .Setup(r => r.Create(Moq.It.IsAny<Domain.Entities.Comment>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var handler = new CreateCommentCommandHandler(commentRepoMock.Object, userContextMock.Object);

            //act
            var result = handler.Handle(request, CancellationToken.None).Result;

            //assert
            result.Should().Be(Unit.Value);
            commentRepoMock.Verify(r => r.Create(Moq.It.IsAny<Domain.Entities.Comment>()), Moq.Times.Never);
        }
    }
}