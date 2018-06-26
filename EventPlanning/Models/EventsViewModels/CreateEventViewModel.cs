using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanning.Models.EventsViewModels
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MaxLength(256, ErrorMessage = "Название не должно превышать 50-ти символов!")]
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

