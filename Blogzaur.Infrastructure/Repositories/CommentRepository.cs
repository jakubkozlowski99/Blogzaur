using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
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

        public async Task Create(Comment comment)
        {
            _dbContext.Add(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
