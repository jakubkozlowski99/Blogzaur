using AutoMapper;
using Blogzaur.Application.Like.Commands.AddBlogEntryLike;
using Blogzaur.Application.Like.Commands.AddCommentLike;
using Blogzaur.Application.Like.Commands.RemoveBlogEntryLike;
using Blogzaur.Application.Like.Commands.RemoveCommentLike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogzaur.MVC.Controllers
{
    public class LikeController : Controller
    {
        private readonly IMediator _mediator;
        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
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
