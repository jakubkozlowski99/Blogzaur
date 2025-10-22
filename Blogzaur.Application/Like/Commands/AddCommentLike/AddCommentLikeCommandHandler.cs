using Blogzaur.Application.ApplicationUser;
using Blogzaur.Application.Like.Commands.AddBlogEntryLike;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.AddCommentLike
{
    public class AddCommentLikeCommandHandler : IRequestHandler<AddCommentLikeCommand>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContext _userContext;
        public AddCommentLikeCommandHandler(ILikeRepository likeRepository, IUserContext userContext)
        {
            _likeRepository = likeRepository;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(AddCommentLikeCommand request, CancellationToken cancellationToken)
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

            await _likeRepository.AddCommentLike(like);
            return Unit.Value;
        }
    }
}
