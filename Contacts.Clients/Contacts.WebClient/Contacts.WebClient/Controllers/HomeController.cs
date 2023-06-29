using Contacts.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Contacts.WebClient.Controllers;

public class HomeController : Controller
{

    public HomeController(){}

    public IActionResult Index()
    {
        return Redirect("Contacts");
    }
    public IActionResult Session()
    {
        return View();
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