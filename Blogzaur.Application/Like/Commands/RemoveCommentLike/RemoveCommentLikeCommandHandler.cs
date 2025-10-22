using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.Like.Commands.RemoveCommentLike
{
    public class RemoveCommentLikeCommandHandler : IRequestHandler<RemoveCommentLikeCommand>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContext _userContext;

        public RemoveCommentLikeCommandHandler(ILikeRepository likeRepository, IUserContext userContext)
        {
            _likeRepository = likeRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(RemoveCommentLikeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser == null)
            {
                return Unit.Value;
            }

            var like = new Domain.Entities.UserCommentLike
            {
                CommentId = request.CommentId,
                UserId = currentUser.Id
            };

            await _likeRepository.RemoveCommentLike(like);

            return Unit.Value;
        }
    }
}
