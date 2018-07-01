using Microsoft.AspNetCore.Mvc;


namespace EventPlanning.Models.EventsViewModels
{
    public class SubscriberViewModel
    {
        [HiddenInput]
        public string SubscriberId { get; set; }

        public string SubscriberEmail { get; set; }
    }
}
