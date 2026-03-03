using EduLedger.Data;
using EduLedger.Entitites.DTOs.AuthDTO;
using EduLedger.Entitites.Models;
using EduLedger.Repository;
using EduLedger.Repository.IRepository;
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
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            var response = await _authRepository.RegisterAsync(dto);
            return response.Status ? Ok(response) : BadRequest(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var response = await _authRepository.LoginAsync(dto);
            return response.Status ? Ok(response) : Unauthorized(response);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassworddto)
        {

            var response = await _authRepository.ForgotPasswordAsync(forgotPassworddto);
            return Ok(response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var response = await _authRepository.ResetPasswordAsync(dto);
            return response.Status ? Ok(response) : BadRequest(response);
        }



        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok("Authenticated user");
        }

    }
}
