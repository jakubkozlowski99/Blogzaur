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
    public class LikeRepository : ILikeRepository
    {
        private readonly BlogzaurDbContext _dbContext;
        public LikeRepository(BlogzaurDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBlogEntryLike(UserBlogEntryLike like)
        {
            if(CheckIfBlogEntryLikeExists(like.BlogEntryId, like.UserId))
                return;
            _dbContext.UserBlogEntryLikes.Add(like);

            var count = GetBlogEntryLikeAmount(like.BlogEntryId);
            _dbContext.BlogEntries
                .Where(be => be.Id == like.BlogEntryId)
                .ToList()
                .ForEach(be => be.LikeAmount = count + 1);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveBlogEntryLike(UserBlogEntryLike like)
        {
            var existingLike = _dbContext.UserBlogEntryLikes
                .FirstOrDefault(l => l.BlogEntryId == like.BlogEntryId && l.UserId == like.UserId);
            if (existingLike == null)
                return;

            _dbContext.UserBlogEntryLikes.Remove(existingLike);
            var count = GetBlogEntryLikeAmount(like.BlogEntryId);
            _dbContext.BlogEntries
                .Where(be => be.Id == like.BlogEntryId)
                .ToList()
                .ForEach(be => be.LikeAmount = count - 1);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCommentLike(UserCommentLike like)
        {
            if (CheckIfCommentLikeExists(like.CommentId, like.UserId))
                return;
            _dbContext.UserCommentLikes.Add(like);

            var count = GetCommentLikeAmount(like.CommentId);
            _dbContext.Comments
                .Where(c => c.Id == like.CommentId)
                .ToList()
                .ForEach(c => c.LikeAmount = count + 1);

            await _dbContext.SaveChangesAsync();
        }

        public bool CheckIfBlogEntryLikeExists(int blogEntryId, string userId)
        {
            var existingLike = _dbContext.UserBlogEntryLikes
                .FirstOrDefault(l => l.BlogEntryId == blogEntryId && l.UserId == userId);

            if (existingLike != null) return true;
            else return false;
        }

        public bool CheckIfCommentLikeExists(int commentId, string userId)
        {
            var existingLike = _dbContext.UserCommentLikes
                .FirstOrDefault(l => l.CommentId == commentId && l.UserId == userId);

            if (existingLike != null) return true;
            else return false;
        }

        public int GetBlogEntryLikeAmount(int blogEntryId)
        {
            return _dbContext.BlogEntries
                .Where(be => be.Id == blogEntryId)
                .Select(be => be.LikeAmount)
                .FirstOrDefault();
        }

        public int GetCommentLikeAmount(int commentId)
        {
            return _dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(c => c.LikeAmount)
                .FirstOrDefault();
        }

        public async Task RemoveCommentLike(UserCommentLike like)
        {
            var existingLike = _dbContext.UserCommentLikes
                .FirstOrDefault(l => l.CommentId == like.CommentId && l.UserId == like.UserId);
            if (existingLike == null)
                return;

            _dbContext.UserCommentLikes.Remove(existingLike);
            var count = GetCommentLikeAmount(like.CommentId);
            _dbContext.Comments
                .Where(c => c.Id == like.CommentId)
                .ToList()
                .ForEach(c => c.LikeAmount = count - 1);

            await _dbContext.SaveChangesAsync();
        }
    }
}
