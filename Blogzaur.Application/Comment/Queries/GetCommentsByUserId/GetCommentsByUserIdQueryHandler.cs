using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Queries.GetCommentsByUserId
{
    public class GetCommentsByUserIdQueryHandler : IRequestHandler<GetCommentsByUserIdQuery, IEnumerable<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;
        public GetCommentsByUserIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetCommentsByUserId(request.UserId);
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                AuthorId = c.AuthorId!,
                Content = c.Content,
                CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            });
        }
    }
}
