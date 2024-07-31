using System.ComponentModel.DataAnnotations;

namespace core.DTOs.AccountDto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage ="الرجاء ادخال الرمز السري")]
        public string ResetToken { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال كلمة المرور الجديدة")]
        public string NewPassword { get; set; }
    }
}
