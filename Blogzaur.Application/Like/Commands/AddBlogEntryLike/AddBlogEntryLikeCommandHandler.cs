using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.AddBlogEntryLike
{
    public class AddBlogEntryLikeCommandHandler : IRequestHandler<AddBlogEntryLikeCommand>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserContext _userContext;
        public AddBlogEntryLikeCommandHandler(ILikeRepository likeRepository, IUserContext userContext)
        {
            _likeRepository = likeRepository;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(AddBlogEntryLikeCommand request, CancellationToken cancellationToken)
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

            await _likeRepository.AddBlogEntryLike(like);
            return Unit.Value;
        }
    }
}
