using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Logging;

namespace EventPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        public ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Toggle localization
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(culture))
            {
                _logger.LogError("Culture is empty.");
                return BadRequest();
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            _logger.LogDebug("Culture '{0}' enabled with url: {1}.", culture, returnUrl);
            return LocalRedirect(returnUrl);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
