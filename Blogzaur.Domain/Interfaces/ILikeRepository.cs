using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task AddBlogEntryLike(Entities.UserBlogEntryLike like);
        Task RemoveBlogEntryLike(Entities.UserBlogEntryLike like);
        Task AddCommentLike(Entities.UserCommentLike like);
        public bool CheckIfBlogEntryLikeExists(int blogEntryId, string userId);
        public int GetBlogEntryLikeAmount(int blogEntryId);
        int GetCommentLikeAmount(int commentId);
    }
}
