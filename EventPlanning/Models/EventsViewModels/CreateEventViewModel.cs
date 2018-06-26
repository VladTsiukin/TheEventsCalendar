using System;
using System.ComponentModel.DataAnnotations;


namespace EventPlanning.Models.EventsViewModels
{
    public class CreateEventViewModel
    {
        public string CreatorId { get; set; }

        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MaxLength(256, ErrorMessage = "Название не должно превышать 256-ти символов!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле 'Количество участников' обязательно для заполнения")]
        public int AmountOfParticipants { get; set; } = 0;

        [Required]
        public DateTimeOffset DateOfCreation { get; set; }

        [Required(ErrorMessage = "Поле 'Дата' обязательно для заполнения")]
        public DateTimeOffset EventDate { get; set; }
       
        public Content[] Content { get; set; }

        public bool IsError { get; set; } = false;
    }
}

