using System;
using System.Collections.Generic;


namespace EventPlanning.Models.EventsViewModels
{
    public class AllEventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset EventDate { get; set; }
    }
}
