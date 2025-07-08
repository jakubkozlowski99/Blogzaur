using AutoMapper;
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

        public CreateBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository, IMapper mapper)
        {
            _blogEntryRepository = blogEntryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var blogEntry = _mapper.Map<Domain.Entities.BlogEntry>(request);

            await _blogEntryRepository.Create(blogEntry);

            return Unit.Value;
        }
    }
}
