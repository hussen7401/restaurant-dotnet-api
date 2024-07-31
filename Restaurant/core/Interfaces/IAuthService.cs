using core.DTOs.AccountDto;
using Microsoft.AspNetCore.Mvc;

namespace core.Interfaces
{
    public interface IAuthService
    {
        Task<ActionResult> Register(RegisterDto registerDto);
        Task<ActionResult> Login(LoginDto loginDto);
        Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
        Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ActionResult> GetUsers();
        Task<ActionResult> GetUserById(int id);
        Task<ActionResult> FindUserByUsername(string username);
        Task<ActionResult> ChangeUserRole(ChangeUserRole userRole);
        Task<ActionResult> Logout();
        Task<ActionResult> DeleteUser(int userId);
    }
}
