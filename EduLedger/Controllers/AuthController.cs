using EduLedger.Data;
using EduLedger.Entitites.DTOs.AuthDTO;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EduLedgerDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(EduLedgerDBContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            // 1️ Create Identity user
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                ClassLevelId = dto.ClassLevelId 
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 2️ Validate role
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return BadRequest("Invalid role");

            await _userManager.AddToRoleAsync(user, dto.Role);

            // 3️ Create UserProfile (dependent entity)
            var profile = new UserProfile
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                ResidentialAddress = dto.ResidentialAddress
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.Email)
    };

            claims.AddRange(roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

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

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresAt = expires,
                roles
            });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassworddto)
        {
            
            var user = await _userManager.FindByEmailAsync(forgotPassworddto.Email );
            if (user == null) 
            return Ok(" a reset link has been sent");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token for URL safety
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            // Example reset link (frontend or Postman)
            var resetLink =
                $"https://yourfrontend.com/reset-password?email={forgotPassworddto.Email}&token={encodedToken}";

            // TODO: Send email (for now log it)
            Console.WriteLine($"Password Reset Link: {resetLink}");

            return Ok("If the email exists, a reset link has been sent.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest("Invalid request");

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(dto.Token));

            var result = await _userManager.ResetPasswordAsync(
                user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully");
        }



        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok("Authenticated user");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-classlevel")]
        public IActionResult CreateClassLevel()
        {
            return Ok("Admin access only");
        }

    }
}
