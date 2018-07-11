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

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Количество участнивов")]
        public int AmountOfParticipants { get; set; } = 0;

        [Display(Name = "Дата создания")]
        public DateTimeOffset DateOfCreation { get; set; }

        [Display(Name = "Дата события")]
        public DateTimeOffset EventDate { get; set; }

        [Display(Name = "Подписано на событие")]
        public int SubscribersCount { get; set; }

        public ICollection<Content> Content { get; set; }
    }
}
