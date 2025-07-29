using AutoMapper;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.Comment.Commands.CreateComment;
using Blogzaur.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blogzaur.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMediator _mediator;
        public CommentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.Send(command);

            return Ok();
        }
    }
}
