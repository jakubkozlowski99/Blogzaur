using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContext _userContext;
        public CreateCommentCommandHandler(ICommentRepository commentRepository, IUserContext userContext)
        {
            _commentRepository = commentRepository;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return Unit.Value;
            }
           
            var comment = new Domain.Entities.Comment
            {
                Content = request.Content,
                AuthorId = currentUser.Id,
                BlogEntryId = request.BlogEntryId,
            };

            await _commentRepository.Create(comment);
            return Unit.Value;
        }
    }
}
