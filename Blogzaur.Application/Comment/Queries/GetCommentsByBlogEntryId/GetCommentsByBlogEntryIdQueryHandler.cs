using AutoMapper;
using Blogzaur.Application.ApplicationUser;
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
        private readonly IUserContext _userContext;
        public GetCommentsByBlogEntryIdQueryHandler(ICommentRepository commentRepository, IMapper mapper, IUserContext userContext)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task<List<CommentDto>> Handle(GetCommentsByBlogEntryIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByBlogEntryId(request.BlogEntryId);
            
            var user = _userContext.GetCurrentUser();
            var dtos = _mapper.Map<List<CommentDto>>(comments);
            foreach (var dto in dtos)
            {
                dto.isLiked = user != null && _commentRepository.HasUserLiked(dto.Id, user.Id);
            }

            return dtos;
        }
    }
}
