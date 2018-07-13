using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanning.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember my password?")]
        public bool RememberMe { get; set; }
    }
}
