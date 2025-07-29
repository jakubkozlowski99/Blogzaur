using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Queries.GetCommentsByBlogEntryId
{
    public class GetCommentsByBlogEntryIdQuery : IRequest<List<CommentDto>>
    {
        public int BlogEntryId { get; set; }
        public GetCommentsByBlogEntryIdQuery(int blogEntryId)
        {
            BlogEntryId = blogEntryId;
        }
    }
}
