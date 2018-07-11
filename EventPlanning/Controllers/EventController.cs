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
using EventPlanning.Services;

namespace EventPlanning.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EventController : Controller
    {
        public EventController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               ILogger<HomeController> logger,
                               IEmailSender emailSender)
        {
            this._emailSender = emailSender;
            this._context = context;
            this._userManager = userManager;
            this._logger = logger;
        }

        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Add new event to db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// The user subscribe to the event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubscribeToEvent(int id)
        {
            if (id > 0)
            {
                // get user from db
                var user = await _userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user.");
                }
                _logger.LogInformation("User id identical.");

                // check if subscriber exist
                var subscriber = await _context.Subscribers
                    .FirstOrDefaultAsync(s => s.EventId == id &&
                    s.AppUserId == user.Id);

                if (subscriber != null)
                {
                    return Ok(new { Result = "exist" });
                }

                // create subscriber
                await _context.Subscribers.AddAsync(new Subscribers
                {
                    AppUserId = user.Id,
                    EventId = id
                });
                await _context.SaveChangesAsync();
                _logger.LogInformation("Subscriber successfully added.");

                subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.EventId == id &&
                s.AppUserId == user.Id);

                if (subscriber == null)
                {
                    _logger.LogError("Can not load a new subscriber");
                    return StatusCode(500);
                }

                // get the event by id
                var e = await _context.Events.FindAsync(id);

                if (e == null)
                {
                    _logger.LogError("Unable to load the event Id: {0}.", id);
                    return StatusCode(500);
                }

                // add subscriber to event and to user
                e.Subscribers.Add(subscriber);
                user.Subscribers.Add(subscriber);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Subscriber successfully added to event and to user.");

                // send email
                var callbackUrl = Url.EventLink(user.Id, id, Request.Scheme);
                await _emailSender.SendEmailToSubscriberAsync(user.Email, callbackUrl);
                _logger.LogInformation("Email successfully send.");

                return Ok(new { Result = "success" });
            }
            _logger.LogError("Fail by events id.");
            return BadRequest(id);
        }

        /// <summary>
        /// Show the event by email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowEventByEmail(string userId, int eventId)
        {
            if (userId == null || eventId <= 0)
            {
                _logger.LogError("User id and eventId not found.");
                return RedirectToAction(nameof(EventController.AllEvents), "Event");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var e = await _context.Events.FindAsync(eventId);

            if (e != null)
            {
                var amountSubscribers = await _context.Subscribers.CountAsync(s => s.EventId == eventId);

                var eventModel = GetEventModel(e, amountSubscribers);

                if (eventModel != null)
                {
                    return View(eventModel);
                }               
            }
            _logger.LogError("Can not show the event.");
            return RedirectToAction(nameof(EventController.AllEvents), "Event");
        }

        /// <summary>
        /// Get detailed info about the event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllEvents()
        {
            var e = await _context.Events.ToArrayAsync();
            var events = GetEventsModel(e);

            return View(events);
        }

        /// <summary>
        /// Get the events by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
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
                if (e == null)
                {
                    throw new NullReferenceException("Event can not be null");
                }

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
