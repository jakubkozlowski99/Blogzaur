using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Queries.GetCommentsByUserId
{
    public class GetCommentsByUserIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public string UserId { get; set; }
        public GetCommentsByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
