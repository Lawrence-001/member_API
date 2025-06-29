using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MemberTestAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}
