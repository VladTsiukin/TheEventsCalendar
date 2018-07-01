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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext context,
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _logger.LogError("Fail to create a new Event in database with userId = {0}", userId);
                return StatusCode(500, new { error = "ERROR", UserId = userId });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
