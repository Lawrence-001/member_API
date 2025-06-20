using MemberTestAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MemberTestAPI.Dtos
{
    public class CreateMemberRegistrationDto
    {
        public string FullName { get; set; }
        [Required]
        [MaxLength(10)]
        public string NationalId { get; set; }
        public DateTime? DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string District { get; set; }
    }
}
