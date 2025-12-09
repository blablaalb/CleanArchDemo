using CleanArch.Mvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;

    public HomeController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult Index()
    {
        return View("./Views/Home/Index.cshtml");
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

    // Temporary diagnostic endpoint: /Home/ProbeView
    public IActionResult ProbeView()
    {
        var path = Path.Combine(_env.ContentRootPath, "Views", "Home", "Index.cshtml");
        var exists = System.IO.File.Exists(path);
        return Content($"ContentRoot: {_env.ContentRootPath}\nViews/Home/Index.cshtml exists: {exists}\nPath: {path}");
    }
}
