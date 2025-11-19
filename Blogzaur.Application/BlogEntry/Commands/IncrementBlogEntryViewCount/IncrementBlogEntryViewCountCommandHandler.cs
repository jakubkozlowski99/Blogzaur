using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.IncrementBlogEntryViewCount
{
    public class IncrementBlogEntryViewCountCommandHandler : IRequestHandler<IncrementBlogEntryViewCountCommand>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        public IncrementBlogEntryViewCountCommandHandler(IBlogEntryRepository blogEntryRepository)
        {
            _blogEntryRepository = blogEntryRepository;
        }
        public async Task<Unit> Handle(IncrementBlogEntryViewCountCommand request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.BlogEntryId);

            if (blogEntry != null) await _blogEntryRepository.IncrementBlogEntryViewCount(request.BlogEntryId);

            return Unit.Value;
        }
    }
}
