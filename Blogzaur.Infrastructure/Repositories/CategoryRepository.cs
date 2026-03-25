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

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();

        public async Task<Category?> GetById(int id)
            => await _dbContext.Categories
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

        public async Task RemoveBlogEntryCategory(BlogEntryCategory blogEntryCategory)
        {
            _dbContext.BlogEntryCategories.Remove(blogEntryCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BlogEntryCategory>> GetBlogEntryCategories(int blogEntryId)
            => await _dbContext.BlogEntryCategories
                .Where(bec => bec.BlogEntryId == blogEntryId)
                .ToListAsync();

        public async Task<BlogEntryCategory?> GetBlogEntryCategory(int blogEntryId, int categoryId)
            => await _dbContext.BlogEntryCategories
                .FirstOrDefaultAsync(bec => bec.BlogEntryId == blogEntryId && bec.CategoryId == categoryId);

        public async Task AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditCategory(Category category)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveCategory(int id)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id);
            if (existingCategory != null)
            {
                _dbContext.Categories.Remove(existingCategory);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
