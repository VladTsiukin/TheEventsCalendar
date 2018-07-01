using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using EventPlanning.Data;
using EventPlanning.Models;
using EventPlanning.Models.EventsViewModels;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        public EventController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               ILogger<HomeController> logger)
        {
            this._context = context;
            this._userManager = userManager;
            this._logger = logger;
        }

        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = this.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                throw new ApplicationException($"Unable to load current user ClaimTypes.NameIdentifier'.");
            }
            _logger.LogInformation("Get Current User ClaimTypes.NameIdentifier");

            try
            {
                Event newEvent = new Event
                {
                    AppUserId = userId,
                    Name = model.Name,
                    AmountOfParticipants = model.AmountOfParticipants,
                    DateOfCreation = model.DateOfCreation,
                    EventDate = model.EventDate,
                    Content = model.Content
                };

                await _context.Set<Event>().AddAsync(newEvent);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successful create a new Event in database");
                return Ok(model);
            }
            catch (Exception)
            {
                _logger.LogError("Fail to create a new Event in database.");
                return StatusCode(500, new { error = "Error", Model = model });
            }
        }

        
        public IActionResult AllEvents()
        {
            var model = _context.Events.ToList();

            var events = getAllEvents(model);

            return View(events);
        }

        private IEnumerable<EventViewModel> getAllEvents(List<Event> model)
        {
            return model.Select(e =>
            {
                return new EventViewModel
                {
                    Name = e.Name,
                    AmountOfParticipants = e.AmountOfParticipants,
                    DateOfCreation = e.DateOfCreation,
                    EventDate = e.EventDate,
                    Content = e.Content
                };
            });
        }
    }
}
