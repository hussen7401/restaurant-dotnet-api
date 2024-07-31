using System.ComponentModel.DataAnnotations;

namespace core.DTOs.AccountDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "الرجاء ادخال اسم المستخدم ")]
        public string Username { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال كلمة السر")]
        public string Password { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "الرجاء ادخال عنوان البريد الالكتروني")]
        public string Email { get; set; }
    }
}
