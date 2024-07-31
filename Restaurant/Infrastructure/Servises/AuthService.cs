using AutoMapper;
using core.DTOs.AccountDto;
using core.Entities;
using core.Helpers;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly Responses _responses;

        public AuthService(MyDbContext context, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IMapper mapper, Responses responses)
        {
            _context = context;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _responses = responses;
        }
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.UserName == registerDto.Username))
                {
                    return _responses.ResponseConflict("اسم المستخدم موجود بالفعل.");
                }

                if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return _responses.ResponseConflict("البريد الالكتروني مسجل مسبقاً.");
                }

                var user = new User
                {
                    UserName = registerDto.Username,
                    PasswordHash = HashPassword(registerDto.Password),
                    Email = registerDto.Email,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess<User>("تم انشاء حساب جديد ",user);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserOrEmail || u.Email == loginDto.UserOrEmail);

                if (user == null)
                {
                    return _responses.ResponseNotFound("المستخدم غير موجود.");
                }

                bool isPasswordValid = VerifyPassword(loginDto.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    return _responses.ResponseUnauthorized("كلمة المرور غير صحيحة.");
                }

                var userDto = _mapper.Map<UserDto>(user);

                var token = GenerateJwtToken(user);
                userDto.Token = token;

                return _responses.ResponseSuccess("تم تسجيل الدخول بنجاح.", userDto);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgetPasswordDto.Email);
                if (user == null)
                {
                    return _responses.ResponseNotFound("لم يتم العثور على المستخدم.");
                }

                var resetToken = GenerateResetToken();
                user.ResetToken = resetToken;
                user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(3);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // هنا لازم اخلي كود يدز رمز على الايميل 

                return _responses.ResponseSuccess("تم إرسال رمز إعادة تعيين كلمة المرور إلى البريد الإلكتروني.");
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == resetPasswordDto.ResetToken && u.ResetTokenExpiry > DateTime.UtcNow);
                if (user == null)
                {
                    return _responses.ResponseNotFound("رمز إعادة التعيين غير صالح أو منتهي الصلاحية.");
                }

                user.PasswordHash = HashPassword(resetPasswordDto.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم إعادة تعيين كلمة المرور بنجاح.");
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users == null || users.Count == 0)
                {
                    return _responses.ResponseNotFound("لا توجد حسابات للعرض.");
                }

                var usersDto = _mapper.Map<List<UserDto>>(users);
                return _responses.ResponseSuccess("تم استرداد بيانات الحسابات بنجاح", usersDto);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return _responses.ResponseNotFound("الحساب المطلوب غير متوفر!");
                }

                var userDto = _mapper.Map<UserDto>(user);
                return _responses.ResponseSuccess("تم استرداد بيانات الحساب بنجاح", userDto);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> FindUserByUsername(string username)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return _responses.ResponseNotFound("لم يتم العثور على المستخدم.");
                }

                var userDto = _mapper.Map<UserDto>(user);
                return _responses.ResponseSuccess("تم استرداد المستخدم بنجاح.", userDto);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> ChangeUserRole(ChangeUserRole userRole)
        {
            try
            {
                var user = await _context.Users.FindAsync(userRole.UserId);
                if (user == null)
                {
                    return _responses.ResponseNotFound("لم يتم العثور على المستخدم.");
                }

                user.Role = userRole.NewRole;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم تحديث دور المستخدم بنجاح.");
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return _responses.ResponseSuccess("تم تسجيل الخروج بنجاح");
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return _responses.ResponseNotFound("لم يتم العثور على المستخدم.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم حذف المستخدم بنجاح.");
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }
        private bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash == hashed;
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["Jwt:ServerSecret"];
            
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string GenerateResetToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
    }
}
