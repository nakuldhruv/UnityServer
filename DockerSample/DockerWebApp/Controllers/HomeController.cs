using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DockerWebApp.Models;
using DockerWebApp.Services;

namespace DockerWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SubmissionService _submissionService;

    public HomeController(ILogger<HomeController> logger, SubmissionService submissionService)
    {
        _logger = logger;
        _submissionService = submissionService;
    }

    public IActionResult Index()
    {
        return View(_submissionService.GetAll());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit(Submission model)
    {
        if (ModelState.IsValid)
        {
            _submissionService.Add(model);
            return RedirectToAction(nameof(Index));
        }

        return View("Index", _submissionService.GetAll());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}