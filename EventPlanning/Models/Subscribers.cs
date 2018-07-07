using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlanning.Models
{
    public class Subscribers
    {
        [Key]
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public int EventId { get; set; }

        [ForeignKey("AppUserId")]
        public ApplicationUser AppUser { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}