using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries
{
    public class GetAllBlogEntriesQueryHandler : IRequestHandler<GetAllBlogEntriesQuery, List<BlogEntryDto>>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;
        public GetAllBlogEntriesQueryHandler(IBlogEntryRepository blogEntryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogEntryDto>> Handle(GetAllBlogEntriesQuery request, CancellationToken cancellationToken)
        {
            var blogEntries = await _blogEntryRepository.GetAll();
            var dtos = _mapper.Map<List<BlogEntryDto>>(blogEntries);

            return dtos;
        }
    }
}
