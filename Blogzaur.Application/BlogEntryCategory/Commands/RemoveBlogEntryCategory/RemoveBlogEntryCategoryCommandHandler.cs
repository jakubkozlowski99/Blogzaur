using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntryCategory.Commands.RemoveBlogEntryCategory
{
    public class RemoveBlogEntryCategoryCommandHandler : IRequestHandler<RemoveBlogEntryCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public RemoveBlogEntryCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Unit> Handle(RemoveBlogEntryCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryRepository.RemoveBlogEntryCategory(new Domain.Entities.BlogEntryCategory { BlogEntryId = request.BlogEntryId, CategoryId = request.CategoryId});

            return Unit.Value;
        }
    }
}
