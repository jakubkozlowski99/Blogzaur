using AutoMapper;
using Blogzaur.Application.Category.Commands.CreateCategory;
using Blogzaur.Application.Category.Commands.EditCategory;
using Blogzaur.Application.Category.Commands.RemoveCategory;
using Blogzaur.Application.Category.Queries.GetAllCategories;
using Blogzaur.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Blogzaur.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AdminController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Panel()
        {
            if (TempData["ActiveTab"] == null)
            {
                TempData["ActiveTab"] = "users"; // default to users tab if not set
            }

            // load categories and expose to the view, ordered by Id
            var categories = (await _mediator.Send(new GetAllCategoriesQuery()))
                .OrderBy(c => c.Id)
                .ToList();

            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(CreateCategoryCommand command)
        {
            // ensure we return to the Categories tab after redirect
            TempData["ActiveTab"] = "categories";

            if (!ModelState.IsValid)
            {
                this.SetNotification("error", "Please provide a valid category name.");
                return RedirectToAction("Panel");
            }

            await _mediator.Send(command);

            this.SetNotification("success", "Category created successfully!");

            return RedirectToAction("Panel");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory(EditCategoryCommand command)
        {
            TempData["ActiveTab"] = "categories";

            if (!ModelState.IsValid)
            {
                this.SetNotification("error", "Please provide a valid category name.");
                return RedirectToAction("Panel");
            }

            await _mediator.Send(command);

            this.SetNotification("success", "Category updated successfully!");

            return RedirectToAction("Panel");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            TempData["ActiveTab"] = "categories";

            var command = new RemoveCategoryCommand { Id = id };

            await _mediator.Send(command);

            this.SetNotification("success", "Category removed successfully!");

            return RedirectToAction("Panel");
        }
    }
}
