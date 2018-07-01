using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;


namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
