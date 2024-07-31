using System.ComponentModel.DataAnnotations;

namespace core.DTOs.AccountDto
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage ="الرجاء كتابة عنوان البريد الالكتروني")]
        public string Email { get; set; }
    }
}
