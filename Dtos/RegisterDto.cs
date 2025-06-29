using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MemberTestAPI.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Remote(action: "IsEmailTaken", controller: "Auth")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();

    }
}
