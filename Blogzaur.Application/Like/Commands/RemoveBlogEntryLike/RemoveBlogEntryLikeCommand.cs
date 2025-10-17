using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.RemoveBlogEntryLike
{
    public class RemoveBlogEntryLikeCommand : IRequest
    {
        public int BlogEntryId { get; set; }
    }
}
