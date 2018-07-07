using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EventPlanning.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Events = new HashSet<Event>();
            this.Subscribers = new HashSet<Subscribers>();
        }

        public virtual ICollection<Event> Events {get; set; }

        public virtual ICollection<Subscribers> Subscribers { get; set; }
    }
}
