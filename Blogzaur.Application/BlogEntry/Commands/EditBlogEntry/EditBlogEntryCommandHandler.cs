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
        public EditBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository)
        {
            _blogEntryRepository = blogEntryRepository;
        }

        public async Task<Unit> Handle(EditBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.Id);

            blogEntry.Title = request.Title;
            blogEntry.Content = request.Content;

            await _blogEntryRepository.Commit();

            return Unit.Value;
        }
    }
}
