using Blogzaur.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface IBlogEntryRepository
    {
        Task Commit();
        Task Create(BlogEntry blogEntry);
        Task<List<BlogEntry>> GetAll();
        Task<BlogEntry> GetById(int id);
        public bool HasUserLiked(int blogEntryId, string userId);
    }
}
