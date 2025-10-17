using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogzaur.Infrastructure.Repositories
{
    public class BlogEntryRepository : IBlogEntryRepository
    {
        private readonly BlogzaurDbContext _dbContext;
        private readonly ILikeRepository _likeRepository;
        public BlogEntryRepository(BlogzaurDbContext dbContext, ILikeRepository likeRepository) 
        {
            _dbContext = dbContext;
            _likeRepository = likeRepository;
        }

        public Task Commit()
            => _dbContext.SaveChangesAsync();

        public async Task Create(BlogEntry blogEntry)
        {
            _dbContext.Add(blogEntry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BlogEntry>> GetAll()
            => await _dbContext.BlogEntries
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<BlogEntry> GetById(int id)
            => await _dbContext.BlogEntries
                .FirstAsync(x => x.Id == id);

        public int GetBlogEntryLikeAmount(int blogEntryId)
        {
            return _dbContext.BlogEntries
                .Where(be => be.Id == blogEntryId)
                .Select(be => be.LikeAmount)
                .FirstOrDefault();
        }

        public bool HasUserLiked(int blogEntryId, string userId)
        {
            return _likeRepository.CheckIfBlogEntryLikeExists(blogEntryId, userId);
        }
    }
}
