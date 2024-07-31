using core.enums;
using System.ComponentModel.DataAnnotations;

namespace core.Entities
{
    public class User : BaseEntity
    {
        [Required, StringLength(50)]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required, StringLength(20)]
        public UserRole Role { get; set; } 
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
}
