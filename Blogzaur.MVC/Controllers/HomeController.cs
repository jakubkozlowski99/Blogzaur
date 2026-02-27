using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blogzaur.MVC.Models;
using MediatR;
using Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries;
using System.Linq;
using System.Threading.Tasks;

namespace Blogzaur.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        // Load all blog entries and prepare trending lists for the view
        var all = (await _mediator.Send(new GetAllBlogEntriesQuery())).ToList();

        ViewBag.TrendingMostLiked = all
            .OrderByDescending(b => b.LikeAmount)
            .Take(3)
            .ToList();

        ViewBag.TrendingMostViewed = all
            .OrderByDescending(b => b.Views)
            .Take(3)
            .ToList();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult NoAccess()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
