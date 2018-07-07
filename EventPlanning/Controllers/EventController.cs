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
    [AutoValidateAntiforgeryToken]
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
                _logger.LogError("FAIL to create a new Event in database.");
                return StatusCode(500, new { error = "Error", Model = model });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEventInfo(int id)
        {
            if (id > 0)
            {
                var res = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

                if (res != null)
                {
                    var amountSubscribers = await _context.Subscribers.CountAsync(s => s.EventId == id);

                    var eventModel = GetEventModel(res, amountSubscribers);

                    if (eventModel != null)
                    {                       
                        return Ok(eventModel);
                    }
                }
            }

            return BadRequest(new { Result = "error" });
        }

        [HttpGet]
        public async Task<IActionResult> AllEvents()
        {
            var e = await _context.Events.ToArrayAsync();
            var events = GetEventsModel(e);

            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventsByDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTimeOffset d;
                var resDate = DateTimeOffset.TryParse(date, out d);
                if (resDate)
                {
                    var events = await _context.Events.Select(e => e)
                        .Where(e => e.EventDate.Date == d.Date)
                        .AsNoTracking().ToListAsync();

                    if (events.Count() > 0)
                    {
                        var eventsModel = GetEventsModel(events);

                        if (eventsModel != null)
                        {
                            return Ok(new { Result = "success", Events = eventsModel });
                        }                       
                    }
                    else
                    {
                        return StatusCode(200, new { Result = "data not found." });
                    }                    
                }            
            }

            _logger.LogError("FAIL to GetEventsByDate().");
            return BadRequest(new { Result = "error"});            
        }

        /// <summary>
        /// Substituting the events entities
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        private IEnumerable<AllEventViewModel> GetEventsModel(IEnumerable<Event> events)
        {
            try
            {
                if (events == null)
                {
                    throw new NullReferenceException("The events can not be null.");
                }

                return events.Select(e =>
                {
                    return new AllEventViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        EventDate = e.EventDate
                    };
                });
            }
            catch (Exception)
            {
                _logger.LogError("FAIL to GetEventsModel().");
                return null;
            }

        }

        /// <summary>
        /// Substituting the events entity
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private EventViewModel GetEventModel(Event e, int amountOfParticipants)
        {
            try
            {
                return new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    AmountOfParticipants = e.AmountOfParticipants,
                    DateOfCreation = e.DateOfCreation,
                    EventDate = e.EventDate,
                    SubscribersCount = amountOfParticipants,
                    Content = e.Content
                };
            }
            catch (Exception)
            {
                _logger.LogError("FAIL to GetEventModel().");
                return null;
            }
        }


    }
}
