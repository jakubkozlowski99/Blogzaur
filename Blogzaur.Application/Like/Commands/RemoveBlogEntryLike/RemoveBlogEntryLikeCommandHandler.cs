using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.RemoveBlogEntryLike
{
    public class RemoveBlogEntryLikeCommandHandler : IRequestHandler<RemoveBlogEntryLikeCommand>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContext _userContext;

        public RemoveBlogEntryLikeCommandHandler(ILikeRepository likeRepository, IUserContext userContext)
        {
            _likeRepository = likeRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(RemoveBlogEntryLikeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return Unit.Value;
            }

            var like = new Domain.Entities.UserBlogEntryLike
            {
                BlogEntryId = request.BlogEntryId,
                UserId = currentUser.Id
            };

            await _likeRepository.RemoveBlogEntryLike(like);

            return Unit.Value;
        }
    }
}
