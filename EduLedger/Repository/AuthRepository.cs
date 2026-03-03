using EduLedger.Data.DTOs.UserDTOs;
using EduLedger.Entitites.DTOs.AuthDTO;
using EduLedger.Entitites.Models;
using EduLedger.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduLedger.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task<BaseResponse> RegisterAsync(RegisterUserDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                ClassLevelId = dto.ClassLevelId,
                UserProfile = new UserProfile
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    ResidentialAddress = dto.ResidentialAddress
                }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Data = null
                };
            }

            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Invalid role",
                    Data = null
                };
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return new BaseResponse
            {
                Status = true,
                Message = "User registered successfully",
                Data = null
            };
        }
        public async Task<BaseResponse> LoginAsync(LoginRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Invalid credentials",
                    Data = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Email ?? "")
        };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var responseData = new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresAt = expires,
                roles
            };

            return new BaseResponse
            {
                Status = true,
                Message = "Login successful",
                Data = responseData
            };
        }
        public Task<BaseResponse> CreateUser(CreateUserDTO createUser)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> DeleteUser(DeleteUserDTO deleteUser)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> ForgotPasswordAsync(ForgotPassword dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new BaseResponse
                {
                    Status = true, // Important: avoid user enumeration
                    Message = "If the email exists, a reset link has been sent.",
                    Data = null
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var resetLink =
                $"https://yourfrontend.com/reset-password?email={dto.Email}&token={encodedToken}";

            Console.WriteLine($"Password Reset Link: {resetLink}");

            return new BaseResponse
            {
                Status = true,
                Message = "If the email exists, a reset link has been sent.",
                Data = null
            };
        }
        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Invalid request",
                    Data = null
                };
            }

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(dto.Token));

            var result = await _userManager.ResetPasswordAsync(
                user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Data = null
                };
            }

            return new BaseResponse
            {
                Status = true,
                Message = "Password has been reset successfully",
                Data = null
            };
        }
        public Task<BaseResponse> GetAllUser(string name)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetUser(int Id)
        {
            throw new NotImplementedException();
        }

        

        
       
    }

    }

