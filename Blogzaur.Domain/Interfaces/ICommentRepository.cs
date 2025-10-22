using Blogzaur.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task Commit();
        Task Create(Comment comment);
        Task<List<Comment>> GetByBlogEntryId(int blogEntryId);
        public bool HasUserLiked(int commentId, string userId);
    }
}
