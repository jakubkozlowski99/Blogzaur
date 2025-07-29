using AutoMapper;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Comment.Queries.GetCommentsByBlogEntryId
{
    public class GetCommentsByBlogEntryIdQueryHandler : IRequestHandler<GetCommentsByBlogEntryIdQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public GetCommentsByBlogEntryIdQueryHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<List<CommentDto>> Handle(GetCommentsByBlogEntryIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByBlogEntryId(request.BlogEntryId);
            
            var dtos = _mapper.Map<List<CommentDto>>(comments);

            return dtos;
        }
    }
}
