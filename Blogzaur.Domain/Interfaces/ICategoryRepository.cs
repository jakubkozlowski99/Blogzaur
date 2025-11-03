using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task Commit();
        Task<Entities.Category?> GetById(int id);
        Task<List<Entities.Category>> GetAllCategories();
        Task<Entities.BlogEntryCategory?> GetBlogEntryCategory(int blogEntryId, int categoryId);
        Task<List<Entities.BlogEntryCategory>> GetBlogEntryCategories(int blogEntryId);
        Task AddBlogEntryCategory(Entities.BlogEntryCategory blogEntryCategory);
        Task RemoveBlogEntryCategory(Entities.BlogEntryCategory blogEntryCategory);
    }
}
