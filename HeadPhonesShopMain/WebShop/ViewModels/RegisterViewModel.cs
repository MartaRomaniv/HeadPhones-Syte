using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Error ", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password dont match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm your password")]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; }
    }
}
