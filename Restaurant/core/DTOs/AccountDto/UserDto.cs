using core.enums;

namespace core.DTOs.AccountDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; } 
        public string? Token { get; set; }
        public DateTime CreateAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }
}
