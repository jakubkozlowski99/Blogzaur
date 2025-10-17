using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Entities
{
    public class UserBlogEntryLike
    {
        public string UserId { get; set; } = default!;
        public IdentityUser? User { get; set; }
        public int BlogEntryId { get; set; } = default!;
        public BlogEntry? BlogEntry { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
