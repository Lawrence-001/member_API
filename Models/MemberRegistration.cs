using MemberTestAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MemberTestAPI.Models
{
    public class MemberRegistration
    {
        [Key]
        public int RegistrationId { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string NationalId { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        [Required]
        public Gender Gender { get; set; } = Gender.Unspecified;
        [Required]
        public string Region { get; set; } = string.Empty;
        [Required]
        public string District { get; set; } = string.Empty;
    }
}
