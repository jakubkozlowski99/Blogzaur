using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById
{
    public class GetBlogEntryByIdQuery : IRequest<BlogEntryDto>
    {
        public int Id { get; set; }
        public GetBlogEntryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
