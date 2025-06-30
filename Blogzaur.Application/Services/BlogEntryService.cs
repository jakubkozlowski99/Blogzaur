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

        public BlogEntryService(IBlogEntryRepository blogEntryRepository)
        {
            _blogEntryRepository = blogEntryRepository;
        }
        public async Task Create(BlogEntry blogEntry)
        {
            await _blogEntryRepository.Create(blogEntry);
        }
    }
}
