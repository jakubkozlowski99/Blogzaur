using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.AddBlogEntryLike
{
    public class AddBlogEntryLikeCommand : IRequest
    {
        public int BlogEntryId { get; set; }
    }
}
