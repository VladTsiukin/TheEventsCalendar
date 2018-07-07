using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanning.Models.EventsViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AmountOfParticipants { get; set; } = 0;

        public DateTimeOffset DateOfCreation { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public int SubscribersCount { get; set; }

        public int MaxSubscribers { get; set; }

        public ICollection<Content> Content { get; set; }
    }
}
