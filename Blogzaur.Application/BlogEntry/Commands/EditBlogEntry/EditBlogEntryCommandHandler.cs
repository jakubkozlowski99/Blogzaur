using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Entities;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry
{
    public class EditBlogEntryCommandHandler : IRequestHandler<EditBlogEntryCommand>
    {
        private readonly IBlogEntryRepository _blogEntryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserContext _userContext;

        public EditBlogEntryCommandHandler(IBlogEntryRepository blogEntryRepository, ICategoryRepository categoryRepository, IUserContext userContext)
        {
            _blogEntryRepository = blogEntryRepository;
            _categoryRepository = categoryRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditBlogEntryCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
            {
                return Unit.Value;
            }

            var blogEntry = await _blogEntryRepository.GetById(request.Id);
            if (blogEntry == null)
            {
                return Unit.Value;
            }

            var isEditable = blogEntry.AuthorId == user.Id || user.IsInRole("Moderator");
            if (!isEditable)
                return Unit.Value;

            // Add new categories (await the existence check)
            foreach (var categoryId in request.CategoryIds)
            {
                if (await _categoryRepository.GetBlogEntryCategory(blogEntry.Id, categoryId) == null)
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
            }

            // Remove unselected categories - remove the tracked instance instead of constructing a new one
            var existingCategories = await _categoryRepository.GetBlogEntryCategories(request.Id);
            foreach (var existingCategory in existingCategories)
            {
                if (!request.CategoryIds.Contains(existingCategory.CategoryId))
                {
                    await _categoryRepository.RemoveBlogEntryCategory(existingCategory);
                }
            }

            blogEntry.Title = request.Title;
            blogEntry.Content = request.Content;
            blogEntry.Description = request.Description;

            await _blogEntryRepository.Update(blogEntry);

            return Unit.Value;
        }
    }
}
