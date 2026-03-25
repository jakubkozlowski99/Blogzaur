using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Category.Commands.RemoveCategory
{
    public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserContext _userContext;
        public RemoveCategoryCommandHandler(ICategoryRepository categoryRepository, IUserContext userContext) 
        {
            _categoryRepository = categoryRepository;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || (!currentUser.IsInRole("Admin") && !currentUser.IsInRole("Moderator")))
            {
                return Unit.Value;
            }

            var category = await _categoryRepository.GetById(request.Id);
            if (category == null)
            {
                return Unit.Value;
            }

            await _categoryRepository.RemoveCategory(request.Id);

            return Unit.Value;
        }
    }
}
