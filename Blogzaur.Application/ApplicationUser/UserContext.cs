using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.ApplicationUser
{
    public interface IUserContext
    {
        User? GetCurrentUser();
        User? GetUserById(string id);
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        public UserContext(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public User? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("Context user is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var username = user.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            return new User(id, username, email, roles);
        }

        public User? GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return new User("0", "User does not exist", "", new List<string>());
            }

            var roles = _userManager.GetRolesAsync(user).Result;
            return new User(user.Id, user.UserName!, user.Email!, roles);
        }
    }
}
