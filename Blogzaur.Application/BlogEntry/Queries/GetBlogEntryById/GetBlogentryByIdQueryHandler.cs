using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById
{
    public class GetBlogentryByIdQueryHandler : IRequestHandler<GetBlogEntryByIdQuery, BlogEntryDto>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;
        public GetBlogentryByIdQueryHandler(IBlogEntryRepository blogEntryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
        }

        public async Task<BlogEntryDto> Handle(GetBlogEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.Id);

            var dto = _mapper.Map<BlogEntryDto>(blogEntry);

            return dto;
        }
    }
}
