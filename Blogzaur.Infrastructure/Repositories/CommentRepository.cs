using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogzaurDbContext _dbContext;
        public CommentRepository(BlogzaurDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Commit()
            => _dbContext.SaveChangesAsync();

        public async Task Create(Comment comment)
        {
            _dbContext.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetByBlogEntryId(int blogEntryId)
            => await _dbContext.Comments
                .Where(x => x.BlogEntryId == blogEntryId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
    }
}
