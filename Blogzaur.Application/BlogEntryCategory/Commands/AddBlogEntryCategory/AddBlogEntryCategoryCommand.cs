using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntryCategory.Commands.AddBlogEntryCategory
{
    public class AddBlogEntryCategoryCommand : IRequest
    {
        public int BlogEntryId { get; set; }
        public int CategoryId { get; set; }
    }
}
