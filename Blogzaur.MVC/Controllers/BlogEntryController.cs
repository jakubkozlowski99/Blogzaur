using AutoMapper;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries;
using Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById;
using Blogzaur.MVC.Extensions;
using Blogzaur.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [Authorize(Roles = "Owner")]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _mediator.Send(new GetBlogEntryByIdQuery(id));
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
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(CreateBlogEntryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(command);

            this.SetNotification("success", "Blog entry created successfully!");

            return RedirectToAction(nameof(List));
        }
    }
}
