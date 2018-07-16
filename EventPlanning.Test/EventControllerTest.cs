using EventPlanning.Controllers;
using EventPlanning.Models;
using EventPlanning.Models.EventsViewModels;
using EventPlanning.Test.FakeData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EventPlanning.Test
{
    public class EventControllerTest
    {
        [Fact]
        public void AllEvents_CheckResultIsView()
        {
            // Arrange
            using (var context = DbContextInitializer.GetContextWithData())           
            using (var eventController = new EventController(context, null, null, null))
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
            using (var context = DbContextInitializer.GetContextWithData())
            using (var eventController = new EventController(context, null, null, null))
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
            using (var context = DbContextInitializer.GetContextWithData())
            using (var eventController = new EventController(context, null, null, null))
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
            using (var context = DbContextInitializer.GetContextWithData())
            using (var eventController = new EventController(context, null, null, null))
            {
                // Act
                var vr = ((eventController.AllEvents()).Result as ViewResult);
                var model = vr.Model as ICollection<AllEventViewModel>;

                // Assert
                Assert.Equal<int>(2, model.Count);
            }
        }
    }
}
