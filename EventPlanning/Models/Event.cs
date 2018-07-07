using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventPlanning.Models
{
    public class Event
    {
        public Event()
        {
            Subscribers = new List<Subscribers>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string AppUserId { get; set; }

        [Required]
        [MaxLength(10000000)]
        public int AmountOfParticipants { get; set; } = 0;

        [Required]
        public DateTimeOffset DateOfCreation{ get; set; }

        [Required]
        public DateTimeOffset EventDate { get; set; }

        public ICollection<Subscribers> Subscribers { get; set; }

        // set json data       
        internal string _Content { get; set; }

        [NotMapped]
        public Content[] Content
        {
            get { return JsonConvert.DeserializeObject<Content[]>(string.IsNullOrEmpty(_Content) ? "{}" : _Content); }
            set { _Content = JsonConvert.SerializeObject(value); }
        }
    
        [ForeignKey("AppUserId")]
        public virtual ApplicationUser AppUser { get; set; }
    }
}
