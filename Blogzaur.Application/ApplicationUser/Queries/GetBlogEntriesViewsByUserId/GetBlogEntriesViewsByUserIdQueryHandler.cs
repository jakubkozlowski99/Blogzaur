using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser.Queries.GetBlogEntriesViewsByUserId
{
    public class GetBlogEntriesViewsByUserIdQueryHandler : IRequestHandler<GetBlogEntriesViewsByUserIdQuery, int>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        public GetBlogEntriesViewsByUserIdQueryHandler(IBlogEntryRepository blogEntryRepository)
        {
            _blogEntryRepository = blogEntryRepository;
        }
        public async Task<int> Handle(GetBlogEntriesViewsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var blogEntries = await _blogEntryRepository.GetByUserId(request.UserId);
            return blogEntries.Sum(be => be.Views);
        }
    }
}
