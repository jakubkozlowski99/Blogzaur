using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser.Queries.GetBlogEntriesViewsByUserId
{
    public class GetBlogEntriesViewsByUserIdQuery : IRequest<int>
    {
        public string UserId { get; set; } = default!;
        public GetBlogEntriesViewsByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
