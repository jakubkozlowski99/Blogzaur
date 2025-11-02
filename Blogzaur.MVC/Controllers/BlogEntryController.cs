using AutoMapper;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries;
using Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById;
using Blogzaur.Application.BlogEntryCategory.Commands;
using Blogzaur.Application.BlogEntryCategory.Commands.AddBlogEntryCategory;
using Blogzaur.Application.BlogEntryCategory.Queries.GetBlogEntryCategories;
using Blogzaur.Application.Category.Queries;
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

        public async Task<IActionResult> List()
        {
            var blogEntries = await _mediator.Send(new GetAllBlogEntriesQuery());
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

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _mediator.Send(new GetBlogEntryByIdQuery(id));

            if (!dto.isEditable)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            EditBlogEntryCommand model = _mapper.Map<EditBlogEntryCommand>(dto);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogEntryCommand command)
        {
            if (!ModelState.IsValid)
            {
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

            var blogEntryId = await _mediator.Send(command);

            foreach (var categoryId in command.CategoryIds)
            {
                await _mediator.Send(new AddBlogEntryCategoryCommand
                {
                    BlogEntryId = blogEntryId,
                    CategoryId = categoryId
                });
            }

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

        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> AddBlogEntryCategory(int blogEntryId, int categoryId)
        {
            var command = new AddBlogEntryCategoryCommand
            {
                BlogEntryId = blogEntryId,
                CategoryId = categoryId
            };

            await _mediator.Send(command);

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
