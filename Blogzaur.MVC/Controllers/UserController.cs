using AutoMapper;
using Blogzaur.Application.ApplicationUser.Queries.GetBlogEntriesViewsByUserId;
using Blogzaur.Application.ApplicationUser.Queries.GetUserByUsername;
using Blogzaur.Application.BlogEntry.Queries.GetBlogEntriesByUserId;
using Blogzaur.Application.Comment.Queries.GetCommentsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;

namespace Blogzaur.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var user = await _mediator.Send(new GetUserByUsernameQuery(username));
            if (user == null) return NotFound();

            var userBlogEntries = await _mediator.Send(new GetBlogEntriesByUserIdQuery(user.Id));
            var userBlogEntriesViews = await _mediator.Send(new GetBlogEntriesViewsByUserIdQuery(user.Id));
            var userComments = await _mediator.Send(new GetCommentsByUserIdQuery(user.Id));

            // Safely compute totals for likes and comments from returned DTOs (reflection tolerant)
            int userBlogsLikesTotal = 0;
            int userCommentsTotal = userComments.Count();

            if (userBlogEntries != null)
            {
                foreach (var blogEntry in userBlogEntries)
                {
                    if (blogEntry == null) continue;
                    //var t = blogEntry.GetType();

                    // Likes: try common property names
                    //var likesProp = t.GetProperty("LikeAmount")
                    //                ?? t.GetProperty("Likes")
                    //                ?? t.GetProperty("LikeCount");
                    //if (likesProp != null)
                    //{
                    //    var v = likesProp.GetValue(blogEntry);
                    //    if (v is int li) likesTotal += li;
                    //    else if (v != null && int.TryParse(v.ToString(), out var pli)) likesTotal += pli;
                    //}

                    userBlogsLikesTotal += blogEntry.LikeAmount;

                    // Comments: try common property names or count a collection if present
                    //var commentsProp = t.GetProperty("CommentCount")
                    //                  ?? t.GetProperty("CommentsCount")
                    //                  ?? t.GetProperty("Comments")
                    //                  ?? t.GetProperty("CommentAmount");

                    //if (commentsProp != null)
                    //{
                    //    var v = commentsProp.GetValue(blogEntry);
                    //    if (v is int ci) commentsTotal += ci;
                    //    else if (v is IEnumerable ie)
                    //    {
                    //        // count items in enumerable
                    //        int c = 0;
                    //        foreach (var _ in ie) c++;
                    //        commentsTotal += c;
                    //    }
                    //    else if (v != null && int.TryParse(v.ToString(), out var pci)) commentsTotal += pci;
                    //}
                }
            }

            ViewBag.UserBlogEntries = userBlogEntries;
            ViewBag.UserBlogEntriesViews = userBlogEntriesViews;
            ViewBag.UserBlogEntriesLikes = userBlogsLikesTotal;
            ViewBag.UserCommentsCount = userCommentsTotal;

            return View(user);
        }
    }
}
