using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry
{
    internal class EditBlogEntryCommandHandler : IRequestHandler<EditBlogEntryCommand>
    {

        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IUserContext _userContext;
        public EditBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.Id);

            var user = _userContext.GetCurrentUser();

            var isEditable = user != null && blogEntry.AuthorId == user.Id || user.IsInRole("Moderator");

            if (!isEditable)
            {
                return Unit.Value;
            }

            blogEntry.Title = request.Title;
            blogEntry.Content = request.Content;

            await _blogEntryRepository.Commit();

            return Unit.Value;
        }
    }
}
