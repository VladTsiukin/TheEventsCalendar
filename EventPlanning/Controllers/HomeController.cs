using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //[Authorize(Roles = "admin")]
        public IActionResult Index(bool isError = false)
        {
            return View(isError);
        }

        [HttpPost]
        public IActionResult CreateEvent()
        {
            return RedirectToAction(nameof(HomeController.Index), new { isError = true });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
