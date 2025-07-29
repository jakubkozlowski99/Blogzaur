using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Commands.CreateComment
{
    public class CreateCommentCommand : CommentDto, IRequest
    {
        public int BlogEntryId { get; set; }
    }
}
