using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntriesByUserId
{
    public class GetBlogEntriesByUserIdQueryHandler : IRequestHandler<GetBlogEntriesByUserIdQuery, List<BlogEntryDto>>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public GetBlogEntriesByUserIdQueryHandler(IBlogEntryRepository blogEntryRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<BlogEntryDto>> Handle(GetBlogEntriesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var blogEntries = await _blogEntryRepository.GetByUserId(request.UserId);

            var dtos = _mapper.Map<List<BlogEntryDto>>(blogEntries);

            foreach (var dto in dtos)
            {
                var blogEntryCategories = await _categoryRepository.GetBlogEntryCategories(dto.Id);
                foreach (var bec in blogEntryCategories)
                {
                    var category = await _categoryRepository.GetById(bec.CategoryId);
                    if (category != null && !string.IsNullOrEmpty(category.Name))
                    {
                        dto.Categories.Add(category.Name);
                    }
                }
            }

            return dtos;
        }
    }
}
