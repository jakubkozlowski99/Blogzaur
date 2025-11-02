using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Domain.Entities
{
    public class BlogEntryCategory
    {
        public int BlogEntryId { get; set; }
        public BlogEntry? BlogEntry { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
