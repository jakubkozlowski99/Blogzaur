using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogzaur.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogzaurDbContext _dbContext;
        public CategoryRepository(BlogzaurDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Category?> GetById(int id)
            => _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Category>> GetAllCategories()
            => await _dbContext.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task AddBlogEntryCategory(BlogEntryCategory blogEntryCategory)
        {
            _dbContext.BlogEntryCategories.Add(blogEntryCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BlogEntryCategory>> GetBlogEntryCategories(int blogEntryId)
            => await _dbContext.BlogEntryCategories
                .Where(bec => bec.BlogEntryId == blogEntryId)
                .ToListAsync();
    }
}
