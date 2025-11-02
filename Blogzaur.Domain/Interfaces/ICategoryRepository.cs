using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Entities.Category?> GetById(int id);
        Task<List<Entities.Category>> GetAllCategories();
        Task<List<Entities.BlogEntryCategory>> GetBlogEntryCategories(int blogEntryId);
        Task AddBlogEntryCategory(Entities.BlogEntryCategory blogEntryCategory);
    }
}
