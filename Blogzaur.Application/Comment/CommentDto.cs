using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment
{
    public class CommentDto
    {
        public string Content { get; set; } = default!;
        public string AuthorId { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public bool isHidden { get; set; } = false;
    }
}
