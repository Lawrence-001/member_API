using System.ComponentModel.DataAnnotations;

namespace MemberTestAPI.Dtos
{
    public class CreateRoleDto
    {
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;
    }
}
