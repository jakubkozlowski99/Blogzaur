using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Category.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUserContext userContext) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || (!currentUser.IsInRole("Admin") && !currentUser.IsInRole("Moderator")))
            {
                return Unit.Value;
            }

            var category = _mapper.Map<Domain.Entities.Category>(request);

            if (category == null) return Unit.Value;

            await _categoryRepository.AddCategory(category);

            return Unit.Value;
        }
    }
}
