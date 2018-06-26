using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using EventPlanning.Models.EventsViewModels;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //[Authorize(Roles = "admin")]
        public IActionResult Index(CreateEventViewModel model = null)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEvent(CreateEventViewModel model)
        {
            model.IsError = true;

            return RedirectToAction(nameof(HomeController.Index), model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
