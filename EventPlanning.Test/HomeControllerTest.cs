using System;
using EventPlanning.Controllers;
using EventPlanning.Models.EventsViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventPlanning.Test
{
    public class HomeControllerTest
    {
        public HomeControllerTest()
        {
            var moq = new Mock<ILogger<HomeController>>();
            logger = moq.Object;
        }

        public ILogger<HomeController> logger;

        [Fact]
        public void Index_CheckViewResult()
        {
            // Arrange
            var controller = new HomeController(logger);

            // Act
            IActionResult vr = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(vr);
        }

        [Fact]
        public void Index_CheckNullModel()
        {
            // Arrange
            var controller = new HomeController(logger);

            // Act
            IActionResult vr = controller.Index();
            var model = (vr as ViewResult).Model;

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void SetLanguage_CheckBadRequest()
        {
            // Arrange
            var controller = new HomeController(logger);

            // Act
            IActionResult br = controller.SetLanguage(string.Empty);

            // Assert
            Assert.IsType<BadRequestResult>(br);
        }

    }
}
