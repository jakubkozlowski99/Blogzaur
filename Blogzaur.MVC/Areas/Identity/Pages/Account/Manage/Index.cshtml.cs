// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace Blogzaur.MVC.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private const string DefaultAvatar = "/images/default-avatar.jpg";

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        // current avatar url for display (fallback to default)
        public string CurrentAvatarUrl { get; private set; } = DefaultAvatar;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            // Read avatar claim from store; fallback to default avatar
            try
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url")?.Value;
                CurrentAvatarUrl = !string.IsNullOrEmpty(avatarClaim) ? avatarClaim : DefaultAvatar;
            }
            catch
            {
                CurrentAvatarUrl = DefaultAvatar;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        // handler for avatar upload (separate form, multipart)
        public async Task<IActionResult> OnPostUploadAsync(IFormFile avatar)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (avatar == null || avatar.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                await LoadAsync(user);
                return Page();
            }

            var ext = Path.GetExtension(avatar.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError(string.Empty, "Allowed file types: jpg, jpeg, png, gif, webp");
                await LoadAsync(user);
                return Page();
            }

            if (avatar.Length > 5 * 1024 * 1024) // 5MB limit
            {
                ModelState.AddModelError(string.Empty, "File too large (max 5MB).");
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(uploadsRoot);

            // store as jpeg normalized filename {userId}.jpg
            var fileName = $"{userId}.jpg";
            var savePath = Path.Combine(uploadsRoot, fileName);

            try
            {
                using var input = avatar.OpenReadStream();
                using var image = Image.Load<Rgba32>(input);

                var options = new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(256, 256),
                    Mode = ResizeMode.Crop
                };
                image.Mutate(x => x.Resize(options));

                // save as jpeg (overwrite existing)
                await image.SaveAsJpegAsync(savePath);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to process image: " + ex.Message);
                await LoadAsync(user);
                return Page();
            }

            // build public url and append version token to bust caches
            var baseUrl = Url.Content($"~/uploads/avatars/{fileName}");
            var versionToken = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var publicUrlWithVersion = $"{baseUrl}?v={versionToken}";

            // persist as claim (remove existing avatar_url first)
            try
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var existing = claims.FirstOrDefault(c => c.Type == "avatar_url");
                if (existing != null)
                {
                    await _userManager.RemoveClaimAsync(user, existing);
                }

                await _userManager.AddClaimAsync(user, new Claim("avatar_url", publicUrlWithVersion));
            }
            catch
            {
                // best-effort; do not fail upload because of claim issue
            }

            // refresh sign-in so claim is present in the current principal
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Avatar uploaded successfully.";
            return RedirectToPage();
        }
    }
}
