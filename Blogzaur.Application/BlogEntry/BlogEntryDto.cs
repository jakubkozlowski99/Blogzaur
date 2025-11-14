using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry
{
    public class BlogEntryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public int LikeAmount { get; set; } = 0;
        public bool isEditable { get; set; }
        public bool isLiked { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
