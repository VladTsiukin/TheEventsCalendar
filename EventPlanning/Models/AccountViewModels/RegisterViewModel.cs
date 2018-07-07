using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanning.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Алиас (роль)")]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "'Подтверждённый пароль' не совпадает с паролем выше.")]
        public string ConfirmPassword { get; set; }
    }
}
