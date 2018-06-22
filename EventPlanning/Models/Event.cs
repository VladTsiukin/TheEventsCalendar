using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventPlanning.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "You can enter no more than 50 characters!")]
        public string Name { get; set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public int AmountOfParticipants { get; set; } = 0;
        [Required]
        public DateTimeOffset DateOfCreation{ get; set; }
        public DateTimeOffset EventDate { get; set; }

        // set json data       
        internal string _Content { get; set; }

        [NotMapped]
        public JObject Content
        {
            get { return JsonConvert.DeserializeObject<JObject>(string.IsNullOrEmpty(_Content) ? "{}" : _Content); }
            set { _Content = JsonConvert.SerializeObject(value); }
        }
        // f-key
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual ApplicationUser AppUser { get; set; }
    }
}
