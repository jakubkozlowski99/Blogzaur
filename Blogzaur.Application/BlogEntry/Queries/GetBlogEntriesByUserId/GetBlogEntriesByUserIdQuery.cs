using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Queries.GetBlogEntriesByUserId
{
    public class GetBlogEntriesByUserIdQuery : IRequest<List<BlogEntryDto>>
    {
        public string UserId { get; set; }
        public GetBlogEntriesByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
