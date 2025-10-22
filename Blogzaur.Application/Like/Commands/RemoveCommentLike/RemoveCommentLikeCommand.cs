using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Like.Commands.RemoveCommentLike
{
    public class RemoveCommentLikeCommand : IRequest
    {
        public int CommentId { get; set; }
    }
}
