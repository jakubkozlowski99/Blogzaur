using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries
{
    public class GetAllBlogEntriesQueryHandler : IRequestHandler<GetAllBlogEntriesQuery, List<BlogEntryDto>>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public GetAllBlogEntriesQueryHandler(IBlogEntryRepository blogEntryRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogEntryDto>> Handle(GetAllBlogEntriesQuery request, CancellationToken cancellationToken)
        {
            var blogEntries = await _blogEntryRepository.GetAll();
            var dtos = _mapper.Map<List<BlogEntryDto>>(blogEntries);

            foreach (var dto in dtos)
            {
                var blogEntryCategories = await _categoryRepository.GetBlogEntryCategories(dto.Id);

                foreach (var bec in blogEntryCategories)
                {
                    var category = await _categoryRepository.GetById(bec.CategoryId);
                    if (category != null)
                    {
                        dto.Categories.Add(category.Name);
                    }
                }
            }

            return dtos;
        }
    }
}
