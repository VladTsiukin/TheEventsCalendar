using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlanning.Models
{
    public class Subscribers
    {
        [Key]
        public string Id { get; set; }

        public string SubscriberId { get; set; }

        public string SubscriberEmail { get; set; }

        [ForeignKey("SubscriberId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}