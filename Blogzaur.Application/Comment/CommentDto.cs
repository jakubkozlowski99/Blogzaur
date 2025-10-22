using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public string AuthorId { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public string CreatedAt { get; set; } = default!;
        public int LikeAmount { get; set; } = 0;
        public bool isHidden { get; set; } = false;
        public bool isLiked { get; set; } = false;
    }
}
