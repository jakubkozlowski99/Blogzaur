using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isHidden { get; set; } = false;
        public int? BlogEntryId { get; set; }
        public BlogEntry? BlogEntry { get; set; }
        public string? AuthorId { get; set; }
        public IdentityUser? Author { get; set; }
        public int LikeAmount { get; set; } = 0;
    }
}
