using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry
{
    public class CreateBlogEntryCommandHandler : IRequestHandler<CreateBlogEntryCommand>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository, IMapper mapper, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("Owner"))
            {
                return Unit.Value;
            }

            var blogEntry = _mapper.Map<Domain.Entities.BlogEntry>(request);

            blogEntry.AuthorId = currentUser.Id;

            await _blogEntryRepository.Create(blogEntry);

            return Unit.Value;
        }
    }
}
