using EventPlanning.Controllers;
using EventPlanning.Models;
using EventPlanning.Models.EventsViewModels;
using EventPlanning.Test.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventPlanning.Test
{
    public class EventControllerTest
    {
        public EventControllerTest()
        {
            var moq = new Mock<ILogger<EventController>>();
            _logger = moq.Object;
        }

        private readonly ILogger<EventController> _logger;

        [Fact]
        public void AllEvents_CheckResultIsView()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext())           
            using (var eventController = new EventController(context, null, _logger, null))
            {
                // Act
                var vr = eventController.AllEvents();

                // Assert
                Assert.IsType<ViewResult>(vr.Result);
            }
        }

        [Fact]
        public void AllEvents_CheckModelResultNotNull()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext())
            using (var eventController = new EventController(context, null, _logger, null))
            {
                // Act
                var vr = ((eventController.AllEvents()).Result as ViewResult);
                var model = vr.Model;

                // Assert
                Assert.NotNull(model);
            }
        }

        [Fact]
        public void AllEvents_CheckIsTypeModelResult()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext())
            using (var eventController = new EventController(context, null, _logger, null))
            {
                // Act
                var vr = ((eventController.AllEvents()).Result as ViewResult);
                var model = vr.Model;

                // Assert
                Assert.IsAssignableFrom<IEnumerable<AllEventViewModel>>(model);
            }
        }

        [Fact]
        public void AllEvents_CheckIsModelCountResult()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext())
            using (var eventController = new EventController(context, null, _logger, null))
            {
                // Act
                var vr = ((eventController.AllEvents()).Result as ViewResult);
                var model = vr.Model as ICollection<AllEventViewModel>;

                // Assert
                Assert.Equal<int>(2, model.Count);
            }
        }

        [Fact]
        public void AllEvents_CheckIfModelIsEmpty()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext(false))
            using (var controller = new EventController(context, null, _logger, null))
            {
                // Act
                var vr = controller.AllEvents().Result as ViewResult;
                var model = vr.Model as ICollection<AllEventViewModel>;

                // Assert
                Assert.IsType<ViewResult>(vr);
                Assert.Equal<int>(0, model.Count);
            }
        }

        [Fact]
        public void CreateEvent_CheckIfModelHasError()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContext())
            using (var controller = new EventController(context, null, _logger, null))
            {
                // Act               
                controller.ModelState.AddModelError("error", "some error");
                var br = controller.CreateEvent(new CreateEventViewModel()).Result;

                // Assert
                Assert.IsType<BadRequestResult>(br);
            }
        }

        [Fact]
        public void CreateEvent_CheckIfNoUser()
        {
            try
            {
                // Arrange
                using (var context = DbContextInitializer.GetContext())
                using (var controller = new EventController(context, null, _logger, null))
                {
                    // Act 
                    Assert.Throws<AggregateException>(() =>
                        controller.CreateEvent(new CreateEventViewModel()).Result);
                }
            }
            catch (AggregateException exception)
            {
                // Assert
                Assert.Equal("Unable to load current user ClaimTypes.NameIdentifier", exception.Message);
            }
        }
    }
}
