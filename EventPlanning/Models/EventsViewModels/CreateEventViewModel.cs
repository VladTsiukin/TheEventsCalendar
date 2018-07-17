using System;
using System.ComponentModel.DataAnnotations;


namespace EventPlanning.Models.EventsViewModels
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "The 'Name' field is required")]
        [MaxLength(256, ErrorMessage = "The name can not exceed 256 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The 'Number of participants' field is required")]
        public int AmountOfParticipants { get; set; } = 0;

        [Required]
        public DateTimeOffset DateOfCreation { get; set; }

        [Required(ErrorMessage = "The 'Date' field is required")]
        public DateTimeOffset EventDate { get; set; }
       
        public Content[] Content { get; set; }
    }
}
