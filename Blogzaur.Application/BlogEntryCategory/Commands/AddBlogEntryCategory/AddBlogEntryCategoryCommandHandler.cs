using Blogzaur.Domain.Interfaces;
using MediatR;

namespace Blogzaur.Application.BlogEntryCategory.Commands.AddBlogEntryCategory
{
    public class AddBlogEntryCategoryCommandHandler : IRequestHandler<AddBlogEntryCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public AddBlogEntryCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Unit> Handle(AddBlogEntryCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryRepository.AddBlogEntryCategory(new Domain.Entities.BlogEntryCategory { BlogEntryId = request.BlogEntryId, CategoryId = request.CategoryId});

            return Unit.Value;
        }
    }
}
