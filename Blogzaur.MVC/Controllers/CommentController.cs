using AutoMapper;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.Comment.Commands.CreateComment;
using Blogzaur.Application.Comment.Queries.GetCommentsByBlogEntryId;
using Blogzaur.Application.Like.Commands.AddCommentLike;
using Blogzaur.Application.Like.Commands.RemoveCommentLike;
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

        [HttpGet]
        [Route("Comment/GetComments/{blogEntryId}")]
        public async Task<IActionResult> GetCommentsByBlogEntryId(int blogEntryId)
        {
            if (blogEntryId <= 0)
            {
                return BadRequest("Invalid blog entry ID.");
            }

            var comments = await _mediator.Send(new GetCommentsByBlogEntryIdQuery(blogEntryId));

            return Ok(comments);
        }

        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> LikeComment(int id)
        {
            await _mediator.Send(new AddCommentLikeCommand { CommentId = id });

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> UnlikeComment(int id)
        {
            await _mediator.Send(new RemoveCommentLikeCommand { CommentId = id });

            return Ok();
        }
    }
}
