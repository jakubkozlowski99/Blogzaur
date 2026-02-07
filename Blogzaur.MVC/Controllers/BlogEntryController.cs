using AutoMapper;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using Blogzaur.Application.BlogEntry.Commands.IncrementBlogEntryViewCount;
using Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries;
using Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById;
using Blogzaur.Application.BlogEntryCategory.Queries.GetBlogEntryCategories;
using Blogzaur.Application.Category.Queries.GetAllCategories;
using Blogzaur.Application.Category.Queries.GetCategoryById;
using Blogzaur.Application.Like.Commands.AddBlogEntryLike;
using Blogzaur.Application.Like.Commands.RemoveBlogEntryLike;
using Blogzaur.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogzaur.MVC.Controllers
{
    public class BlogEntryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BlogEntryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // Added searchAuthor parameter and exposed it to the view.
        public async Task<IActionResult> List(string? searchTitle, string? searchAuthor, string[]? selectedCategories, string? sortBy)
        {
            // load all blog entries (existing application query)
            var blogEntries = (await _mediator.Send(new GetAllBlogEntriesQuery())).ToList();

            // load categories for the search droplist (values are category IDs)
            var searchSelectItems = await GetAllCategories();

            // expose UI state so the view can rehydrate selected values
            ViewBag.SearchCategories = searchSelectItems;
            ViewBag.SelectedCategories = selectedCategories ?? Array.Empty<string>();
            ViewBag.SearchTitle = searchTitle ?? string.Empty;
            ViewBag.SearchAuthor = searchAuthor ?? string.Empty;
            ViewBag.SortBy = sortBy ?? "date_desc";

            // filter by title (case-insensitive, contains)
            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                var normalized = searchTitle.Trim();
                blogEntries = blogEntries
                    .Where(b => !string.IsNullOrEmpty(b.Title) &&
                                b.Title.Contains(normalized, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // filter by author name (case-insensitive, contains)
            if (!string.IsNullOrWhiteSpace(searchAuthor))
            {
                var normalizedAuthor = searchAuthor.Trim();
                blogEntries = blogEntries
                    .Where(b => !string.IsNullOrEmpty(b.AuthorName) &&
                                b.AuthorName.Contains(normalizedAuthor, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // filter by selected category ids (map ids -> names because BlogEntryDto.Categories holds names)
            if (selectedCategories?.Any() == true)
            {
                // build set of category names from the select items; fall back to the raw value if not found
                var selectedCategoryNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var val in selectedCategories)
                {
                    var item = searchSelectItems.FirstOrDefault(s => string.Equals(s.Value, val, StringComparison.OrdinalIgnoreCase));
                    if (item != null && !string.IsNullOrEmpty(item.Text))
                    {
                        selectedCategoryNames.Add(item.Text);
                    }
                    else
                    {   
                        // if the client somehow sent names instead of ids, accept them too
                        selectedCategoryNames.Add(val);
                    }
                }

                blogEntries = blogEntries
                    .Where(b => (b.Categories ?? new List<string>()).Any(cat => selectedCategoryNames.Contains(cat)))
                    .ToList();
            }

            // sorting
            blogEntries = (sortBy ?? "date_desc") switch
            {
                "date_asc" => blogEntries.OrderBy(b => b.CreatedAt).ToList(),
                "date_desc" => blogEntries.OrderByDescending(b => b.CreatedAt).ToList(),
                "views_asc" => blogEntries.OrderBy(b => b.Views).ToList(),
                "views_desc" => blogEntries.OrderByDescending(b => b.Views).ToList(),
                "likes_asc" => blogEntries.OrderBy(b => b.LikeAmount).ToList(),
                "likes_desc" => blogEntries.OrderByDescending(b => b.LikeAmount).ToList(),
                _ => blogEntries.OrderByDescending(b => b.CreatedAt).ToList(),
            };

            return View(blogEntries);
        }

        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> Create()
        {
            var selectItems = GetAllCategories().Result;

            ViewBag.Categories = selectItems;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            await _mediator.Send(new IncrementBlogEntryViewCountCommand { BlogEntryId = id });

            var dto = await _mediator.Send(new GetBlogEntryByIdQuery(id));

            var blogEntryCategories = await _mediator.Send(new GetBlogEntryCategoriesQuery()
            {
                BlogEntryId = id
            });

            var categoryNames = new List<string>();

            foreach (var bec in blogEntryCategories)
            {
                var categoryDto = await _mediator.Send(new GetCategoryByIdQuery()
                {
                    Id = bec.CategoryId
                });
                if (categoryDto != null && !string.IsNullOrEmpty(categoryDto.Name))
                {
                    categoryNames.Add(categoryDto.Name);
                }
            }

            ViewBag.CategoryNames = categoryNames;

            return View(dto);
        }

        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _mediator.Send(new GetBlogEntryByIdQuery(id));

            if (!dto.isEditable)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            EditBlogEntryCommand model = _mapper.Map<EditBlogEntryCommand>(dto);

            // Populate currently selected category ids for this blog entry
            var blogEntryCategories = await _mediator.Send(new GetBlogEntryCategoriesQuery()
            {
                BlogEntryId = id
            });

            model.CategoryIds = blogEntryCategories.Select(bc => bc.CategoryId).ToList();

            // Provide all categories for the UI (same structure as Create view)
            ViewBag.Categories = await GetAllCategories();

            return View(model);
        }

        [Authorize(Roles = "RegularUser")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogEntryCommand command)
        {
            if (!ModelState.IsValid)
            {
                // repopulate categories so the view can render them again
                ViewBag.Categories = await GetAllCategories();
                return View(command);
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> Create(CreateBlogEntryCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetAllCategories().Result;

                return View(command);
            }

            await _mediator.Send(command);

            this.SetNotification("success", "Blog entry created successfully!");

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> LikeBlogEntry(int id)
        {
            await _mediator.Send(new AddBlogEntryLikeCommand { BlogEntryId = id });

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> UnlikeBlogEntry(int id)
        {
            await _mediator.Send(new RemoveBlogEntryLikeCommand { BlogEntryId = id });

            return Ok();
        }

        public async Task<List<SelectListItem>> GetAllCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            var selectItems = categories
                .Select(c => new SelectListItem(c.Name ?? c.ToString(), c.Id.ToString()))
                .ToList();

            return selectItems;
        }
    }
}
