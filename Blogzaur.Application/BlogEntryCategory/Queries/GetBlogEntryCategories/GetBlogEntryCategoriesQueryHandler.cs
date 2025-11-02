using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntryCategory.Queries.GetBlogEntryCategories
{
    public class GetBlogEntryCategoriesQueryHandler : IRequestHandler<GetBlogEntryCategoriesQuery, List<BlogEntryCategoryDto>>
    {
        private readonly ICategoryRepository _blogEntryCategoryRepository;
        private readonly IMapper _mapper;
        public GetBlogEntryCategoriesQueryHandler(ICategoryRepository blogEntryCategoryRepository, IMapper mapper)
        {
            _blogEntryCategoryRepository = blogEntryCategoryRepository;
            _mapper = mapper;
        }
        public async Task<List<BlogEntryCategoryDto>> Handle(GetBlogEntryCategoriesQuery request, CancellationToken cancellationToken)
        {
            var blogEntryCategories = await _blogEntryCategoryRepository.GetBlogEntryCategories(request.BlogEntryId);

            var dtos = _mapper.Map<List<BlogEntryCategoryDto>>(blogEntryCategories);

            return dtos;
        }
    }
}
