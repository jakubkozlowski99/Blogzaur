using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById
{
    public class GetBlogentryByIdQueryHandler : IRequestHandler<GetBlogEntryByIdQuery, BlogEntryDto>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        public GetBlogentryByIdQueryHandler(IBlogEntryRepository blogEntryRepository, IMapper mapper, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<BlogEntryDto> Handle(GetBlogEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.Id);
            var dto = _mapper.Map<BlogEntryDto>(blogEntry);

            var user = _userContext.GetCurrentUser();
            dto.isLiked = user != null &&  _blogEntryRepository.HasUserLiked(blogEntry.Id, user.Id);

            return dto;
        }
    }
}
