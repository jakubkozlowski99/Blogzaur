using Blogzaur.Application.Services;
using Blogzaur.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blogzaur.MVC.Controllers
{
    public class BlogEntryController : Controller
    {
        private readonly IBlogEntryService _blogEntryService;
        public BlogEntryController(IBlogEntryService blogEntryService)
        {
            _blogEntryService = blogEntryService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogEntry blogEntry)
        {
            await _blogEntryService.Create(blogEntry);
            return RedirectToAction(nameof(Create));
        }
    }
}
