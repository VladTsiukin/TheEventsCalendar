using EventPlanning.Data;
using EventPlanning.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventPlanning.Test.FakeData
{
    public static class DbContextInitializer
    {
        public static ApplicationDbContext GetContext(bool withData = true)
        {
            return SetContext(withData);
        }

        private static ApplicationDbContext SetContext(bool withData)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            if (withData)
            {
                // seed data
                SeedEvents(context);
                SeedSubscribers(context);
            }

            return context;
        }

        private static async void SeedEvents(ApplicationDbContext context)
        {
            var eventsList = new[]
            {
                new Event {
                    Id = 1,
                    AppUserId = "1",
                    Name = "Event1",
                    AmountOfParticipants = 100,
                    DateOfCreation = new DateTimeOffset(new DateTime(2018, 10, 18)),
                    EventDate = DateTimeOffset.Now,
                    Content = new[]
                    {
                        new Content{Name ="name55", Value ="value55"},
                        new Content{Name ="name66", Value ="value66"},
                        new Content{Name ="name77", Value ="value77"}
                    }
                },
                 new Event {
                    Id = 2,
                    AppUserId = "2",
                    Name = "Event2",
                    AmountOfParticipants = 10,
                    DateOfCreation = new DateTimeOffset(new DateTime(2020, 10, 18)),
                    EventDate = DateTimeOffset.Now,
                    Content = new[]
                    {
                        new Content{Name ="name99", Value ="value99"}
                    }
                }
            };

            await context.Events.AddRangeAsync(eventsList);
            await context.SaveChangesAsync();
        }

        private static async void SeedSubscribers(ApplicationDbContext context)
        {
            var subscribersList = new List<Subscribers>
            {
                new Subscribers { EventId = 1, AppUserId = "1"},
                new Subscribers { EventId = 2, AppUserId = "2"}
            };

            await context.Subscribers.AddRangeAsync(subscribersList);
        }
    }
}
