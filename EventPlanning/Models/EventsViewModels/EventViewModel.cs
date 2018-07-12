using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanning.Models.EventsViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Amount of participants")]
        public int AmountOfParticipants { get; set; } = 0;

        [Display(Name = "Date of creation")]
        public DateTimeOffset DateOfCreation { get; set; }

        [Display(Name = "Event date")]
        public DateTimeOffset EventDate { get; set; }

        [Display(Name = "Signed on event")]
        public int SubscribersCount { get; set; }

        public ICollection<Content> Content { get; set; }
    }
}
