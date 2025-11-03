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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository, ICategoryRepository categoryRepository, IMapper mapper, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("RegularUser"))
            {
                return Unit.Value;
            }

            var blogEntry = _mapper.Map<Domain.Entities.BlogEntry>(request);

            blogEntry.AuthorId = currentUser.Id;

            await _blogEntryRepository.Create(blogEntry);

            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _categoryRepository.GetById(categoryId);
                if (category != null)
                {
                    await _categoryRepository.AddBlogEntryCategory(new Domain.Entities.BlogEntryCategory
                    {
                        BlogEntryId = blogEntry.Id,
                        CategoryId = categoryId
                    });
                }
            }

            return Unit.Value;
        }
    }
}
