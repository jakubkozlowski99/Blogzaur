using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntryCategory.Queries.GetBlogEntryCategories
{
    public class GetBlogEntryCategoriesQuery : IRequest<List<BlogEntryCategoryDto>>
    {
        public int BlogEntryId { get; set; }
    }
}
