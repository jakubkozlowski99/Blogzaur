using AutoMapper;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Services
{
    public class BlogEntryService : IBlogEntryService
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;

        public BlogEntryService(IBlogEntryRepository blogEntryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
        }
        public async Task Create(BlogEntryDto blogEntryDto)
        {
            var blogEntry = _mapper.Map<Domain.Entities.BlogEntry>(blogEntryDto);

            await _blogEntryRepository.Create(blogEntry);
        }
    }
}
