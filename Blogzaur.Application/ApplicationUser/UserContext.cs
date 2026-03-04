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
        User? GetUserByUsername(string username);
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private const string DefaultAvatar = "/images/default-avatar.jpg";

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

            var result = new User(id, username, email, roles);

            // read avatar claim from the principal (if present) else fallback to default
            var avatarFromPrincipal = user.FindFirst("avatar_url")?.Value;
            result.AvatarUrl = !string.IsNullOrEmpty(avatarFromPrincipal) ? avatarFromPrincipal : DefaultAvatar;

            return result;
        }

        public User? GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return new User("0", "User does not exist", "", new List<string>());
            }

            var roles = GetRoles(user);
            var result = new User(user.Id, user.UserName!, user.Email!, roles);

            // read avatar claim from store, fallback to default
            var claims = _userManager.GetClaimsAsync(user).Result;
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url")?.Value;
            result.AvatarUrl = !string.IsNullOrEmpty(avatarClaim) ? avatarClaim : DefaultAvatar;

            return result;
        }

        public User? GetUserByUsername(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;

            if (user == null)
            {
                return new User("0", "User does not exist", "", new List<string>());
            }

            var roles = GetRoles(user);
            var result = new User(user.Id, user.UserName!, user.Email!, roles);

            // read avatar claim from store, fallback to default
            var claims = _userManager.GetClaimsAsync(user).Result;
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url")?.Value;
            result.AvatarUrl = !string.IsNullOrEmpty(avatarClaim) ? avatarClaim : DefaultAvatar;

            return result;
        }

        // helper to avoid blocking/exception surface if roles call fails
        private IEnumerable<string> GetRoles(IdentityUser user)
        {
            try
            {
                return _userManager.GetRolesAsync(user).Result;
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
