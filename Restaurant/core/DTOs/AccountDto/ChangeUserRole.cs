using core.enums;
using System.ComponentModel.DataAnnotations;

namespace core.DTOs.AccountDto
{
    public class ChangeUserRole
    {
        [Required]
        public int UserId { get; set; }
        [Required(ErrorMessage ="الرجاء اختيار الدور")]
        public UserRole NewRole { get; set; }
    }
}
