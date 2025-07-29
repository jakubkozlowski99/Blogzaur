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
        public int AuthorId { get; set; }
        public bool isHidden { get; set; } = false;
    }
}
