using System;
using System.Collections.Generic;


namespace EventPlanning.Models.EventsViewModels
{
    public class EventViewModel
    {
        public string Name { get; set; }

        public int AmountOfParticipants { get; set; } = 0;

        public DateTimeOffset DateOfCreation { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public Content[] Content { get; set; }
    }
}
