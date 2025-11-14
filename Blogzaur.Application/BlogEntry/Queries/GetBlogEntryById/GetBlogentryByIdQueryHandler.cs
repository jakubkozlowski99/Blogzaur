using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Application.Category;
using MediatR;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById
{
    public class GetBlogEntryByIdQueryHandler : IRequestHandler<GetBlogEntryByIdQuery, BlogEntryDto>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ICategoryRepository _categoryRepository;

        public GetBlogEntryByIdQueryHandler(IBlogEntryRepository blogEntryRepository, ICategoryRepository categoryRepository, IMapper mapper, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<BlogEntryDto> Handle(GetBlogEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var blogEntry = await _blogEntryRepository.GetById(request.Id);
            var dto = _mapper.Map<BlogEntryDto>(blogEntry);

            var user = _userContext.GetCurrentUser();
            dto.isLiked = user != null &&  _blogEntryRepository.HasUserLiked(blogEntry.Id, user.Id);

            var blogEntryCategories = await _categoryRepository.GetBlogEntryCategories(blogEntry.Id);

            foreach (var bec in blogEntryCategories)
            {
                var category = await _categoryRepository.GetById(bec.CategoryId);
                if (category != null)
                {
                    dto.Categories.Add(category.Name);
                }
            }

            return dto;
        }
    }
}
