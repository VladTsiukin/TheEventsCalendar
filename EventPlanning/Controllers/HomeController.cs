using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using EventPlanning.Models.EventsViewModels;
using EventPlanning.Data;
using Newtonsoft.Json.Linq;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext context)
        {
            this._context = context;
        }

        private readonly ApplicationDbContext _context;

        [TempData]
        public string OnError { get; set; } = null;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                int countContent = model.Content.Count();

                Event newEvent = new Event
                {
                    AppUserId = model.CreatorId,
                    Name = model.Name,
                    AmountOfParticipants = model.AmountOfParticipants,
                    DateOfCreation = model.DateOfCreation,
                    EventDate = model.EventDate,
                    Content = model.Content
                };

                await _context.Set<Event>().AddAsync(newEvent);
                await _context.SaveChangesAsync();

                OnError = "true";
                return RedirectToAction(nameof(HomeController.Index));
            }

            OnError = "false";
            return RedirectToAction(nameof(HomeController.Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
