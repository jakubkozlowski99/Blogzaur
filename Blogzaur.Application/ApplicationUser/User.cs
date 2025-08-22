using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser
{
    public class User
    {
        public User(string id, string username, string email, IEnumerable<string> roles)
        {
            Id = id;
            Username = username;
            Email = email;
            Roles = roles;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
