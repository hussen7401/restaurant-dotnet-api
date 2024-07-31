using System.ComponentModel.DataAnnotations;

namespace core.DTOs.AccountDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "الرجاء كتابة اسم المستخدم او عنوان البريد الالكتروني")]
        public string UserOrEmail { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال كلمة السر")]
        public string Password { get; set; }
    }
}
