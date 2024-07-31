using core.DTOs.AccountDto;
using core.Interfaces;
using Infrastructure.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            return await _authService.Register(registerDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            return await _authService.ForgetPassword(forgetPasswordDto);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return await _authService.ResetPassword(resetPasswordDto);
        }


        //[Authorize]
        [HttpGet("get-users")]
        public async Task<ActionResult> GetUsers()
        {
            return await _authService.GetUsers();
        }
        //[Authorize]
        [HttpGet("get-user-by-id/{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            return await _authService.GetUserById(id);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("change-role")]
        public async Task<ActionResult> ChangeUserRole(ChangeUserRole changeRoleDto)
        {
            return await _authService.ChangeUserRole(changeRoleDto);
        }

        //[Authorize]
        [HttpGet("get-user-by-username/{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            return await _authService.FindUserByUsername(username);
        }
        //[Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        { 
            return await _authService.Logout();
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            return await _authService.DeleteUser(userId);
        }
    }
}

