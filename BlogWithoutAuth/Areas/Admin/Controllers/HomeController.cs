using BlogWithoutAuth.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWithoutAuth.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}