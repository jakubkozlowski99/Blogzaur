using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser.Queries.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, User>
    {
        private readonly IUserContext _userContext;
        public GetUserByUsernameQueryHandler(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetUserByUsername(request.Username);
            if (user == null)
            {
                return Task.FromResult(new User("0", "User does not exist", "", new List<string>()));
            }
            return Task.FromResult(user);
        }
    }
}
