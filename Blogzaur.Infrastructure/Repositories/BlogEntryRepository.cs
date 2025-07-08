using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogzaur.Infrastructure.Repositories
{
    public class BlogEntryRepository : IBlogEntryRepository
    {
        private readonly BlogzaurDbContext _dbContext;
        public BlogEntryRepository(BlogzaurDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task Create(BlogEntry blogEntry)
        {
            _dbContext.Add(blogEntry);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<BlogEntry>> GetAll()
            => _dbContext.BlogEntries
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
    }
}
