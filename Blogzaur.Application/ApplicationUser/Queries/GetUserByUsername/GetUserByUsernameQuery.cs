using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser.Queries.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<User>
    {
        public string Username { get; set; }
        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
